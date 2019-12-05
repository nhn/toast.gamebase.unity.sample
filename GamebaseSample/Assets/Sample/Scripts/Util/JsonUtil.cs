using LitJson;
using System;
using System.IO;

namespace GamebaseSample
{
    public static class JsonUtil
    {
        private const bool PRETTY_PRINT = true;
        private const int INDENT = 4;

        public static void InitializeLitJson()
        {
            JsonMapper.RegisterExporter<float>((obj, writer) => writer.Write(Convert.ToDouble(obj)));
            JsonMapper.RegisterImporter<float, int>((float input) => { return (int)input; });
            JsonMapper.RegisterImporter<float, long>((float input) => { return (long)input; });
            JsonMapper.RegisterImporter<int, long>((int input) => { return (long)input; });
            JsonMapper.RegisterImporter<int, double>((int input) => { return (double)input; });
            JsonMapper.RegisterImporter<int, float>((int input) => { return (float)input; });
            JsonMapper.RegisterImporter<double, int>((double input) => { return (int)input; });
            JsonMapper.RegisterImporter<double, long>((double input) => { return (long)input; });
            JsonMapper.RegisterImporter<double, float>(input => Convert.ToSingle(input));
        }

        public static string ToPrettyJsonString(object obj)
        {
            if (obj == null)
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