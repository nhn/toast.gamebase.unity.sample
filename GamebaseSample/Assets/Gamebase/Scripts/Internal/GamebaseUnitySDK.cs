namespace Toast.Gamebase.Internal
{
    public static class GamebaseUnitySDK
    {
        public const string SDK_VERSION = "2.69.0";

        public static bool IsInitialized { get; set; }
        public static string SDKVersion { get { return SDK_VERSION; } }
        public static string AppID { get; set; }
        public static string AppVersion { get; set; }
        public static string DisplayLanguageCode { get; set; }
        public static string ZoneType { get; set; }                
        public static bool EnablePopup { get; set; }
        public static bool EnableLaunchingStatusPopup { get; set; }
        public static bool EnableBanPopup { get; set; }
        public static string StoreCode { get; set; }
        public static string FcmSenderId { get; set; }
        public static bool UseWebViewLogin { get; set; }
    }
}