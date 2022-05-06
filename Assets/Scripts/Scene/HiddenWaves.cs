using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logic
{
    public class HiddenWaves : MonoBehaviour
    {
        public GameObject notification;
        public GameObject trayNotification;
        public GameObject notificationsHolder;
        public GameObject trayHolder;
        public GameObject timer;
        internal string typeName = "HiddenWaves";
        private Color markAsReadColor = new Color32(0, 0, 194, 255);
        private Color hideColor = new Color32(255, 36, 0, 255);
        public Material red;
        public Material blue;
        public Material yellow;
        public Material green;
        public Material grey;

        [SerializeField] private TMP_Text nameText;
        private System.Random random = new System.Random(); 
        // public int correctAuthor;
        public delegate GameObject Generator(GameObject prefabToCreate, Notification notification,
                                          Vector3 position, Vector3 scale, Quaternion rotation,
                                          bool doesHaveGroupIcon);

        public delegate List<Coordinates> Coordinate();

        public void Start()
        {
            EventManager.AddHandler(EVENT.NotificationCreated, rebuildScene);
            EventManager.AddHandler(EVENT.ShowTray, showTray);
            EventManager.AddHandler(EVENT.HideTray, hideTray);
            EventManager.AddHandler(EVENT.TimerShow, showTimer);
            EventManager.AddHandler(EVENT.TimerHide, hideTimer);
            if(notificationsHolder == null)
            {
                notificationsHolder = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()[3];
            }
            if (trayHolder == null)
            {
                trayHolder = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()[4];
            }
            FindObjectOfType<GeneratorRunner>().isRunning = true;
            
            // отображение автора
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
            GameObject[] notificationsObjects = GameObject.FindGameObjectsWithTag("Notification");
            try
            {
                foreach (GameObject notification in notificationsObjects)
                {
                    Destroy(notification);
                }
            }
            catch (Exception e) { }
            trayHolder.SetActive(true);
            notificationsHolder.SetActive(false);
            rebuildScene();
        }

        private void hideTray()
        {
            try
            {
                Vector3 trayPosBefore = trayHolder.transform.position;
                trayPosBefore.y = -0.15f;
                trayHolder.transform.position = trayPosBefore;
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
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
            catch (Exception e) {  }
            try
            {
                foreach (Transform childTransform in trayHolder.transform)
                {
                    Destroy(childTransform.gameObject);
                }
            }
            catch (Exception e) { }
        }

        public void rebuildScene()
        {
           buildHiddenWaves(addMobileNotification, NotificationCoordinates.formTrayCoordinatesArrayMobile);
        }

        public void buildHiddenWaves(Generator notificationGenerator, Coordinate traysCoordinates)
        {
            try
            {
                var storage = FindObjectOfType<Storage>();
                Dictionary<string, NotificationsStorage> orderedNotifications = storage.getStorage();
                Notification n = orderedNotifications.Values.First().Storage.Peek();
                int columnIndex = 1;
                int notififcationsNumberInTraysColumnNow = 0;
                Debug.Log("TRAYHOLD1 " + trayHolder);
                if ((!n.isSilent && trayHolder == null) || (!n.isSilent && trayHolder != null && !trayHolder.activeSelf))
                {
                    GameObject wave = Instantiate(notification) as GameObject;
                    Color c = n.Color;
                    c.a = 0.5f;
                /*
                    if (n.SourceName == "YouTube") wave.GetComponentInChildren<Image>().material = red;
                    if (n.SourceName == "Telegram") wave.GetComponentInChildren<Image>().material = blue;
                    if (n.SourceName == "Яндекс.Почта") wave.GetComponentInChildren<Image>().material = yellow;
                    if (n.SourceName == "WhatsApp") wave.GetComponentInChildren<Image>().material = green;
                    if (n.SourceName == GlobalCommon.silentGroupKey) wave.GetComponentInChildren<Image>().material = grey;
                    wave.GetComponentInChildren<Image>().material.SetFloat("_Glossiness", 1f);
                    */
                    try
                    {
                        wave.transform.SetParent(notificationsHolder.GetComponent<Transform>());
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
                Debug.Log("TRAYHOLD2 " + trayHolder);
                if (trayHolder != null && trayHolder.activeSelf)
                { 
                    /*
                    clearScene();
                    List<Coordinates> coordinates = traysCoordinates();
                    int indexPosition = 0;
                    int maxNotificationsInTray = GlobalCommon.notificationsInColumnTray * GlobalCommon.notificationColumnsTray;
                    foreach (KeyValuePair<string, NotificationsStorage> notificationGroup in orderedNotifications)
                    {
                        Stack<Notification> groupNotifications = notificationGroup.Value.Storage;
                        for (int i = 0; i < groupNotifications.Count; i++)
                        {
                            Notification notification = groupNotifications.ToArray()[i];
                            bool doesHaveGroupIconTray = i == groupNotifications.Count - 1 || indexPosition == columnIndex * GlobalCommon.notificationsInColumnTray - 1;
                            if (indexPosition < maxNotificationsInTray)
                            {
                                Vector3 position = coordinates[indexPosition].Position;
                                Quaternion rotation = Quaternion.Euler(coordinates[indexPosition].Rotation.x, coordinates[indexPosition].Rotation.y, coordinates[indexPosition].Rotation.z);
                                Vector3 scale = coordinates[indexPosition].Scale;
                                GameObject trayN = notificationGenerator(trayNotification, notification, position, scale, rotation, doesHaveGroupIconTray);
                                try
                                {
                                    //trayN.transform.parent = trayHolder.transform;
                                    trayN.transform.SetParent(trayHolder.transform);
                                    trayN.transform.localPosition = position;
                                    trayN.transform.localRotation = rotation;
                                }
                                catch (Exception e) { }
                                indexPosition += 1;
                                notififcationsNumberInTraysColumnNow += 1;
                                if (notififcationsNumberInTraysColumnNow == GlobalCommon.notificationsInColumnTray)
                                {
                                    notififcationsNumberInTraysColumnNow = 0;
                                    columnIndex += 1;
                                }
                                
                            }

                            else
                            {
                                break;
                            }
                        }
                    }
                    */
                    
                     clearScene();
                    List<Coordinates> coordinates = traysCoordinates();
                    int indexPosition = 0;
                    int maxNotificationsInTray = GlobalCommon.notificationsInColumnTray * GlobalCommon.notificationColumnsTray;
                    foreach (KeyValuePair<string, NotificationsStorage> notificationGroup in orderedNotifications)
                    {
                        Stack<Notification> groupNotifications = notificationGroup.Value.Storage;
                        //for (int i = groupNotifications.Count-1; i >=0; i--)
                        for (int i = 0; i < groupNotifications.Count; i++)
                        {
                            Notification notification = groupNotifications.ToArray()[i];
                            bool doesHaveGroupIconTray = i == groupNotifications.Count - 1 || indexPosition == columnIndex * GlobalCommon.notificationsInColumnTray - 1;
                            if (indexPosition < maxNotificationsInTray)
                            {
                                Vector3 position = coordinates[indexPosition].Position;
                                Quaternion rotation = Quaternion.Euler(coordinates[indexPosition].Rotation.x, coordinates[indexPosition].Rotation.y, coordinates[indexPosition].Rotation.z);
                                Vector3 scale = coordinates[indexPosition].Scale;
                                GameObject trayN = notificationGenerator(trayNotification, notification, position, scale, rotation, doesHaveGroupIconTray);
                                try
                                {
                                    //trayN.transform.parent = trayHolder.transform;
                                    trayN.transform.SetParent(trayHolder.transform);
                                    trayN.transform.localPosition = position;
                                    trayN.transform.localRotation = rotation;
                                }
                                catch (Exception e) { }
                                indexPosition += 1;
                                notififcationsNumberInTraysColumnNow += 1;
                                if (notififcationsNumberInTraysColumnNow == GlobalCommon.notificationsInColumnTray)
                                {
                                    notififcationsNumberInTraysColumnNow = 0;
                                    columnIndex += 1;
                                }
                                
                            }

                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                clearScene();
            }
            
        }

        private GameObject addMobileNotification(GameObject prefabToCreate, Notification notification,
                                            Vector3 position, Vector3 scale, Quaternion rotation,
                                            bool doesHaveGroupIcon)
        {
            GameObject notificationObject = Instantiate(prefabToCreate, position, rotation) as GameObject;
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