using GamePlatform.Logger.ThirdParty;
using System.Collections.Generic;

namespace GamePlatform.Logger.Internal
{
    public class LogItemContainer
    {
        private List<BaseLogItem> itemList;

        public string LogContents { get; set; }
        public long CreateTime { get; set; }
        public string TransactionId { get; set; }

        public LogItemContainer(List<BaseLogItem> list = null)
        {
            if (list == null || list.Count == 0)
            {
                itemList = new List<BaseLogItem>();
            }
            else
            {
                itemList = new List<BaseLogItem>(list);
            }
        }

        public int Count
        {
            get { return itemList.Count; }
        }
        
        public void Add(BaseLogItem item)
        {
            itemList.Add(item);
        }

        public List<BaseLogItem> GetItemList()
        {
            return itemList;
        }

        public string GetItemListAsJsonString()
        {
            var list = new List<Dictionary<string, string>>();

            foreach (var item in itemList)
            {
                list.Add(item.GetLogDictionary());
            }

            return JsonMapper.ToJson(list);
        }

        public void RemoveAll()
        {
            itemList.Clear();
        }
    }
}