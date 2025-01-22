#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)
namespace Toast.Gamebase.Internal.Single.Communicator
{
    public static class ImageNoticeMessage
    {
        public static WebSocketRequest.RequestVO GetImageNoticesMessage()
        {
            var vo = new ImageNoticeRequest.ReqImageNoticeInfoVO();
            vo.parameter.osCode = GamebaseSystemInfo.Platform;
            vo.parameter.clientVersion = GamebaseUnitySDK.AppVersion;
            vo.parameter.storeCode = GamebaseUnitySDK.StoreCode;
            vo.parameter.displayLanguage = GamebaseUnitySDK.DisplayLanguageCode;
            vo.parameter.deviceLanguage = GamebaseSystemInfo.DeviceLanguageCode;
            vo.parameter.deviceCountryCode = GamebaseSystemInfo.CountryCode;            
            vo.parameter.usimCountryCode = "ZZ";

            var requestVO = new WebSocketRequest.RequestVO(Lighthouse.API.Launching.PRODUCT_ID, Lighthouse.API.VERSION, GamebaseUnitySDK.AppID)
            {
                apiId = Lighthouse.API.Launching.ID.GET_IMAGE_NOTICES,
                parameters = vo.parameter
            };

            return requestVO;
        }
    }
}
#endif
