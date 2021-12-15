using System;
using System.Collections;
using System.Net;
using UnityEngine.Networking;

namespace NhnCloud.GamebaseTools.SettingTool.Util
{
    public class UnityWebRequestHelper
    {
        public const string NETWORK_ERROR_MESSAGE = "Failed to send request. (Error has occurred in network.)";
        public const string PROTOCOL_ERROR_MESSAGE = "Failed to send request. responseCode:{0}, error:{1}, text:{2}";
        public const string EMPTY_CONTENT_MESSAGE = "received a null/empty buffer";

        private const int TIMEOUT = 120;

        private UnityWebRequest request;

        public UnityWebRequestHelper(UnityWebRequest request)
        {
            this.request = request;
        }

        public IEnumerator SendWebRequest(Action callback = null, Action<float> callbackProgress = null)
        {
            request.timeout = TIMEOUT;
            //request.SetRequestHeader("Content-Type", "application/json");
            
            yield return request.SendWebRequest();

            while (request.isDone == false)
            {
                if (callbackProgress != null)
                {
                    callbackProgress(request.downloadProgress);
                }

                yield return null;
            }

            if (callback != null)
            {
                callback();
            }
        }

        public string GetData()
        {
            if (IsSuccess() == true)
            {
                return request.downloadHandler.text;
            }

            return string.Empty;
        }

        private bool IsSuccess()
        {
            if (IsError() == true)
            {
                SettingToolLog.Warn(NETWORK_ERROR_MESSAGE, GetType(), "IsSuccess");
                return false;
            }

            if (IsProtocolError() == true)
            {
                SettingToolLog.Warn(string.Format(
                    PROTOCOL_ERROR_MESSAGE,
                    request.responseCode,
                    request.error,
                    request.downloadHandler.text), GetType(), "IsSuccess");

                return false;
            }

            if (string.IsNullOrEmpty(request.downloadHandler.text) == true)
            {
                SettingToolLog.Warn(EMPTY_CONTENT_MESSAGE, GetType(), "IsSuccess");
                return false;
            }

            return true;
        }

        private bool IsError()
        {
#if UNITY_2020_2_OR_NEWER
            if (request.result != UnityWebRequest.Result.Success)
#else
            if (request.isNetworkError == true)
#endif
            {
                SettingToolLog.Warn(string.Format("UnityWebRqeuest isNetworkError error occurred. error:{0}", request.error), GetType(), "IsError");
                return true;
            }
#if UNITY_2020_2_OR_NEWER
            else
            {
                if (request.result == UnityWebRequest.Result.InProgress ||
                    request.result == UnityWebRequest.Result.ConnectionError ||
                    request.result == UnityWebRequest.Result.DataProcessingError)
                {
                    return true; 
                }
            }
#endif
            return false;
        }

        private bool IsProtocolError()
        {
#if UNITY_2020_2_OR_NEWER
            if (request.result == UnityWebRequest.Result.ProtocolError)
#else
            if (request.isNetworkError == false)
#endif
            {
                if ((HttpStatusCode)request.responseCode != HttpStatusCode.OK)
                {
                    return true;
                }
            }

            return false;
        }
    }
}