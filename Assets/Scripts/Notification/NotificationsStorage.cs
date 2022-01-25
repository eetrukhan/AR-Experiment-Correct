using System.Collections.Generic;

namespace Logic
{
    public class NotificationsStorage
    {
        public NotificationsStorage(Stack<Notification> notificationsStorage, long latestTimestamp)
        {
            Storage = notificationsStorage;
            LatestTimestamp = latestTimestamp;
        }

        public Stack<Notification> Storage
        {
            get; set;
        }

        public long LatestTimestamp
        {
            get;
        }
    }
}
