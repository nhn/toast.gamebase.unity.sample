using System.Text.RegularExpressions;
using UnityEngine;

namespace Toast.Iap.Ongate
{
    public class JsonUtilityHelper : ScriptableObject
    {

        //Usage:
        //YouObject[] objects = JsonUtilityHelper.getJsonArray<YouObject> (jsonString);
        public static T[] GetJsonArray<T>(string json)
        {
            string newJson = "{ \"array\": " + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.array;
        }

        //Usage:
        //string jsonString = JsonUtilityHelper.arrayToJson<YouObject>(objects);
        public static string ArrayToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>()
            {
                array = array
            };

            string pattern = Regex.Escape("[") + "(.*)]";
            string input = JsonUtility.ToJson(wrapper);
            if (Regex.IsMatch(input, pattern))
            {
                return Regex.Match(input, pattern).Value;
            }

            return null;
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] array;
        }
    }
}
