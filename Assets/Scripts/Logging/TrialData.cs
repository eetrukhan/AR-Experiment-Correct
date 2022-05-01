using System;
using System.Collections.Generic;

namespace Logic
{
    [Serializable]
    public class TrialData
    {
        public int SubjectNumber;
        public string Design;
        public int TrialNumber;
        public float Time; // In seconds
        public int NotificationsNumber; 
        public int NumberOfHaveToActNotifications;

        /// 

        public float SumOfReactionTimeOnDesiredNotifications;

        public float SumOfReactionTimeOnUnnecessaryNotifications;

        public int NumberOfCorrectReactedDesiredNotifications;

        public int NumberOfCorrectReactedUnnecessaryNotifications;

        public int NumberOfMissedDesiredNotifications;

        public int NumberOfMissedUnnecessaryNotifications;




       public int NumberOfNonIgnoredHaveToActNotifications;
        public float SumOfReactionTimeToNonIgnoredHaveToActNotifications;
        public int NumberOfInCorrectlyActedNotifications;
        public float sumOfAllReactionTime;
        public int numberOfCorrectReactedNaveToHideNotifications;
        // public int 

        // Instructions were taken from here: https://youtu.be/z9b5aRfrz7M
       // private static readonly string _formURI = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSeMgJ_QKYVj_roNo8w9kyQv465foOt-ePB5z_4rV-srn6TxwA/formResponse";

       private static readonly string _formURI =
           "https://docs.google.com/forms/d/e/1FAIpQLSfpncztPD3x4MhGmUgx-Mb2VgvAu11EC7_LtuXDcaCuUAnn3w/formResponse";
        public static string GetFormURI()
        {
            return _formURI;
        }

        public Dictionary<string, string> GetFormFields()
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>();
            //SubjectNumber
            formFields.Add("entry.1891286050", SubjectNumber.ToString()); //entry.1891286050
            //Design
            formFields.Add("entry.1114929033", Design.ToString()); //entry.1114929033
            //TrialNumber
            formFields.Add("entry.757602727", TrialNumber.ToString()); //entry.757602727
            //Time
            formFields.Add("entry.71396515", Time.ToString()); //entry.71396515
            //NotificationsNumber
            formFields.Add("entry.1870216573", NotificationsNumber.ToString()); //entry.1870216573
            //NumberOfDesiredNotifications
            formFields.Add("entry.1850179261", NumberOfHaveToActNotifications.ToString()); //entry.1850179261
            
            ////
            
            //SumOfReactionTimeOnDesiredNotifications
            formFields.Add("entry.1255739382", (SumOfReactionTimeOnDesiredNotifications/TimeSpan.TicksPerSecond).ToString()); //entry.1082228265
            //SumOfReactionTimeOnUnnecessaryNotifications
            formFields.Add("entry.1374584132", (SumOfReactionTimeOnUnnecessaryNotifications/TimeSpan.TicksPerSecond).ToString()); //entry.1255739382
            //NumberOfCorrectReactedDesiredNotifications
            formFields.Add("entry.1082228265", NumberOfCorrectReactedDesiredNotifications.ToString()); //entry.102635401
           //NumberOfCorrectReactedUnnecessaryNotifications
            formFields.Add("entry.102635401",NumberOfCorrectReactedUnnecessaryNotifications.ToString()); //entry.1374584132
            //NumberOfMissedDesiredNotifications
            formFields.Add("entry.1311894071",NumberOfMissedDesiredNotifications.ToString()); //entry.1311894071
            // NumberOfMissedUnnecessaryNotifications
            formFields.Add( "entry.1146943836", NumberOfMissedUnnecessaryNotifications.ToString() ); 
            return formFields;
        }
        /*
        public int SubjectNumber;
        public string Design;
        public int TrialNumber;
        public float Time; // In seconds
        public int NotificationsNumber; 
        public int NumberOfHaveToActNotifications;
        public int NumberOfNonIgnoredHaveToActNotifications;
        public float SumOfReactionTimeToNonIgnoredHaveToActNotifications;
        public int NumberOfInCorrectlyActedNotifications;
        public float sumOfAllReactionTime;
        public int numberOfCorrectReactedNaveToHideNotifications;

        // Instructions were taken from here: https://youtu.be/z9b5aRfrz7M
       // private static readonly string _formURI = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSeMgJ_QKYVj_roNo8w9kyQv465foOt-ePB5z_4rV-srn6TxwA/formResponse";

       private static readonly string _formURI =
           "https://docs.google.com/forms/u/0/d/e/1FAIpQLSeEZjbhSpk9TRz1S94eTx-svFbkYFFxpaGXigRU6e02lL9jXg/formResponse";
        public static string GetFormURI()
        {
            return _formURI;
        }

        public Dictionary<string, string> GetFormFields()
        {
            Dictionary<string, string> formFields = new Dictionary<string, string>();
            formFields.Add("entry.1891286050", SubjectNumber.ToString()); //entry.2005620554
            formFields.Add("entry.1114929033", Design.ToString()); //entry.1630268306
            formFields.Add("entry.757602727", TrialNumber.ToString()); //entry.1276049454
            formFields.Add("entry.71396515", Time.ToString()); //entry.388397209
            formFields.Add("entry.1870216573", NotificationsNumber.ToString()); //entry.1289494810
            formFields.Add("entry.1850179261", NumberOfHaveToActNotifications.ToString()); //entry.123982960
            formFields.Add("entry.1082228265", NumberOfNonIgnoredHaveToActNotifications.ToString()); //entry.1215856783
            formFields.Add("entry.1255739382", (SumOfReactionTimeToNonIgnoredHaveToActNotifications/TimeSpan.TicksPerSecond).ToString()); //entry.1734028322
            formFields.Add("entry.102635401", NumberOfInCorrectlyActedNotifications.ToString()); //entry.588295555
            formFields.Add("entry.1374584132",(sumOfAllReactionTime/TimeSpan.TicksPerSecond).ToString());
            formFields.Add("entry.1311894071",numberOfCorrectReactedNaveToHideNotifications.ToString());
            return formFields;
        }l
        */
    }
}