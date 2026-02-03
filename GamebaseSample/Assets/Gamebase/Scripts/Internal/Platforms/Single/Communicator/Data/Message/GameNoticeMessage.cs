#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)
namespace Toast.Gamebase.Internal.Single.Communicator
{
    public static class GameNoticeMessage
    {
        public static WebSocketRequest.RequestVO GetGameNoticesMessage(GamebaseRequest.GameNotice.Configuration configuration)
        {
            var vo = new GameNoticeRequest.ReqGameNoticeInfoVO();
            vo.parameter.osCode = GamebaseSystemInfo.Platform;
            vo.parameter.clientVersion = GamebaseUnitySDK.AppVersion;
            vo.parameter.storeCode = GamebaseUnitySDK.StoreCode;
            vo.parameter.displayLanguage = GamebaseUnitySDK.DisplayLanguageCode;
            vo.parameter.deviceLanguage = GamebaseSystemInfo.DeviceLanguageCode;
            vo.parameter.deviceCountryCode = GamebaseSystemInfo.CountryCode;            
            vo.parameter.usimCountryCode = "ZZ";
            if (configuration?.categoryNames != null)
            {
                vo.parameter.categoryNames = configuration.categoryNames.ToArray();
                vo.parameter.filterCategory = true;
            }
            else
            {
                vo.parameter.filterCategory = false;
            }

            var requestVO = new WebSocketRequest.RequestVO(Lighthouse.API.Launching.PRODUCT_ID, Lighthouse.API.VERSION, GamebaseUnitySDK.AppID)
            {
                apiId = Lighthouse.API.Launching.ID.GET_GAME_NOTICES,
                parameters = vo.parameter
            };

            return requestVO;
        }
    }
}
#endif
