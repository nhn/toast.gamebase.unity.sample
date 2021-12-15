namespace Toast.Logger
{
    using Toast.Core;

    public class ToastLoggerSessionData : ToastLoggerLogObject
    {
        public void SetLogData(string projectKey, string logType, ToastLogLevel logLevel, string logMessage, string transactionId = "")
        {
            SetLogObject(projectKey, logType, logLevel, logMessage, transactionId);
        }

        public void SetSessionId(string sessionId)
        {
            Put(LogFields.SESSION_ID, sessionId);
        }
    }
}