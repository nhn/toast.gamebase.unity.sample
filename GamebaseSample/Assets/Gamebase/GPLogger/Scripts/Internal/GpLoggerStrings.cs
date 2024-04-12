namespace GamePlatform.Logger
{
    public static class GpLoggerStrings
    {
        public const string NOT_INITIALIZED_LOGGER                      = "The GPLogger must be initialized.";
        public const string NOT_INITIALIZED_CRASH                       = "The Crash of GPLogger must be initialized.";

        public const string INVALID_PARAMETER                           = "Invalid parameter.";
        public const string INVALID_PARAMETER_APP_KEY_IS_NULL_OR_EMPTY  = "AppKey is null or empty string.";
        public const string INVALID_PARAMETER_KEY_IS_NULL_OR_EMPTY      = "Key is null or empty string";

        public const string NOT_FOUND_CRASH_REPORT                      = "CrashReport not found.";

        public const string ALREADY_INITIALIZED_LOGGER                  = "GpLogger has already been initialized.";

        public const string NOT_SUPPORTED                               = "Not supported.";
        public const string INVALID_DATA_RECEIVED                       = "Invalid data received.";
    }
}