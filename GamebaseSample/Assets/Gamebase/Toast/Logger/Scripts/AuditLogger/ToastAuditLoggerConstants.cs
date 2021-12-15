namespace Toast.Logger
{
    public static class ToastAuditLoggerContants
    {
        public const string AUDIT_APP_ID = "appID";
        public const string AUDIT_APP_NAME = "appName";
        public const string AUDIT_SDK_FUNCTION = "sdkFunction";
        public const string AUDIT_APP_VERSION = "appVersion";
        public const string AUDIT_USER_ID = "userID";
        public const string AUDIT_OS = "os";
        public const string AUDIT_OS_VERSION = "osVersion";
        //public const string AUDIT_DEVICE_ID = "deviceID";
        public const string AUDIT_SETUP_ID = "setupID";
        public const string AUDIT_LAUNCHED_ID = "launchedID";
        public const string AUDIT_COUNTRY_CODE = "countryCode";
        public const string AUDIT_SESSION_ID = "sessionID";

#if UNITY_STANDALONE_OSX
        public const string AUDIT_VALUE_OS = "Unity-OSX";
#elif UNITY_STANDALONE_WIN
        public const string AUDIT_VALUE_OS = "Unity-Windows";
#elif UNITY_STANDALONE_LINUX
        public const string AUDIT_VALUE_OS = "Unity-Linux";
#elif UNITY_WEBGL
        public const string AUDIT_VALUE_OS = "Unity-WebGL";
#else
        public const string AUDIT_VALUE_OS = "";
#endif

        public const string AUDIT_VALUE_LOGSOURCE_DEV = "dev";
        public const string AUDIT_VALUE_LOGSOURCE_REAL = "real";

        public const string AUDIT_VALUE_API_VERSION = "v2";
        public const string AUDIT_VALUE_SDK_FUNCTION = "l&c";

        public const string USAGE_AUDIT_LOG_TYPE = "sdk-audit";
        public const string USAGE_AUDIT_LOG_MESSAGE = "TOAST SDK usage log.";
    }
}