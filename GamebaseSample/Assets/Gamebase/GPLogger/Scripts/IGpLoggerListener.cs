namespace GamePlatform.Logger
{
    public interface IGpLoggerListener
    {
        void OnSuccess(LogEntry log);
        void OnFilter(LogEntry log, LogFilter filter);
        void OnSave(LogEntry log);
        void OnError(LogEntry log, string errorMessage);
    }
}