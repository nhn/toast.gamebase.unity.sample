using UnityEngine;

namespace GamebaseSample
{
    public static class UnityCompatibility
    {
        public static class UnityWebRequest
        {
            public static AsyncOperation Send(UnityEngine.Networking.UnityWebRequest request)
            {
#if UNITY_2017_2_OR_NEWER
                return request.SendWebRequest();
#else
                return request.Send();
#endif
            }

            public static bool IsError(UnityEngine.Networking.UnityWebRequest request)
            {
#if UNITY_2020_2_OR_NEWER
                return request.result == UnityEngine.Networking.UnityWebRequest.Result.ConnectionError;
#elif UNITY_2017_1_OR_NEWER
                return request.isNetworkError;
#else
                return request.isError;
#endif
            }

            public static string EscapeURL(string resource)
            {
#if UNITY_2017_3_OR_NEWER
                return UnityWebRequest.EscapeURL(resource);
#else
                return UnityEngine.WWW.EscapeURL(resource);
#endif
            }

            public static string UnEscapeURL(string resource)
            {
#if UNITY_2017_3_OR_NEWER
                return UnityWebRequest.UnEscapeURL(resource);
#else
                return UnityEngine.WWW.UnEscapeURL(resource);
#endif
            }
        }
    }
}