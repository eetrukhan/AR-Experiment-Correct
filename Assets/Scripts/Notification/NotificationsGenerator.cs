using System;
using System.Globalization;
using UnityEngine;

namespace Logic
{
    public class NotificationsGenerator
    {
        private System.Random random = new System.Random();
        private const int sourcesNumber = 1; //4

        private int mapNameToIndex()
        {
            string source = ExperimentData.notificationSource;
            int index = 0;
            var eType = typeof(NotificationSource);
            foreach (NotificationSource notificationSource in Enum.GetValues(eType))
            {
                var name = EnumDescription.getDescription(notificationSource);
                if (name.Equals(source))
                {
                    return index;
                }
                index += 1;
            }
            return index;
        }

        private int mapAuthorToIndex()
        {
            string author = ExperimentData.notificationAuthor;
            int index = 0;
            var eType = typeof(NotificationAuthor);
            foreach (NotificationAuthor notificationAuthor in Enum.GetValues(eType))
            {
                var name = EnumDescription.getDescription(notificationAuthor);
                if (name.Equals(author))
                {
                    return index;
                }
                index += 1;
            }
            return index;
        }    

        public Notification getNotification(bool generateHaveToAct)
        {
            //int sourceIndex = random.Next(0, sourcesNumber);
            int sourceIndex = 0;
            //bool isSilent = random.Next(0, 2) == 0;
            bool isSilent = false;
           // if (generateHaveToAct)
            {
               // sourceIndex = mapNameToIndex();
                isSilent = false;
            }
            string id = Guid.NewGuid().ToString();
            Debug.Log(sourceIndex);
            NotificationSource notificationSource = (NotificationSource)Enum.GetValues(typeof(NotificationSource)).GetValue(sourceIndex);
            string sourceName = EnumDescription.getDescription(notificationSource);
            NotificationImage notificationImage = (NotificationImage)Enum.GetValues(typeof(NotificationImage)).GetValue(sourceIndex);
            string sourceImage = EnumDescription.getDescription(notificationImage);
			NotificationColor notificationColor = (NotificationColor)Enum.GetValues(typeof(NotificationColor)).GetValue(sourceIndex);
            Color sourceColor = EnumDescription.getColor(EnumDescription.getDescription(notificationColor));
            Array values = Enum.GetValues(typeof(NotificationAuthor));
            int authorIndex = random.Next(values.Length);
            //bool generateHaveToAct = authorIndex == correctAuthorIndex;
          /*  if (generateHaveToAct)
            {
                authorIndex = mapAuthorToIndex();
            }*/
            Debug.Log(authorIndex);
            string author;
            NotificationAuthor notificationAuthor;
            if(generateHaveToAct)
            {
                authorIndex = GeneratorRunner.correctAuthorIndex;
                notificationAuthor =
                (NotificationAuthor) values.GetValue(GeneratorRunner.correctAuthorIndex);
                author = EnumDescription.getDescription(notificationAuthor);
            }
            else
            {
                int notificationAuthorIndex = random.Next(values.Length);
                while(notificationAuthorIndex==GeneratorRunner.correctAuthorIndex)
                    notificationAuthorIndex = random.Next(values.Length);
                notificationAuthor =
                    (NotificationAuthor) values.GetValue(notificationAuthorIndex);
                author = EnumDescription.getDescription(notificationAuthor);
                authorIndex = notificationAuthorIndex;

            }
            values = Enum.GetValues(typeof(NotificationIcon));
            NotificationIcon notificationIcon = (NotificationIcon)values.GetValue(authorIndex);
            string icon = EnumDescription.getDescription(notificationIcon);
            string text;
            if(sourceIndex == 2 || sourceIndex == 3) // post or youtube
            {
                values = Enum.GetValues(typeof(NotificationHeader));
                NotificationHeader notificationHeader = (NotificationHeader)values.GetValue(random.Next(values.Length));
                text = EnumDescription.getDescription(notificationHeader);
            }
            else // messengers
            {
                values = Enum.GetValues(typeof(NotificationText));
                NotificationText notificationText = (NotificationText)values.GetValue(random.Next(values.Length));
                text = EnumDescription.getDescription(notificationText);
            }
           /* if (isSilent)
            {
                sourceColor = EnumDescription.getColor(EnumDescription.getDescription(NotificationColor.Silent));
                sourceImage = "_silent_";
            }*/
            DateTime date =DateTime.Now;
            long timestamp = date.Ticks;
            Notification notification = new Notification(id, sourceImage, sourceName, author, icon, text, timestamp, isSilent, sourceColor, generateHaveToAct, date);
            string timestamp1 = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff",
                CultureInfo.InvariantCulture);
            Debug.Log(timestamp1 + " : "+ string.Format("Notification which is {0} and has the following data: {1} was created", generateHaveToAct ? "correct" : "incorrect", notification));
            return notification;
        }
    }
}