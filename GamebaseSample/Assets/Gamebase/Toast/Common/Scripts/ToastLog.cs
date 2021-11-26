using System;

namespace Toast
{
    public static class ToastLog
    {
        public enum LogLevel
        {
            Developer,
            Debug,
            Info,
            Warn,
            Error
        }

        public static LogLevel Level { get; set; }

        public static event Action<LogLevel, string> OnReceiveLog;

        static ToastLog()
        {
            Level = LogLevel.Warn;
        }

        private static void Log(LogLevel level, string message)
        {
            if ((int)Level <= (int)level)
            {
                switch (level)
                {
                    case LogLevel.Developer:
                    case LogLevel.Debug:
                    case LogLevel.Info:
                        UnityEngine.Debug.Log(message);
                        break;
                    case LogLevel.Warn:
                        UnityEngine.Debug.LogWarning(message);
                        break;
                    case LogLevel.Error:
                        UnityEngine.Debug.LogError(message);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("level", level, null);
                }

                if (OnReceiveLog != null)
                {
                    OnReceiveLog(level, message);
                }
            }
        }

        private static void LogException(Exception exception)
        {
            UnityEngine.Debug.LogException(exception);
        }

        internal static void Developer(string message)
        {
            Log(LogLevel.Developer, message);
        }

        internal static void Developer(string fmt, params object[] args)
        {
            Log(LogLevel.Developer, string.Format(fmt, args));
        }

        public static void Debug(string message)
        {
            Log(LogLevel.Debug, message);
        }

        public static void Debug(string fmt, params object[] args)
        {
            Log(LogLevel.Debug, string.Format(fmt, args));
        }

        public static void Info(string message)
        {
            Log(LogLevel.Info, message);
        }

        public static void Info(string fmt, params object[] args)
        {
            Log(LogLevel.Info, string.Format(fmt, args));
        }

        public static void Warn(string message)
        {
            Log(LogLevel.Warn, message);
        }

        public static void Warn(string fmt, params object[] args)
        {
            Log(LogLevel.Warn, string.Format(fmt, args));
        }

        public static void Error(string message)
        {
            Log(LogLevel.Error, message);
        }

        public static void Error(string fmt, params object[] args)
        {
            Log(LogLevel.Error, string.Format(fmt, args));
        }

        public static void Exception(Exception exception)
        {
            LogException(exception);
        }
    }
}