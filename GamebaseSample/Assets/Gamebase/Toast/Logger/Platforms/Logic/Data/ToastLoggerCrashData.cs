using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toast.Logger
{
    public class ToastLoggerCrashData : ToastLoggerLogObject
    {
        public void SetCrashData(string projectKey, string logType, ToastLogLevel logLevel, string logMessage, string transactionId = "")
        {
            SetLogObject(projectKey, logType, logLevel, logMessage, transactionId);
        }
        
        public void SetCrashStyle(string crashStyle)
        {
            Put(ToastLoggerFields.CRASH_STYLE, crashStyle);
        }

        public void SetCrashSymbol(string crashSymbol)
        {
            Put(ToastLoggerFields.CRASH_SYMBOL, crashSymbol);
        }

        public void SetCrashDump(string dumpData)
        {
            Put(ToastLoggerFields.CRASH_DUMP_DATA, dumpData);
        }

        public void SetSessionId(string sessionId)
        {
            Put(ToastLoggerFields.SESSION_ID, sessionId);
        }
    }
}