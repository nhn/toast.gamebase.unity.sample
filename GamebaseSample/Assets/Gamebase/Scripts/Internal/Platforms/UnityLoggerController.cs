using GamePlatform.Logger;
using System.Collections.Generic;
using System.Linq;

namespace Toast.Gamebase.Internal
{ 
    public class UnityLoggerController
    {
        private static readonly UnityLoggerController instance = new UnityLoggerController();

        public static UnityLoggerController Instance
        {
            get { return instance; }
        }

        private readonly Dictionary<GamebaseCallback.Logger.CrashFilter, GpLogger.CrashFilter> crashFilterDictionary;
        private string appKey;

        public UnityLoggerController()
        {
            crashFilterDictionary = new Dictionary<GamebaseCallback.Logger.CrashFilter, GpLogger.CrashFilter>();
        }

        public void Initialize(GamebaseRequest.Logger.Configuration config)
        {
            GamebaseLog.Debug("Initialize", this);

            appKey = config.appKey;

            var param = new GpLoggerParams.Initialization(config.appKey)
            {
                enableCrashErrorLog = config.enableCrashErrorLog,
                enableCrashReporter = config.enableCrashReporter
            };

            switch (config.serviceZone.ToLower())
            {
                case "alpha":
                    {
                        param.serviceZone = GamePlatform.Logger.ServiceZone.ALPHA;
                        break;
                    }
                case "beta":
                    {
                        param.serviceZone = GamePlatform.Logger.ServiceZone.ALPHA;
                        break;
                    }
                case "real":
                default:
                    {
                        param.serviceZone = GamePlatform.Logger.ServiceZone.REAL;
                        break;
                    }
            }

            GamebaseLog.Debug(GamebaseJsonUtil.ToPrettyJsonString(param), typeof(UnityLoggerController));

            GpLogger.Initialize(param, true);
        }

        public void SetUserId(string userId)
        {
            GamebaseLog.Debug(string.Format("userId:{0}", userId), this);
            GpLogger.UserId = userId;
        }

        public void Debug(string message, Dictionary<string, string> userFields = null)
        {
            GamebaseLog.Debug("Debug", this);
            GpLogger.Debug(appKey, message, userFields);
        }

        public void Info(string message, Dictionary<string, string> userFields = null)
        {
            GamebaseLog.Debug("Info", this);
            GpLogger.Info(appKey, message, userFields);
        }

        public void Warn(string message, Dictionary<string, string> userFields = null)
        {
            GamebaseLog.Debug("Warn", this);
            GpLogger.Warn(appKey, message, userFields);
        }

        public void Error(string message, Dictionary<string, string> userFields = null)
        {
            GamebaseLog.Debug("Error", this);
            GpLogger.Error(appKey, message, userFields);
        }

        public void Fatal(string message, Dictionary<string, string> userFields = null)
        {
            GamebaseLog.Debug("Fatal", this);
            GpLogger.Fatal(appKey, message, userFields);
        }

        public void Report(GamebaseLoggerConst.LogLevel logLevel, string message, string logString, string stackTrace)
        {
            GamebaseLog.Debug("Report", this);
            GpLogger.Report(appKey, (GpLogLevel)logLevel, message, logString, stackTrace);
        }

        public void SetUserField(string key, string value)
        {
            GamebaseLog.Debug("SetUserField", this);
            GpLogger.SetUserField(appKey, key, value);
        }

        public void SetLoggerListener(GamebaseCallback.Logger.ILoggerListener listener)
        {
            GamebaseLog.Debug("SetLoggerListener", this);

            GpLogger.SetLoggerListener(appKey, new GamebaseLoggerListener(listener));
        }

        public void SetCrashListener(GamebaseCallback.Logger.CrashListener listener)
        {
            GamebaseLog.Debug("SetCrashListener", this);
            GpLogger.SetCrashListener(appKey, (isSuccess, logEntry) =>
            {
                GamebaseLog.Debug("OnCrashListener", this);
                listener(isSuccess, ConvertGamebaseLogEntry(logEntry));
            });
        }

        public void AddCrashFilter(GamebaseCallback.Logger.CrashFilter filter)
        {
            GamebaseLog.Debug("AddCrashFilter", this);
            GpLogger.CrashFilter crashFilter =
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

            GpLogger.AddCrashFilter(appKey, crashFilter);

            crashFilterDictionary.Add(filter, crashFilter);
        }

        public void RemoveCrashFilter(GamebaseCallback.Logger.CrashFilter filter)
        {
            GamebaseLog.Debug("RemoveCrashFilter", this);

            GpLogger.CrashFilter crashFilter = crashFilterDictionary[filter];
            GpLogger.RemoveCrashFilter(appKey, crashFilter);

            crashFilterDictionary.Remove(filter);
        }

        private class GamebaseLoggerListener : IGpLoggerListener
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
            GamebaseResponse.Logger.LogEntry logEntry = new GamebaseResponse.Logger.LogEntry
            {
                logType = log.LogType,
                createTime = log.CreateTime,
                logLevel = (GamebaseLoggerConst.LogLevel)log.LogLevel,
                message = log.Message,
                transactionId = log.TransactionId
            };

            if (log.UserFields != null)
            {
                logEntry.userFields = log.UserFields.ToDictionary(kvp => kvp.Key, pair => (object)pair.Value);
            }

            return logEntry;
        }
    }
}