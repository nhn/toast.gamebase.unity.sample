using System;
using UnityEngine;

namespace Toast.Cef.Webview.Internal.Util
{
    public static class Assert
    {
        public enum LogLevel
        {
            WARN,
            ERROR
        }

        public static T CheckNotNull<T>(T value, LogLevel logLevel)
        {
            if (IsNull(value) == true)
            {
                if (logLevel == LogLevel.WARN)
                {
                    Debug.LogWarning(new ArgumentNullException().Message);
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }

            return value;
        }

        public static T CheckNotNull<T>(T value, string paramName, LogLevel logLevel)
        {
            if (IsNull(value) == true)
            {
                if (logLevel == LogLevel.WARN)
                {
                    Debug.LogWarning(new ArgumentNullException(paramName).Message);
                }
                else
                {
                    throw new ArgumentNullException(paramName);
                }
            }

            return value;
        }

        private static bool IsNull<T>(T value)
        {
            var type = typeof(T);

            if (type == typeof(string))
            {
                if (string.IsNullOrEmpty(Convert.ToString(value).Trim()) == true)
                {
                    return true;
                }
            }
            else
            {
                if (value == null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}