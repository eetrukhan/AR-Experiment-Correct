using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Logic
{
    public class GeneratorRunner : MonoBehaviour
    {
        private NotificationsGenerator notificationsGenerator = new NotificationsGenerator();
        private int notificationIndex = 0;
        private int alreadyCorrect = 0;
        public bool isRunning = false;
        private float pause = 0;
        private HashSet<int> runningNums = new HashSet<int>();

        public void Stop()
        {
            StopAllCoroutines();
            Cleaner();
            EventManager.RemoveHandlers();
            ReturnToMainMenu();
        }

        private void Cleaner()
        {
            EventManager.Broadcast(EVENT.HideTray);
            FindObjectOfType<Storage>().removeAllFromStorage();
            isRunning = false;
            alreadyCorrect = 0;
            notificationIndex = 0;
            runningNums = new HashSet<int>();
            GameObject[] toClean = GameObject.FindGameObjectsWithTag("Notification");
            foreach (GameObject clean in toClean)
            {
                Destroy(clean);
            }
        }

        private void ReturnToMainMenu()
        {
            try
            {
                Scene mainMenuScene = SceneManager.GetSceneByName("MainMenu");
                if (mainMenuScene.isLoaded)
                    SceneManager.SetActiveScene(mainMenuScene);
                else
                    SceneManager.LoadScene("MainMenu");
                SceneManager.UnloadSceneAsync(GlobalCommon.currentTypeName);
            }
            catch(Exception e)
            {
                Debug.LogError(e);
                SceneManager.LoadScene("MainMenu");
            }
        }

        public void Update()
        {
            if (isRunning)
            {
                isRunning = false;
                pause = ExperimentData.timeInSeconds / ExperimentData.notificationsNumber;
                StartCoroutine(Runner());
            }
        }

        private IEnumerator Runner()
        {
            Debug.Log("Started" + DateTime.Now);
            for (int k = 0; k < ExperimentData.trialsNumber; k++)
            {
                Debug.Log("Trial: " + (k + 1));
                if (k != 0)
                {
                    Cleaner();
                }
                EventManager.Broadcast(EVENT.TimerShow);
                yield return new WaitForSeconds(GlobalCommon.pauseBetweenTrials);
                EventManager.Broadcast(EVENT.TimerHide);
                for (int i = 0; i < ExperimentData.notificationsNumber; ++i)
                {
                    if (!runningNums.Contains(i))
                    {
                        runningNums.Add(i);
                        Generator();                        
                    }
                    yield return new WaitForSeconds(pause);
                }
                SaveTrialData();
                FindObjectOfType<Storage>().removeAllFromStorage();
                if (k == ExperimentData.trialsNumber - 1) {
                    yield return new WaitForSeconds(0);
                }
            }
            Debug.Log("Stoped" + DateTime.Now);
            Stop();
        }

        private void Generator()
        {
            int atWhichToGenerateHaveToActNotification = ExperimentData.notificationsNumber / ExperimentData.numberOfHaveToActNotifications;
            bool generateHaveToAct = notificationIndex % atWhichToGenerateHaveToActNotification == 0 && alreadyCorrect < ExperimentData.numberOfHaveToActNotifications;
            if (generateHaveToAct)
            {
                alreadyCorrect += 1;
            }
            Notification notification = notificationsGenerator.getNotification(generateHaveToAct);
            var storage = FindObjectOfType<Storage>();
            storage.addToStorage(notification);
            EventManager.Broadcast(EVENT.NotificationCreated);
            SaveLogData(notification);
            notificationIndex += 1;
        }

        private void SaveLogData(Notification notification)
        {
            string logInfo = notification.ToString(GlobalCommon.currentTypeName, "CREATED", "-");
            FileSaver.saveToFile(logInfo);
        }

        private void SaveTrialData()
        {
            FindObjectOfType<TrialDataStorage>().NextTrialExperiment(ExperimentData.subjectNumber, GlobalCommon.currentTypeName, ExperimentData.trialsNumber,
                    ExperimentData.timeInSeconds, ExperimentData.notificationsNumber,
                    ExperimentData.numberOfHaveToActNotifications, ExperimentData.numberOfNonIgnoredHaveToActNotifications,
                    ExperimentData.sumOfReactionTimeToNonIgnoredHaveToActNotifications, ExperimentData.numberOfInCorrectlyActedNotifications);
            FindObjectOfType<TrialDataStorage>().SaveExperimentData();
        }
    }
}
