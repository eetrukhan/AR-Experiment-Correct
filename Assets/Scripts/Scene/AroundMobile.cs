﻿using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Logic
{
    public class AroundMobile : MonoBehaviour
    {
        public GameObject notification;
        public GameObject trayNotification;
        public GameObject notificationsHolder;
        public GameObject trayHolder;
        public GameObject timer;
        internal string typeName = "AroundMobile";
        internal int notificationsInColumn = 3;
        internal int notificationColumns = 5;
        private Color markAsReadColor = new Color32(0, 0, 194, 255);
        private Color hideColor = new Color32(255, 36, 0, 255);
        public Material red;
        public Material blue;
        public Material yellow;
        public Material green;
        public Material grey;
        
        [SerializeField] private TMP_Text nameText;

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
           // UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()[14].SetActive(false);
           timer.SetActive(false);
        }

        private void showTimer()
        {
           // UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects()[14].SetActive(true);
           timer.SetActive(true);
        }

        private void showTray()
        {
            notificationsHolder.SetActive(false);
            trayHolder.SetActive(true);
        }

        private void hideTray()
        {
            try
            {
                Vector3 trayPosBefore = trayHolder.transform.position;
                trayPosBefore.y = 10;
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
            buildAround(addMobileNotification, NotificationCoordinates.formAroundMobileCoordinatesArray, NotificationCoordinates.formTrayCoordinatesArrayMobile);
        }

        public void buildAround(Generator notificationGenerator, Coordinate notificationCoordinates, Coordinate traysCoordinates)
        {
            var storage = FindObjectOfType<Storage>();
            Dictionary<string, NotificationsStorage> orderedNotifications = storage.getStorage();
            clearScene();
            var coordinates = notificationCoordinates();
            var trayCoordinates = traysCoordinates();
            var trayCoordinatesIndex = 0;
            int groupIndex = 0;
            int columnIndex = 1;
            int notififcationsNumberInTraysColumnNow = 0;
            foreach (KeyValuePair<string, NotificationsStorage> notificationGroup in orderedNotifications)
            {
                Stack<Notification> groupNotifications = notificationGroup.Value.Storage;
                int usualCoordinatesIndex = groupIndex * notificationsInColumn;
                //for (int i = 0; i < groupNotifications.Count; i++)
                for (int i = groupNotifications.Count-1; i >=0; i--)
                {
                    Notification notificationInGroup = groupNotifications.ToArray()[i];
                    //if (trayCoordinatesIndex < maxNotificationsInTray) // tray case
                    {
                        bool doesHaveGroupIconTray = i == groupNotifications.Count - 1 || trayCoordinatesIndex == columnIndex * GlobalCommon.notificationsInColumnTray - 1;
                        Vector3 position = coordinates[usualCoordinatesIndex].Position;//trayCoordinates[trayCoordinatesIndex].Position;
                        Quaternion rotation = Quaternion.Euler(coordinates[usualCoordinatesIndex].Rotation.x, coordinates[usualCoordinatesIndex].Rotation.y, coordinates[usualCoordinatesIndex].Rotation.z); //Quaternion.Euler(trayCoordinates[trayCoordinatesIndex].Rotation.x, trayCoordinates[trayCoordinatesIndex].Rotation.y, trayCoordinates[trayCoordinatesIndex].Rotation.z);
                        Vector3 scale = coordinates[usualCoordinatesIndex].Scale; //trayCoordinates[trayCoordinatesIndex].Scale;
                        GameObject trayN = notificationGenerator(trayNotification,
                                              notificationInGroup,
                                              position,
                                              scale,
                                              rotation,
                                              doesHaveGroupIconTray);
                        try
                        {
                            //trayN.transform.parent = trayHolder.transform;
                            trayN.transform.SetParent(trayHolder.transform);
                            //trayN.transform.localPosition = position;
                            //trayN.transform.localRotation = rotation;
                            usualCoordinatesIndex += 1;
                        }
                        catch (Exception e)
                        {
                            clearScene();
                        }
                        trayCoordinatesIndex += 1;
                        notififcationsNumberInTraysColumnNow += 1;
                        if (notififcationsNumberInTraysColumnNow == GlobalCommon.notificationsInColumnTray)
                        {
                            notififcationsNumberInTraysColumnNow = 0;
                            columnIndex += 1;
                        }
                    }
                }
                groupIndex += 1;
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
                notificationObject.GetComponentsInChildren<MeshRenderer>()[10].gameObject.transform.localScale = new Vector3(60, 0, 0);
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
