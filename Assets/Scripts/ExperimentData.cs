using UnityEngine;

namespace Logic
{
    public class ExperimentData : MonoBehaviour
    {
        public static int subjectNumber;
        public static int trialsNumber;
        public static int timeInSeconds;
        public static int notificationsNumber;
        public static string notificationSource;
        public static string notificationAuthor;
        public static int numberOfHaveToActNotifications;

        //internal static int numberOfNonIgnoredHaveToActNotifications;
        //internal static float sumOfReactionTimeToNonIgnoredHaveToActNotifications;
        
        
        public static float SumOfReactionTimeOnDesiredNotifications;

        public static float SumOfReactionTimeOnUnnecessaryNotifications;

        public static int NumberOfCorrectReactedDesiredNotifications;

        public static int NumberOfCorrectReactedUnnecessaryNotifications;

        public static int NumberOfMissedDesiredNotifications = 2;

        public static int NumberOfMissedUnnecessaryNotifications = 2;

        

        /// <summary>
        /// NEW for time spent on making decision
        /// </summary>
        //internal static float sumOfAllReactionTime;

        /// <summary>
        /// NEW number of non-needed notifications on which user reacted correstly
        /// </summary>
        //internal static int numberOfCorrectReactedNaveToHideNotifications;
        
        //internal static int numberOfInCorrectlyActedNotifications;
        
        
        
    }
}