using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class TrialDataStorage : MonoBehaviour
{
    [Serializable]
    private struct SerializableWrapper
    {
        public SerializableWrapper(Queue<TrialData> allTrialsData)
        {
            AllTrialsData = allTrialsData.ToArray();
        }

        public TrialData[] AllTrialsData;
    }

    private Queue<TrialData> _storedTrialData;
    private TrialData _currentTrialData;

    private const string FILE_NAME = "/AllTrialData.json";

    void Awake()
    {
        try
        { 
            StreamReader reader = new StreamReader(Application.persistentDataPath + FILE_NAME, System.Text.Encoding.UTF8);
            string json = reader.ReadToEnd();
            if (json.Length > 0)
            {
                SerializableWrapper data = JsonUtility.FromJson<SerializableWrapper>(json);
                _storedTrialData = new Queue<TrialData>(data.AllTrialsData);
            }
            else
                _storedTrialData = new Queue<TrialData>();

        }
        catch (Exception e)
        {
            Debug.LogException(e);
            _storedTrialData = new Queue<TrialData>();
        }
    }

    void OnDestroy()
    {
        if (IsThereUnsavedData())
            SaveEverythingToLocalStorage();
    }

    public TrialData GetCurrectTrialData()
    {
        return _currentTrialData;
    }

    public bool IsThereUnsavedData()
    {
        return (_storedTrialData.Count > 0);
    }

    public void NextTrial(uint subjectNo, float intensity, DesignOptions design, string procedure, uint trialNo)
    {
        // Fool proffing
        if (_currentTrialData != null)
            _storedTrialData.Enqueue(_currentTrialData);

        _currentTrialData = new TrialData();
        _currentTrialData.SubjectNo = subjectNo;
        _currentTrialData.IntensityOfObstacleAppearance = intensity;
        _currentTrialData.Design = (uint) design;
        _currentTrialData.Procedure = procedure;
        _currentTrialData.TrialNo = trialNo;
    }

    public void Save()
    {
        if (_currentTrialData != null)
        {
            _storedTrialData.Enqueue(_currentTrialData);
            _currentTrialData = null;
        }

        if (IsThereUnsavedData())
            StartCoroutine(TryToSaveToGoogleSheets());
    }

    private IEnumerator TryToSaveToGoogleSheets()
    {
        TrialData earliestData = _storedTrialData.Peek();

        using (UnityWebRequest www = UnityWebRequest.Post(TrialData.GetFormURI(), earliestData.GetFormFields()))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
                SaveEverythingToLocalStorage();
            }
            else
            {
                // Yepp, we will do this one by one
                _storedTrialData.Dequeue();
                if (IsThereUnsavedData())
                    StartCoroutine(TryToSaveToGoogleSheets());
                else
                    ClearLocalStorage();
            }
        }
    }

    private void SaveEverythingToLocalStorage()
    {
        try
        {
            // We store all the data to a disk but do not clear the _storedTrialData. The letter is quite important
            StreamWriter writer = new StreamWriter(Application.persistentDataPath + FILE_NAME, false, System.Text.Encoding.UTF8);
            writer.Write(JsonUtility.ToJson(new SerializableWrapper(_storedTrialData)));
            writer.Close();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    private void ClearLocalStorage()
    {
        try
        {
            StreamWriter writer = new StreamWriter(Application.persistentDataPath + FILE_NAME, false, System.Text.Encoding.UTF8);
            writer.Close();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}
