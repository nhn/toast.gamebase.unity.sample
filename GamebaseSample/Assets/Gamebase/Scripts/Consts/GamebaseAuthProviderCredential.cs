namespace Toast.Gamebase
{
    public static class GamebaseAuthProviderCredential
    {
        public const string PROVIDER_NAME = "provider_name";
        public const string ACCESS_TOKEN = "access_token";
        public const string ACCESS_TOKEN_SECRET = "access_token_secret";
        public const string AUTHORIZATION_CODE = "authorization_code";
        public const string THIRD_IDP_CODE = "thirdIdPCode";
        public const string GAMEBASE_ACCESS_TOKEN = "gamebase_access_token";
        public const string IGNORE_ALREADY_LOGGED_IN = "ignore_already_logged_in";
        public const string SUB_CODE = "sub_code";
        public const string LINE_CHANNEL_REGION = "sub_code";

        /// <summary>
        /// Only Android
        /// </summary>
        public const string SHOW_LOADING_ANIMATION = "show_loading_animation";

        /// <summary>
        /// Only Standalone, WebGL
        /// </summary>
        public const string REDIRECT_URI = "redirect_uri";
    }
}