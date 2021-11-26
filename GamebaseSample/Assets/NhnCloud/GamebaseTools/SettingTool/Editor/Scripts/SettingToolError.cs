using NhnCloud.GamebaseTools.SettingTool.ThirdParty;
using System;
using System.Reflection;

namespace NhnCloud.GamebaseTools.SettingTool
{
    [Serializable]
    public class SettingToolError
    {
        public string domain = string.Empty;
        public int code;
        public string message = string.Empty;

        public SettingToolError error;
        
        public SettingToolError(int code, string domain, string message = null, SettingToolError error = null)
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
            FieldInfo[] fields = typeof(SettingToolErrorCode).GetFields();
            var fieldIndex = Array.FindIndex(fields, SearchFieldIndex);

            errorName = fields[fieldIndex].Name;

            if (string.IsNullOrEmpty(errorName) == true)
            {
                SettingToolLog.Debug(string.Format("Not found error message for errorCode {0}", code), GetType(), "RetrieveErrorMessage");
                return string.Empty;
            }

            FieldInfo field = typeof(SettingToolStrings).GetField(errorName);
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