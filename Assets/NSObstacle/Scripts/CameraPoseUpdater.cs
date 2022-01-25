using System.Collections;
using System.Collections.Generic;
using Intel.RealSense;
using UnityEngine;

public class CameraPoseUpdater : MonoBehaviour
{
    [SerializeField]
    private RsFrameProvider _source = null;
    public RsFrameProvider Source
    {
        get
        {
            return _source;
        }
        set
        {
            if (value == null)
            {
                Debug.LogError("CameraPoseUpdater: You cannot pass 'null' as a value for the 'Source' field");
                return;
            }

            _source = value;
        }
    }

    private IExtrapolation _extrapolationMethod;
    [SerializeField, Tooltip("Delta time")]
    private float _secondsIntoTheFuture = 0.04f; // One twenty-fifth of a second
    
    [Header("Prediction Accuracy")]
    [SerializeField, Tooltip("The time in seconds since the start of the app when to sum it up")]
    private float _calculateErrorAt = 10f;

    private FrameQueue _frameQueue;
    private RsPoseStreamTransformer.RsPose _pose = new RsPoseStreamTransformer.RsPose();

    private class PredictionData
    {
        public float time;
        public Vector3 position;
        public Quaternion rotation;
    }
    private Queue<PredictionData> _predictionDataBuffer;
    private PredictionData _cumulativePredictionError;
    private bool _predictionErrorHasBeenPrinted = false;

    private static readonly float TIME_INTERVAL = 0.008333f; // One one hundred and twentieth of a second

    private void Start()
    {
        if ((_extrapolationMethod = GetComponent<IExtrapolation>()) == null)
        {
            Debug.LogError("CameraPoseUpdater: Couldn't find an 'IExtrapolation' component. Disabling the script.");
            enabled = false;
            return;
        }
        
        if (_source == null)
        {
            GameObject frameProviderObj = GameObject.Find("RsDevice"); // Yeah, I know it isn't a good practice (better to use FindWithTag) but it is a safeguard mechanism so let it be. (c) John Lennon ;-)
            if (frameProviderObj == null)
            {
                Debug.LogError("CameraPoseUpdater: A 'RsDevice' object cannot be found in the scene. Disabling the script.");
                enabled = false;
                return;
            }

            if ((_source = frameProviderObj.GetComponent<RsFrameProvider>()) == null)
            {
                Debug.LogError("CameraPoseUpdater: The 'RsDevice' object has no 'RsFrameProvider' component. Disabling the script.");
                enabled = false;
                return;
            }

            // Since the tracking is already strated, no need to wait anything
            OnStartStreaming(null);
        }
        else
        {
            _source.OnStart += OnStartStreaming;
            _source.OnStop += OnStopStreaming;
        }

#if DEBUG
        _predictionDataBuffer = new Queue<PredictionData>();
        _cumulativePredictionError = new PredictionData();
#endif
    }

    private void Update()
    {
        if (!TryToUpdatePose())
            return;

        if (_extrapolationMethod == null || !((MonoBehaviour)_extrapolationMethod).enabled)
        {
            transform.localRotation = _pose.rotation;
            transform.position = _pose.translation;
            return;
        }

        try
        {
            float currentTime = Time.unscaledTime;
            /** FIX IT The extrapolation for _secondsIntoTheFuture leads to a lot of noise when a person stays still. It is especially noticeable
             *  on objects that are close to the camera. One way to deal with it would be reducing the delta time depending on the camera velocity.
             *  However, it requires to predict the rotation and position separatly.
             */
            RsPoseStreamTransformer.RsPose predictedPose = _extrapolationMethod.Extrapolate(_pose, currentTime, _secondsIntoTheFuture);
            transform.localRotation = predictedPose.rotation;
            transform.position = predictedPose.translation;
#if DEBUG
            // Store new predictions and calculate cumulative prediction error for previous predictions
            if (currentTime <= _calculateErrorAt)
            {
                _predictionDataBuffer.Enqueue(new PredictionData
                {
                    time = currentTime,
                    position = predictedPose.translation,
                    rotation = predictedPose.rotation
                });
                UpdatePredictionError(currentTime);
            }
            else
                if (!_predictionErrorHasBeenPrinted)
                    PrintPredictionError();
#endif
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }

    private void OnStartStreaming(PipelineProfile profile)
    {
        _frameQueue = new FrameQueue(1);
        _source.OnNewSample += OnNewSample;
    }

    private void OnStopStreaming()
    {
        _source.OnNewSample -= OnNewSample;

        if (_frameQueue != null)
        {
            _frameQueue.Dispose();
            _frameQueue = null;
        }
    }

    private void OnNewSample(Frame f)
    {
        if (f.IsComposite)
        {
            using (var fs = f.As<FrameSet>())
            using (var poseFrame = fs.FirstOrDefault(Stream.Pose, Format.SixDOF))
                if (poseFrame != null)
                    _frameQueue.Enqueue(poseFrame);
        }
        else
        {
            using (var p = f.Profile)
                if (p.Stream == Stream.Pose && p.Format == Format.SixDOF)
                    _frameQueue.Enqueue(f);
        }
    }

    private bool TryToUpdatePose()
    {
        if (_frameQueue == null)
            return false;

        PoseFrame frame;
        if (!_frameQueue.PollForFrame<PoseFrame>(out frame))
            return false;

        using (frame)
        {
            frame.CopyTo(_pose);

            // Convert T265 coordinate system to Unity's
            // see https://realsense.intel.com/how-to-getting-imu-data-from-d435i-and-t265/
            _pose.translation.z = -_pose.translation.z;

            Vector3 eulerAngles = _pose.rotation.eulerAngles;
            _pose.rotation = Quaternion.Euler(
                -eulerAngles.x,
                -eulerAngles.y,
                 eulerAngles.z);
        }
        return true;
    }

    private void UpdatePredictionError(float currentTime)
    {
        PredictionData oldestPrediction = _predictionDataBuffer.Peek();
        while (DoTimesMatch(oldestPrediction.time, currentTime, TIME_INTERVAL))
        {
            _cumulativePredictionError.position.x += Mathf.Abs(oldestPrediction.position.x - _pose.translation.x);
            _cumulativePredictionError.position.y += Mathf.Abs(oldestPrediction.position.y - _pose.translation.y);
            _cumulativePredictionError.position.z += Mathf.Abs(oldestPrediction.position.z - _pose.translation.z);

            _cumulativePredictionError.rotation.x += Mathf.Abs(oldestPrediction.rotation.x - _pose.rotation.x);
            _cumulativePredictionError.rotation.y += Mathf.Abs(oldestPrediction.rotation.y - _pose.rotation.y);
            _cumulativePredictionError.rotation.z += Mathf.Abs(oldestPrediction.rotation.z - _pose.rotation.z);
            _cumulativePredictionError.rotation.w += Mathf.Abs(oldestPrediction.rotation.w - _pose.rotation.w);

            _predictionDataBuffer.Dequeue();

            if (_predictionDataBuffer.Count == 0)
                break;
            else
                oldestPrediction = _predictionDataBuffer.Peek();
        }
    }

    private void PrintPredictionError()
    {
        Debug.Log(string.Format("Cumulative prediction error at {0} seconds", _calculateErrorAt));
        Debug.Log(string.Format("Position: {0}, {1}, {2}", _cumulativePredictionError.position.x, _cumulativePredictionError.position.y, _cumulativePredictionError.position.z));
        Debug.Log(string.Format("Rotation: {0}, {1}, {2}, {3}", _cumulativePredictionError.rotation.x, _cumulativePredictionError.rotation.y, _cumulativePredictionError.rotation.z, _cumulativePredictionError.rotation.w));
        _predictionErrorHasBeenPrinted = true;
    }

    private static bool DoTimesMatch(float predictionTime, float currentTime, float interval)
    {
        interval /= 2f;
        return (predictionTime >= (currentTime - interval));
    }
}
