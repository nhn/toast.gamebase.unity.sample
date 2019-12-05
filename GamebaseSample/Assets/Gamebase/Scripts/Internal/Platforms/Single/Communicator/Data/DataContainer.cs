#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)
using System;
using System.Collections.Generic;

namespace Toast.Gamebase.Internal.Single.Communicator
{
    public static class DataContainer
    {
        private static Dictionary<string, BaseVO> VODictionary = new Dictionary<string, BaseVO>();

        public static T GetData<T>(string key)
        {
            if (VODictionary.ContainsKey(key))
                return (T)Convert.ChangeType(VODictionary[key], typeof(T));

            return default(T);
        }

        public static void SetData(string key, BaseVO vo)
        {
            if (VODictionary.ContainsKey(key))
            {
                VODictionary[key] = vo;
                return;
            }

            VODictionary.Add(key, vo);
        }

        public static void RemoveData(string key)
        {
            if (VODictionary.ContainsKey(key))
                VODictionary.Remove(key);
        }
    }
}
#endif