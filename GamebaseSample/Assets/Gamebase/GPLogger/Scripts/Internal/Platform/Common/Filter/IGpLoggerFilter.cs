namespace GamePlatform.Logger.Internal
{
    public interface IGpLoggerFilter
    {
        bool IsLoggable(BaseLogItem logData);
    }
}