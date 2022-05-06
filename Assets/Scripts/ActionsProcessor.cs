using UnityEngine;
using System;
using System.Globalization;
using TMPro;

namespace Logic
{
    public class ActionsProcessor : MonoBehaviour
    {
        private bool reactionCounted = false;
        private long decisionDuration;
        [SerializeField] private TextMeshPro id;
        public void OnCollision()
        {
            if (!reactionCounted)
            {
                var storage = FindObjectOfType<Storage>();
                Notification notificationObj = storage.getFromStorage(id.text, "Telegram");
                long reactionDuration = DateTime.Now.Ticks - notificationObj.Timestamp;
                decisionDuration += reactionDuration;
                reactionCounted = true;
                Debug.Log("Reaction counted");
            }
        }

        internal void actionOpenSourceApplication(GameObject notification)
        {
            string id = notification.transform.Find("Id").GetComponent<TextMeshPro>().text;
            Color groupColor;
            string sourceName;
            if (!GlobalCommon.currentTypeName.Contains("Sticker"))
            {
                groupColor = notification.transform.Find("GroupIcon").GetComponent<MeshRenderer>().material.color;
                sourceName = groupColor.Equals(Color.gray) ? GlobalCommon.silentGroupKey :
                    notification.transform.Find("Source").GetComponent<TextMeshPro>().text;
            }
            else
            {
                groupColor = notification.transform.Find("Box").GetComponent<SpriteRenderer>().material.color;
                sourceName = groupColor.Equals(Color.gray) ? GlobalCommon.silentGroupKey :
                    notification.transform.Find("Source").GetComponent<TextMeshPro>().text;
            }
            var storage = FindObjectOfType<Storage>();
            Notification notificationObj = storage.getFromStorage(id, sourceName);
            processExperimentData(notificationObj, tag);
            processHideAndMarkAsRead(id, sourceName, tag);
        }

        internal void actionProcessLocalAction(GameObject notification, string tag)
        {
//            Debug.Log("HERERERERERER");
            string id = notification.transform.Find("Id").GetComponent<TextMeshPro>().text;
            Color groupColor;
            if (!GlobalCommon.currentTypeName.Contains("Sticker"))
            {
                groupColor = notification.transform.Find("GroupIcon").GetComponent<MeshRenderer>().material.color;
            }
            else
            {
                groupColor = notification.transform.Find("Box").GetComponent<SpriteRenderer>().material.color;
            }
            string sourceName = groupColor.Equals(Color.gray) ? GlobalCommon.silentGroupKey :
                notification.transform.Find("Source").GetComponent<TextMeshPro>().text;
            var storage = FindObjectOfType<Storage>();
            Notification notificationObj = storage.getFromStorage(id, sourceName);
            if (notificationObj == null)
            {
                Debug.Log(storage.getStorage().Values.Count);
            }
            else
            {
                Debug.Log("ok");
                processExperimentData(notificationObj, tag);
                processHideAndMarkAsRead(id, sourceName, tag);
            }
            
        }

        internal void actionProcessGroup(GameObject notification, string tag)
        {
            /*
            Color groupColor = notification.transform.GetComponent<MeshRenderer>().material.color;
            string sourceName = groupColor.Equals(Color.gray) ? GlobalCommon.silentGroupKey :
                notification.transform.Find("Source").GetComponent<TextMeshPro>().text;
            string id = notification.transform.Find("Id").GetComponent<TextMeshPro>().text;
            var storage = FindObjectOfType<Storage>();
            Notification notificationObj = storage.getFromStorage(id, sourceName);
            int a = processExperimentData(notificationObj, tag);
            processHideAndMarkAsReadAll(sourceName, tag);
            */
        }

        internal void processExperimentData(Notification notification, string tag)
        {
            Debug.Log("fIRST");
            // var storage = FindObjectOfType<Storage>();
            // Notification notification = storage.getFromStorage(id, sourceName);
            long reactionDuration = DateTime.Now.Ticks - notification.Timestamp;

            ExperimentData.SumOfDecisionMakingTime += decisionDuration;
            reactionCounted = false;
            Debug.Log("Reaction saved");
            
            if (notification.isCorrect && tag == "MarkAsRead" )
            {
                Debug.Log(1);
                ExperimentData.NumberOfCorrectReactedDesiredNotifications += 1;
                ExperimentData.SumOfReactionTimeOnDesiredNotifications += reactionDuration;
                ExperimentData.NumberOfMissedDesiredNotifications -= 1;
            } 
            if (!notification.isCorrect && tag == "Hide")
            {
                Debug.Log(2);
                ExperimentData.NumberOfCorrectReactedUnnecessaryNotifications += 1;
                //ExperimentData.SumOfDecisionMakingTime += reactionDuration;
                ExperimentData.NumberOfMissedUnnecessaryNotifications -= 1;
            }
            if (!notification.isCorrect && tag == "MarkAsRead")
            {
                Debug.Log(3);
                ExperimentData.NumberOfMissedUnnecessaryNotifications -= 1;
                //ExperimentData.SumOfDecisionMakingTime += reactionDuration;
            }
            if (notification.isCorrect && tag == "Hide")
            {
                Debug.Log(4);
                ExperimentData.NumberOfMissedDesiredNotifications -= 1;
                ExperimentData.SumOfReactionTimeOnDesiredNotifications += reactionDuration;
            }
            
            DateTime reactiondate = DateTime.Now;
            string logInfo = notification.ToString(GlobalCommon.currentTypeName, "REACTED", (((float)reactionDuration)/TimeSpan.TicksPerSecond).ToString(), reactiondate);
            FileSaver.saveToFile(logInfo);
            
        }
        
        /*
        internal void processExperimentData(Notification notification, string tag)
        {
            Debug.Log("fIRST");
           // var storage = FindObjectOfType<Storage>();
           // Notification notification = storage.getFromStorage(id, sourceName);
            long reactionDuration = DateTime.Now.Ticks - notification.Timestamp;
            
            
            if (notification.isCorrect && tag == "MarkAsRead" )
            {
                ExperimentData.numberOfNonIgnoredHaveToActNotifications += 1;
                ExperimentData.sumOfReactionTimeToNonIgnoredHaveToActNotifications += reactionDuration;
                ExperimentData.sumOfAllReactionTime += reactionDuration;
            }
            else if (!notification.isCorrect && tag == "Hide")
            {
                
            }
            else if (!notification.isCorrect && tag == "MarkAsRead")
            {
                
            }
            else if (notification.isCorrect && tag == "Hide")
            {
                
            }
            else if (tag == "Hide")
            {
                ExperimentData.numberOfCorrectReactedNaveToHideNotifications += 1;
                ExperimentData.sumOfAllReactionTime += reactionDuration;

            }
            else
            {
                ExperimentData.sumOfAllReactionTime += reactionDuration;
                ExperimentData.numberOfInCorrectlyActedNotifications += 1;
            }
            DateTime reactiondate = DateTime.Now;
            string logInfo = notification.ToString(GlobalCommon.currentTypeName, "REACTED", (((float)reactionDuration)/TimeSpan.TicksPerSecond).ToString(), reactiondate);
            FileSaver.saveToFile(logInfo);
        }
        */

        internal void processHideAndMarkAsRead(string id, string sourceName, string tag)
        {
            Debug.Log("second");
            string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff",
                CultureInfo.InvariantCulture);
            Debug.Log(string.Format(timestamp + " : "+"Notification with id {0} from source {1} was chosen to {2}", id, sourceName, tag));
            var storage = FindObjectOfType<Storage>();
            storage.removeFromStorage(id, sourceName, tag);
            rebuildSwitcher();
        }

        internal void processHideAndMarkAsReadAll(string sourceName, string tag)
        {
            
            Debug.Log(string.Format("Notifications from source {0} were chosen to {1}", sourceName, tag));
            var storage = FindObjectOfType<Storage>();
            storage.removeAllFromStorage(sourceName, tag);
            rebuildSwitcher();
        }

        private void rebuildSwitcher()
        {
            switch (GlobalCommon.currentTypeName)
            {
                case "InFrontOfMobile": {
                        FindObjectOfType<InFrontOfMobile>().rebuildScene();
                        break;
                    }
                case "InFrontOfStickers": {
                        FindObjectOfType<InFrontOfStickers>().rebuildScene();
                        break;
                    }
                case "AroundMobile": {
                        FindObjectOfType<AroundMobile>().rebuildScene();
                        break;
                    }
                case "NewAroundStickers": {
                        FindObjectOfType<AroundStickers>().rebuildScene();
                        break;
                    }
                case "HiddenWaves": {
                        FindObjectOfType<HiddenWaves>().rebuildScene();
                        break;
                    }
            }
        }
    }
}
