using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toast.Logger
{
    public class ToastLoggerSessionData : ToastLoggerLogObject
    {
        public void SetLogData(string projectKey, string logType, ToastLogLevel logLevel, string logMessage, string transactionId = "")
        {
            SetLogObject(projectKey, logType, logLevel, logMessage, transactionId);
        }

        public void SetSessionId(string sessionId)
        {
            Put(ToastLoggerFields.SESSION_ID, sessionId);
        }
    }
}