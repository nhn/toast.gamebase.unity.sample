using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Toast.Internal;

#if UNITY_2017_2_OR_NEWER
using UnityEngine.Networking;
#endif  // UNITY_2017_2_OR_NEWER

namespace Toast.Logger
{
    public class ToastLoggerSettings : MonoBehaviour
    {
        private const string STATUS_KEY = "status";
        private const string RESULT_KEY = "result";

        private const string NORMAL_LOG_KEY = "log";
        private const string SESSION_LOG_KEY = "session";
        private const string CRASH_LOG_KEY = "crash";
        private const string ENABLE_KEY = "enable";

        private const string NETWORKINSIGHTS_KEY = "networkinsights";

        private const string FILTER_KEY = "filter";

        private const string LOG_LEVEL_FILTER_KEY = "logLevelFilter";
        private const string LOG_TYPE_FILTER_KEY = "logTypeFilter";
        private const string LOG_DUPLICATE_FILTER_KEY = "logDuplicateFilter";

        private const string LOG_LEVEL_KEY = "logLevel";
        private const string LOG_TYPE_KEY = "logType";
        private const string EXPIRED_TIME_KEY = "expiredTime";

        public bool isNormal = true;
        public bool isSession = true;
        public bool isCrash = true;
        public bool isNetworkInsights = false;

        public bool isLogLevelFilter = false;
        public bool isLogTypeFilter = false;
        public bool isLogDuplicateFilter = false;

        public ToastLogLevel filterLogLevel = ToastLogLevel.WARN;
        public List<string> filterLogTypes = new List<string>();
        public long filterDuplicateLogExpiredTimeSeconds = 2;

        public delegate void SettingLoadFinishDelegate();

        private static ToastLoggerSettings _instance;
        public static ToastLoggerSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType(typeof(ToastLoggerSettings)) as ToastLoggerSettings;
                    if (!_instance)
                    {
                        var container = GameObject.Find(Constants.SdkPluginObjectName);
                        if (container == null)
                        {
                            container = new GameObject(Constants.SdkPluginObjectName);
                        }

                        _instance = container.AddComponent<ToastLoggerSettings>();
                        DontDestroyOnLoad(_instance);
                    }
                }

                return _instance;
            }
        }

        public void LoadToastLoggerSettings(SettingLoadFinishDelegate loadFinishDelegate)
        {
            StartCoroutine(SetToastLoggerSettingsByJson(ToastLoggerCommonLogic.SettingUrl, loadFinishDelegate));
        }

        public IEnumerator SetToastLoggerSettingsByJson(string url, SettingLoadFinishDelegate loadFinishDelegate)
        {
            string errorString = "";
            string jsonString = "";

            float timeout = 5.0f;


            #pragma warning disable 0219
            bool isTimeout = false;
            #pragma warning restore 0219
#if UNITY_2017_2_OR_NEWER
            using (var request = UnityWebRequest.Get(url))
            {
                request.timeout = System.Convert.ToInt32(timeout);

                yield return request.SendWebRequest();
                errorString = request.error;
#if UNITY_2020_1_OR_NEWER
                    if (request.result == UnityWebRequest.Result.ConnectionError ||
                    request.result == UnityWebRequest.Result.ProtocolError)
                    {
                        isTimeout = true;
                    }
#else
                if (request.isNetworkError || request.isHttpError)
                {
                    isTimeout = true;
                }
#endif
                else
                {
                    jsonString = request.downloadHandler.text;
                }
            }
#else            
            float timer = 0f;

            using (WWW www = new WWW(url))
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

            if (isTimeout == false && string.IsNullOrEmpty(errorString)) // success
            {
#if UNITY_STANDALONE || UNITY_EDITOR
                SettingsFileManager.FileSave(ToastLoggerCommonLogic.AppKey, jsonString);
#endif  // UNITY_STANDALONE || UNITY_EDITOR

                if (string.IsNullOrEmpty(jsonString) == false)
                {
                    LoadSettingsInfoByJson(jsonString);
                }
            }
            else
            {
#if UNITY_STANDALONE || UNITY_EDITOR
                // Get information from a file
                jsonString = SettingsFileManager.FileLoad(ToastLoggerCommonLogic.AppKey);

                if (string.IsNullOrEmpty(jsonString) == false)
                {
                    LoadSettingsInfoByJson(jsonString);
                }
#endif  // UNITY_STANDALONE || UNITY_EDITOR
            }

            if (loadFinishDelegate != null)
            {
                loadFinishDelegate();
            }
        }

        public bool LoadSettingsInfoByJson(string jsonString)
        {
            JSONNode jsonNode = JSONNode.Parse(jsonString);

            if (IsSuccessful(jsonNode))
            {
                GetNodeEnable(jsonNode, NORMAL_LOG_KEY, ref isNormal);
                GetNodeEnable(jsonNode, SESSION_LOG_KEY, ref isSession);
                GetNodeEnable(jsonNode, CRASH_LOG_KEY, ref isCrash);
                GetNodeEnable(jsonNode, NETWORKINSIGHTS_KEY, ref isNetworkInsights);

                LoadLogLevelFilter(jsonNode);
                LoadLogTypeFilter(jsonNode);
                LoadLogDuplcateFilter(jsonNode);
            }
            else
            {
                return false;
            }

            return true;
        }

        public int GetStatus(JSONNode jsonNode)
        {
            if (jsonNode[STATUS_KEY] != null)
            {
                return int.Parse(jsonNode["status"]);
            }
            else
            {
                return -1;
            }
        }

        public bool IsSuccessful(JSONNode jsonNode)
        {
            return (GetStatus(jsonNode) == 200);
        }

        public void GetNodeEnable(JSONNode jsonNode, string key, ref bool value)
        {
            if (jsonNode[RESULT_KEY][key] != null)
            {
                if (jsonNode[RESULT_KEY][key][ENABLE_KEY] != null)
                {
                    value = jsonNode[RESULT_KEY][key][ENABLE_KEY].AsBool;
                }
            }
        }

        public void LoadLogLevelFilter(JSONNode jsonNode)
        {
            if (jsonNode[RESULT_KEY][FILTER_KEY][LOG_LEVEL_FILTER_KEY] != null)
            {
                if (jsonNode[RESULT_KEY][FILTER_KEY][LOG_LEVEL_FILTER_KEY][ENABLE_KEY] != null)
                {
                    isLogLevelFilter = jsonNode[RESULT_KEY][FILTER_KEY][LOG_LEVEL_FILTER_KEY][ENABLE_KEY].AsBool;
                    filterLogLevel = (ToastLogLevel)Enum.Parse(typeof(ToastLogLevel), jsonNode[RESULT_KEY][FILTER_KEY][LOG_LEVEL_FILTER_KEY][LOG_LEVEL_KEY]);
                }
            }
        }

        public void LoadLogTypeFilter(JSONNode jsonNode)
        {
            if (jsonNode[RESULT_KEY][FILTER_KEY][LOG_TYPE_FILTER_KEY] != null)
            {
                if (jsonNode[RESULT_KEY][FILTER_KEY][LOG_TYPE_FILTER_KEY][ENABLE_KEY] != null)
                {
                    isLogTypeFilter = bool.Parse(jsonNode[RESULT_KEY][FILTER_KEY][LOG_TYPE_FILTER_KEY][ENABLE_KEY]);

                    JSONNode node = jsonNode[RESULT_KEY][FILTER_KEY][LOG_TYPE_FILTER_KEY][LOG_TYPE_KEY];

                    if (node != null)
                    {
                        if (node.IsArray)
                        {
                            JSONArray array = node.AsArray;
                            int size = array.Count;

                            for (int i = 0; i < size; i++)
                            {
                                filterLogTypes.Add(array[i]);
                            }
                        }
                    }
                }
                else
                {
                    // ...
                }
            }
        }

        public void LoadLogDuplcateFilter(JSONNode jsonNode)
        {
            if (jsonNode[RESULT_KEY][FILTER_KEY][LOG_DUPLICATE_FILTER_KEY] != null)
            {
                if (jsonNode[RESULT_KEY][FILTER_KEY][LOG_DUPLICATE_FILTER_KEY][ENABLE_KEY] != null)
                {
                    isLogDuplicateFilter = bool.Parse(jsonNode[RESULT_KEY][FILTER_KEY][LOG_DUPLICATE_FILTER_KEY][ENABLE_KEY]);
                    filterDuplicateLogExpiredTimeSeconds = int.Parse(jsonNode[RESULT_KEY][FILTER_KEY][LOG_DUPLICATE_FILTER_KEY][EXPIRED_TIME_KEY]);
                }
            }
        }
    }
}