using System.IO;
using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal
{
    public static class GamebaseJsonUtil
    {
        private const bool  PRETTY_PRINT    = true;
        private const int   INDENT          = 4;

        public static string ToPrettyJsonString(object obj)
        {
            if(obj == null)
            {
                return "obj is null";
            }

            TextWriter stringWriter = new StringWriter();

            JsonWriter writer = new JsonWriter(stringWriter);
            writer.PrettyPrint = PRETTY_PRINT;
            writer.IndentValue = INDENT;

            JsonMapper.ToJson(obj, writer);

            string returnString = stringWriter.ToString();

            stringWriter.Close();

            return returnString;
        }

        public static string ToPrettyJsonString(string jsonString)
        {
            if (string.IsNullOrEmpty(jsonString) == true)
            {
                return "jsonString is null or empty";
            }
            return ToPrettyJsonString(JsonMapper.ToObject(jsonString));
        }
    }
}