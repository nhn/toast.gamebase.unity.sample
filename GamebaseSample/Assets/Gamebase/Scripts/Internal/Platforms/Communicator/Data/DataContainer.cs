using System;
using System.Collections.Generic;

namespace Toast.Gamebase.Internal
{
    public static class DataContainer
    {
        private static Dictionary<string, object> dataDictionary = new Dictionary<string, object>();

        public static T GetData<T>(string key)
        {
            if (dataDictionary.TryGetValue(key, out object value))
            {
                if (value is T castingValue)
                {
                    return castingValue;
                }
            }

            return default(T);
        }

        public static void SetData(string key, object vo)
        {
            if (dataDictionary.ContainsKey(key))
            {
                dataDictionary[key] = vo;
                return;
            }

            dataDictionary.Add(key, vo);
        }

        public static void RemoveData(string key)
        {
            if (dataDictionary.ContainsKey(key))
                dataDictionary.Remove(key);
        }
    }
}