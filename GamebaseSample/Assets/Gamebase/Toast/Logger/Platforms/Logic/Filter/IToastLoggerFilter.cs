namespace Toast.Logger
{
    public interface IToastLoggerFilter
    {
        bool Filter(ToastLoggerLogObject logData);
    }
}