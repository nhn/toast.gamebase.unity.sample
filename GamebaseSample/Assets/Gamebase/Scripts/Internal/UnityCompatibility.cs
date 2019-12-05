using UnityEngine;

namespace Toast.Gamebase.Internal
{
    public class UnityCompatibility
    {
        public class UnityWebRequest
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
#if UNITY_2017_1_OR_NEWER
                return request.isNetworkError;
#else
                return request.isError;
#endif
            }
        }
    }
}

