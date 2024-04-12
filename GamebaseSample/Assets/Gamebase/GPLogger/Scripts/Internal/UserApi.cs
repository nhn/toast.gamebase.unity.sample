using System.Collections.Generic;
using UnityEngine;

namespace GamePlatform.Logger.Internal
{
    public class UserApi : ILoggerApi
    {
        private ILogger logger;

        public void Initialize(GpLoggerParams.Initialization param)
        {
            GameObjectManager.AddCrashLoggerListener(GameObjectType.UNITY_TOAST_SDK_PLUGIN);

            logger = GetLogger();
            logger.Initialize(param);

            GpCrashManager.Instance.Initialize(param.enableCrashReporter, param.enableCrashErrorLog);
            GpCrashManager.Instance.AddCrashHandler((crashData) =>
            {
                Report(crashData);
            });
        }

        public void SetLoggerListener(IGpLoggerListener listener)
        {
            logger.SetLoggerListener(listener);

            CrashLoggerReceiver.Instance.SetLoggerListener(listener);
        }

        public void SetCrashListener(GpLogger.CrashListener listener)
        {
            logger.SetCrashListener(listener);

            CrashLoggerReceiver.Instance.SetCrashListener(listener);
        }

        public void SetUserField(string key, string value)
        {
            logger.SetUserField(key, value);
        }

        public void AddCrashFilter(GpLogger.CrashFilter filter)
        {
            GpCrashManager.Instance.AddCrashFilter(filter);
        }

        public void RemoveCrashFilter(GpLogger.CrashFilter filter)
        {
            GpCrashManager.Instance.RemoveCrashFilter(filter);
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

        public void Report(GpLogLevel logLevel, string message, string logString, string stackTrace)
        {
            var userFields = new Dictionary<string, string>
            {
                { "Unity", Application.unityVersion }
            };

            var crashData = new CrashData
            {
                logType = GpLoggerType.HANDLED,
                logLevel = logLevel.ToString().ToUpper(),
                message = message,
                userFields = userFields
            };

            crashData.SetDmpData(logString, stackTrace);
            Report(crashData);
        }

        private void Log(GpLogLevel logLevel, string message, Dictionary<string, string> userFields = null, string logType = "")
        {
            logger.Log(logLevel, message, userFields, logType);
        }

        private void Report(CrashData crashData)
        {
            logger.Report(crashData);
        }

        private ILogger GetLogger()
        {
#if UNITY_EDITOR
            logger = new StandaloneLogger(true);
#else
#if UNITY_ANDROID
            logger = new AndroidLogger(true);
#elif UNITY_IOS
            logger = new IOSLogger(true);
#elif UNITY_STANDALONE
            logger = new StandaloneLogger(true);
#elif UNITY_WEBGL
            logger = new WebGLLogger(true);
#endif
#endif
            return logger;
        }
    }
}