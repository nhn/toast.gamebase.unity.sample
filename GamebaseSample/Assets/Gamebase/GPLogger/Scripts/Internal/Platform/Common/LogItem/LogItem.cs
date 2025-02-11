using GamePlatform.Logger.ThirdParty;

namespace GamePlatform.Logger.Internal
{
    public class LogItem : BaseLogItem
    {
        public int GetBulkIndex()
        {
            return int.Parse(Get(LogFields.LOG_BULK_INDEX));
        }

        public void SetBulkIndex(int index)
        {
            Add(LogFields.LOG_BULK_INDEX, index.ToString());
        }

        public void SetSendTime(long sendTime)
        {
            Add(LogFields.LOG_SEND_TIME, sendTime.ToString());
        }

        public string GetLogDataString()
        {
            return JsonMapper.ToJson(GetLogDictionary());
        }
    }
}