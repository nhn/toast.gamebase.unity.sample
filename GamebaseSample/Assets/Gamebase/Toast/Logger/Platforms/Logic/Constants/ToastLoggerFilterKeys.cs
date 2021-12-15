
namespace Toast.Logger
{
    public static class ToastLoggerFilterKeys
    {
        public static string LOG_TYPE_FILTER = typeof(ToastLoggerLogTypeFilter).FullName;
        public static string LOG_LEVEL_FILTER = typeof(ToastLoggerLogLevelFilter).FullName;
        public static string LOG_DUPLICATE_FILTER = typeof(ToastLoggerDuplicateFilter).FullName;
        public static string SESSION_LOG_FILTER = typeof(ToastLoggerSessionFilter).FullName;
        public static string NORMAL_LOG_FILTER = typeof(ToastLoggerNormalFilter).FullName;
        public static string CRASH_LOG_FILTER = typeof(ToastLoggerCrashFilter).FullName;
    }
}