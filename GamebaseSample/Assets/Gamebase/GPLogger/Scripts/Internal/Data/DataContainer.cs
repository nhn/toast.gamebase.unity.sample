using System;
using System.Collections.Generic;

namespace GamePlatform.Logger.Internal
{
    public static class DataContainer
    {
        private static Dictionary<string, Data> datas;

        public static T GetData<T>(string appkey, string key)
        {
            if (datas == null)
            {
                return default(T);
            }

            Data data = null;
            if (datas.TryGetValue(appkey, out data) == false)
            {
                return default(T);
            }

            return data.GetData<T>(key);
        }

        public static void SetData(string appkey, string key, object obj)
        {
            if (datas == null)
            {
                datas = new Dictionary<string, Data>();
            }

            Data data = null;
            if (datas.TryGetValue(appkey, out data) == false)
            {
                data = new Data();
                datas.Add(appkey, data);
            }

            data.SetData(key, obj);
        }

        public static void Destroy(string productName)
        {
            if (datas == null)
            {
                return;
            }

            Data data = null;
            if (datas.TryGetValue(productName, out data) == false)
            {
                return;
            }

            data.Destroy();
        }
    }

    public class Data
    {
        private Dictionary<string, object> data;

        public T GetData<T>(string key)
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

        public void SetData(string key, object obj)
        {
            if (data == null)
            {
                data = new Dictionary<string, object>();
            }

            if (data.ContainsKey(key) == true)
            {
                data[key] = obj;
                return;
            }

            data.Add(key, obj);
        }

        public void Destroy()
        {
            if (data != null)
            {
                data.Clear();
                data = null;
            }
        }
    }
}