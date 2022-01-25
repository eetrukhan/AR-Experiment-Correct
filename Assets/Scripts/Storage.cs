using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Logic
{
    public class Storage : MonoBehaviour
    {
        private Dictionary<string, NotificationsStorage> orderedNotifications = new Dictionary<string, NotificationsStorage>();

        public Dictionary<string, NotificationsStorage> getStorage()
        {
            return orderedNotifications;
        }

        internal void addToStorage(Notification notification)
        {
            Stack<Notification> sourceNotifications = new Stack<Notification>();
            string sourceName = notification.SourceName;
            if (notification.isSilent) sourceName = GlobalCommon.silentGroupKey;
            if (orderedNotifications.ContainsKey(sourceName)) sourceNotifications = orderedNotifications[sourceName].Storage;
            sourceNotifications.Push(notification);
            NotificationsStorage newNotificationsStorage = new NotificationsStorage(sourceNotifications, notification.Timestamp);
            orderedNotifications[sourceName] = newNotificationsStorage;
            createOrderedStorage(sourceName);
        }

        public void removeFromStorage(string id, string sourceName, string tag)
        {
            NotificationsStorage newStorage = null;
            try
            {
                newStorage = orderedNotifications[sourceName];
            }
            catch (KeyNotFoundException e)
            {
                Debug.LogError(e);
            }
            Stack<Notification> newNotificationsStorage = new Stack<Notification>();
            foreach (Notification notification in newStorage.Storage)
            {
                if (!notification.Id.Equals(id))
                {
                    newNotificationsStorage.Push(notification);
                }
                else
                {
                    if (tag.Contains("MarkAsRead"))
                    {
                        notification.isMarkedAsRead = true;
                        newNotificationsStorage.Push(notification);
                    }
                }
            }
            newStorage.Storage = newNotificationsStorage;
            orderedNotifications[sourceName] = newStorage;
            createOrderedStorage(sourceName);
        }

        public Notification getFromStorage(string id, string sourceName)
        {
            if (orderedNotifications.ContainsKey(sourceName))
            {
                foreach (Notification notification in orderedNotifications[sourceName].Storage)
                {
                    if (notification.Id.Equals(id))
                    {
                        return notification;
                    }
                }
            }
            if (orderedNotifications.ContainsKey(GlobalCommon.silentGroupKey))
            {
                foreach (Notification notification in orderedNotifications[GlobalCommon.silentGroupKey].Storage)
                {
                    if (notification.Id.Equals(id))
                    {
                        return notification;
                    }
                }
            }
            return null;
        }

        public void removeAllFromStorage(string sourceName, string tag)
        {
            if (!tag.Contains("MarkAsRead"))
            {
                orderedNotifications.Remove(sourceName);
                sourceName = null;
                createOrderedStorage(sourceName);
            }
            else
            {
                NotificationsStorage newStorage = orderedNotifications[sourceName];
                Stack<Notification> newNotificationsStorage = new Stack<Notification>();
                foreach (Notification notification in newStorage.Storage)
                {
                    notification.isMarkedAsRead = true;
                    newNotificationsStorage.Push(notification);
                }
                newStorage.Storage = newNotificationsStorage;
                orderedNotifications[sourceName] = newStorage;
                createOrderedStorage(sourceName);
            }
        }

        public void removeAllFromStorage()
        {
            orderedNotifications = new Dictionary<string, NotificationsStorage>();
        }

        private void createOrderedStorage(string sourceName)
        {
            NotificationsStorage silentGroup = null;
            if (orderedNotifications.ContainsKey(GlobalCommon.silentGroupKey))
            {
                silentGroup = orderedNotifications[GlobalCommon.silentGroupKey];
                orderedNotifications.Remove(GlobalCommon.silentGroupKey);
            }
            orderedNotifications = orderedNotifications.OrderByDescending(x => x.Value.LatestTimestamp)
                                                                                         .ToDictionary(d => d.Key, d => d.Value);
            if (silentGroup != null || sourceName == GlobalCommon.silentGroupKey)
            {
                orderedNotifications.Add(GlobalCommon.silentGroupKey, silentGroup); // silent are always the last
            }
        }
    }
}
