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
            formFields.Add("entry.1255739382", SumOfReactionTimeToNonIgnoredHaveToActNotifications.ToString()); //entry.1734028322
            formFields.Add("entry.102635401", NumberOfInCorrectlyActedNotifications.ToString()); //entry.588295555
            formFields.Add("entry.1374584132",sumOfAllReactionTime.ToString());
            formFields.Add("entry.1311894071",numberOfCorrectReactedNaveToHideNotifications.ToString());
            return formFields;
        }
    }
}