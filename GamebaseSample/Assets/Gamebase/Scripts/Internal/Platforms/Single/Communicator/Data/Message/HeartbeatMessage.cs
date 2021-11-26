#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)

using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal.Single.Communicator
{
    public static class HeartbeatMessage
    {
        public static WebSocketRequest.RequestVO GetHeartbeatMessage()
        {
            var vo = new LaunchingRequest.HeartBeatVO();

            vo.parameter.appId = GamebaseUnitySDK.AppID;
            vo.payload.appId = GamebaseUnitySDK.AppID;
            vo.payload.clientVersion = GamebaseUnitySDK.AppVersion;
            vo.payload.deviceCountryCode = GamebaseUnitySDK.CountryCode;
            vo.payload.osCode = GamebaseUnitySDK.Platform;

            if (string.IsNullOrEmpty(Gamebase.GetUserID()) == true)
            {
                vo.payload.userId = "0";
            }                
            else
            {
                vo.payload.userId = Gamebase.GetUserID();
            }

            vo.payload.usimCountryCode = "ZZ";
            vo.payload.storeCode = GamebaseUnitySDK.StoreCode;
            vo.payload.idpCode = GamebaseAnalytics.Instance.IdPCode;
            vo.payload.deviceModel = GamebaseUnitySDK.DeviceModel;


            var requestVO = new WebSocketRequest.RequestVO(Lighthouse.API.Presence.PRODUCT_ID, Lighthouse.API.VERSION, GamebaseUnitySDK.AppID)
            {
                apiId = Lighthouse.API.Presence.ID.HEARTBEAT,
                parameters = vo.parameter,
                payload = JsonMapper.ToJson(vo.payload)
            };
            return requestVO;
        }
    }
}
#endif