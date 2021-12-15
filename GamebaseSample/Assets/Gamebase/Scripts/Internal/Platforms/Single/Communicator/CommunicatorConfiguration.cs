#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)

namespace Toast.Gamebase.Internal.Single.Communicator
{
    public class CommunicatorConfiguration
    {
        public const int connectionTimeout  = 30;
        public const int timeout            = 5;
        public const int heartbeatInterval  = 120;
        public const int launchingInterval  = 120;
    }
}
#endif