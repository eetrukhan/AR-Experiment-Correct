using System;
using System.Linq;
using Intel.RealSense;

[Serializable]
public struct RsConfiguration
{
    public enum Mode
    {
        Live, Playback, Record
    }

    public Mode mode;
    public RsVideoStreamRequest[] Profiles;
    public string RequestedSerialNumber;
    public string PlaybackFile;
    public string RecordPath;
    public string LocalizationMapName;
    // Learn more about these flags here: https://github.com/IntelRealSense/librealsense/blob/83f952a4bd6b70d72459f66c7f67ddaba9d337a0/doc/t265.md#are-there-any-t265-specific-options
    // They are all enabled by default
    public bool EnableMapping;
    public bool EnablePoseJumping;
    public bool EnableRelocalization;
    public bool EnableDynamicCalibration;


    public Config ToPipelineConfig(Pipeline pipeline = null)
    {
        Config cfg = new Config();

        switch (mode)
        {
            case Mode.Live:
                cfg.EnableDevice(RequestedSerialNumber);
                foreach (var p in Profiles)
                    cfg.EnableStream(p.Stream, p.StreamIndex, p.Width, p.Height, p.Format, p.Framerate);

                // Our magic with flags begins here
                if (pipeline == null) break;

                if (!cfg.CanResolve(pipeline))
                    throw new Exception("Couldn't resolve the pipeline");
                PipelineProfile profile = cfg.Resolve(pipeline);
                
                var t265 = profile.Device;
                if (t265 == null)
                    throw new Exception("Couldn't find a tracking camera");

                var sensor = t265.Sensors.First(s => s.Is(Extension.PoseSensor));
                if (sensor == null)
                    throw new Exception("Couldn't find a pose sensor on the T265 camera");

                using (PoseSensor poseSensor = sensor.As<PoseSensor>())
                {
                    poseSensor.Options[Option.EnableMapping].Value = EnableMapping ? 1 : 0;
                    poseSensor.Options[Option.EnablePoseJumping].Value = EnablePoseJumping ? 1 : 0;
                    poseSensor.Options[Option.EnableRelocalization].Value = EnableRelocalization ? 1 : 0;
                    poseSensor.Options[Option.EnableDynamicCalibration].Value = EnableDynamicCalibration ? 1 : 0;
                }

                sensor.Dispose();
                t265.Dispose();
                break;

            case Mode.Playback:
                if (String.IsNullOrEmpty(PlaybackFile))
                {
                    mode = Mode.Live;
                }
                else
                {
                    cfg.EnableDeviceFromFile(PlaybackFile);
                }
                break;

            case Mode.Record:
                foreach (var p in Profiles)
                    cfg.EnableStream(p.Stream, p.StreamIndex, p.Width, p.Height, p.Format, p.Framerate);
                if (!String.IsNullOrEmpty(RecordPath))
                    cfg.EnableRecordToFile(RecordPath);
                break;

        }

        return cfg;
    }
}