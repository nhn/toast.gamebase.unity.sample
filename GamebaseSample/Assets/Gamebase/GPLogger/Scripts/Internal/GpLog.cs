using System;
using System.Text;

namespace GamePlatform.Logger.Internal
{
    public static class GpLog
    {
        public enum LogLevel
        {
            DEVELOPER,
            DEBUG,
            INFO,
            WARN,
            ERROR
        }

        public static LogLevel Level { get; set; }

        static GpLog()
        {
            Level = LogLevel.WARN;
        }

        /// <summary>
        /// Generates a log message.
        /// </summary>
        /// <param name="message">required</param>
        /// <param name="classType">required</param>
        /// <param name="methodName">optional</param>
        /// <returns>[SettingTool][ClassName::MethodName] message</returns>
        private static void PrintLog(LogLevel level, object message, Type classType, string methodName)
        {
            if ((int)Level > (int)level)
            {
                return;
            }

            StringBuilder log = new StringBuilder("[GPLogger]");
            log.AppendFormat("[{0}", classType.Name);
            log.AppendFormat("::{0}]", methodName);
            log.AppendFormat(" {0}", message);

            switch (level)
            {
                case LogLevel.DEVELOPER:
                case LogLevel.DEBUG:
                case LogLevel.INFO:
                    {
                        UnityEngine.Debug.Log(log);
                        break;
                    }
                case LogLevel.WARN:
                    {
                        UnityEngine.Debug.LogWarning(log);
                        break;
                    }
                case LogLevel.ERROR:
                    {
                        UnityEngine.Debug.LogError(log);
                        break;
                    }
            }
        }

        /// <summary>
        /// 개발자 로그
        /// </summary>
        public static void Developer(object message, Type classType, string methodName = "")
        {
            PrintLog(LogLevel.DEVELOPER, message, classType, methodName);
        }

        /// <summary>
        /// 디버그 로그
        /// </summary>
        public static void Debug(object message, Type classType, string methodName = "")
        {
            PrintLog(LogLevel.DEBUG, message, classType, methodName);
        }

        /// <summary>
        /// 정보성 로그
        /// </summary>
        public static void Info(object message, Type classType, string methodName = "")
        {
            PrintLog(LogLevel.INFO, message, classType, methodName);
        }

        /// <summary>
        /// 애플리케이션 흐름에는 영향이 없으나 제한되거나 권장하지 않는 흐름에 대한 로그
        /// </summary>
        public static void Warn(object message, Type classType, string methodName = "")
        {
            PrintLog(LogLevel.WARN, message, classType, methodName);
        }

        /// <summary>
        /// 애플리케이션 흐름에 치명적인 영향이 있는 오류
        /// </summary>
        public static void Error(object message, Type classType, string methodName = "")
        {
            PrintLog(LogLevel.ERROR, message, classType, methodName);
        }

        public static void Exception(Exception exception)
        {
            UnityEngine.Debug.LogException(exception);
        }
    }
}