using System;
using System.Collections.Generic;

namespace GamePlatform.Logger.Internal
{
    public class GpLoggerSendQueue
    {
        private const int MAX_QUEUE_SIZE = 2048;

        private readonly LogItemContainer itemContainer;
        private readonly Queue<LogItemContainer> queue;
        private readonly GpLoggerFilter loggerFilter;
        private readonly string appKey;

        public int Count { get { return queue.Count; } }

        public GpLoggerSendQueue(string appKey, GpLoggerResponse.LogNCrashSettings settings = null)
        {
            this.appKey = appKey;

            itemContainer = new LogItemContainer();
            queue = new Queue<LogItemContainer>();

            // LogNCrash Setting is empty when called from PlatformSdk API.
            if (settings == null)
            {
                return;
            }

            loggerFilter = new GpLoggerFilter(settings);
        }

        public void AddLogItem(BaseLogItem item)
        {
            if (loggerFilter == null)
            {
                itemContainer.Add(item);
            }
            else
            {
                if (loggerFilter.CheckFilters(item) == true)
                {
                    itemContainer.Add(item);
                }
            }
        }

        public bool EnqueueInFile()
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            string firstFile = BackupLogManager.GetFirstFile(appKey);

            if (string.IsNullOrEmpty(firstFile) == true)
            {
                return false;
            }

            var container = new LogItemContainer();
            string fileName = firstFile.Substring(firstFile.LastIndexOf("\\", StringComparison.OrdinalIgnoreCase) + 1);

            if (firstFile.LastIndexOf("\\", StringComparison.OrdinalIgnoreCase) == -1)
            {
                fileName = firstFile.Substring(firstFile.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1);
            }

            string[] subString = fileName.Split('_');
            long createTime;

            if (long.TryParse(subString[0], out createTime) == true)
            {
                container.CreateTime = createTime;
                container.TransactionId = subString[1];

                string strLogContents = BackupLogManager.LoadFile(appKey, container.CreateTime, container.TransactionId);
                BackupLogManager.DeleteFile(appKey, container.CreateTime, container.TransactionId);
                if (string.IsNullOrEmpty(strLogContents) == false)
                {
                    container.LogContents = strLogContents;
                    queue.Enqueue(container);
                    return true;
                }
            }
#endif
            return false;
        }

        public bool Enqueue()
        {
            if (itemContainer.Count == 0)
            {
                return false;
            }

            var container = new LogItemContainer(itemContainer.GetItemList());
            itemContainer.RemoveAll();

            container.CreateTime = GpUtil.GetEpochMilliSeconds();
            container.TransactionId = Guid.NewGuid().ToString().Replace("-", "");
            queue.Enqueue(container);

            return true;
        }

        public LogItemContainer Dequeue()
        {
            var container = queue.Dequeue();

            if (string.IsNullOrEmpty(container.LogContents) == true)
            {
                container.LogContents = container.GetItemListAsJsonString();
            }

            return container;
        }
    }
}