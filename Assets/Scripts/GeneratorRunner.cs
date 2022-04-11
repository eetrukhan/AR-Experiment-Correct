using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Logic
{
    public class GeneratorRunner : MonoBehaviour
    {
        private System.Random random = new System.Random();
        private NotificationsGenerator notificationsGenerator = new NotificationsGenerator();
        private int notificationIndex = 0;
        private int alreadyCorrect = 0;
        public bool isRunning = false;
        private float pause = 0;
        private HashSet<int> runningNums = new HashSet<int>();
        public static bool startPressed = false;
        private string correctName;
        public static int correctAuthorIndex;
        private ArrayList boolsArrayList;
        private int numberOfNotification;

        [SerializeField] private GameObject aroundObject;
        
        Array values;

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

        void GenerateCorrectName()
        {
            values = Enum.GetValues(typeof(NotificationAuthor));
            random.Next();
            correctAuthorIndex = random.Next(values.Length);
            Debug.Log((NotificationAuthor)correctAuthorIndex);
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
            aroundObject.SetActive(true);
        }

        public void Awake()
        {
            GenerateCorrectName();
            
        }

       

        public void Update()
        {
            if (isRunning && startPressed)
            {
                aroundObject.SetActive(false);
                isRunning = false;
                startPressed = false;
                pause = ExperimentData.timeInSeconds / ExperimentData.notificationsNumber;
                
                boolsArrayList = GenerateBoolArray();
                numberOfNotification = 0;
                
                StartCoroutine(Runner());
            }
        }
        
        private IEnumerator Runner()
        {
            // Added pause formula
            pause = CountingPause(5, ExperimentData.timeInSeconds,ExperimentData.notificationsNumber);

            
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
                yield return new WaitForSeconds(pause);
                for (int i = 0; i < ExperimentData.notificationsNumber; ++i)
                {
                    if (!runningNums.Contains(i))
                    {
                        runningNums.Add(i);
                        Generator();                        
                    }
                     //
                     pause = CountingPause(5, ExperimentData.timeInSeconds,ExperimentData.notificationsNumber);

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

        private IEnumerator StartingPause()
        {
            pause = CountingPause(5, ExperimentData.timeInSeconds,ExperimentData.notificationsNumber);
            yield return new WaitForSeconds(pause);
        }

        private void Generator()
        {
            int atWhichToGenerateHaveToActNotification = ExperimentData.notificationsNumber / ExperimentData.numberOfHaveToActNotifications;
            //bool generateHaveToAct = notificationIndex % atWhichToGenerateHaveToActNotification == 0 && alreadyCorrect < ExperimentData.numberOfHaveToActNotifications;
            bool generateHaveToAct = (bool) boolsArrayList[numberOfNotification];
            Notification notification = notificationsGenerator.getNotification(generateHaveToAct);
            numberOfNotification++;
           // NotificationAuthor notificationAuthor = (NotificationAuthor)values.GetValue(correctAuthor);
           // string author = EnumDescription.getDescription(notificationAuthor);
           if (generateHaveToAct)
            {
                alreadyCorrect += 1;
            }
            
         // Notification notification = notificationsGenerator.getNotification(correctAuthorIndex);
          
          
            var storage = FindObjectOfType<Storage>();
            storage.addToStorage(notification);
            EventManager.Broadcast(EVENT.NotificationCreated);
            SaveLogData(notification);
            notificationIndex += 1;
        }

        
        private ArrayList GenerateBoolArray()
        {
            Debug.Log("BoolArray");
            Debug.Log(ExperimentData.notificationsNumber);
            Debug.Log(ExperimentData.numberOfHaveToActNotifications);
            ArrayList array = new ArrayList();
            for (int i = 0; i < ExperimentData.notificationsNumber; i++)
            {
                array.Add(false);
            }
            
            Stack<int> numbers = new Stack<int>();
            for (int i = 0; i < ExperimentData.numberOfHaveToActNotifications; i++)
            {
                int rnd = random.Next(0, ExperimentData.notificationsNumber);
                while ((bool)array[rnd])
                {
                    rnd = random.Next(0, ExperimentData.notificationsNumber);
                }
                Debug.Log(rnd.ToString());
                array[rnd] = true;
            }
            for (int i = 0; i < array.Count; i++)
            {
                Debug.Log(array[i]);
            }

            return array;
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
                    ExperimentData.sumOfReactionTimeToNonIgnoredHaveToActNotifications, ExperimentData.numberOfInCorrectlyActedNotifications, ExperimentData.sumOfAllReactionTime,ExperimentData.numberOfCorrectReactedNaveToHideNotifications);
            FindObjectOfType<TrialDataStorage>().SaveExperimentData();
        }

        private float CountingPause(int notificationTime, int sessionTime, int notificationsNum)
        {
            float countedPause = 0;
            System.Random random = new System.Random();
            countedPause = random.Next((int)Math.Ceiling(notificationTime / 2f), (int)Math.Ceiling(sessionTime / (float)notificationsNum));
            return countedPause;
        }
    }
}
