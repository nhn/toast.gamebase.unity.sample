namespace Toast.Gamebase
{
    public class GamebaseLoggerConst
    {
        public enum ServiceZone
        {
            ALPHA = 0,
            REAL = 1
        }

        public enum LogLevel
        {
            DEBUG = 0,
            INFO = 1,
            WARN = 2,
            ERROR = 3,
            FATAL = 4,
            NONE = 5
        }

        public enum LogType
        {
            ERROR = 0,
            ASSERT = 1,
            WARNING = 2,
            LOG = 3,
            EXCEPTION = 4
        }
    }
}
