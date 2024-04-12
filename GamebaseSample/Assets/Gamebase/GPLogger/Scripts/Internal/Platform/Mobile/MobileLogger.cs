using GamePlatform.Logger.ThirdParty;
using System.Collections.Generic;

namespace GamePlatform.Logger.Internal
{
    public abstract class MobileLogger : ILogger
    {
        private readonly string DOMAIN = typeof(MobileLogger).Name;
        protected readonly bool isUserAccess;

        private string appKey;

        public MobileLogger(bool isUserAccess)
        {
            this.isUserAccess = isUserAccess;
        }

        public void Initialize(GpLoggerParams.Initialization param)
        {
            GpLog.Debug(string.Format("param:{0}", JsonMapper.ToJson(param)), GetType(), "Initialize");
            appKey = param.appKey;

            string apiScheme;

            if (isUserAccess == true)
            {
                apiScheme = ApiScheme.Generate("Initialize");
            }
            else
            {
                apiScheme = ApiScheme.GenerateWithAppKey(param.appKey, "initialize");
            }

            var protocol = new LoggerProtocol(apiScheme);
            protocol.AddParameter("projectKey", param.appKey);
            protocol.AddParameter("serviceZone", param.serviceZone.ToString().ToUpper());

            if (isUserAccess == true)
            {
                protocol.AddParameter("enableCrashReporter", param.enableCrashReporter);
            }
            else
            {
                protocol.AddParameter("setting", "DEFAULT");
            }

            SendMessage(protocol.ToString());
        }

        public void Log(GpLogLevel logLevel, string message, Dictionary<string, string> userFields, string logType = "")
        {
            GpLog.Debug(string.Format("logLevel:{0}, message:{1}, userFields:{2}, logType:{3}",
                    logLevel.ToString().ToUpper(),
                    message,
                    (userFields == null) ? "" : JsonMapper.ToJson(userFields),
                    logType), GetType(), "Log");

            string apiScheme;

            if (isUserAccess == true)
            {
                apiScheme = ApiScheme.Generate("Log");
            }
            else
            {
                apiScheme = ApiScheme.GenerateWithAppKey(appKey, "Log");
            }

            var protocol = new LoggerProtocol(apiScheme);
            protocol.AddParameter("level", logLevel.ToString().ToUpper());
            protocol.AddParameter("message", message);

            if (isUserAccess == false)
            {
                if (string.IsNullOrEmpty(logType) == true)
                {
                    protocol.AddParameter("type", "NORMAL");
                }
                else
                {
                    protocol.AddParameter("type", logType);
                }
            }

            if (userFields != null)
            {
                var userFieldKeys = userFields.Keys;
                foreach (string key in userFieldKeys)
                {
                    if (string.IsNullOrEmpty(key) == true)
                    {
                        var error = new GpLoggerError(
                            GpLoggerErrorCode.INVALID_PARAMETER,
                            DOMAIN,
                            GpLoggerStrings.INVALID_PARAMETER_KEY_IS_NULL_OR_EMPTY);
                        GpLog.Error(error, GetType(), "Log");

                        return;
                    }
                }

                protocol.AddParameter("userFields", userFields);
            }

            SendMessage(protocol.ToString());
        }

        public void SetUserField(string key, string value)
        {
            GpLog.Debug(string.Format("key:{0}, value:{1}", key, value), GetType(), "SetUserField");

            if (string.IsNullOrEmpty(key) == true)
            {
                var error = new GpLoggerError(
                    GpLoggerErrorCode.INVALID_PARAMETER,
                    DOMAIN,
                    GpLoggerStrings.INVALID_PARAMETER_KEY_IS_NULL_OR_EMPTY);
                GpLog.Error(error, GetType(), "SetUserField");

                return;
            }

            string apiScheme;

            if (isUserAccess == true)
            {
                apiScheme = ApiScheme.Generate("SetUserField");
            }
            else
            {
                apiScheme = ApiScheme.GenerateWithAppKey(appKey, "SetUserField");
            }

            var protocol = new LoggerProtocol(apiScheme);
            protocol.AddParameter("key", key);
            protocol.AddParameter("value", value);

            SendMessage(protocol.ToString());
        }

        public abstract string SendMessage(string jsonString);

        //----------------------------------------
        //
        //  이하 API는 사용자 로거에서만 호출된다.
        //
        //----------------------------------------

        public void Report(CrashData crashData)
        {
            GpLog.Debug(string.Format("crashData:{0}", JsonMapper.ToJson(crashData)), GetType(), "Report");

            var apiScheme = ApiScheme.Generate("Exception");
            var protocol = new LoggerProtocol(apiScheme);
            protocol.AddParameter("logType", crashData.logType);
            protocol.AddParameter("logLevel", crashData.logLevel);
            protocol.AddParameter("message", crashData.message);
            protocol.AddParameter("dmpData", crashData.DmpData);
            protocol.AddParameter("userFields", crashData.userFields);

            SendMessage(protocol.ToString());
        }

        public void SetLoggerListener(IGpLoggerListener listener)
        {
            GpLog.Debug("SetLoggerListener", GetType(), "SetLoggerListener");
            SetCrashLoggerListener();
        }

        public void SetCrashListener(GpLogger.CrashListener listener)
        {
            GpLog.Debug("SetCrashListener", GetType(), "SetCrashListener");
            SetCrashLoggerListener();
        }

        private void SetCrashLoggerListener()
        {
            var apiScheme = ApiScheme.Generate("SetLoggerListener");
            var protocol = new LoggerProtocol(apiScheme);

            protocol.AddParameter("success", "OnLogSuccess");
            protocol.AddParameter("filter", "OnLogFilter");
            protocol.AddParameter("save", "OnLogSave");
            protocol.AddParameter("error", "OnLogError");

            SendMessage(protocol.ToString());
        }
    }
}