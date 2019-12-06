#if UNITY_EDITOR || UNITY_STANDALONE
namespace Toast.Gamebase.Internal.Single.Standalone
{
    public class StandaloneGamebaseAnalytics : CommonGamebaseAnalytics
    {
        private const string SCHEME_AUTH_LOGIN = "gamebase://toast.gamebase/analytics";
        private const string ACCESS_TOKEN_KEY = "token";
        private const string FACEBOOK_PREMISSION = "facebook_permission";
        private const string SERVICE_CODE = "service_code";

        public StandaloneGamebaseAnalytics()
        {
            Domain = typeof(StandaloneGamebaseAnalytics).Name;
        }
    }
}
#endif