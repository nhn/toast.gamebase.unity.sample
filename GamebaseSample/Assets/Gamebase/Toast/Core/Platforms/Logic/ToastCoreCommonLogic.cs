using System.Collections.Generic;

namespace Toast.Core
{
    public static class ToastCoreCommonLogic
    {
        public static bool DebugMode { get; set; }
        public static string UserId { get; set; }
        public static List<string> OptionalPolicesProperties { get; set; }
        public static ServiceZone Zone { get; set; }

        public static void Initialize()
        {

        }

        public static void SetOptionalPolices(List<string> properties)
        {
            OptionalPolicesProperties = properties;

            RequestLaunchingInfo();
        }

        private static void RequestLaunchingInfo()
        {

        }
    }
}
