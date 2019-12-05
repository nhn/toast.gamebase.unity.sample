using System;

namespace Toast.Logger
{
    public enum ToastServiceZone
    {
        ALPHA = 0,
        BETA = 1,
        REAL = 2
    }

    //string projectKey, ServiceZone serviceZone = ServiceZone.Real, bool EnableCrashReporter = true
    public class ToastLoggerConfiguration
    {
        public ToastLoggerConfiguration()
        {
            ServiceZone = ToastServiceZone.REAL;
            EnableCrashReporter = true;
            EnableCrashErrorLog = false;
        }

        [Obsolete("Please use AppKey instead of ProjectKey")]
        public string ProjectKey { get; set; }

        public string AppKey
        {
#pragma warning disable 618
            get { return ProjectKey; }
            set { ProjectKey = value; }
#pragma warning restore 618
        }

        public bool EnableCrashReporter { get; set; }
        public bool EnableCrashErrorLog { get; set; }
        public ToastServiceZone ServiceZone { get; set; }
    }
}