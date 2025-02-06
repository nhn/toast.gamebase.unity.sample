#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)

using Toast.Gamebase.LitJson;

namespace Toast.Gamebase.Internal.Single.Communicator
{
    public static class TermsMessage
    {
        public static WebSocketRequest.RequestVO GetQueryTermsMessage()
        {
            var vo = new TermsRequest.QueryTermsVO();
            var loginInfoVO = DataContainer.GetData<AuthResponse.LoginInfo>(VOKey.Auth.LOGIN_INFO);

            if (loginInfoVO != null)
            {
                vo.parameter.userId = loginInfoVO.member.userId;
            }
            
            vo.parameter.usimCountryCode = "ZZ";
            vo.parameter.deviceCountryCode = GamebaseSystemInfo.CountryCode;
            vo.parameter.displayLanguage = GamebaseUnitySDK.DisplayLanguageCode;
            vo.parameter.deviceLanguage = GamebaseSystemInfo.DeviceLanguageCode;

            WebSocketRequest.RequestVO requestVO = new WebSocketRequest.RequestVO(Lighthouse.API.Gateway.PRODUCT_ID_TOS, Lighthouse.API.VERSION, GamebaseUnitySDK.AppID)
            {
                apiId = Lighthouse.API.Gateway.ID.QUERY_TERMS,
                parameters = vo.parameter
            };

            return requestVO;
        }

        public static WebSocketRequest.RequestVO GetUpdateTermsMessage(GamebaseRequest.Terms.UpdateTermsConfiguration configuration)
        {
            var vo = new TermsRequest.UpdateTermsVO();
            var loginInfoVO = DataContainer.GetData<AuthResponse.LoginInfo>(VOKey.Auth.LOGIN_INFO);

            if (loginInfoVO == null)
            {
                return null;                
            }

            vo.parameter.userId = loginInfoVO.member.userId;
            vo.parameter.termsSeq = configuration.termsSeq;
            vo.payload.contents = configuration.contents;

            WebSocketRequest.RequestVO requestVO = new WebSocketRequest.RequestVO(Lighthouse.API.Gateway.PRODUCT_ID_TOS, Lighthouse.API.VERSION, GamebaseUnitySDK.AppID)
            {
                apiId = Lighthouse.API.Gateway.ID.UPDATE_TERMS,
                parameters = vo.parameter,
                payload = JsonMapper.ToJson(vo.payload)
            };

            return requestVO;

        }
    }
}
#endif