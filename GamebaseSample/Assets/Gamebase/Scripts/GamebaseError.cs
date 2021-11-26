using System;
using System.Collections.Generic;
using System.Reflection;
using Toast.Gamebase.Internal;
using Toast.Gamebase.LitJson;

namespace Toast.Gamebase
{    
    public class GamebaseError
    {
        public string domain = string.Empty;
        public int code;
        public string message = string.Empty;
        public string transactionId = string.Empty;
        public Dictionary<string, string> extras = new Dictionary<string, string>();
        public GamebaseError error;

        public GamebaseError()
        {
        }

        public GamebaseError(int code, string domain = null, string message = null, GamebaseError error = null, string transactionId = null)
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

            this.transactionId = transactionId;
            this.error = error;
        }

        public override string ToString()
        {
            return JsonMapper.ToJson(this);
        }

        private string RetrieveErrorMessage()
        {
            string errorName = string.Empty;
            FieldInfo[] fields = typeof(GamebaseErrorCode).GetFields();

            var fieldIndex = Array.FindIndex(fields, SearchFieldIndex);
            errorName = fields[fieldIndex].Name;

            if (string.IsNullOrEmpty(errorName) == true)
            {
                GamebaseLog.Debug(string.Format("Not found error message for errorCode {0}", code), this);
                return string.Empty;
            }

            FieldInfo field = typeof(GamebaseStrings).GetField(errorName);
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