#if UNITY_EDITOR || UNITY_WEBGL
using System.Collections;
using Toast.Gamebase.Internal.Single.Communicator;
using UnityEngine.Networking;

namespace Toast.Gamebase.Internal.Single.WebGL
{
    public class WebGLGamebaseNetwork : CommonGamebaseNetwork
    {
        public WebGLGamebaseNetwork()
        {
            Domain = typeof(WebGLGamebaseNetwork).Name;
        }
        
        public override void IsConnected(int handle)
        {
            GamebaseCoroutineManager.StartCoroutine(GamebaseGameObjectManager.GameObjectType.WEBSOCKET_TYPE, HealthCheck(handle));
        }

        private IEnumerator HealthCheck(int handle)
        {
            var callback = GamebaseCallbackHandler.GetCallback<GamebaseCallback.DataDelegate<bool>>(handle);

            UnityWebRequest www = UnityWebRequest.Get(HealthCheckURL());
            www.SetRequestHeader("X-TCGB-Transaction-Id", Lighthouse.CreateTransactionId().ToString().ToLower());
            www.timeout = CommunicatorConfiguration.timeout;

            yield return UnityCompatibility.WebRequest.Send(www);

            if (true == UnityCompatibility.WebRequest.IsError(www))
            {
                GamebaseLog.Debug(string.Format("error:{0}", www.error), this);
            }

            callback(string.IsNullOrEmpty(www.error));
        }

        #region healthCheck
        private string HealthCheckURL()
        {
            switch (GamebaseUnitySDK.ZoneType)
            {
                case "real":
                    var info = string.Format("/tcgb-gateway/{0}/apps/{1}/health", Lighthouse.API.VERSION, GamebaseUnitySDK.AppID);

                    if (GamebaseUnitySDK.IsInitialized == false)
                        return "https://api-gamebase.cloud.toast.com/tcgb-gateway/v1.0/apps/MASf2WiO/health";

                    return (Gamebase.IsSandbox()) ?
                        string.Format("https://sandbox-api-gamebase.cloud.toast.com{0}", info) :
                        string.Format("https://api-gamebase.cloud.toast.com{0}", info);
                default:
                    return "https://sandbox-api-gamebase.cloud.toast.com/tcgb-gateway/v1.0/apps/gwJZCFnR/health";
            }
        }
        #endregion
    }
}
#endif