using GamePlatform.Logger.ThirdParty;
using UnityEngine;

namespace GamePlatform.Logger.Internal
{
    public class CrashLoggerReceiver : MonoBehaviour
    {
        private IGpLoggerListener loggerListener;
        private GpLogger.CrashListener crashListener;

        public static CrashLoggerReceiver Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public void SetLoggerListener(IGpLoggerListener loggerListener)
        {
            this.loggerListener = loggerListener; 
        }

        public void SetCrashListener(GpLogger.CrashListener crashListener)
        {
            this.crashListener = crashListener;
        }

        #region Received from MobileSDK.
        public void OnLogSuccess(string jsonString)
        {
            GpLog.Debug(jsonString, GetType(), "OnLogSuccess");

            if (ValidateReceivedJsonString(jsonString) == false)
            {
                GpLog.Warn(GpLoggerStrings.INVALID_DATA_RECEIVED, GetType(), "OnLogSuccess");
                return;
            }

            var vo = JsonMapper.ToObject<GpLoggerResponse.SetLoggerListener.Success>(jsonString);
            DispatchOnLogSuccess(LogEntry.From(vo.body.log));
        }

        public void OnLogFilter(string jsonString)
        {
            GpLog.Debug(jsonString, GetType(), "OnLogFilter");

            if (ValidateReceivedJsonString(jsonString) == false)
            {
                GpLog.Warn(GpLoggerStrings.INVALID_DATA_RECEIVED, GetType(), "OnLogFilter");
                return;
            }

            var vo = JsonMapper.ToObject<GpLoggerResponse.SetLoggerListener.Filter>(jsonString);
            DispatchOnLogFilter(
                LogEntry.From(vo.body.log),
                (vo.body.filter == null) ? null : LogFilter.FromName(vo.body.filter.name));
        }

        public void OnLogSave(string jsonString)
        {
            GpLog.Debug(jsonString, GetType(), "OnLogSave");

            if (ValidateReceivedJsonString(jsonString) == false)
            {
                GpLog.Warn(GpLoggerStrings.INVALID_DATA_RECEIVED, GetType(), "OnLogSave");
                return;
            }

            var vo = JsonMapper.ToObject<GpLoggerResponse.SetLoggerListener.Save>(jsonString);
            DispatchOnLogSave(LogEntry.From(vo.body.log));
        }

        public void OnLogError(string jsonString)
        {
            GpLog.Debug(jsonString, GetType(), "OnLogError");

            if (ValidateReceivedJsonString(jsonString) == false)
            {
                GpLog.Warn(GpLoggerStrings.INVALID_DATA_RECEIVED, GetType(), "OnLogError");
                return;
            }

            var vo = JsonMapper.ToObject<GpLoggerResponse.SetLoggerListener.Error>(jsonString);
            DispatchOnLogError(LogEntry.From(vo.body.log), vo.body.errorMessage);
        }
        #endregion

        #region Received from Standalone and WebGL.
        public void OnLogSuccessWithLogItem(BaseLogItem item)
        {
            if (ValidateReceivedItem(item) == false)
            {
                GpLog.Warn(GpLoggerStrings.INVALID_DATA_RECEIVED, GetType(), "OnLogSuccessWithLogItem");
                return;
            }

            GpLog.Debug(JsonMapper.ToJson(item.GetLogDictionary()), GetType(), "OnLogSuccessWithLogItem");

            DispatchOnLogSuccess(ConvertLogEntry(item));
        }

        public void OnLogFilterWithLogItem(string filterName, BaseLogItem item)
        {
            if (ValidateReceivedItem(item) == false)
            {
                GpLog.Warn(GpLoggerStrings.INVALID_DATA_RECEIVED, GetType(), "OnLogFilterWithLogItem");
                return;
            }

            GpLog.Debug(JsonMapper.ToJson(item.GetLogDictionary()), GetType(), "OnLogFilterWithLogItem");

            DispatchOnLogFilter(ConvertLogEntry(item), LogFilter.FromName(filterName));
        }

        public void OnLogSaveWithLogItem(BaseLogItem item)
        {
            if (ValidateReceivedItem(item) == false)
            {
                GpLog.Warn(GpLoggerStrings.INVALID_DATA_RECEIVED, GetType(), "OnLogSaveWithLogItem");
                return;
            }

            GpLog.Debug(JsonMapper.ToJson(item.GetLogDictionary()), GetType(), "OnLogSaveWithLogItem");

            DispatchOnLogSave(ConvertLogEntry(item));
        }

        public void OnLogErrorWithLogItem(BaseLogItem item, string errorMessage)
        {
            if (ValidateReceivedItem(item) == false)
            {
                GpLog.Warn(GpLoggerStrings.INVALID_DATA_RECEIVED, GetType(), "OnLogErrorWithLogItem");
                return;
            }

            GpLog.Debug(
                string.Format("item:{0}, errorMessage:{1}", JsonMapper.ToJson(item.GetLogDictionary()), errorMessage),
                GetType(),
                "OnLogErrorWithLogItem");

            DispatchOnLogError(ConvertLogEntry(item), errorMessage);
        }
        #endregion

        private void DispatchOnLogSuccess(LogEntry logEntry)
        {
            if (logEntry.LogType.Equals(GpLoggerType.CRASH_FROM_UNITY) == true)
            {
                InvokeCrashListener(true, logEntry);
                return;
            }

            if (loggerListener == null)
            {
                return;
            }

            loggerListener.OnSuccess(logEntry);
        }

        private void DispatchOnLogFilter(LogEntry logEntry, LogFilter logFilter)
        {
            if (logEntry.LogType.Equals(GpLoggerType.CRASH_FROM_UNITY) == true)
            {
                InvokeCrashListener(true, logEntry);
                return;
            }

            if (loggerListener == null)
            {
                return;
            }

            loggerListener.OnFilter(logEntry, logFilter);
        }

        private void DispatchOnLogSave(LogEntry logEntry)
        {
            if (logEntry.LogType.Equals(GpLoggerType.CRASH_FROM_UNITY) == true)
            {
                InvokeCrashListener(true, logEntry);
                return;
            }

            if (loggerListener == null)
            {
                return;
            }

            loggerListener.OnSave(logEntry);
        }

        private void DispatchOnLogError(LogEntry logEntry, string errorMessage)
        {
            if (logEntry.LogType == GpLoggerType.CRASH_FROM_UNITY)
            {
                InvokeCrashListener(false, logEntry);
                return;
            }

            if (loggerListener == null)
            {
                return;
            }

            loggerListener.OnError(logEntry, string.IsNullOrEmpty(errorMessage) ? "Unknown error" : errorMessage);
        }

        /// <summary>
        /// Invoke crash listener with LogEntry that clear the user fields
        /// </summary>
        private void InvokeCrashListener(bool isSuccess, LogEntry logEntry)
        {
            if (crashListener == null)
            {
                return;
            }

            logEntry.ClearUserFields();
            crashListener(isSuccess, logEntry);
        }

        ///----------------------------------------------------------------------
        ///
        /// OnLogSuccess, OnLogFilter, OnLogSave, OnLogError
        /// 모두 같은 데이터 형식으므로 가장 기본이 되는 Success로 데이터를 검증한다.
        /// 
        ///----------------------------------------------------------------------
        private bool ValidateReceivedJsonString(string jsonString)
        {
            if (string.IsNullOrEmpty(jsonString) == true)
            {
                return false;
            }

            var vo = JsonMapper.ToObject<GpLoggerResponse.SetLoggerListener.Success>(jsonString);
            if (vo == null || vo.body == null || vo.body.log == null)
            {
                return false;
            }

            return true;
        }

        private bool ValidateReceivedItem(BaseLogItem item)
        {
            if (item == null)
            {
                return false;
            }

            var dict = item.GetLogDictionary();
            if (dict == null || dict.Count == 0)
            {
                return false;
            }

            return true;
        }

        private LogEntry ConvertLogEntry(BaseLogItem item)
        {
            LogEntry log = new LogEntry();
            JsonData jsonData = JsonMapper.ToObject(JsonMapper.ToJson(item.GetLogDictionary()));

            foreach (string key in jsonData.Keys)
            {
                switch (key)
                {
                    case LogFields.PROJECT_KEY:
                    case LogFields.LOG_SOURCE:
                    case LogFields.LOG_VERSION:
                    case LogFields.USER_ID:
                    case LogFields.PROJECT_VERSION:
                    case LogFields.DEVICE_ID:
                    case LogFields.PLATFORM_NAME:
                    case LogFields.LAUNCHED_ID:
                    case LogFields.SDK_VERSION:
                    case LogFields.SESSION_ID:
                    case LogFields.DEVICE_MODEL:
                    case LogFields.COUNTRY_CODE:
                    case LogFields.LOG_BULK_INDEX:
                        {
                            continue;
                        }
                    case LogFields.LOG_LEVEL:
                        {
                            log.LogLevel = item.GetLogLevel();
                            break;
                        }
                    case LogFields.LOG_TYPE:
                        {
                            log.LogType = item.GetLoggerType();
                            break;
                        }
                    case LogFields.LOG_MESSAGE:
                        {
                            log.Message = item.GetLogMessage();
                            break;
                        }
                    case LogFields.LOG_CREATE_TIME:
                        {
                            log.CreateTime = item.GetCreateTime();
                            break;
                        }
                    case LogFields.LOG_TRANSACTION_ID:
                        {
                            log.TransactionId = item.GetTransactionId();
                            break;
                        }
                    default:
                        {
                            log.UserFields.Add(key, jsonData[key].ToString());
                            break;
                        }
                }
            }

            return log;
        }
    }
}