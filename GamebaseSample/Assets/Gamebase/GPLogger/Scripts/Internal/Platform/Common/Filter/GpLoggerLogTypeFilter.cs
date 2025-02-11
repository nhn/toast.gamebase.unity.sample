namespace GamePlatform.Logger.Internal
{
    public class GpLoggerLogTypeFilter : IGpLoggerFilter
    {
        private readonly GpLoggerResponse.LogNCrashSettings.Result.Filter.LogTypeFilter filter;

        public GpLoggerLogTypeFilter(GpLoggerResponse.LogNCrashSettings.Result.Filter.LogTypeFilter filter)
        {
            this.filter = filter;
        }

        public bool IsLoggable(BaseLogItem item)
        {
            if (filter.enable == false)
            {
                return true;
            }

            foreach (string logType in filter.logType)
            {
                if (item.GetLoggerType().Equals(logType) == true)
                {
                    return false;
                }
            }

            return true;
        }
    }
}

