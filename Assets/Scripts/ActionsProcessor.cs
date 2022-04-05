using UnityEngine;
using System;
using TMPro;

namespace Logic
{
    public class ActionsProcessor : MonoBehaviour
    {
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
            int a = processExperimentData(notificationObj, tag);
            processHideAndMarkAsRead(id, sourceName, tag, a);
        }

        internal void actionProcessLocalAction(GameObject notification, string tag)
        {
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
            int a = processExperimentData(notificationObj, tag);
            processHideAndMarkAsRead(id, sourceName, tag, a);
        }

        internal void actionProcessGroup(GameObject notification, string tag)
        {
            Color groupColor = notification.transform.GetComponent<MeshRenderer>().material.color;
            string sourceName = groupColor.Equals(Color.gray) ? GlobalCommon.silentGroupKey :
                notification.transform.Find("Source").GetComponent<TextMeshPro>().text;
            string id = notification.transform.Find("Id").GetComponent<TextMeshPro>().text;
            var storage = FindObjectOfType<Storage>();
            Notification notificationObj = storage.getFromStorage(id, sourceName);
            int a = processExperimentData(notificationObj, tag);
            processHideAndMarkAsReadAll(sourceName, tag);
        }

        internal int processExperimentData(Notification notification, string tag)
        {
            Debug.Log("fIRST");
            //var storage = FindObjectOfType<Storage>();
            //Notification notification = storage.getFromStorage(id, sourceName);
            long reactionDuration = DateTime.Now.Ticks - notification.Timestamp;
            if (notification.isCorrect && tag == "MarkAsRead" )
            {
                ExperimentData.numberOfNonIgnoredHaveToActNotifications += 1;
                ExperimentData.sumOfReactionTimeToNonIgnoredHaveToActNotifications += reactionDuration;
                ExperimentData.sumOfAllReactionTime += reactionDuration;
            }
            else if (tag == "Hide")
            {
                ExperimentData.numberOfCorrectReactedNaveToHideNotifications += 1;
                ExperimentData.sumOfAllReactionTime += reactionDuration;

            }
            else
            {
                ExperimentData.numberOfInCorrectlyActedNotifications += 1;
            }
            string logInfo = notification.ToString(GlobalCommon.currentTypeName, "REACTED", reactionDuration.ToString());
            FileSaver.saveToFile(logInfo);
            return 0;
        }

        internal void processHideAndMarkAsRead(string id, string sourceName, string tag, int a)
        {
            Debug.Log("seond");
            Debug.Log(string.Format("Notification with id {0} from source {1} was chosen to {2}", id, sourceName, tag));
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
                case "AroundStickers": {
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
