using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace GamebaseSample
{
    public class SampleWebRequestObject : MonoBehaviour
    {
        private static SampleWebRequestObject instance = null;

        public static SampleWebRequestObject Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject obj = new GameObject("SampleWebRequest");
                    instance = obj.AddComponent<SampleWebRequestObject>();

                    DontDestroyOnLoad(obj);
                }

                return instance;
            }
        }

        private const string HEADER_CONTENT_TYPE = "Content-Type";
        private const string HEADER_CONTENT_VALUE_JSON = "application/json";

        public void Get(string url, Action<string> callback)
        {
            UnityWebRequest request = UnityWebRequest.Get(url);
            StartCoroutine(SendRequest(request, callback));
        }

        public void Request(UnityWebRequest request, Action<string> callback)
        {
            StartCoroutine(SendRequest(request, callback));
        }

        private IEnumerator SendRequest(UnityWebRequest request, Action<string> callback)
        {
            request.SetRequestHeader(HEADER_CONTENT_TYPE, HEADER_CONTENT_VALUE_JSON);

            yield return UnityCompatibility.UnityWebRequest.Send(request);

            if (request.isDone == true)
            {
                if (request.responseCode != 200)
                {
                    callback(null);
                    yield break;
                }

                if (UnityCompatibility.UnityWebRequest.IsError(request) == true)
                {
                    callback(null);
                    yield break;
                }

                if (string.IsNullOrEmpty(request.downloadHandler.text) == true)
                {
                    callback(null);
                    yield break;
                }

                callback(request.downloadHandler.text);
            }

            request.Dispose();
        }
    }
}