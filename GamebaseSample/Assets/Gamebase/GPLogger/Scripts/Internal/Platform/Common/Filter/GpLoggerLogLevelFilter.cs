using System;

namespace GamePlatform.Logger.Internal
{
    public class GpLoggerLogLevelFilter : IGpLoggerFilter
    {
        private readonly GpLoggerResponse.LogNCrashSettings.Result.Filter.LogLevelFilter filter;

        public GpLoggerLogLevelFilter(GpLoggerResponse.LogNCrashSettings.Result.Filter.LogLevelFilter filter)
        {
            this.filter = filter;
        }

        public bool IsLoggable(BaseLogItem item)
        {
            if (filter.enable == false)
            {
                return true;
            }

            var logLevel = (GpLogLevel)Enum.Parse(typeof(GpLogLevel), filter.logLevel);
            return (item.GetLogLevel() >= logLevel);
        }
    }
}