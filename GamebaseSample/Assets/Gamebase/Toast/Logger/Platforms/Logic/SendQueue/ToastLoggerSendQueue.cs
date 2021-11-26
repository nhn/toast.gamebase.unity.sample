using System;
using System.Collections.Generic;
using Toast.Internal;
using UnityEngine;

namespace Toast.Logger
{
    public class ToastLoggerSendQueue : MonoBehaviour
    {
        private const int MAX_QUEUE_SIZE = 2048;

        private ToastLoggerBulkLog _bulkLog = new ToastLoggerBulkLog();
        private Queue<ToastLoggerBulkLog> _queueBulkLog = new Queue<ToastLoggerBulkLog>();
        private ToastLoggerFilter loggerFilter = new ToastLoggerFilter();

        public int Count { get { return _queueBulkLog.Count; } }

        private static ToastLoggerSendQueue _instance;
        public static ToastLoggerSendQueue Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType(typeof(ToastLoggerSendQueue)) as ToastLoggerSendQueue;
                    if (!_instance)
                    {
                        var container = GameObject.Find(Constants.ToastLoggerSendQueueObjectName);
                        if (container == null)
                        {
                            container = new GameObject(Constants.ToastLoggerSendQueueObjectName);
                        }

                        _instance = container.AddComponent<ToastLoggerSendQueue>();
                        DontDestroyOnLoad(_instance);
                    }
                }

                return _instance;
            }
        }

        public void AddToastLoggerLogObject(ToastLoggerLogObject logObject)
        {
            if (loggerFilter.CheckFilters(logObject) == true)
            {
                _bulkLog.Add(logObject);
            }
        }

        public bool EnqueueInFile()
        {
#if UNITY_STANDALONE || UNITY_EDITOR 
            string firstFile = BackupLogManager.GetFirstFile(ToastLoggerCommonLogic.AppKey);

            if (!string.IsNullOrEmpty(firstFile))
            {
                ToastLoggerBulkLog bulkLog = new ToastLoggerBulkLog();

                string fileName = firstFile.Substring(firstFile.LastIndexOf("\\", StringComparison.OrdinalIgnoreCase) + 1);
                
                if (firstFile.LastIndexOf("\\", StringComparison.OrdinalIgnoreCase) == -1)
                {
                    fileName = firstFile.Substring(firstFile.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1);
                }

                string[] subString = fileName.Split('_');
                long createTime;
                if (long.TryParse(subString[0], out createTime))
                {
                    bulkLog.CreateTime = createTime;
                    bulkLog.TransactionId = subString[1];

                    string strLogContents = BackupLogManager.FileLoad(ToastLoggerCommonLogic.AppKey, bulkLog.CreateTime, bulkLog.TransactionId);
                    BackupLogManager.FileDelete(ToastLoggerCommonLogic.AppKey, bulkLog.CreateTime, bulkLog.TransactionId);
                    if (!string.IsNullOrEmpty(strLogContents))
                    {
                        bulkLog.LogContents = strLogContents;
                        _queueBulkLog.Enqueue(bulkLog);
                        return true;
                    }
                    return false;
                }

                return false;
            }
#endif

            return false;
        }

        public bool Enqueue()
        {
            if (_bulkLog.Count == 0)
            {
                return false;
            }

            ToastLoggerBulkLog bulkLog = new ToastLoggerBulkLog(_bulkLog);
            _bulkLog.RemoveAll();

            bulkLog.CreateTime = ToastUtil.GetEpochMilliSeconds();
            bulkLog.TransactionId = Guid.NewGuid().ToString().Replace("-", "");
            _queueBulkLog.Enqueue(bulkLog);

            return true;
        }

        public ToastLoggerBulkLog Dequeue()
        {
            ToastLoggerBulkLog bulkLog = _queueBulkLog.Dequeue();

            if (string.IsNullOrEmpty(bulkLog.LogContents))
            {
                bulkLog.LogContents = bulkLog.GetString();
            }

            return bulkLog;
        }
    }
}