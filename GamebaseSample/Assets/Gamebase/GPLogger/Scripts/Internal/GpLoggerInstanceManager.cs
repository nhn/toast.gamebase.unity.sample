using System.Collections.Generic;

namespace GamePlatform.Logger.Internal
{
    public static class GpLoggerInstanceManager
    {
        private static Dictionary<string, ILoggerApi> loggerInstanceDict = new Dictionary<string, ILoggerApi>();

        public static ILoggerApi CreateLoggerInstance(string appkey, bool isUserAccess)
        {
            if (loggerInstanceDict.ContainsKey(appkey) == true)
            {
                return GetLoggerInstance(appkey);
            }

            ILoggerApi api = null;

            if (isUserAccess == true)
            {
                api = new UserApi();
            }
            else
            {
                api = new PlatformSdkApi();
            }

            loggerInstanceDict.Add(appkey, api);

            return api;
        }

        public static ILoggerApi GetLoggerInstance(string appkey)
        {
            if (loggerInstanceDict.ContainsKey(appkey) == true)
            {
                return loggerInstanceDict[appkey];
            }

            GpLog.Debug(string.Format("The logger object for the appkey[{0}] does not exist. (Creates a logger object.)", appkey), typeof(GpLoggerInstanceManager));
            return null;
        }

        public static void Destroy()
        {
            loggerInstanceDict.Clear();
        }
    }
}