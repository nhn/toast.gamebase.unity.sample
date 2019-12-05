using System.Collections.Generic;
using System.Linq;

namespace Toast.Internal
{
    public static class JSONObjectExtension
    {
        public static Dictionary<string, object> ToDictionary(this JSONObject json)
        {
            return json.Linq.Select(kv => new KeyValuePair<string, object>(kv.Key, ToObject(kv.Value)))
                .ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        public static object ToObject(JSONNode json)
        {
            if (json.IsBoolean)
            {
                return json.AsBool;
            }

            if (json.IsNumber)
            {
                return json.AsDouble;
            }

            if (json.IsObject)
            {
                return ToDictionary(json.AsObject);
            }

            if (json.IsArray)
            {
                var jsonArray = json.AsArray;
                var array = new object[jsonArray.Count];
                for (int i = 0; i < jsonArray.Count; i++)
                {
                    array[i] = ToObject(jsonArray[i]);
                }

                return array;
            }

            return json.IsNull ? null : json.Value;
        }
    }
}