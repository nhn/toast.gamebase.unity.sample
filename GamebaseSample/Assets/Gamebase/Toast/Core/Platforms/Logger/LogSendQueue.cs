using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Toast.Internal;

namespace Toast.Core
{
    public class LogSendQueue : MonoBehaviour
    {
        private const int MAX_QUEUE_SIZE = 2048;

        private LogBulkData _bulkLog = new LogBulkData();
        private Queue<LogBulkData> _queueBulkLog = new Queue<LogBulkData>();
        private LogFilter loggerFilter = new LogFilter();

        public int Count { get { return _queueBulkLog.Count; } }

        private static LogSendQueue _instance;
        public static LogSendQueue Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType(typeof(LogSendQueue)) as LogSendQueue;
                    if (!_instance)
                    {
                        var container = GameObject.Find(Constants.LogSendQueueObjectName);
                        if (container == null)
                        {
                            container = new GameObject(Constants.LogSendQueueObjectName);
                        }

                        _instance = container.AddComponent<LogSendQueue>();
                        DontDestroyOnLoad(_instance);
                    }
                }

                return _instance;
            }
        }

        public void AddToastLoggerLogObject(LogObject logObject)
        {
            if (loggerFilter.CheckFilters(logObject) == false)
            {
                _bulkLog.Add(logObject);
            }
        }

        public bool EnqueueInFile()
        {
#if UNITY_STANDALONE || UNITY_EDITOR 
            //string firstFile = ToastFileManager.GetFirstFile(ToastLoggerCommonLogic.AppKey);

            //if (!string.IsNullOrEmpty(firstFile))
            //{
            //    LogBulkData bulkLog = new LogBulkData();

            //    string[] subString = firstFile.Split('_');
            //    bulkLog.CreateTime = long.Parse(subString[0]);
            //    bulkLog.TransactionId = subString[1];

            //    string strLogContents = ToastFileManager.FileLoad(ToastLoggerCommonLogic.AppKey, bulkLog.CreateTime, bulkLog.TransactionId);
            //    bulkLog.LogContents = strLogContents;
            //    _queueBulkLog.Enqueue(bulkLog);

            //    return true;
            //}
#endif

            return false;
        }

        public bool Enqueue()
        {
            if (_bulkLog.Count == 0)
            {
                return false;
            }

            LogBulkData bulkLog = new LogBulkData(_bulkLog);
            _bulkLog.RemoveAll();

            bulkLog.CreateTime = ToastUtil.GetEpochMilliSeconds();
            bulkLog.TransactionId = Guid.NewGuid().ToString().Replace("-", "");
            _queueBulkLog.Enqueue(bulkLog);

            return true;
        }

        public LogBulkData Dequeue()
        {
            LogBulkData bulkLog = _queueBulkLog.Dequeue();

            if (string.IsNullOrEmpty(bulkLog.LogContents))
            {
                bulkLog.LogContents = bulkLog.GetString();
            }

            return bulkLog;
        }
    }
}