namespace Toast.Logger
{
    public interface IToastLoggerListener
    {
        void OnSuccess(LogEntry log);
        void OnFilter(LogEntry log, LogFilter filter);
        void OnSave(LogEntry log);
        void OnError(LogEntry log, string errorMessage);
    }
}