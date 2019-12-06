using System.Collections.Generic;

namespace Toast.Gamebase.Internal
{
    public class GamebaseCallbackHandler
    {
        private static int handle = 0;
        private static Dictionary<int, object> callbackDic = new Dictionary<int, object>();

        public static int RegisterCallback(object callback)
        {
            if (callback == null)
                return -1;

            callbackDic.Add(handle, callback);
            return handle++;
        }

        public static T GetCallback<T>(int handle) where T : class
        {
            if (callbackDic.ContainsKey(handle))
                return (T)callbackDic[handle];

            return default(T);
        }

        public static void UnregisterCallback(int handle)
        {
            if (callbackDic.ContainsKey(handle))
                callbackDic.Remove(handle);
        }
    }
}