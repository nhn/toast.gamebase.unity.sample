namespace Toast.Gamebase
{
    public class GamebaseCallback
    {
        public delegate void VoidDelegate();
        public delegate void ErrorDelegate(GamebaseError error);
        public delegate void DataDelegate<T>(T data);
        public delegate void GamebaseDelegate<T>(T data, GamebaseError error);

        public class Logger
        {
            public interface ILoggerListener
            {
                void OnSuccess(GamebaseResponse.Logger.LogEntry log);
                void OnFilter(GamebaseResponse.Logger.LogEntry log, GamebaseResponse.Logger.LogFilter filter);
                void OnSave(GamebaseResponse.Logger.LogEntry log);
                void OnError(GamebaseResponse.Logger.LogEntry log, string errorMessage);
            }

            public delegate void CrashListener(bool isSuccess, GamebaseResponse.Logger.LogEntry logEntry);

            public delegate bool CrashFilter(GamebaseResponse.Logger.CrashLogData logData);
        }
    }
}