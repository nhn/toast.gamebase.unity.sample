using System.Text;

#if !UNITY_5_6_OR_NEWER || (UNITY_5_6_OR_NEWER && (NET_2_0 || NET_2_0_SUBSET))

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public sealed class CallerMemberNameAttribute : Attribute
    {
    }
}

#endif

namespace Toast.Gamebase.Internal
{
    public static class GamebaseLog
    {
        public const string SERVICE_NAME    = "TCGB";
        public const string PLATFORM_NAME   = "Unity";

        public const string GAMEBASE_LOG    = "GAMEBASE_LOG";

        public static bool isDebugLog      = false;

        public static void SetDebugLog(bool isDebugLog)
        {
            GamebaseLog.isDebugLog = isDebugLog;
        }

        /// <summary>
        /// Generates a log message.
        /// </summary>
        /// <param name="message">required</param>
        /// <param name="classObj">required</param>
        /// <param name="methodName">required</param>
        /// <returns>[TCGB][Unity][ClassName::MethodName]</returns>
        private static string MakeLog(object message, object classObj, string methodName)
        {
            if (string.IsNullOrEmpty(message.ToString().Trim()) == true
                || classObj == null
                || string.IsNullOrEmpty(methodName.Trim()) == true)
            {
                throw new System.NullReferenceException("[GamebaseLog] message, class instance, method name can not be null.");
            }

            StringBuilder log = new StringBuilder();
            log.AppendFormat("[{0}]", SERVICE_NAME);
            log.AppendFormat("[{0}]", PLATFORM_NAME);
            log.AppendFormat("[{0}", classObj.GetType().Name);
            log.AppendFormat("::{0}]", methodName);
            log.AppendFormat(" {0}", message);

            return log.ToString();
        }

        /// <summary>
        /// 1. 함수 내부에 표시되는 로그
        /// 2. 함수 시작 및 플로우 표시를 위한 로그
        /// 3. 데이터와 관련된 로그 표시
        /// </summary>
        public static void Debug(object message, object classObj, [System.Runtime.CompilerServices.CallerMemberName] string methodName = "")
        {
            if (isDebugLog == false)
            {
                return;
            }
            
            string log = MakeLog(message, classObj, methodName);

            UnityEngine.Debug.Log(log);
        }

        /// <summary>
        /// 게임 흐름에는 영향이 없으나 제한되거나 권장하지 않는 흐름에 대한 로그
        /// </summary>
        public static void Warn(object message, object classObj, [System.Runtime.CompilerServices.CallerMemberName] string methodName = "")
        {
            string log = MakeLog(message, classObj, methodName);

            UnityEngine.Debug.LogWarning(log);

            GamebaseInternalReport.Instance.SendWarnLog(
                new System.Collections.Generic.Dictionary<string, string>
                {
                    {GAMEBASE_LOG, log},
                });
        }

        /// <summary>
        /// 게임 흐름에 치명적인 영향이 있는 에러
        /// </summary>
        public static void Error(object message, object classObj, [System.Runtime.CompilerServices.CallerMemberName] string methodName = "")
        {
            string log = MakeLog(message, classObj, methodName);

            UnityEngine.Debug.LogError(MakeLog(message, classObj, methodName));

            GamebaseInternalReport.Instance.SendErrorLog(
                new System.Collections.Generic.Dictionary<string, string>
                {
                    {GAMEBASE_LOG, log},
                });
        }
    }
}
