namespace Toast.Gamebase
{
    public static class GamebaseLaunchingStatus
    {
        /// <summary>
        /// Service is now normally provided.
        /// </summary>
        public const int IN_SERVICE                     = 200;
        /// <summary>
        /// Update is recommended.
        /// </summary>
        public const int RECOMMEND_UPDATE               = 201;
        /// <summary>
        /// Under maintenance now but QA user service is available.
        /// </summary>
        public const int IN_SERVICE_BY_QA_WHITE_LIST    = 202;
        /// <summary>
        /// Test.
        /// </summary>
        public const int IN_TEST                        = 203;
        /// <summary>
        /// Review.
        /// </summary>
        public const int IN_REVIEW                      = 204;
        /// <summary>
        /// Beta.
        /// </summary>
        public const int IN_BETA                        = 205;
        /// <summary>
        /// Update is required.
        /// </summary>
        public const int REQUIRE_UPDATE                 = 300;
        /// <summary>
        /// User whose access has been blocked.
        /// </summary>
        public const int BLOCKED_USER                   = 301;
        /// <summary>
        /// Service has been terminated.
        /// </summary>
        public const int TERMINATED_SERVICE             = 302;
        /// <summary>
        /// Under maintenance now.
        /// </summary>
        public const int INSPECTING_SERVICE             = 303;
        /// <summary>
        /// Under maintenance for the whole service.
        /// </summary>
        public const int INSPECTING_ALL_SERVICES        = 304;
        /// <summary>
        /// Error of internal server.
        /// </summary>
        public const int INTERNAL_SERVER_ERROR          = 500;
    }
}