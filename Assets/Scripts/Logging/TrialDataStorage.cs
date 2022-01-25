using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Logic
{
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
                Debug.Log(e);
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
            if (_storedTrialData != null)
                return _storedTrialData.Count > 0;
            return false;
        }

        public void NextTrialExperiment(int SubjectNumber, string Design, int TrialNumber, float Time,
            int NotificationsNumber, int NumberOfHaveToActNotifications, int NumberOfNonIgnoredHaveToActNotifications,
            float SumOfReactionTimeToNonIgnoredHaveToActNotifications, int NumberOfInCorrectlyActedNotifications)
        {
            // Fool proffing
            if (_currentTrialData != null)
                _storedTrialData.Enqueue(_currentTrialData);

            _currentTrialData = new TrialData();
            _currentTrialData.SubjectNumber = SubjectNumber;
            _currentTrialData.Design = Design;
            _currentTrialData.TrialNumber = TrialNumber;
            _currentTrialData.Time = Time;
            _currentTrialData.NotificationsNumber = NotificationsNumber;
            _currentTrialData.NumberOfHaveToActNotifications = NumberOfHaveToActNotifications;
            _currentTrialData.NumberOfNonIgnoredHaveToActNotifications = NumberOfNonIgnoredHaveToActNotifications;
            _currentTrialData.SumOfReactionTimeToNonIgnoredHaveToActNotifications = SumOfReactionTimeToNonIgnoredHaveToActNotifications;
            _currentTrialData.NumberOfInCorrectlyActedNotifications = NumberOfInCorrectlyActedNotifications;
        }

        public void SaveExperimentData()
        {
            if (_currentTrialData != null)
            {
                if (_storedTrialData == null)
                {
                    _storedTrialData = new Queue<TrialData>();
                }
                _storedTrialData.Enqueue(_currentTrialData);
                _currentTrialData = null;
            }

            if (IsThereUnsavedData())
                StartCoroutine(TryToSaveToGoogleSheets());
        }

        public IEnumerator TryToSaveToGoogleSheets()
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
                    if (_storedTrialData.Count > 0)
                    {
                        _storedTrialData.Dequeue();
                        if (IsThereUnsavedData())
                            StartCoroutine(TryToSaveToGoogleSheets());
                        else
                            ClearLocalStorage();
                    }                
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
}