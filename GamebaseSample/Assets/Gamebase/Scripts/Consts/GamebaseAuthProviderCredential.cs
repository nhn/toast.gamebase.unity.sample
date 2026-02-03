namespace Toast.Gamebase
{
    public static class GamebaseAuthProviderCredential
    {
        public const string PROVIDER_NAME = "provider_name";
        public const string ACCESS_TOKEN = "access_token";
        public const string ACCESS_TOKEN_SECRET = "access_token_secret";
        public const string CLIENT_ID = "client_id";
        public const string AUTHORIZATION_CODE = "authorization_code";
        public const string GAMEBASE_ACCESS_TOKEN = "gamebase_access_token";
        public const string IGNORE_ALREADY_LOGGED_IN = "ignore_already_logged_in";
        public const string SUB_CODE = "sub_code";
        public const string CODE_VERIFIER = "code_verifier";
        public const string LINE_CHANNEL_REGION = "sub_code";
        public const string EXTRA_PARAMS = "extra_params";
        public const string AUTH_PROTOCOL = "authorization_protocol";
        
        public const string THIRD_IDP_CODE = "thirdIdPCode";
        public const string EXTRA_KEY_THIRD_AUTH_PROVIDER = "thirdAuthProvider";
        public const string EXTRA_KEY_THIRD_AUTH_PROVIDER_CONFIGURATION = "thirdAuthProviderConfiguration";
        
        public const string EXTRA_KEY_SESSION_LOGIN_TOKEN_KIND = "tokenKind";

        public const string VERIFICATION_TYPE = "verification_type";
        public const string PROFILE_API_VERSION = "profile_api_version";
        
        public const string SKIP_IDP_LOGOUT  = "skip_idp_logout";
        public const string SKIP_EXPIRE_GAMEBASE_TOKEN  = "skip_expire_gamebase_token";
        public const string IS_INTERNAL_CALL  = "is_internal_call";

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