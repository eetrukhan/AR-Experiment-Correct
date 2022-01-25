using System;
using System.IO;
using UnityEngine;

public class TrackTransformPreserver : MonoBehaviour
{
    public Transform Track;

    public bool RestoreOnStart = true;

    private static readonly string FILE_NAME = "/Track.json";

    private struct TrackTransformData
    {
        public Vector3 trackPosition;
        public Quaternion trackRotation;
        public Vector3 trackScale;
    }

    void Start()
    {
        if (Track == null)
        {
            Debug.LogError("TrackTransformPreserver: The 'Track' field cannot be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        try
        {
            StreamReader reader = new StreamReader(Application.persistentDataPath + FILE_NAME, System.Text.Encoding.UTF8);
            string json = reader.ReadToEnd();
            if (json.Length > 0 && RestoreOnStart)
            {
                TrackTransformData data = JsonUtility.FromJson<TrackTransformData>(json);
                Track.localPosition = data.trackPosition;
                Track.localRotation = data.trackRotation;
                Track.localScale = data.trackScale;
            }
            reader.Close();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public void Preserve()
    {
        if (Track == null)
            return;

        try
        {
            TrackTransformData data = new TrackTransformData();
            data.trackPosition = Track.localPosition;
            data.trackRotation = Track.localRotation;
            data.trackScale = Track.localScale;

            StreamWriter writer = new StreamWriter(Application.persistentDataPath + FILE_NAME, false, System.Text.Encoding.UTF8);
            writer.Write(JsonUtility.ToJson(data));
            writer.Close();
        }
        catch(Exception e)
        {
            Debug.LogException(e);
        }
    }

    public static void ClearData()
    {
        try
        {
            StreamWriter writer = new StreamWriter(Application.persistentDataPath + FILE_NAME, false);
            writer.Close();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public static bool IsThereAnyData()
    {
        try
        {
            StreamReader reader = new StreamReader(Application.persistentDataPath + FILE_NAME, System.Text.Encoding.UTF8);
            string json = reader.ReadToEnd();
            reader.Close();

            return (json.Length > 0);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return false;
        }
    }
}
