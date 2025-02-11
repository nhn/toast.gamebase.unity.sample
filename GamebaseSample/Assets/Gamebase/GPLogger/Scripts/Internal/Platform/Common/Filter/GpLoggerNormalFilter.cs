namespace GamePlatform.Logger.Internal
{
    public class GpLoggerNormalFilter : IGpLoggerFilter
    {
        private readonly bool isEnabled;

        public GpLoggerNormalFilter(bool isEnabled)
        {
            this.isEnabled = isEnabled;
        }

        public bool IsLoggable(BaseLogItem item)
        {
            if (isEnabled == true)
            {
                return true;
            }

            if (item.GetLoggerType().Equals(GpLoggerType.NORMAL) == true)
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