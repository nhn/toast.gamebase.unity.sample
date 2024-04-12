using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace GamePlatform.Logger.Internal
{
    public class GpLoggerLogSender
    {
        private const int MAX_SEND_SIZE = 2097152;
        private const int MAX_COROUTINE_SIZE = 2048;
        private const int MAX_FILE_SIZE = 2048;

        private readonly LogNCrash logNCrash;
        private readonly GpLoggerSendQueue sendQueue;

        private bool isStartSender = false;
        private int couroutineCount = 0;

        public GpLoggerLogSender(LogNCrash logNCrash)
        {
            this.logNCrash = logNCrash;
            sendQueue = new GpLoggerSendQueue(logNCrash.GetAppKey(), logNCrash.Settings);

            GameObjectManager.GetCoroutineComponent(GameObjectType.GP_LOGGER).StartCoroutine(StartSender());
        }

        public void Start()
        {
            isStartSender = true;
        }

        public void Stop()
        {
            isStartSender = false;
        }

        public void AddLogItem(BaseLogItem item)
        {
            sendQueue.AddLogItem(item);
        }

        private IEnumerator StartSender()
        {
            if (isStartSender == true)
            {
                sendQueue.Enqueue();
                if (sendQueue.Count > 0 && couroutineCount < MAX_COROUTINE_SIZE)
                {
                    LogItemContainer container = sendQueue.Dequeue();
                    yield return GameObjectManager.GetCoroutineComponent(GameObjectType.GP_LOGGER).StartCoroutine(
                        SendReport(container, container.LogContents, container.CreateTime, container.TransactionId));
                }
            }

            yield return new WaitForSecondsRealtime(0.1f);

            GameObjectManager.GetCoroutineComponent(GameObjectType.GP_LOGGER).StartCoroutine(StartSender());
        }

        private IEnumerator SendReport(LogItemContainer container, string logContents, long createTime, string transactionId)
        {
            GpLog.Debug(string.Format("logContents:{0}, createTime:{1}, transactionId:{2}", logContents, createTime, transactionId), GetType(), "SendReport");

            couroutineCount++;

            var downloadHandler = new DownloadHandlerBuffer();
            var uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(logContents));

            using (var request = new UnityWebRequest(logNCrash.GetCollectorUrl(), UnityWebRequest.kHttpVerbPOST, downloadHandler, uploadHandler))
            {
                request.SetRequestHeader("Content-Type", "application/json");

                var helper = new UnityWebRequestHelper(request);
                yield return GameObjectManager.GetCoroutineComponent(GameObjectType.GP_LOGGER).StartCoroutine(helper.SendWebRequest(() =>
                {
#if UNITY_STANDALONE || UNITY_EDITOR
                    var jsonString = helper.GetData();
                    if (string.IsNullOrEmpty(jsonString) == false) // success
                    {
                        if (BackupLogManager.HasFile(logNCrash.GetAppKey(), createTime, transactionId) == true)
                        {
                            BackupLogManager.DeleteFile(logNCrash.GetAppKey(), createTime, transactionId);
                        }

                        sendQueue.EnqueueInFile();

                        foreach (BaseLogItem item in container.GetItemList())
                        {
                            if (CrashLoggerReceiver.Instance != null)
                            {
                                CrashLoggerReceiver.Instance.OnLogSuccessWithLogItem(item);
                            }
                        }
                    }
                    else
                    {
                        if (BackupLogManager.GetProjectFileCount(logNCrash.GetAppKey()) < MAX_FILE_SIZE)
                        {
                            BackupLogManager.SaveFile(logNCrash.GetAppKey(), createTime, transactionId, logContents);

                            foreach (BaseLogItem item in container.GetItemList())
                            {
                                if (CrashLoggerReceiver.Instance != null)
                                {
                                    CrashLoggerReceiver.Instance.OnLogSaveWithLogItem(item);
                                }
                            }
                        }
                    }
#elif UNITY_WEBGL
                    var jsonString = helper.GetData();
                    if (string.IsNullOrEmpty(jsonString) == true)
                    {
                        foreach (BaseLogItem item in container.GetItemList())
                        {
                            if (CrashLoggerReceiver.Instance != null)
                            {
                                CrashLoggerReceiver.Instance.OnLogErrorWithLogItem(item, request.error);
                            }
                        }
                    }
#endif

                    couroutineCount--;
                }));
            }

            yield return null;
        }
    }
}