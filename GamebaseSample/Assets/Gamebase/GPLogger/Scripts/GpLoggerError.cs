using GamePlatform.Logger.Internal;
using GamePlatform.Logger.ThirdParty;
using System;
using System.Reflection;

namespace GamePlatform.Logger
{
    public class GpLoggerError
    {
        public string domain = string.Empty;
        public int code;
        public string message = string.Empty;

        public GpLoggerError error;

        public GpLoggerError(int code, string domain, string message = null, GpLoggerError error = null)
        {
            this.code = code;
            this.domain = domain;

            if (string.IsNullOrEmpty(message) == true)
            {
                this.message = RetrieveErrorMessage();
            }
            else
            {
                this.message = message;
            }

            this.error = error;
        }

        public override string ToString()
        {
            return JsonMapper.ToJson(this);
        }

        private string RetrieveErrorMessage()
        {
            string errorName = string.Empty;
            FieldInfo[] fields = typeof(GpLoggerErrorCode).GetFields();
            var fieldIndex = Array.FindIndex(fields, SearchFieldIndex);

            errorName = fields[fieldIndex].Name;

            if (string.IsNullOrEmpty(errorName) == true)
            {
                GpLog.Debug(string.Format("Not found error message for errorCode {0}", code), GetType(), "RetrieveErrorMessage");
                return string.Empty;
            }

            FieldInfo field = typeof(GpLoggerStrings).GetField(errorName);
            if (field == null)
            {
                return string.Empty;
            }

            return field.GetValue(null).ToString();
        }

        private bool SearchFieldIndex(FieldInfo field)
        {
            return (int)field.GetValue(null) == code;
        }
    }
}