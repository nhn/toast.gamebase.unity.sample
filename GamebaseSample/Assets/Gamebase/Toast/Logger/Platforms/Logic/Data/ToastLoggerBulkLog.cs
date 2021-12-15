using System.Collections.Generic;
using Toast.Internal;

namespace Toast.Logger
{
    public class ToastLoggerBulkLog
    {
        private List<ToastLoggerLogObject> _logDatas;

        public string LogContents { get; set; }
        public long CreateTime { get; set; }
        public string TransactionId { get; set; }

        public ToastLoggerBulkLog()
        {
            _logDatas = new List<ToastLoggerLogObject>();
        }

        public ToastLoggerBulkLog(ToastLoggerBulkLog bulkLog)
        {
            _logDatas = new List<ToastLoggerLogObject>(bulkLog.Gets());
        }

        public int Count
        {
            get { return _logDatas.Count; }
        }

        public void Add(List<ToastLoggerLogObject> logDatas)
        {
            foreach (ToastLoggerLogObject logData in logDatas)
            {
                Add(logData);
            }
        }

        public void Add(ToastLoggerLogObject logData)
        {
            _logDatas.Add(logData);
        }

        public List<ToastLoggerLogObject> Gets()
        {
            return _logDatas;
        }

        public string GetString()
        {
            JSONArray array = new JSONArray();
            foreach (ToastLoggerLogObject logData in _logDatas)
            {
                array.Add(logData.GetJSONNode());
            }
            return array.ToString();
        }

        public void RemoveAll()
        {
            _logDatas.Clear();
        }
    }
}