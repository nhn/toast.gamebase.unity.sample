namespace NhnCloud.GamebaseTools.SettingTool
{
    public static class SettingToolErrorCode
    {
        public const int NOT_INITIALIZED = 1;
        public const int NETWORK_ERROR = 2;
        public const int UNITY_INTERNAL_ERROR = 3;
        public const int FILE_NOT_FOUND = 4;
        public const int FILE_DATA_EMPTY = 5;

        /// <summary>
        /// LitJson
        /// </summary>
        public const int LIT_JSON_EXCEPTION = 201;

        /// <summary>
        /// Failed to load file
        /// </summary>
        public const int FAILED_TO_LOAD_FILE = 301;
        public const int FAILED_TO_LOAD_LOCAL_FILE_INFO_FILE = 302;
        public const int FAILED_TO_LOAD_CDN_FILE = 303;
        public const int FAILED_TO_LOAD_MASTER_FILE = 304;
        public const int FAILED_TO_LOAD_LAUNCHING_DATA = 305;
        public const int FAILED_TO_LOAD_VERSION_FILE = 306;
        public const int FAILED_TO_LOAD_LOCALIZED_STRING_FILE = 307;
        public const int FAILED_TO_LOAD_INSTALLED_VERSION_FILE = 308;
        public const int FAILED_TO_LOAD_ADAPTER_SETTINGS_FILE = 309;


        public const int UNKNOWN_ERROR = 999;
    }
}