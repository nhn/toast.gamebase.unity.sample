using System.Collections.Generic;

namespace Toast.Gamebase.Internal
{
    public class GamebaseExtraDataHandler
    {
        private static Dictionary<int, string> extraDataDic = new Dictionary<int, string>();

        public static void RegisterExtraData(int key, string value)
        {
            if (!extraDataDic.ContainsKey(key))
            {
                extraDataDic.Add(key, value);
            }
        }

        public static string GetExtraData(int handle)
        {
            if (extraDataDic.ContainsKey(handle))
            {
                return extraDataDic[handle];
            }

            return null;
        }

        public static void UnregisterExtraData(int handle)
        {
            if (extraDataDic.ContainsKey(handle))
            {
                extraDataDic.Remove(handle);
            }
        }
    }
}