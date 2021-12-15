using System;
using System.Collections.Generic;

namespace NhnCloud.GamebaseTools.SettingTool.Data
{
    public static class DataManager
    {
        private static Dictionary<string, object> data;

        public static void Initialize()
        {
            data = new Dictionary<string, object>();
        }

        public static T GetData<T>(string key)
        {
            if (data == null)
            {
                return default(T);
            }

            if (data.ContainsKey(key) == true)
            {
                return (T)Convert.ChangeType(data[key], typeof(T));
            }

            return default(T);
        }

        public static void SetData(string key, object obj)
        {
            if (data == null)
            {
                return;
            }

            if (data.ContainsKey(key) == true)
            {
                data[key] = obj;
                return;
            }

            data.Add(key, obj);
        }

        public static void Destroy()
        {
            if (data != null)
            {
                data.Clear();
                data = null;
            }
        }
    }
}