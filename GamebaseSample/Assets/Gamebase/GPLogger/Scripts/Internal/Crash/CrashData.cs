using System;
using System.Collections.Generic;
using System.Text;

namespace GamePlatform.Logger.Internal
{
    public class CrashData
    {
        public string logType;
        public string logLevel;
        public string message;
        public Dictionary<string, string> userFields;

        public string DmpData { get; private set; }

        public void SetDmpData(string logString, string stackTrace)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0}\n{1}", logString, stackTrace);

            var bytesToEncode = Encoding.UTF8.GetBytes(sb.ToString());
            var encodedText = Convert.ToBase64String(bytesToEncode);

            DmpData = encodedText;
        }
    }
}