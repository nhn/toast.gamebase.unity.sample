namespace Toast.Logger
{
    using Toast.Core;

    public class ToastLoggerCrashData : ToastLoggerLogObject
    {
        public void SetCrashData(string projectKey, string logType, ToastLogLevel logLevel, string logMessage, string transactionId = "")
        {
            SetLogObject(projectKey, logType, logLevel, logMessage, transactionId);
        }

        public void SetCrashStyle(string crashStyle)
        {
            Put(LogFields.CRASH_STYLE, crashStyle);
        }

        public void SetCrashSymbol(string crashSymbol)
        {
            Put(LogFields.CRASH_SYMBOL, crashSymbol);
        }

        public void SetCrashDump(string dumpData)
        {
            Put(LogFields.CRASH_DUMP_DATA, dumpData);
        }

        public void SetSessionId(string sessionId)
        {
            Put(LogFields.SESSION_ID, sessionId);
        }
    }
}