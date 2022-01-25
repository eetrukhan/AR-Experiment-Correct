using UnityEngine;

namespace Logic
{
    public class GlobalCommon : MonoBehaviour
    {
        public static int notificationsInColumnTray = 5;
        public static int notificationColumnsTray = 8;
        public static string silentGroupKey = "_silent_";
        public static float waitForActionToBeAcceptedPeriod = 2f;
        public static float pauseBetweenTrials = 5f;

        internal static string currentTypeName;
    }
}