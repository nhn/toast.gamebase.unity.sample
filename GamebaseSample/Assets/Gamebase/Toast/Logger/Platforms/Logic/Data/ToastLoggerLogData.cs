using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Toast.Internal;

namespace Toast.Logger
{
    public class ToastLoggerLogData : ToastLoggerLogObject
    {
        public void SetLogData(string projectKey, string logType, ToastLogLevel logLevel, string logMessage, string transactionId = "")
        {
            SetLogObject(projectKey, logType, logLevel, logMessage, transactionId);
        }

        private void SetCollectionData(Dictionary<string, string> data)
        {
            PutAll(data);
        }

        public int GetBulkIndex()
        {
            return int.Parse(Get(ToastLoggerFields.LOG_BULK_INDEX));
        }

        public void SetBulkIndex(int index)
        {
            Put(ToastLoggerFields.LOG_BULK_INDEX, index.ToString());
        }

        public void SetSendTime(long sendTime)
        {
            Put(ToastLoggerFields.LOG_SEND_TIME, sendTime.ToString());
        }

        public long Size()
        {
            return Gets().ToString().Length;
        }

        public string GetLogDataString()
        {
            JSONArray array = new JSONArray();
            array.Add(GetJSONNode());
            return array.ToString();
        }
    }
}