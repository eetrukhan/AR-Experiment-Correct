using UnityEngine;

namespace Logic
{
    public class Notification
    {
        private string sourceImage;
        private string sourceName;
        private string author;
        private string icon;
        private string text;
        private long timestamp;
        private bool silent;
        private Color color;
        private string id;
        private bool correct;
        private bool markedAsRead; // also used for those notifications which was hiden after n seconds in the notifications hodler and no reaction

        public Notification(string id, string sourceImage, string sourceName, string author, string icon, string text, long timestamp, bool silent, Color color, bool correct) { 
            this.sourceImage = sourceImage;
            this.sourceName = sourceName;
            this.author = author;
            this.text = text;
            this.timestamp = timestamp;
            this.silent = silent;
            this.color = color;
            this.icon = icon;
            this.id = id;
            this.correct = correct;
            this.markedAsRead = false;
        }

        public override string ToString()
        {
            return string.Format("id: {0}; sourceImage: {1}; sourceName: {2}; author: {3}; icon: {4}; text: {5}; timestamp: {6}; silent: {7}; color: {8}; isCorrect: {9}",
                                                                              id, sourceImage, sourceName, author, icon, text, timestamp, silent, color, isCorrect);
        }

        public string ToString(string design, string status, string reactionTime)
        {
            return string.Format("{0}; {1}; {2}; {3}; {4}; {5}; {6}; {7}; {8}; {9}; {10}; {11}; {12}; {13}; {14}",
                                                                              ExperimentData.subjectNumber, design, ExperimentData.trialsNumber, id, sourceImage, sourceName, author, icon, text, timestamp, silent, color, isCorrect, status, reactionTime);
        }

        public string Id
        {
            get
            {
                return id;
            }
        }

        public string SourceImage
        {
            get
            {
                return sourceImage;
            }
        }

        public string Icon
        {
            get
            {
                return icon;
            }
        }

        public string SourceName
        {
            get
            {
                return sourceName;
            }
        }

        public string Author
        {
            get
            {
                return author;
            }
        }

        public Color Color
        {
            get
            {
                return color;
            }
        }

        public string Text
        {
            get
            {
                return text;
            }
        }

        public long Timestamp
        {
            get
            {
                return timestamp;
            }
        }

        public bool isSilent
        {
            get
            {
                return silent;
            }
        }

        public bool isCorrect
        {
            get
            {
                return correct;
            }
        }

        public bool isMarkedAsRead
        {
            set
            {
                markedAsRead = value;
            }
            get
            {
                return markedAsRead;
            }
        }
    }
}