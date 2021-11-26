#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)
namespace Toast.Gamebase.Internal.Single.Communicator
{
    public static class IntrospectMessage
    {
        public static WebSocketRequest.RequestVO MakeIntrospectMessage()
        {
            var vo = new LaunchingRequest.IntrospectVO();
            if (string.IsNullOrEmpty(Gamebase.GetUserID()) == true)
            {
                vo.parameter.userId = "0";
            }
            else
            {
                vo.parameter.userId = Gamebase.GetUserID();
            }
            vo.parameter.idPCode = GamebaseImplementation.Instance.GetLastLoggedInProvider();
            vo.parameter.accessToken = GamebaseImplementation.Instance.GetAccessToken(); ;

            var requestVO = new WebSocketRequest.RequestVO(Lighthouse.API.Gateway.PRODUCT_ID, Lighthouse.API.VERSION, GamebaseUnitySDK.AppID)
            {
                apiId = Lighthouse.API.Gateway.ID.INTROSPECT_ACCESS_TOKEN,
                parameters = vo.parameter
            };

            return requestVO;
        }
    }
}
#endif
