namespace Toast.Logger
{
    public interface ILoggerSettings
    {
        string SettingName { get; }
    }

    public class DefaultSettings : ILoggerSettings
    {
        // Not implementation
        // Now just placeholder for mobile platform
        string ILoggerSettings.SettingName
        {
            get { return "Default"; }
        }
    }

    public class ConsoleSettings : ILoggerSettings
    {
        // Not implementation
        // Now just placeholder for mobile platform
        string ILoggerSettings.SettingName
        {
            get { return "Console"; }
        }
    }
}