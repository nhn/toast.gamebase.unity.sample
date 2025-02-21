namespace GamePlatform.Logger.Internal
{
    public class SessionLogItem : BaseLogItem
    {
        public void SetSessionId(string sessionId)
        {
            Add(LogFields.SESSION_ID, sessionId);
        }
    }
}