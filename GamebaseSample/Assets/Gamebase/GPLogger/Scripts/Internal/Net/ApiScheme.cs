namespace GamePlatform.Logger.Internal
{
    public static class ApiScheme
    {
        private const string DEFAULT_API_SCHEME = "toast://logger";
        private const string PC_PLATFORM_SDK_API_SCHEME = "toast://instancelogger";

        public static string Generate(string apiName)
        {
            return string.Format("{0}/{1}", DEFAULT_API_SCHEME, apiName.ToLower());
        }

        public static string GenerateWithAppKey(string appKey, string apiName)
        {
#if UNITY_STANDALONE || UNITY_WEBGL
            return string.Format("{0}/{1}/{2}", PC_PLATFORM_SDK_API_SCHEME, appKey, apiName.ToLower());
#else
            return string.Format("{0}/{1}/{2}", DEFAULT_API_SCHEME, appKey, apiName.ToLower());
#endif
        }
    }
}