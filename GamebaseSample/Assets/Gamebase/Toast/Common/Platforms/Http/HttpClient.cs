using System;
using System.Collections;

#if UNITY_2017_2_OR_NEWER
using UnityEngine.Networking;
#endif  // UNITY_2017_2_OR_NEWER

namespace Toast.Internal
{
    public static class HttpClient
    {
        public delegate void WebRequestDelegate(bool isTimeout, string errorString, string jsonString);

        public static IEnumerator WebRequest(string appKey, string host, string path, float timeout, string jsonRequest, WebRequestDelegate callback)
        {
            var uriBuilder = new UriBuilder("https", host) { Path = path };

            #pragma warning disable 0219
            bool isTimeout = false;
            #pragma warning restore 0219
            string errorString = "";
            string jsonString = "";

#if UNITY_2017_2_OR_NEWER

            if (string.IsNullOrEmpty(jsonRequest))
            {
                using (var request = UnityWebRequest.Get(uriBuilder.Uri.ToString()))
                {
                    request.timeout = System.Convert.ToInt32(timeout);
                    request.SetRequestHeader("Content-Type", "application/json");
                    request.SetRequestHeader("X-NHN-TCIAP-AppKey", appKey);

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
            }
            else
            {
                var downloadHandler = new DownloadHandlerBuffer();
                var uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonRequest));

                using (var request = new UnityWebRequest(uriBuilder.Uri.ToString(),
                    UnityWebRequest.kHttpVerbPOST,
                    downloadHandler, uploadHandler))
                {
                    request.timeout = System.Convert.ToInt32(timeout);
                    request.SetRequestHeader("Content-Type", "application/json");
                    request.SetRequestHeader("X-NHN-TCIAP-AppKey", appKey);

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
            }

#else

                    float timer = 0;

            Dictionary<string, string> header = new Dictionary<string, string>();
            header.Add("Content-Type", "application/json");
            header.Add("X-NHN-TCIAP-AppKey", appKey);

            using (WWW www = new WWW(uriBuilder.Uri.ToString(), System.Text.Encoding.UTF8.GetBytes(jsonRequest), header))
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

            callback(isTimeout, errorString, jsonString);
        }
    }

}
