using System.Collections.Generic;
using Toast.Logger;

namespace Toast.Gamebase.Internal
{ 
    public class UnityLoggerControlle
    {
        private static readonly UnityLoggerControlle instance = new UnityLoggerControlle();

        public static UnityLoggerControlle Instance
        {
            get { return instance; }
        }

        private Dictionary<GamebaseCallback.Logger.CrashFilter, ToastLogger.CrashFilter> crashFilterDictionary = new Dictionary<GamebaseCallback.Logger.CrashFilter, ToastLogger.CrashFilter>();

        public void Initialize(GamebaseRequest.Logger.Configuration loggerConfiguration)
        {
            GamebaseLog.Debug("Initialize", this);

            ToastLoggerConfiguration configuration = new ToastLoggerConfiguration();
            configuration.AppKey = loggerConfiguration.appKey;
            configuration.EnableCrashErrorLog = loggerConfiguration.enableCrashErrorLog;
            configuration.EnableCrashReporter = loggerConfiguration.enableCrashReporter;

            switch (loggerConfiguration.serviceZone.ToLower())
            {
                case "alpha":
                    {
                        configuration.ServiceZone = ToastServiceZone.ALPHA;
                        break;
                    }
                case "beta":
                    {
                        configuration.ServiceZone = ToastServiceZone.BETA;
                        break;
                    }
                case "real":
                default:
                    {
                        configuration.ServiceZone = ToastServiceZone.REAL;
                        break;
                    }
            }

            GamebaseLog.Debug(GamebaseJsonUtil.ToPrettyJsonString(configuration), typeof(UnityLoggerControlle));

            ToastLogger.Initialize(configuration);
        }

        public void Debug(string message, Dictionary<string, string> userFields = null)
        {
            GamebaseLog.Debug("Debug", this);
            ToastLogger.Debug(message, userFields);
        }

        public void Info(string message, Dictionary<string, string> userFields = null)
        {
            GamebaseLog.Debug("Info", this);
            ToastLogger.Info(message, userFields);
        }

        public void Warn(string message, Dictionary<string, string> userFields = null)
        {
            GamebaseLog.Debug("Warn", this);
            ToastLogger.Warn(message, userFields);
        }

        public void Error(string message, Dictionary<string, string> userFields = null)
        {
            GamebaseLog.Debug("Error", this);
            ToastLogger.Error(message, userFields);
        }

        public void Fatal(string message, Dictionary<string, string> userFields = null)
        {
            GamebaseLog.Debug("Fatal", this);
            ToastLogger.Fatal(message, userFields);
        }

        public void SetUserField(string key, string value)
        {
            GamebaseLog.Debug("SetUserField", this);
            ToastLogger.SetUserField(key, value);
        }

        public void SetLoggerListener(GamebaseCallback.Logger.ILoggerListener listener)
        {
            GamebaseLog.Debug("SetLoggerListener", this);
            ToastLogger.SetLoggerListener(new GamebaseLoggerListener(listener));
        }

        public void SetCrashListener(GamebaseCallback.Logger.CrashListener listener)
        {
            GamebaseLog.Debug("SetCrashListener", this);
            ToastLogger.SetCrashListener(
                (isSuccess, logEntry) =>
                {
                    GamebaseLog.Debug("OnCrashListener", this);
                    listener(isSuccess, ConvertGamebaseLogEntry(logEntry));
                });
        }

        public void AddCrashFilter(GamebaseCallback.Logger.CrashFilter filter)
        {
            GamebaseLog.Debug("AddCrashFilter", this);
            ToastLogger.CrashFilter crashFilter =
                (logData) =>
                {
                    GamebaseLog.Debug("OnCrashFilter", this);
                    GamebaseResponse.Logger.CrashLogData crashLogData = new GamebaseResponse.Logger.CrashLogData
                    {
                        logType = logData.LogType,
                        condition = logData.Condition,
                        stackTrace = logData.StackTrace
                    };

                    return filter(crashLogData);
                };
                
            ToastLogger.AddCrashFilter(crashFilter);

            crashFilterDictionary.Add(filter, crashFilter);
        }

        public void RemoveCrashFilter(GamebaseCallback.Logger.CrashFilter filter)
        {
            GamebaseLog.Debug("RemoveCrashFilter", this);
            ToastLogger.CrashFilter crashFilter = crashFilterDictionary[filter];

            ToastLogger.RemoveCrashFilter(crashFilter);

            crashFilterDictionary.Remove(filter);
        }

        private class GamebaseLoggerListener : IToastLoggerListener
        {
            private GamebaseCallback.Logger.ILoggerListener listene;

            public GamebaseLoggerListener(GamebaseCallback.Logger.ILoggerListener listene)
            {
                this.listene = listene;
            }

            public void OnError(LogEntry log, string errorMessage)
            {
                GamebaseLog.Debug("OnError", this);
                listene.OnError(ConvertGamebaseLogEntry(log), errorMessage);
            }

            public void OnFilter(LogEntry log, LogFilter filter)
            {
                GamebaseLog.Debug("OnFilter", this);
                GamebaseResponse.Logger.LogFilter logFilter = new GamebaseResponse.Logger.LogFilter();
                logFilter.name = filter.Name;

                listene.OnFilter(ConvertGamebaseLogEntry(log), logFilter);
            }

            public void OnSave(LogEntry log)
            {
                GamebaseLog.Debug("OnSave", this);
                listene.OnSave(ConvertGamebaseLogEntry(log));
            }

            public void OnSuccess(LogEntry log)
            {
                GamebaseLog.Debug("OnSuccess", this);
                listene.OnSuccess(ConvertGamebaseLogEntry(log));
            }
        }

        private static GamebaseResponse.Logger.LogEntry ConvertGamebaseLogEntry(LogEntry log)
        {
            GamebaseResponse.Logger.LogEntry logEntry = new GamebaseResponse.Logger.LogEntry();
            logEntry.logType = log.LogType;
            logEntry.createTime = log.CreateTime;
            logEntry.logLevel = (GamebaseLoggerConst.LogLevel)log.LogLevel;
            logEntry.message = log.Message;
            logEntry.transactionId = log.TransactionId;
            logEntry.userFields = log.UserFields;

            return logEntry;
        }
    }
}