using Toast.Internal;
using UnityEngine;

namespace Toast.Logger
{
    public class CrashLoggerListenerReceiver : MonoBehaviour
    {
        private const string SERVICE_NAME = "logger";

        private IToastLoggerListener _listenerLogger;

        private ToastLogger.CrashListener _listenerCrash;
        private static CrashLoggerListenerReceiver _instance;

        public static CrashLoggerListenerReceiver Instance
        {
            get
            {
                return _instance;
            }            
        }

        public static bool TryAttachReceiver()
        {
            if (_instance == null)
            {
                var gameObject = ToastNativePlugin.Instance.gameObject;
                var currentReceiver = gameObject.GetComponent<CrashLoggerListenerReceiver>();
                if (currentReceiver == null)
                {
                    _instance = gameObject.AddComponent<CrashLoggerListenerReceiver>();
                }

                return true;
            }

            return false;
        }

        public static void SetLoggerListener(IToastLoggerListener listener)
        {
            if (_instance != null)
            {
                _instance._listenerLogger = listener;
            }
        }

        public static void SetCrashListener(ToastLogger.CrashListener delegateCrash)
        {
            if (_instance != null)
            {
                _instance._listenerCrash = delegateCrash;
            }
        }

        public void OnLogSuccess(ToastLoggerLogObject logObject)
        {
            LogEntry log = new LogEntry();
            log.LogType = logObject.GetLoggerType();
            log.LogLevel = logObject.GetLogLevel();
            log.Message = logObject.GetLogMessage();
            log.TransactionId = logObject.GetTransactionId();
            log.CreateTime = logObject.GetCreateTime();

            if (log.LogType == ToastLoggerType.CRASH_FROM_UNITY)
            {
                InvokeCrashListener(true, log);
            }
            else
            {
                if (_listenerLogger != null)
                {
                    _listenerLogger.OnSuccess(log);
                }
            }
        }

        public void OnLogSuccess(string jsonString)
        {
            ToastLog.Debug("[OnLogSuccess] {0}", jsonString);

            LogEntry log;
            if (TryParseLog(jsonString, out log))
            {
                if (log.LogType == ToastLoggerType.CRASH_FROM_UNITY)
                {
                    InvokeCrashListener(true, log);
                }
                else
                {
                    if (_listenerLogger != null)
                    {
                        _listenerLogger.OnSuccess(log);
                    }
                }
            }
        }

        public void OnLogFilter(string filterName, ToastLoggerLogObject logObject)
        {
            LogEntry log = new LogEntry();
            log.LogType = logObject.GetLoggerType();
            log.LogLevel = logObject.GetLogLevel();
            log.Message = logObject.GetLogMessage();
            log.TransactionId = logObject.GetTransactionId();
            log.CreateTime = logObject.GetCreateTime();

            if (_listenerLogger != null)
            {
                _listenerLogger.OnFilter(log, LogFilter.FromName(filterName));
            }
        }

        public void OnLogFilter(string jsonString)
        {
            ToastLog.Debug("[OnLogFilter] {0}", jsonString);

            LogEntry log;
            if (TryParseLog(jsonString, out log))
            {
                JSONNode filter;
                if (JsonUtils.TrySelectJsonObject(jsonString, out filter, "body", "filter"))
                {
                    if (filter.IsObject)
                    {
                        if (_listenerLogger != null)
                        {
                            _listenerLogger.OnFilter(log, LogFilter.From(filter.AsObject));
                        }
                    }
                }
            }
        }

        public void OnLogSave(ToastLoggerLogObject logObject)
        {
            LogEntry log = new LogEntry();
            log.LogType = logObject.GetLoggerType();
            log.LogLevel = logObject.GetLogLevel();
            log.Message = logObject.GetLogMessage();
            log.TransactionId = logObject.GetTransactionId();
            log.CreateTime = logObject.GetCreateTime();

            if (_listenerLogger != null)
            {
                _listenerLogger.OnSave(log);
            }
        }

        public void OnLogSave(string jsonString)
        {
            ToastLog.Debug("[OnLogSave] {0}", jsonString);

            LogEntry log;
            if (TryParseLog(jsonString, out log))
            {
                if (_listenerLogger != null)
                {
                    _listenerLogger.OnSave(log);
                }
            }
        }

        public void OnLogError(ToastLoggerLogObject logObject, string errorMessage)
        {
            LogEntry log = new LogEntry();
            log.LogType = logObject.GetLoggerType();
            log.LogLevel = logObject.GetLogLevel();
            log.Message = logObject.GetLogMessage();
            log.TransactionId = logObject.GetTransactionId();
            log.CreateTime = logObject.GetCreateTime();

            if (log.LogType == ToastLoggerType.CRASH_FROM_UNITY)
            {
                InvokeCrashListener(false, log);
            }
            else
            {
                if (_listenerLogger != null)
                {
                    _listenerLogger.OnError(log,
                        string.IsNullOrEmpty(errorMessage) ? "Unknown error" : errorMessage );
                }
            }

        }

        public void OnLogError(string jsonString)
        {
            ToastLog.Debug("[OnLogError] {0}", jsonString);

            LogEntry log;
            if (TryParseLog(jsonString, out log))
            {
                JSONObject body;
                if (TryParseBody(jsonString, out body))
                {
                    if (log.LogType == ToastLoggerType.CRASH_FROM_UNITY)
                    {
                        InvokeCrashListener(false, log);
                    }
                    else
                    {
                        if (_listenerLogger != null)
                        {
                            _listenerLogger.OnError(log,
                                body.ContainsKey("errorMessage") ? (string) body["errorMessage"] : "Unknown error");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Invoke crash listener with LogEntry that clear the user fields
        /// </summary>
        private void InvokeCrashListener(bool isSuccess, LogEntry logEntry)
        {
            if (_listenerCrash != null)
            {
                logEntry.ClearUserFields();
                _listenerCrash(isSuccess, logEntry);
            }
        }

        private bool TryParseLog(string jsonString, out LogEntry log)
        {
            log = null;

            JSONNode logJson;
            if (JsonUtils.TrySelectJsonObject(JSONNode.Parse(jsonString), out logJson, "body", "log"))
            {
                if (logJson.IsObject)
                {
                    log = LogEntry.From(logJson.AsObject);
                }

                return log != null;
            }

            return false;
        }

        private bool TryParseBody(string jsonString, out JSONObject body)
        {
            body = null;

            JSONNode outNode;
            if (JsonUtils.TrySelectJsonObject(JSONNode.Parse(jsonString), out outNode, "body"))
            {
                if (outNode.IsObject)
                {
                    body = outNode.AsObject;
                }

                return body != null;
            }

            return false;
        }
    }
}