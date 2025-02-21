namespace GamePlatform.Logger.Internal
{
    public class CrashLogItem : BaseLogItem
    {
        public void SetCrashStyle(string crashStyle)
        {
            Add(LogFields.CRASH_STYLE, crashStyle);
        }

        public void SetCrashSymbol(string crashSymbol)
        {
            Add(LogFields.CRASH_SYMBOL, crashSymbol);
        }

        public void SetCrashDump(string dumpData)
        {
            Add(LogFields.CRASH_DUMP_DATA, dumpData);
        }

        public void SetSessionId(string sessionId)
        {
            Add(LogFields.SESSION_ID, sessionId);
        }
    }
}