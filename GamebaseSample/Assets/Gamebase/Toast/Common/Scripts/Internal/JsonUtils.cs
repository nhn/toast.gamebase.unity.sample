namespace Toast.Internal
{
    public static class JsonUtils
    {
        /// <summary>
        /// Parse string to json object. If raise exception when parsing, return null.
        /// </summary>
        public static JSONNode SafeParse(string json)
        {
            try
            {
                return JSON.Parse(json);
            }
            catch
            {
                return null;
            }
        }

        public static bool TrySelectJsonObject(string jsonString, out JSONNode outNode, params string[] paths)
        {
            var jsonNode = JSONNode.Parse(jsonString);
            return TrySelectJsonObject(jsonNode, out outNode, paths);
        }

        public static bool TrySelectJsonObject(JSONNode jsonNode, out JSONNode outNode, params string[] paths)
        {
            outNode = null;

            if (!jsonNode.IsObject)
            {
                ToastLog.Error("Json is not object type : {0}", jsonNode);
                return false;
            }

            var jsonObject = jsonNode.AsObject;
            var cursor = jsonObject;
            foreach (var path in paths)
            {
                if (!cursor.ContainsKey(path))
                {
                    ToastLog.Error("Json doesn't contain ({0}) path", path);
                    return false;
                }

                var node = cursor[path];
                if (!node.IsObject)
                {
                    return false;
                }

                cursor = node.AsObject;
            }

            outNode = cursor;
            return outNode != null;
        }

        public static bool TrySelectJsonArray(JSONNode jsonNode, out JSONArray outArray, params string[] paths)
        {
            outArray = null;

            if (!jsonNode.IsObject)
            {
                ToastLog.Error("Json is not object type : {0}", jsonNode);
                return false;
            }

            var jsonObject = jsonNode.AsObject;
            var cursor = jsonObject;
            foreach (var path in paths)
            {
                if (!cursor.ContainsKey(path))
                {
                    ToastLog.Error("Json doesn't contain ({0}) path", path);
                    return false;
                }

                var node = cursor[path];
                if (!node.IsArray)
                {
                    return false;
                }

                outArray = node.AsArray;
            }

            return outArray != null;
        }
    }
}