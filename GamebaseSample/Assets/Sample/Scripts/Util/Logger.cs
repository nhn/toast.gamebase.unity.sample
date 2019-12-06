using System.Text;

namespace GamebaseSample
{
    public static class Logger
    {
        public const string SERVICE_NAME = "GamebaseSample";

        private static bool isDebugLog = false;

        public static void SetDebugLog(bool isDebugLog)
        {
            Logger.isDebugLog = isDebugLog;
        }

        private static string MakeLog(object message, object classObj, string methodName)
        {
            if (string.IsNullOrEmpty(message.ToString().Trim()) == true
                || classObj == null
                || string.IsNullOrEmpty(methodName.Trim()) == true)
            {
                throw new System.NullReferenceException("[GamebaseSampleLogger] message, class instance, method name can not be null.");
            }

            StringBuilder log = new StringBuilder();
            log.AppendFormat("[{0}]", SERVICE_NAME);
            log.AppendFormat("[{0}", classObj.GetType().Name);
            log.AppendFormat("::{0}]", methodName);
            log.AppendFormat(" {0}", message);

            return log.ToString();
        }

        public static void Debug(object message, object classObj, [System.Runtime.CompilerServices.CallerMemberName] string methodName = "")
        {
            string log = MakeLog(message, classObj, methodName);

            if (isDebugLog == true)
            {
                UnityEngine.Debug.Log(log);
            }
        }

        public static void Warn(object message, object classObj, [System.Runtime.CompilerServices.CallerMemberName] string methodName = "")
        {
            string log = MakeLog(message, classObj, methodName);

            UnityEngine.Debug.LogWarning(log);
        }

        public static void Error(object message, object classObj, [System.Runtime.CompilerServices.CallerMemberName] string methodName = "")
        {
            UnityEngine.Debug.LogError(MakeLog(message, classObj, methodName));
        }
    }
}