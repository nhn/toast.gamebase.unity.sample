using System;
using System.Xml.Serialization;
using System.IO;
using System.Text;

namespace NhnCloud.GamebaseTools.SettingTool.Util
{
    public static class XMLManager
    {
        public enum ResponseCode
        {
            SUCCESS,
            FILE_NOT_FOUND_ERROR,
            PATH_IS_NULL_ERROR,
            DATA_IS_NULL_ERROR,
            UNKNOWN_ERROR,
        }

        public static void SaveXMLToFile<T>(string path, T data, Action<ResponseCode, string> callback)
        {
            if (true == string.IsNullOrEmpty(path))
            {
                callback(ResponseCode.PATH_IS_NULL_ERROR, path);
                return;
            }

            if (null == data)
            {
                callback(ResponseCode.DATA_IS_NULL_ERROR, null);
                return;
            }

            try
            {
                var serializer = new XmlSerializer(typeof(T));
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    serializer.Serialize(new StreamWriter(stream, Encoding.UTF8), data);
                    callback(ResponseCode.SUCCESS, null);
                    return;
                }
            }
            catch (Exception e)
            {
                callback(ResponseCode.UNKNOWN_ERROR, e.Message);
            }
        }

        public static void LoadXMLFromFile<T>(string path, Action<ResponseCode, T, string> callback)
        {
            if (true == string.IsNullOrEmpty(path))
            {
                callback(ResponseCode.FILE_NOT_FOUND_ERROR, default(T), path);
                return;
            }

            try
            {
                var serializer = new XmlSerializer(typeof(T));
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    callback(ResponseCode.SUCCESS, (T)serializer.Deserialize(stream), null);
                }
            }
            catch (Exception e)
            {
                callback(ResponseCode.UNKNOWN_ERROR, default(T), e.Message);
            }
        }


        public static void LoadXMLFromText<T>(string text, Action<ResponseCode, T, string> callback)
        {
            if (true == string.IsNullOrEmpty(text))
            {
                callback(ResponseCode.DATA_IS_NULL_ERROR, default(T), null);
                return;
            }

            try
            {
                var serializer = new XmlSerializer(typeof(T));
                using (TextReader textReader = new StringReader(text))
                {
                    callback(ResponseCode.SUCCESS, (T)serializer.Deserialize(textReader), null);
                }
            }
            catch (Exception e)
            {
                callback(ResponseCode.UNKNOWN_ERROR, default(T), e.Message);
            }
        }
    }
}