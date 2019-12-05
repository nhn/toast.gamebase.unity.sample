using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Toast.Internal;
using Toast.Core;

#if UNITY_2017_2_OR_NEWER
using UnityEngine.Networking;
#endif  // UNITY_2017_2_OR_NEWER

namespace Toast.Logger
{
    public class ToastLoggerLogSender : MonoBehaviour
    {
        private const int MAX_SEND_SIZE = 2097152;
        private const int MAX_COROUTINE_SIZE = 2048;
        private const int MAX_FILE_SIZE = 2048;

        private bool _isStartSender = false;
        private int _couroutineCount = 0;

        private static ToastLoggerLogSender _instance;
        public static ToastLoggerLogSender Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType(typeof(ToastLoggerLogSender)) as ToastLoggerLogSender;
                    if (!_instance)
                    {
                        var container = GameObject.Find(Constants.ToastLoggerSenderObjectName);
                        if (container == null)
                        {
                            container = new GameObject(Constants.ToastLoggerSenderObjectName);
                        }

                        _instance = container.AddComponent<ToastLoggerLogSender>();
                        DontDestroyOnLoad(_instance);
                    }
                }

                return _instance;
            }
        }

        public void StartSender()
        {
            _isStartSender = true;
        }

        public void StopSender()
        {
            _isStartSender = false;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (!_isStartSender)
            {
                return;
            }

            ToastLoggerSendQueue.Instance.Enqueue();
            if (ToastLoggerSendQueue.Instance.Count > 0 && _couroutineCount < MAX_COROUTINE_SIZE)
            {
                ToastLoggerBulkLog bulkLog = ToastLoggerSendQueue.Instance.Dequeue();
                StartCoroutine(SendReport(bulkLog, bulkLog.LogContents, bulkLog.CreateTime, bulkLog.TransactionId));
            }
        }

        IEnumerator SendReport(ToastLoggerBulkLog bulkLog, string logContents, long createTime, string transactionId)
        {
            _couroutineCount++;

            string url = ToastLoggerCommonLogic.CollectorUrl;

            string errorString = "";
            string jsonString = "";

            float timeout = 5.0f;
            bool isTimeout = false;

#if UNITY_2017_2_OR_NEWER
            var downloadHandler = new DownloadHandlerBuffer();
            var uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(logContents));

            using (var request = new UnityWebRequest(url,
                UnityWebRequest.kHttpVerbPOST,
                downloadHandler, uploadHandler))
            {                
                request.SetRequestHeader("Content-Type", "application/json");
                request.timeout = System.Convert.ToInt32(timeout);

                yield return request.SendWebRequest();

                errorString = request.error;
                if (request.isNetworkError || request.isHttpError)
                {
                    isTimeout = true;
                }
                else
                {
                    jsonString = request.downloadHandler.text;
                }
            }
#else
            float timer = 0f;

            Dictionary<string, string> header = new Dictionary<string, string>();
            header.Add("Content-Type", "application/json");
            using (WWW www = new WWW(url, System.Text.Encoding.UTF8.GetBytes(logContents), header))
            {
               do
                {
                    if (timer > timeout)
                    {
                        isTimeout = true;
                        break;
                    }
                    timer += Time.deltaTime;

                    yield return null;
                }
                while (!www.isDone);

                if (isTimeout)
                {
                    www.Dispose();
                }
                else
                {
                    errorString = www.error;
                    jsonString = www.text;
                }
            }
#endif  // UNITY_2017_2_OR_NEWER

#if UNITY_STANDALONE || UNITY_EDITOR
            if (isTimeout == false && string.IsNullOrEmpty(errorString)) // success
            {
                if (ToastFileManager.FileCheck(ToastLoggerCommonLogic.AppKey, createTime, transactionId))
                {
                    ToastFileManager.FileDelete(ToastLoggerCommonLogic.AppKey, createTime, transactionId);
                }
                ToastLoggerSendQueue.Instance.EnqueueInFile();

                if (ToastLoggerCommonLogic.IsLoggerListener)
                {
                    foreach (ToastLoggerLogObject logData in bulkLog.Gets())
                    {
                        CrashLoggerListenerReceiver.Instance.OnLogSuccess(logData);
                    }
                }
            }
            else
            {
                if (ToastFileManager.GetProjectFileCount(ToastLoggerCommonLogic.AppKey) < MAX_FILE_SIZE)
                {
                    ToastFileManager.FileSave(ToastLoggerCommonLogic.AppKey, createTime, transactionId, logContents);

                    if (ToastLoggerCommonLogic.IsLoggerListener)
                    {
                        foreach (ToastLoggerLogObject logData in bulkLog.Gets())
                        {
                            CrashLoggerListenerReceiver.Instance.OnLogSave(logData);
                        }
                    }
                }
            }
#elif UNITY_WEBGL
            if (isTimeout == false && string.IsNullOrEmpty(errorString)) // success
            {

            }
            else
            {
                if (ToastLoggerCommonLogic.IsLoggerListener)
                {
                    foreach (ToastLoggerLogObject logData in bulkLog.Gets())
                    {
                        CrashLoggerListenerReceiver.Instance.OnLogError(logData, errorString);
                    }
                }
            }
#endif

            _couroutineCount--;

            yield return null;
        }
    }
}