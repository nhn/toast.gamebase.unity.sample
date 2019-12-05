using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Toast.Gamebase.LitJson;
using UnityEngine;
using UnityEngine.Networking;

namespace Toast.Gamebase.Internal
{
    public sealed class GamebaseLogReport
    {
        private class SendData
        {
            public string levelType;
            public string bodyType;
            public Dictionary<string, string> sendDataDic;

            public SendData(string levelType, string bodyType, Dictionary<string, string> sendDataDic)
            {
                this.levelType = levelType;
                this.bodyType = bodyType;
                this.sendDataDic = sendDataDic;
            }
        }

        public static class IndicatorReport
        {
            public static class LevelType
            {                
                public const string INDICATOR_REPORT = "INDICATOR_REPORT";
            }
        }

        private static class Log
        {
            public static class LevelType
            {
                public const string TRACE = "TRACE";
                public const string DEBUG = "DEBUG";
                public const string WARN = "WARN";
                public const string ERROR = "ERROR";
            }

            public static class BodyType
            {
                public const string UNITY_TRACE_REPORT = "UNITY_TRACE_REPORT";
                public const string UNITY_DEBUG_REPORT = "UNITY_DEBUG_REPORT";
                public const string UNITY_WARN_REPORT = "UNITY_WARN_REPORT";
                public const string UNITY_ERROR_REPORT = "UNITY_ERROR_REPORT";
            }
        }

        private const string URL = "https://api-logncrash.cloud.toast.com/v2/log";
        private const string PROJECT_VERSION = "1.0.0";
        private const string LOG_VERSION = "v2";
        
        #region key for basic data        
        private const string BASIC_DATA_KEY_APP_ID = "AppID";
        private const string BASIC_DATA_KEY_UUID = "UUID";
        private const string BASIC_DATA_KEY_USER_ID = "UserId";
        private const string BASIC_DATA_KEY_APP_VERSION = "ClientVersion";
        private const string BASIC_DATA_KEY_PLATFORM = "Platform";
        private const string BASIC_DATA_KEY_PROJECT_VERSION = "projectVersion";
        private const string BASIC_DATA_KEY_LOG_VERSION = "logVersion";
        #endregion

        #region key for send data
        private const string SEND_DATA_KEY_PROJECT_NAME = "projectName";
        private const string SEND_DATA_KEY_LOG_LEVEL = "logLevel";
        private const string SEND_DATA_KEY_BODY = "body";
        private const string SEND_DATA_KEY_CLIENT_LOG_TIME = "ClientLogTime";
        #endregion

        public const float LONG_INTERVAL = 1f;
                
        private static readonly GamebaseLogReport instance = new GamebaseLogReport();

        public static GamebaseLogReport Instance
        {
            get { return instance; }
        }

        private bool isDebugIndicatorReport = false;
        private bool isInitialized = false;
        private Dictionary<string, string> basicDataDic;        
        private Queue<SendData> sendIndicatorDataQueue = new Queue<SendData>();
        private Queue<SendData> sendLogDataQueue = new Queue<SendData>();
        private string appKeyLog = "rPf9A4PrR6t0mGHs";
        private string appKeyIndicator = "5247A7zrh1RPFQI6";

        private GamebaseLogReport()
        {
            CreateBasicData();            
        }

        public void Initialize(bool isDebugIndicatorReport, string appKeyIndicator, string appKeyLog)
        {
            if (isInitialized == false)
            {
                isInitialized = true;

                this.isDebugIndicatorReport = isDebugIndicatorReport;                

                if (string.IsNullOrEmpty(appKeyIndicator) == false)
                {
                    this.appKeyIndicator = appKeyIndicator;
                }

                if (string.IsNullOrEmpty(appKeyLog) == false)
                {
                    this.appKeyLog = appKeyLog;
                }

                GamebaseCoroutineManager.StartCoroutine(
                    GamebaseGameObjectManager.GameObjectType.INDICATOR_REPORT_TYPE,
                    SendIndicatorPolling());

                if (this.isDebugIndicatorReport == true)
                {
                    GamebaseCoroutineManager.StartCoroutine(
                        GamebaseGameObjectManager.GameObjectType.INDICATOR_REPORT_TYPE,
                        SendLogPolling());                    
                    return;
                }
                else
                {
                    sendLogDataQueue.Clear();
                }                
            }            
        }

        public void SetUserId(string userId)
        {            
            MergeDictionary(
                ref basicDataDic, 
                new Dictionary<string, string>
                {
                    { BASIC_DATA_KEY_USER_ID, userId }
                });
        }

        public void SendDebugLog(Dictionary<string, string> data)
        {
            if (CheckLogStatus(appKeyLog, data) == false)
            {
                return;
            }

            SendLogHTTPPost(
                Log.LevelType.DEBUG,
                Log.BodyType.UNITY_DEBUG_REPORT,
                data);
        }

        public void SendWarnLog(Dictionary<string, string> data)
        {
            if (CheckLogStatus(appKeyLog, data) == false)
            {
                return;
            }

            SendLogHTTPPost(
                Log.LevelType.WARN,
                Log.BodyType.UNITY_WARN_REPORT,
                data);
        }

        public void SendErrorLog(Dictionary<string, string> data)
        {
            if (CheckLogStatus(appKeyLog, data) == false)
            {
                return;
            }

            SendLogHTTPPost(
                Log.LevelType.ERROR,
                Log.BodyType.UNITY_ERROR_REPORT,
                data);
        }

        public void SendIndicatorReport(string levelType, string bodyType, Dictionary<string, string> data)
        {
            if(CheckIndicatorStatus(appKeyIndicator, data) == false)
            {
                return;
            }

            SendIndicatorReportHTTPPost(
                levelType,
                bodyType,
                data);
        }


        private bool CheckIndicatorStatus(string appKey, Dictionary<string, string> data)
        {
            if (isInitialized == false)
            {
                return false;    
            }
            
            if (string.IsNullOrEmpty(appKey) == true)
            {
                return false;
            }

            if (data == null || data.Count == 0)
            {
                return false;
            }            

            return true;
        }

        private bool CheckLogStatus(string appKey, Dictionary<string, string> data)
        {
            if (isInitialized == false)
            {
                return true;
            }

            if (string.IsNullOrEmpty(appKey) == true)
            {
                return false;
            }

            if (data == null || data.Count == 0)
            {
                return false;
            }

            if (isDebugIndicatorReport == true)
            {
                return true;
            }

            return false;
        }

        private void SendLogHTTPPost(string levelType, string bodyType, Dictionary<string, string> data)
        {    
            Dictionary<string, string> sendDataDic = CreateLogData(levelType, bodyType, data);

            lock (sendLogDataQueue)
            {
                sendLogDataQueue.Enqueue(new SendData(levelType, bodyType, sendDataDic));
            }
        }

        private void SendIndicatorReportHTTPPost(string levelType, string bodyType, Dictionary<string, string> data)
        {            
            Dictionary<string, string> sendDataDic = CreateIndicatorReportData(levelType, bodyType, data);           

            lock (sendIndicatorDataQueue)
            {
                SendData sendData = new SendData(levelType, bodyType, sendDataDic);
                sendIndicatorDataQueue.Enqueue(sendData);
            }
        }

        private IEnumerator SendLogPolling()
        {
            while(true)
            {
                if (isDebugIndicatorReport == true)
                {
                    if (sendLogDataQueue.Count > 0)
                    {
                        SendData sendData = null;
                        lock (sendLogDataQueue)
                        {
                            sendData = sendLogDataQueue.Dequeue();
                        }

                        if (sendData != null)
                        {
                            MergeDictionary(
                                ref sendData.sendDataDic, 
                                new Dictionary<string, string>
                                {
                                    { SEND_DATA_KEY_PROJECT_NAME, appKeyLog}
                                });

                            yield return GamebaseCoroutineManager.StartCoroutine(
                                            GamebaseGameObjectManager.GameObjectType.INDICATOR_REPORT_TYPE,
                                            SendHTTPPost(
                                            sendData.levelType,
                                            sendData.bodyType,
                                            sendData.sendDataDic));
                        }
                        
                    }
                    else
                    {
                        yield return new WaitForSecondsRealtime(LONG_INTERVAL);
                    }
                }
                else
                {
                    yield return new WaitForSecondsRealtime(LONG_INTERVAL);
                }
            }            
        }

        private IEnumerator SendIndicatorPolling()
        {
            while (true)
            {
                if (sendIndicatorDataQueue.Count > 0)
                {
                    SendData sendData = null;
                    lock (sendIndicatorDataQueue)
                    {
                        sendData = sendIndicatorDataQueue.Dequeue();
                    }

                    if (sendData != null)
                    {
                        MergeDictionary(
                                ref sendData.sendDataDic,
                                new Dictionary<string, string>
                                {
                                    { SEND_DATA_KEY_PROJECT_NAME, appKeyIndicator}
                                });

                        yield return GamebaseCoroutineManager.StartCoroutine(
                                        GamebaseGameObjectManager.GameObjectType.INDICATOR_REPORT_TYPE,
                                        SendHTTPPost(
                                        sendData.levelType,
                                        sendData.bodyType,
                                        sendData.sendDataDic));
                    }

                }
                else
                {
                    yield return new WaitForSecondsRealtime(LONG_INTERVAL);
                }
            }
        }

        private IEnumerator SendHTTPPost(string levelType, string bodyType, Dictionary<string, string> data)
        {
            var jsonString      = JsonMapper.ToJson(data);
            var encoding        = new UTF8Encoding().GetBytes(jsonString);

            UnityWebRequest www = UnityWebRequest.Put(URL, encoding);
            www.timeout         = 10;
            www.method          = "POST";
            www.SetRequestHeader("Content-Type", "application/json");            

            yield return UnityCompatibility.UnityWebRequest.Send(www);
        }

        private void CreateBasicData()
        {
            basicDataDic = new Dictionary<string, string>
            {                
                { BASIC_DATA_KEY_APP_ID,            GamebaseUnitySDK.AppID },
                { BASIC_DATA_KEY_APP_VERSION,       GamebaseUnitySDK.AppVersion },
                { BASIC_DATA_KEY_UUID,              GamebaseUnitySDK.UUID },
                { BASIC_DATA_KEY_PLATFORM,          Application.platform.ToString() },            
                { BASIC_DATA_KEY_PROJECT_VERSION,   PROJECT_VERSION },
                { BASIC_DATA_KEY_LOG_VERSION,       LOG_VERSION }
            };
        }

        private Dictionary<string, string> CreateIndicatorReportData(string levelType, string bodyType, Dictionary<string, string> data)
        {
            Dictionary<string, string> sendDataDic = new Dictionary<string, string>(basicDataDic)
            {
                { SEND_DATA_KEY_BODY,               bodyType },
                { SEND_DATA_KEY_LOG_LEVEL,          levelType },
                { SEND_DATA_KEY_CLIENT_LOG_TIME,    GetDateTimeNow()}
            };

            MergeDictionary(ref sendDataDic, data);            
            return sendDataDic;
        }

        private Dictionary<string, string> CreateLogData(string levelType, string bodyType, Dictionary<string, string> data)
        {
            Dictionary<string, string> sendDataDic = new Dictionary<string, string>(basicDataDic)
            {
                { SEND_DATA_KEY_BODY,               bodyType },
                { SEND_DATA_KEY_LOG_LEVEL,          levelType },
                { SEND_DATA_KEY_CLIENT_LOG_TIME,    GetDateTimeNow() }
            };

            MergeDictionary(ref sendDataDic, data);
            return sendDataDic;
        }

        private void MergeDictionary(ref Dictionary<string, string> originalData, Dictionary<string, string> additionalData)
        {
            if (additionalData == null)
            {
                return;
            }

            if (originalData == null)
            {
                originalData = new Dictionary<string, string>();
            }
            
            foreach (string key in additionalData.Keys)
            {
                if (originalData.ContainsKey(key) == false)
                {
                    originalData.Add(key, additionalData[key]);
                }
                else
                {
                    originalData[key] = additionalData[key];
                }
            }
        }

        private string GetDateTimeNow()
        {
            string dataTimeNow = string.Empty;

            try
            {                
                dataTimeNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff 'GMT'zzz");
            }
            finally
            {
            }            

            return dataTimeNow;
        }
    }
}