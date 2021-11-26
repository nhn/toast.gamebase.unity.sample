#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)
namespace Toast.Gamebase.Internal.Single.Communicator
{
    public static class ImageNoticeMessage
    {
        public static WebSocketRequest.RequestVO GetImageNoticesMessage()
        {
            var vo = new ImageNoticeRequest.ReqImageNoticeInfoVO();

            vo.parameter.osCode = GamebaseUnitySDK.Platform;
            vo.parameter.clientVersion = GamebaseUnitySDK.AppVersion;
            vo.parameter.storeCode = GamebaseUnitySDK.StoreCode;
            vo.parameter.displayLanguage = GamebaseUnitySDK.DisplayLanguageCode;
            vo.parameter.deviceLanguage = GamebaseUnitySDK.DeviceLanguageCode;
            vo.parameter.deviceCountryCode = GamebaseUnitySDK.CountryCode;            
            vo.parameter.usimCountryCode = "ZZ";

            var requestVO = new WebSocketRequest.RequestVO(Lighthouse.API.Launching.PRODUCT_ID, Lighthouse.API.VERSION, GamebaseUnitySDK.AppID)
            {
                parameters = vo.parameter
            };

            return requestVO;
        }
    }
}
#endif
