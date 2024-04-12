namespace GamePlatform.Logger
{
    public static class GpLoggerParams
    {
        public class Initialization
        {
            /// <summary>
            /// Appkey issued by Log&amp;Crash.
            /// </summary>
            public readonly string appKey;

            /// <summary>
            /// Whether to enable crash logs.
            /// Crash logs are automatically sent by SDK when enableCrashReporter is true.
            /// <para/>Default: true
            /// </summary>
            public bool enableCrashReporter = true;

            /// <summary>
            /// Error log is excluded by default. Use it if you want to collect error logs.
            /// <para/>Default: false
            /// </summary>
            public bool enableCrashErrorLog = false;

            /// <summary>
            /// NHN Cloud service zone
            /// <para/>Default: REAL
            /// </summary>
            public ServiceZone serviceZone = ServiceZone.REAL;

            public Initialization(string appKey)
            {
                this.appKey = appKey;
            }

        }
    }
}