using System.Collections.Generic;
using System.Reflection;

namespace GamePlatform.Logger.Internal
{
    public class PlatformSdkApi : ILoggerApi
    {
        private ILogger logger;

        public void Initialize(GpLoggerParams.Initialization param)
        {
            logger = GetLogger();
            logger.Initialize(param);
        }

        public void SetUserField(string key, string value)
        {
            logger.SetUserField(key, value);
        }

        public void Debug(string message, Dictionary<string, string> userFields = null, string logType = "")
        {
            Log(GpLogLevel.DEBUG, message, userFields, logType);
        }

        public void Info(string message, Dictionary<string, string> userFields = null, string logType = "")
        {
            Log(GpLogLevel.INFO, message, userFields, logType);
        }

        public void Warn(string message, Dictionary<string, string> userFields = null, string logType = "")
        {
            Log(GpLogLevel.WARN, message, userFields, logType);
        }

        public void Error(string message, Dictionary<string, string> userFields = null, string logType = "")
        {
            Log(GpLogLevel.ERROR, message, userFields, logType);
        }

        public void Fatal(string message, Dictionary<string, string> userFields = null, string logType = "")
        {
            Log(GpLogLevel.FATAL, message, userFields, logType);
        }

        private void Log(GpLogLevel logLevel, string message, Dictionary<string, string> userFields = null, string logType = "")
        {
            logger.Log(logLevel, message, userFields, logType);
        }

        private ILogger GetLogger()
        {
#if UNITY_EDITOR
            logger = new StandaloneLogger(false);
#else
#if UNITY_ANDROID
            logger = new AndroidLogger(false);
#elif UNITY_IOS
            logger = new IOSLogger(false);
#elif UNITY_STANDALONE
            logger = new StandaloneLogger(false);
#elif UNITY_WEBGL
            logger = new WebGLLogger(false);
#endif
#endif
            return logger;
        }

        //--------------------------------------------------------------------------------
        //
        //  Not Supported method.
        //  Platform SDKs cannot use this API.
        //
        //--------------------------------------------------------------------------------
        public void SetLoggerListener(IGpLoggerListener listener)
        {
            GpLog.Warn(GpLoggerStrings.NOT_SUPPORTED, GetType(), MethodBase.GetCurrentMethod().Name);
        }

        public void SetCrashListener(GpLogger.CrashListener listener)
        {
            GpLog.Warn(GpLoggerStrings.NOT_SUPPORTED, GetType(), MethodBase.GetCurrentMethod().Name);
        }

        public void AddCrashFilter(GpLogger.CrashFilter filter)
        {
            GpLog.Warn(GpLoggerStrings.NOT_SUPPORTED, GetType(), MethodBase.GetCurrentMethod().Name);
        }

        public void RemoveCrashFilter(GpLogger.CrashFilter filter)
        {
            GpLog.Warn(GpLoggerStrings.NOT_SUPPORTED, GetType(), MethodBase.GetCurrentMethod().Name);
        }

        public void Report(GpLogLevel logLevel, string message, string logString, string stackTrace)
        {
            GpLog.Warn(GpLoggerStrings.NOT_SUPPORTED, GetType(), MethodBase.GetCurrentMethod().Name);
        }
    }
}