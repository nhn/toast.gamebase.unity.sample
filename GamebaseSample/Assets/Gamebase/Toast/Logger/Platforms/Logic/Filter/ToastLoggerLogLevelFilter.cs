namespace Toast.Logger
{
    public class ToastLoggerLogLevelFilter : IToastLoggerFilter
    {
        public bool Filter(ToastLoggerLogObject logData)
        {
            if (ToastLoggerSettings.Instance.isLogLevelFilter == false)
            {
                return true;
            }
            else
            {
                return (logData.GetLogLevel() >= ToastLoggerSettings.Instance.filterLogLevel);
            }
        }
    }
}

