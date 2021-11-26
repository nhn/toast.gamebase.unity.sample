namespace Toast.Logger
{
    public class ToastLoggerNormalFilter : IToastLoggerFilter
    {
        public bool Filter(ToastLoggerLogObject logData)
        {
            if (ToastLoggerSettings.Instance.isNormal)
            {
                return true;
            }
            else
            {
                if (logData.GetLoggerType().Equals(ToastLoggerType.NORMAL))
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
}