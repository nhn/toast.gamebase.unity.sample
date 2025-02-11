namespace GamePlatform.Logger.Internal
{
    public class GpLoggerSessionFilter : IGpLoggerFilter
    {
        private readonly bool isEnabled;

        public GpLoggerSessionFilter(bool isEnabled)
        {
            this.isEnabled = isEnabled;
        }

        public bool IsLoggable(BaseLogItem item)
        {
            if (isEnabled == true)
            {
                return true;
            }

            if (item.GetLoggerType().Equals(GpLoggerType.SESSION) == true)
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