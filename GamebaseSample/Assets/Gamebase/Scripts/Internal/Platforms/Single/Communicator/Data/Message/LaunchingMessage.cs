#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)

namespace Toast.Gamebase.Internal.Single.Communicator
{
    public static class LaunchingMessage
    {
        public static WebSocketRequest.RequestVO GetLaunchingInfoMessage()
        {
            var vo = new LaunchingRequest.ReqLaunchingInfoVO();
            vo.parameter.appId = GamebaseUnitySDK.AppID;

            if (string.IsNullOrEmpty(Gamebase.GetUserID()) == true)
            {
                vo.parameter.userId = "0";
            }   
            else
            {
                vo.parameter.userId = Gamebase.GetUserID();
            }

            vo.parameter.clientVersion = GamebaseUnitySDK.AppVersion;
            vo.parameter.sdkVersion = GamebaseUnitySDK.SDKVersion;
            vo.parameter.uuid = GamebaseUnitySDK.UUID;
            vo.parameter.deviceKey = GamebaseUnitySDK.DeviceKey;
            vo.parameter.osCode = GamebaseUnitySDK.Platform;
            vo.parameter.osVersion = GamebaseUnitySDK.OsVersion;
            vo.parameter.deviceModel = GamebaseUnitySDK.DeviceModel;
            vo.parameter.deviceLanguage = GamebaseUnitySDK.DeviceLanguageCode;
            vo.parameter.displayLanguage = GamebaseUnitySDK.DisplayLanguageCode;
            vo.parameter.deviceCountryCode = GamebaseUnitySDK.CountryCode;
            vo.parameter.usimCountryCode = "ZZ";
            vo.parameter.lcnt = 0;
            vo.parameter.storeCode = GamebaseUnitySDK.StoreCode;

            var requestVO = new WebSocketRequest.RequestVO(Lighthouse.API.Launching.PRODUCT_ID, Lighthouse.API.VERSION, GamebaseUnitySDK.AppID);
            requestVO.parameters = vo.parameter;

            return requestVO;
        }
    }
}
#endif