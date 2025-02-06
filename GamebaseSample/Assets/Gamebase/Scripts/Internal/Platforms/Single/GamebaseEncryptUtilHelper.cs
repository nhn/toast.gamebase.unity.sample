#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
using Toast.Gamebase.Encrypt;
#endif

namespace Toast.Gamebase.Internal.Single
{
    public static class GamebaseEncryptUtilHelper
    {
        public static void Initialize(string gamebaseAppId)
        {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
            GamebaseEncryptUtil.Initialize(gamebaseAppId);
#else
            GamebaseErrorNotifier.FireNotSupportedAPI(typeof(GamebaseEncryptUtilHelper));
#endif
        }

        public static string DecryptDefaultStabilityEncryptedKey(string input)
        {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
            return GamebaseEncryptUtil.DecryptDefaultStabilityEncryptedKey(input);
#else
            GamebaseErrorNotifier.FireNotSupportedAPI(typeof(GamebaseEncryptUtilHelper));
            return string.Empty;
#endif
        }

        public static string DecryptLaunchingEncryptedKey(string input)
        {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
            return GamebaseEncryptUtil.DecryptLaunchingEncryptedKey(input);
#else
            GamebaseErrorNotifier.FireNotSupportedAPI(typeof(GamebaseEncryptUtilHelper));
            return string.Empty;
#endif
        }
    }
}