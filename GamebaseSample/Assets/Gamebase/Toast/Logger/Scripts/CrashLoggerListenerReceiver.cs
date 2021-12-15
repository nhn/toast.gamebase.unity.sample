namespace Toast.Logger
{
    using UnityEngine;
    using Toast.Core;
    using Toast.Internal;
    
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

        private LogEntry ConvertLogEntry(ToastLoggerLogObject logObject)
        {
            LogEntry log = new LogEntry();
            JSONNode jSONNode = logObject.GetJSONNode();

            foreach (string key in jSONNode.Keys)
            {
                if (key.Equals(LogFields.PROJECT_KEY) || key.Equals(LogFields.LOG_SOURCE)
                    || key.Equals(LogFields.LOG_VERSION) || key.Equals(LogFields.USER_ID)
                    || key.Equals(LogFields.PROJECT_VERSION) || key.Equals(LogFields.DEVICE_ID)
                    || key.Equals(LogFields.PLATFORM_NAME) || key.Equals(LogFields.LAUNCHED_ID)
                    || key.Equals(LogFields.SDK_VERSION) || key.Equals(LogFields.SESSION_ID)
                    || key.Equals(LogFields.DEVICE_MODEL) || key.Equals(LogFields.COUNTRY_CODE)
                    || key.Equals(LogFields.LOG_BULK_INDEX))
                {
                    continue;
                }
                else if (key.Equals(LogFields.LOG_LEVEL))
                {
                    log.LogLevel = logObject.GetLogLevel();
                }
                else if (key.Equals(LogFields.LOG_TYPE))
                {
                    log.LogType = logObject.GetLoggerType();
                }
                else if (key.Equals(LogFields.LOG_MESSAGE))
                {
                    log.Message = logObject.GetLogMessage();
                }
                else if (key.Equals(LogFields.LOG_CREATE_TIME))
                {
                    log.CreateTime = logObject.GetCreateTime();
                }
                else if (key.Equals(LogFields.LOG_TRANSACTION_ID))
                {
                    log.TransactionId = logObject.GetTransactionId();
                }
                else
                {
                    log.UserFields.Add(key, jSONNode[key].Value);
                }
            }

            return log;
        }

        public void OnLogSuccessWithToastLoggerObject(ToastLoggerLogObject logObject)
        {
            ToastLog.Debug("[OnLogSuccessWithToastLoggerObject] {0}", logObject.GetJSONString());

            LogEntry log = ConvertLogEntry(logObject);

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

        public void OnLogFilterWithToastLoggerObject(string filterName, ToastLoggerLogObject logObject)
        {
            LogEntry log = ConvertLogEntry(logObject);

            if (log.LogType == ToastLoggerType.CRASH_FROM_UNITY)
            {
                InvokeCrashListener(true, log);
            }
            else
            {
                if (_listenerLogger != null)
                {
                    _listenerLogger.OnFilter(log, LogFilter.FromName(filterName));
                }
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
                        if (log.LogType == ToastLoggerType.CRASH_FROM_UNITY)
                        {
                            InvokeCrashListener(true, log);
                        }
                        else
                        {
                            if (_listenerLogger != null)
                            {
                                _listenerLogger.OnFilter(log, LogFilter.From(filter.AsObject));
                            }
                        }
                    }
                }
            }
        }

        public void OnLogSaveWithToastLoggerObject(ToastLoggerLogObject logObject)
        {
            LogEntry log = ConvertLogEntry(logObject);

            if (log.LogType == ToastLoggerType.CRASH_FROM_UNITY)
            {
                InvokeCrashListener(true, log);
            }
            else
            {
                if (_listenerLogger != null)
                {
                    _listenerLogger.OnSave(log);
                }
            }
        }

        public void OnLogSave(string jsonString)
        {
            ToastLog.Debug("[OnLogSave] {0}", jsonString);

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
                        _listenerLogger.OnSave(log);
                    }
                }
            }
        }

        public void OnLogErrorWithToastLoggerObject(ToastLoggerLogObject logObject, string errorMessage)
        {
            LogEntry log = ConvertLogEntry(logObject);

            if (log.LogType == ToastLoggerType.CRASH_FROM_UNITY)
            {
                InvokeCrashListener(false, log);
            }
            else
            {
                if (_listenerLogger != null)
                {
                    _listenerLogger.OnError(log,
                        string.IsNullOrEmpty(errorMessage) ? "Unknown error" : errorMessage);
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
                                body.ContainsKey("errorMessage") ? (string)body["errorMessage"] : "Unknown error");
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