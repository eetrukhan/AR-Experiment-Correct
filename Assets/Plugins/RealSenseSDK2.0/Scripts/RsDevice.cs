using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Windows;
using Intel.RealSense;
using System.Collections;
using System.Linq;

/// <summary>
/// Manages streaming using a RealSense Device
/// </summary>
[HelpURL("https://github.com/IntelRealSense/librealsense/tree/master/wrappers/unity")]
public class RsDevice : RsFrameProvider
{
    /// <summary>
    /// The parallelism mode of the module
    /// </summary>
    public enum ProcessMode
    {
        Multithread,
        UnityThread,
    }

    // public static RsDevice Instance { get; private set; }

    /// <summary>
    /// Threading mode of operation, Multithread or UnityThread
    /// </summary>
    [Tooltip("Threading mode of operation, Multithreads or Unitythread")]
    public ProcessMode processMode;

    // public bool Streaming { get; private set; }

    /// <summary>
    /// Notifies upon streaming start
    /// </summary>
    public override event Action<PipelineProfile> OnStart;

    /// <summary>
    /// Notifies when streaming has stopped
    /// </summary>
    public override event Action OnStop;

    /// <summary>
    /// Fired when a new frame is available
    /// </summary>
    public override event Action<Frame> OnNewSample;

    /// <summary>
    /// User configuration
    /// </summary>
    public RsConfiguration DeviceConfiguration = new RsConfiguration
    {
        mode = RsConfiguration.Mode.Live,
        RequestedSerialNumber = string.Empty,
        LocalizationMapName = string.Empty,
        Profiles = new RsVideoStreamRequest[] {
            new RsVideoStreamRequest {Stream = Stream.Depth, StreamIndex = -1, Width = 640, Height = 480, Format = Format.Z16 , Framerate = 30 },
            new RsVideoStreamRequest {Stream = Stream.Infrared, StreamIndex = -1, Width = 640, Height = 480, Format = Format.Y8 , Framerate = 30 },
            new RsVideoStreamRequest {Stream = Stream.Color, StreamIndex = -1, Width = 640, Height = 480, Format = Format.Rgb8 , Framerate = 30 }
        }
    };

    private Thread worker;
    private readonly AutoResetEvent stopEvent = new AutoResetEvent(false);
    private Pipeline m_pipeline;

    void OnEnable()
    {
        if (!string.IsNullOrEmpty(DeviceConfiguration.LocalizationMapName))
            if (ImportLocalizationMap(Application.streamingAssetsPath + "/" + DeviceConfiguration.LocalizationMapName))
                Debug.Log("Localization map is successfully loaded");
        
        m_pipeline = new Pipeline();

        using (var cfg = DeviceConfiguration.ToPipelineConfig(m_pipeline))
            ActiveProfile = m_pipeline.Start(cfg);

        DeviceConfiguration.Profiles = ActiveProfile.Streams.Select(RsVideoStreamRequest.FromProfile).ToArray();

        if (processMode == ProcessMode.Multithread)
        {
            stopEvent.Reset();
            worker = new Thread(WaitForFrames);
            worker.IsBackground = true;
            worker.Start();
        }

        StartCoroutine(WaitAndStart());
    }

    IEnumerator WaitAndStart()
    {
        yield return new WaitForEndOfFrame();
        Streaming = true;
        if (OnStart != null)
            OnStart(ActiveProfile);
    }

    void OnDisable()
    {
        OnNewSample = null;
        // OnNewSampleSet = null;

        if (worker != null)
        {
            stopEvent.Set();
            worker.Join();
        }

        if (Streaming && OnStop != null)
            OnStop();

        if (ActiveProfile != null)
        {
            ActiveProfile.Dispose();
            ActiveProfile = null;
        }

        if (m_pipeline != null)
        {
            // if (Streaming)
            // m_pipeline.Stop();
            m_pipeline.Dispose();
            m_pipeline = null;
        }

        Streaming = false;
    }

    void OnDestroy()
    {
        // OnStart = null;
        OnStop = null;

        if (ActiveProfile != null)
        {
            ActiveProfile.Dispose();
            ActiveProfile = null;
        }

        if (m_pipeline != null)
        {
            m_pipeline.Dispose();
            m_pipeline = null;
        }
    }

    private void RaiseSampleEvent(Frame frame)
    {
        var onNewSample = OnNewSample;
        if (onNewSample != null)
        {
            onNewSample(frame);
        }
    }

    /// <summary>
    /// Worker Thread for multithreaded operations
    /// </summary>
    private void WaitForFrames()
    {
        while (!stopEvent.WaitOne(0))
        {
            using (var frames = m_pipeline.WaitForFrames())
                RaiseSampleEvent(frames);
        }
    }

    void Update()
    {
        if (!Streaming)
            return;

        if (processMode != ProcessMode.UnityThread)
            return;

        FrameSet frames;
        if (m_pipeline.PollForFrames(out frames))
        {
            using (frames)
                RaiseSampleEvent(frames);
        }
    }

    private bool ImportLocalizationMap(string path)
    {
        Context context = new Context();

        Device t265 = context.Devices.First();
        if (t265 == null) return false;

        Sensor sensor = t265.Sensors.First(s => s.Is(Extension.PoseSensor));
        if (sensor == null) return false;

        bool res = false;
        using (PoseSensor poseSensor = sensor.As<PoseSensor>())
        {
            byte[] map = File.ReadAllBytes(path);
            if (map != null && poseSensor.ImportLocalizationMap(map))
                res = true;
        }

        sensor.Dispose();
        t265.Dispose();
        context.Dispose();

        return res;
    }

    public bool ExportLocalizationMap(string path)
    {
        Sensor sensor = ActiveProfile.Device.Sensors.First(s => s.Is(Extension.PoseSensor));
        if (sensor == null) return false;

        bool res = false;
        using (PoseSensor poseSensor = sensor.As<PoseSensor>())
        {
            byte[] map = poseSensor.ExportLocalizationMap();
            if (map != null)
            {
                File.WriteAllBytes(path, map);
                res = true;
            }
        }

        sensor.Dispose();

        return res;
    }

    public bool CreateARAnchor(string name, Vector3 position, Quaternion rotation)
    {
        Sensor sensor = ActiveProfile.Device.Sensors.First(s => s.Is(Extension.PoseSensor));
        if (sensor == null) return false;

        bool res;
        using (PoseSensor poseSensor = PoseSensor.FromSensor(sensor))
        {
            Intel.RealSense.Math.Vector pos = new Intel.RealSense.Math.Vector() { x = position.x, y = position.y, z = position.z };
            Intel.RealSense.Math.Quaternion rot = new Intel.RealSense.Math.Quaternion() { x = rotation.x, y = rotation.y, z = rotation.z, w = rotation.w };

            res = poseSensor.SetStaticNode(name, pos, rot);
        }

        sensor.Dispose();

        return res;
    }

    public bool GetARAnchor(string name, out Vector3 position, out Quaternion rotation)
    {
        Sensor sensor = ActiveProfile.Device.Sensors.First(s => s.Is(Extension.PoseSensor));
        if (sensor == null)
        {
            position = Vector3.zero;
            rotation = Quaternion.identity;
            return false;
        }

        bool res;
        using (PoseSensor poseSensor = PoseSensor.FromSensor(sensor))
        {
            res = poseSensor.GetStaticNode(name, out Intel.RealSense.Math.Vector pos, out Intel.RealSense.Math.Quaternion rot);

            position = new Vector3(pos.x, pos.y, pos.z);
            rotation = new Quaternion(rot.x, rot.y, rot.z, rot.w);
        }

        sensor.Dispose();

        return res;
    }
}
