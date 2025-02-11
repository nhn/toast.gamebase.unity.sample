namespace GamePlatform.Logger.Internal
{
    public class GpLoggerCrashFilter : IGpLoggerFilter
    {
        private readonly bool isEnabled;

        public GpLoggerCrashFilter(bool isEnabled)
        {
            this.isEnabled = isEnabled;
        }

        public bool IsLoggable(BaseLogItem item)
        {
            if (isEnabled == true)
            {
                return true;
            }

            if (item.GetLoggerType().Equals(GpLoggerType.CRASH) == true)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
