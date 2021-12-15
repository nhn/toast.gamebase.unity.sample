namespace Toast.Logger
{
    public class ToastLoggerLogTypeFilter : IToastLoggerFilter
    {
        public bool Filter(ToastLoggerLogObject logData)
        {
            if (ToastLoggerSettings.Instance.isLogTypeFilter == false)
            {
                return true;
            }
            else
            {
                foreach (string logType in ToastLoggerSettings.Instance.filterLogTypes)
                {
                    string strLogType = logData.GetLoggerType();
                    if (strLogType.Equals(logType))
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}

