using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorsoReferencedContent : MonoBehaviour
{
    public enum RotationInHorizontalPlane
    {
        None = 0,
        WithTheHead = 1,
        InTheDirectionOfMotion = 2,
        AlongTheTrack = 3
    }

    [Tooltip("The camera is needed to emulate a torso reference frame")]
    public GameObject Camera;

    [Tooltip("The distance from the camera that this object should be placed")]
    public float DistanceFromCamera = 0.75f;

    public Transform Track;

    [Tooltip("If checked, makes objecta move smoothly")]
    public bool SimulateInertia = false;

    [Tooltip("The speed at which this object changes its position, if the inertia effect is enabled")]
    public float PositionLerpSpeed = 5f;

    [Tooltip("The speed at which this object changes its rotation, if the inertia effect is enabled")]
    public float RotationLerpSpeed = 5f;

    public float Pitch = 10f;

    public float Yaw = 0f;

    // TODO: Add comments
    [SerializeField]
    private RotationInHorizontalPlane _rotationMode = RotationInHorizontalPlane.WithTheHead;

    public RotationInHorizontalPlane RotationMode
    {
        get => _rotationMode;
        set
        {
            if (value == RotationInHorizontalPlane.None)
                _initialVector = getVectorFromCameraToObject();
            _rotationMode = value;
        }
    }

    private Vector3 _initialVector;

    public int MotionNum = 5;

    private Queue<Vector3> _lastMotions = new Queue<Vector3>();

    public float TimeThreshold = 0.25f;

    private float _lastInterval;

    public float DistanceThreshold = 0.2f;

    private Vector3 _lastKnownPosition;

    /// <summary>
    /// Ensures that everying is ready.
    /// </summary>
    void OnEnable()
    {
        // Disable the script if there is no camera
        if (Camera == null)
        {
            Debug.LogError("Error: TorsoReferencedContent.Camera is not set. Disabling the script.");
            enabled = false;
            return;
        }

        if (_rotationMode == RotationInHorizontalPlane.AlongTheTrack && Track == null)
        {
            Debug.LogError("Error: You can't use the AlongTheTrack mode and leave the Track field unassigned. Disabling the script.");
            enabled = false;
            return;
        }
        
        // TODO: Deal with the default vector. It's always to the left
        _initialVector = getVectorFromCameraToObject();

        // Preventing the object from flying around when it's just been activated
        bool SimulateInertiaTrueValue = SimulateInertia;
        SimulateInertia = false;
        Update();
        SimulateInertia = SimulateInertiaTrueValue;
    }

    void Update()
    {
        Vector3 vec;
        switch(_rotationMode)
        {
            case RotationInHorizontalPlane.None:
                vec = _initialVector;
                break;
            case RotationInHorizontalPlane.WithTheHead:
                vec = getVectorFromCameraToObject();
                break;
            case RotationInHorizontalPlane.InTheDirectionOfMotion:
                // Trying to figure out approximate torso rotation
                float timeNow = Time.realtimeSinceStartup;
                if (timeNow > _lastInterval + TimeThreshold)
                {
                    Vector3 travel = Camera.transform.position - _lastKnownPosition;
                    if (travel.magnitude > DistanceThreshold)
                    {
                        Quaternion rotation = Quaternion.LookRotation(travel);
                        rotation = Quaternion.Euler(Pitch,
                            rotation.eulerAngles.y + Yaw,
                            rotation.eulerAngles.z);
                        addNewMotion(rotation * (Vector3.forward * DistanceFromCamera));
                        _initialVector = avgVector(_lastMotions);
                    }
                    else
                    {
                        if (_lastMotions.Count > 0)
                            _lastMotions.Dequeue();
                    }
                    _lastKnownPosition = Camera.transform.position;
                    _lastInterval = timeNow;
                }
                vec = _initialVector;
                break;
            case RotationInHorizontalPlane.AlongTheTrack:
                float correctionY = Vector3.Dot(Track.forward, Camera.transform.forward) > 0 ? 0f : 180f;
                Quaternion r = Quaternion.Euler(Pitch,
                    Track.rotation.eulerAngles.y + Yaw + correctionY,
                    Track.rotation.eulerAngles.z);
                vec = r * (Vector3.forward * DistanceFromCamera);
                break;
            default:
                return;
        }

        Vector3 posTo = Camera.transform.position + vec;
        Quaternion rotTo = Quaternion.LookRotation(transform.position - Camera.transform.position);

        if (SimulateInertia)
        {
            float posSpeed = Time.deltaTime * PositionLerpSpeed;
            transform.position = Vector3.SlerpUnclamped(transform.position, posTo, posSpeed);

            float rotSpeed = Time.deltaTime * RotationLerpSpeed;
            transform.rotation = Quaternion.Slerp(transform.rotation, rotTo, rotSpeed);
        }
        else
        {
            transform.position = posTo;

            transform.rotation = rotTo;
        }
    }

    private Vector3 getVectorFromCameraToObject()
    {
        Quaternion rotation = Quaternion.Euler(Pitch,
            Camera.transform.rotation.eulerAngles.y + Yaw,
            Camera.transform.rotation.eulerAngles.z);
        return rotation * (Vector3.forward * DistanceFromCamera);
    }

    private void addNewMotion(Vector3 v)
    {
        if (_lastMotions.Count >= MotionNum)
            do
            {
                _lastMotions.Dequeue();
            }
            while (_lastMotions.Count == MotionNum - 1);

        _lastMotions.Enqueue(v);
    }

    private static Vector3 avgVector(Queue<Vector3> q)
    {
        Vector3 res = Vector3.zero;
        if (q.Count == 0)
            return res;
        
        foreach (Vector3 vec in q)
            res = res + vec;
        return res / q.Count;
    }

    public void SetDistanceFromCamera(float value)
    {
        DistanceFromCamera = value;
    }

    public void SetPitch(float value)
    {
        Pitch = value;
    }

    public void SetYaw(float value)
    {
        Yaw = value;
    }

    public void SetRotationMode(int value)
    {
        _rotationMode = (RotationInHorizontalPlane)value;
    }
}
