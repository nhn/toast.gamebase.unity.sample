#if UNITY_EDITOR || UNITY_STANDALONE
namespace GamePlatform.Logger.Internal
{
    using GamePlatform.Logger.Internal.Utils;
    using System;
    using UnityEngine.Networking;

    public static class GpFileSecure
    {
        private static readonly string EncryptKey = "DE52351AEDF53D44DA6AE1C4B4F7F";

        public static string EncryptProjectKey(string projectKey)
        {
            var encryptedProjectKey = GpAES.AESEncrypt256(projectKey, EncryptKey);
            return UnityWebRequest.EscapeURL(encryptedProjectKey);
        }

        public static string EncryptText(string text)
        {
            return GpAES.AESEncrypt256(text, EncryptKey);
        }

        public static string DecryptText(string text)
        {
            if (string.IsNullOrEmpty(text) == true || string.IsNullOrEmpty(text.Trim()) == true)
            {
                return string.Empty;
            }

            try
            {
                return GpAES.AESDecrypt256(text.Trim(), EncryptKey);
            }
            catch (Exception)
            {
                // Files that fail to decrypt are deleted.
                return string.Empty;
            }
        }
    }
}
#endif