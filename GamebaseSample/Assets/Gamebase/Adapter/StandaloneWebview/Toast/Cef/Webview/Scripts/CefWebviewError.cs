using System;
using System.Reflection;
using Toast.Cef.Webview.Internal;
using UnityEngine;

namespace Toast.Cef.Webview
{
    public class CefWebviewError
    {
        public int code;
        public string message = string.Empty;
        public CefWebviewError error;

        public CefWebviewError()
        {
        }

        public CefWebviewError(int code, string message = null, CefWebviewError error = null)
        {
            this.code = code;

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
            return JsonUtility.ToJson(this);
        }

        private string RetrieveErrorMessage()
        {
            string errorName = string.Empty;
            FieldInfo[] fields = typeof(CefWebviewErrorCode).GetFields();

            var fieldIndex = Array.FindIndex(fields, SearchFieldIndex);
            errorName = fields[fieldIndex].Name;

            if (string.IsNullOrEmpty(errorName) == true)
            {
                CefWebviewLogger.Debug(string.Format("Not found error message for errorCode {0}", code), GetType());
                return string.Empty;
            }

            FieldInfo field = typeof(CefWebviewStrings).GetField(errorName);
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