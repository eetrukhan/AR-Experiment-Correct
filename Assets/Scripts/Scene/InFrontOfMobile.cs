using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Logic
{
    public class InFrontOfMobile : MonoBehaviour
    {
        public GameObject notification;
        public GameObject trayNotification;
        public GameObject notificationsHolder;
        public GameObject trayHolder;
        public GameObject timer;
        internal string typeName = "InFrontOfMobile";
        internal int notificationsInColumn = 3;
        internal int notificationColumns = 1;
        private Color markAsReadColor = new Color32(0, 0, 194, 255);
        private Color hideColor = new Color32(255, 36, 0, 255);
        public Material red;
        public Material blue;
        public Material yellow;
        public Material green;
        public Material grey;
        
        [SerializeField] private TMP_Text nameText;

        public delegate GameObject Generator(GameObject prefabToCreate, Notification notification, Vector3 scale, bool doesHaveGroupIcon);

        public delegate List<Coordinates> Coordinate();

        public void Start()
        {
            EventManager.AddHandler(EVENT.NotificationCreated, rebuildScene);
            EventManager.AddHandler(EVENT.ShowTray, showTray);
            EventManager.AddHandler(EVENT.HideTray, hideTray);
            EventManager.AddHandler(EVENT.TimerShow, showTimer);
            EventManager.AddHandler(EVENT.TimerHide, hideTimer);
            FindObjectOfType<GeneratorRunner>().isRunning = true;
            
            DisplayName();
            
            
        }

        private void DisplayName()
        {
            Array values = Enum.GetValues(typeof(NotificationAuthor));
            NotificationAuthor notificationAuthor = (NotificationAuthor)values.GetValue(GeneratorRunner.correctAuthorIndex);
            string author = EnumDescription.getDescription(notificationAuthor);
            nameText.text = author;
        }

        private void hideTimer()
        {
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()[5].SetActive(false);
        }

        private void showTimer()
        {
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()[5].SetActive(true);
        }

        private void showTray()
        {
           // UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()[4].SetActive(false);
           // UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()[3].SetActive(true);
           trayHolder.SetActive(true);
           notificationsHolder.SetActive(false);
        }

        private void hideTray()
        {
            try
            {
                Vector3 trayPosBefore = trayHolder.transform.position;
                //trayPosBefore.y = 10;
                trayPosBefore.y = 5;
                trayHolder.transform.position = trayPosBefore;
            }
            catch(Exception e)
            {
                Debug.Log(e);
            }
            //UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()[3].SetActive(false);
           // UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()[4].SetActive(true);
           trayHolder.SetActive(false);
           notificationsHolder.SetActive(true);
        }

        private void clearScene()
        {
            GameObject[] notificationsObjects = GameObject.FindGameObjectsWithTag("Notification");
            try
            {
                foreach (GameObject notification in notificationsObjects)
                {
                    Destroy(notification);
                }
            }
            catch (Exception e) { }
            try
            {
                foreach (Transform childTransform in trayHolder.transform)
                {
                    Destroy(childTransform.gameObject);
                }
            }
            catch (Exception e) {  }
        }

        public void rebuildScene()
        {
            buildInFrontOf(addMobileNotification, NotificationCoordinates.formInFrontOfMobileCoordinatesArray, NotificationCoordinates.formTrayCoordinatesArrayMobile);
        }

        public void buildInFrontOf(Generator notificationGenerator, Coordinate notificationCoordinates, Coordinate traysCoordinates)
        {
            var storage = FindObjectOfType<Storage>();
            Dictionary<string, NotificationsStorage> orderedNotifications = storage.getStorage();
            clearScene();
            List<Coordinates> coordinates = notificationCoordinates();
            List<Coordinates> trayCoordinates = traysCoordinates();
            int usualCoordinatesIndex = 0;
            int trayCoordinatesIndex = 0;
            int columnIndex = 1;
            int notififcationsNumberInTraysColumnNow = 0;
            int maxNotifications = notificationsInColumn * notificationColumns;
            int maxNotificationsInTray = GlobalCommon.notificationsInColumnTray * GlobalCommon.notificationColumnsTray;
            foreach (KeyValuePair<string, NotificationsStorage> notificationGroup in orderedNotifications)
            {
                Stack<Notification> groupNotifications = notificationGroup.Value.Storage;
                for (int i = 0; i < groupNotifications.Count; i++)
                {
                    Notification notificationInGroup = groupNotifications.ToArray()[i];
                    if (trayCoordinatesIndex < maxNotificationsInTray) // tray case
                    {
                        bool doesHaveGroupIconTray = i == groupNotifications.Count - 1 || trayCoordinatesIndex == columnIndex * GlobalCommon.notificationsInColumnTray - 1;
                        Vector3 position = trayCoordinates[trayCoordinatesIndex].Position;
                        Quaternion rotation = Quaternion.Euler(trayCoordinates[trayCoordinatesIndex].Rotation.x, trayCoordinates[trayCoordinatesIndex].Rotation.y, trayCoordinates[trayCoordinatesIndex].Rotation.z);
                        Vector3 scale = trayCoordinates[trayCoordinatesIndex].Scale;
                        GameObject trayN = notificationGenerator(trayNotification,
                                              notificationInGroup,
                                              scale,
                                              doesHaveGroupIconTray);
                        try
                        {
                            //trayN.transform.parent = trayHolder.transform;
                            trayN.transform.SetParent(trayHolder.transform);
                            trayN.transform.localPosition = position;
                            trayN.transform.localRotation = rotation;
                        }
                        catch (Exception e) { }
                        trayCoordinatesIndex += 1;
                        notififcationsNumberInTraysColumnNow += 1;
                        if (notififcationsNumberInTraysColumnNow == GlobalCommon.notificationsInColumnTray)
                        {
                            notififcationsNumberInTraysColumnNow = 0;
                            columnIndex += 1;
                        }

                    }
                    if (usualCoordinatesIndex < maxNotifications
                        && trayHolder != null
                        && !trayHolder.activeSelf
                        && notificationInGroup != null
                        && !notificationInGroup.isMarkedAsRead
                        && !notificationInGroup.isSilent) // usual case 
                    {
                        bool doesHaveGroupIcon = i == 0 || usualCoordinatesIndex == 0;
                        Vector3 position = coordinates[usualCoordinatesIndex].Position;
                        Quaternion rotation = Quaternion.Euler(coordinates[usualCoordinatesIndex].Rotation.x, coordinates[usualCoordinatesIndex].Rotation.y, coordinates[usualCoordinatesIndex].Rotation.z);
                        Vector3 scale = coordinates[usualCoordinatesIndex].Scale;
                        GameObject n = notificationGenerator(notification,
                                             notificationInGroup,
                                             scale,
                                             doesHaveGroupIcon);
                       //n.transform.parent = notificationsHolder.transform;
                       n.transform.SetParent(notificationsHolder.transform);
                        n.transform.localPosition = position;
                        n.transform.localRotation = rotation;
                        usualCoordinatesIndex += 1;
                    }
                }
            }
        }

        private GameObject addMobileNotification(GameObject prefabToCreate, Notification notification,
                                            Vector3 scale, bool doesHaveGroupIcon)
        {
            GameObject notificationObject = Instantiate(prefabToCreate) as GameObject;
            if (doesHaveGroupIcon)
            {
                notificationObject.GetComponentsInChildren<MeshRenderer>()[10].gameObject.transform.localScale = new Vector3(0.5f, 0.05f, 0.5f);
                notificationObject.GetComponentsInChildren<SpriteRenderer>()[2].sprite = Resources.Load<Sprite>("Sprites/" + notification.SourceImage);
            }
            else
            {
                notificationObject.GetComponentsInChildren<MeshRenderer>()[10].gameObject.transform.localScale = new Vector3(0, 0, 0);
            }
            notificationObject.GetComponentsInChildren<TextMeshPro>()[0].text = notification.Text;
            notificationObject.GetComponentsInChildren<TextMeshPro>()[1].text = notification.Author;
            notificationObject.GetComponentsInChildren<TextMeshPro>()[2].text = notification.SourceName;
            notificationObject.GetComponentsInChildren<TextMeshPro>()[4].text = notification.Id;
            DateTime currentTime = DateTime.Now;
            double minutes = currentTime.Subtract(new DateTime(notification.Timestamp)).TotalMinutes;
            double seconds = currentTime.Subtract(new DateTime(notification.Timestamp)).TotalSeconds;
            notificationObject.GetComponentsInChildren<TextMeshPro>()[3].text = minutes < 1 ? seconds < 1 ? "Just now" :
                                                                                                                      string.Format("{0:00}s ago", seconds) :
                                                                                                        string.Format("{0:00}m ago", minutes);
            notificationObject.GetComponentsInChildren<SpriteRenderer>()[1].sprite = Resources.Load<Sprite>("Sprites/" + notification.Icon);
            notificationObject.transform.localScale = scale;
            notificationObject.GetComponentsInChildren<MeshRenderer>()[10].material.SetColor("_Color", notification.Color);
            notificationObject.GetComponentsInChildren<MeshRenderer>()[10].material.SetFloat("_Glossiness", 1f);
            notificationObject.GetComponentsInChildren<SpriteRenderer>(true)[8].material.SetColor("_Color", markAsReadColor);
            notificationObject.GetComponentsInChildren<SpriteRenderer>(true)[10].material.SetColor("_Color", hideColor);
            notificationObject.GetComponentsInChildren<SpriteRenderer>(true)[6].material.SetColor("_Color", markAsReadColor);
            notificationObject.GetComponentsInChildren<SpriteRenderer>(true)[4].material.SetColor("_Color", hideColor);
            return notificationObject;
        }
    }
}
