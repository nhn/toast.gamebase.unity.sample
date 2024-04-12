using GamePlatform.Logger.ThirdParty;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlatform.Logger.Internal
{
    public class PCLogger : ILogger
    {
        protected readonly bool isUserAccess;
        public readonly Dictionary<string, string> UserFields;
        private GpLoggerLogSender sender;

        private LogNCrash logNCrash;
        private bool isSentSessionLog = false;

        /// <summary>
        /// 초기화가 완료되기 전에 호출된 로그를 저장하는 Queue.
        /// </summary>
        private Queue<BaseLogItem> preInitializationLogQueue;

        public PCLogger(bool isUserAccess)
        {
            this.isUserAccess = isUserAccess;

            UserFields = new Dictionary<string, string>();
        }

        public void Initialize(GpLoggerParams.Initialization param)
        {
            GpLog.Debug(string.Format("param:{0}", JsonMapper.ToJson(param)), GetType(), "Initialize");

            logNCrash = new LogNCrash(param.appKey, param.serviceZone);

            if (isUserAccess == true)
            {
                LoadLogNCrashSettings(() =>
                {
                    CreateLogSender(logNCrash);

                    if (param.enableCrashReporter == true)
                    {
                        if (isSentSessionLog == false)
                        {
                            SendSessionData();
                            isSentSessionLog = true;
                        }
                    }

                    SendPreInitializationLog();
                });
            }
            else
            {
                CreateLogSender(logNCrash);
                SendPreInitializationLog();
            }

#if UNITY_STANDALONE || UNITY_EDITOR
            BackupLogManager.RemoveOldFiles(param.appKey);
#endif
        }

        private void CreateLogSender(LogNCrash logNCrash)
        {
            sender = new GpLoggerLogSender(logNCrash);
            sender.Start();
        }

        private void LoadLogNCrashSettings(Action callback)
        {
            GameObjectManager.GetCoroutineComponent(GameObjectType.GP_LOGGER).StartCoroutine(
                logNCrash.RequestLogNCrashSettings(() =>
                {
                    GpLog.Debug(string.Format("settings:{0}", JsonMapper.ToJson(logNCrash.Settings)), GetType(), "LoadLogNCrashSettings");
                    callback();
                }));
        }

        public void Log(GpLogLevel logLevel, string message, Dictionary<string, string> userFields, string logType = "")
        {
            GpLog.Debug(string.Format("logLevel:{0}, message:{1}, userFields:{2}, logType:{3}",
                    logLevel.ToString().ToUpper(),
                    message,
                    (userFields == null) ? "" : JsonMapper.ToJson(userFields),
                    logType), GetType(), "Log");

            BaseLogItem item = new BaseLogItem();

            item.SetDefaultLog(logNCrash.GetAppKey(), GpLoggerType.NORMAL, logLevel, message);

            if (string.IsNullOrEmpty(GpLogger.UserId) == false)
            {
                item.Add(LogFields.USER_ID, GpLogger.UserId);
            }

            SetCommonData(item, GpLoggerType.NORMAL);

            if (userFields != null)
            {
                item.SetUserFields(userFields);
            }

            GpLog.Debug(JsonMapper.ToJson(item.GetLogDictionary()), GetType(), "Log");

            AddLogItem(item);
        }

        public void SetUserField(string key, string value)
        {
            GpLog.Debug(string.Format("key:{0}, value:{1}", key, value), GetType(), "SetUserField");

            if (string.IsNullOrEmpty(key) == true || string.IsNullOrEmpty(value) == true)
            {
                return;
            }

            string convertedKey = LogFields.ConvertField(key);

            if (UserFields.ContainsKey(convertedKey) == true)
            {
                UserFields[convertedKey] = value;
            }
            else
            {
                UserFields.Add(convertedKey, value);
            }
        }

        //----------------------------------------
        //
        //  이하 API는 사용자 로거에서만 호출된다.
        //
        //----------------------------------------

        public void Report(CrashData crashData)
        {
            GpLog.Debug(string.Format("crashData:{0}", JsonMapper.ToJson(crashData)), GetType(), "Report");

            CrashLogItem item = new CrashLogItem();
            item.SetDefaultLog(logNCrash.GetAppKey(), crashData.logType, (GpLogLevel)Enum.Parse(typeof(GpLogLevel), crashData.logLevel), crashData.message);
            item.SetCrashDump(crashData.DmpData);
            item.SetCrashStyle("unity-cs");
            item.SetCrashSymbol("none");
            item.SetLogSource("CrashDump");

            if (string.IsNullOrEmpty(GpLogger.UserId) == false)
            {
                item.Add(LogFields.USER_ID, GpLogger.UserId);
            }

            SetCommonData(item, crashData.logType);

            if (crashData.userFields != null)
            {
                item.SetUserFields(crashData.userFields);
            }

            AddLogItem(item);
        }

        public void SetLoggerListener(IGpLoggerListener listener)
        {
            // not work.
            GpLog.Debug("SetLoggerListener", GetType(), "SetLoggerListener");
        }

        public void SetCrashListener(GpLogger.CrashListener listener)
        {
            // not work.
            GpLog.Debug("SetCrashListener", GetType(), "SetCrashListener");
        }

        private void SendSessionData()
        {
            var item = new SessionLogItem();
            item.SetDefaultLog(logNCrash.GetAppKey(), GpLoggerType.SESSION, GpLogLevel.NONE, "SESSION");
            SetCommonData(item, GpLoggerType.SESSION);

            AddLogItem(item);
        }

        private void SetCommonData(BaseLogItem item, string loggerType)
        {
            item.Add(LogFields.PROJECT_VERSION, Application.version);
            item.Add(LogFields.DEVICE_ID, GpAppInfo.deviceId);
            item.Add(LogFields.PLATFORM_NAME, GpAppInfo.platform);
            item.Add(LogFields.LAUNCHED_ID, GpAppInfo.launchedId);
            item.Add(LogFields.SDK_VERSION, GpLogger.VERSION);

            foreach (var field in UserFields)
            {
                item.SetUserField(field.Key, field.Value);
            }

            if (isUserAccess == true)
            {
                item.Add(LogFields.SESSION_ID, GpAppInfo.sessionId);

                switch (loggerType)
                {
                    case GpLoggerType.SESSION:
                    case GpLoggerType.CRASH:
                    case GpLoggerType.HANDLED:
                    case GpLoggerType.CRASH_FROM_INACTIVATED_STATE:
                    case GpLoggerType.CRASH_FROM_UNITY:
                        {
                            item.Add(LogFields.DEVICE_MODEL, GpAppInfo.deviceModel);
                            item.Add(LogFields.COUNTRY_CODE, GpAppInfo.countryCode);
                            break;
                        }
                }
            }
        }

        private void AddLogItem(BaseLogItem item)
        {
            if (sender == null)
            {
                if (preInitializationLogQueue == null)
                {
                    preInitializationLogQueue = new Queue<BaseLogItem>();
                }

                preInitializationLogQueue.Enqueue(item);
            }
            else
            {
                sender.AddLogItem(item);
            }
        }

        private void SendPreInitializationLog()
        {
            if (preInitializationLogQueue == null || preInitializationLogQueue.Count == 0)
            {
                return;
            }

            while (preInitializationLogQueue.Count > 0)
            {
                sender.AddLogItem(preInitializationLogQueue.Dequeue());
            }
        }
    }
}