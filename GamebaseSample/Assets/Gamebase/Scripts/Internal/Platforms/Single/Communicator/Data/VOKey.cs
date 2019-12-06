#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)
namespace Toast.Gamebase.Internal.Single.Communicator
{
    public class VOKey
    {
        public class Auth
        {
            public const string LOGIN_INFO = "VOKey.Auth.LOGIN_INFO";
            public const string BAN_INFO = "VOKey.Auth.BAN_INFO";
        }

        public class Launching
        {
            public const string LAUNCHING_INFO = "VOKey.Launching.LAUNCHING_INFO";
            public const string LAUNCHING_STATE = "VOKey.Launching.LAUNCHING_STATE";
        }
    }
}
#endif