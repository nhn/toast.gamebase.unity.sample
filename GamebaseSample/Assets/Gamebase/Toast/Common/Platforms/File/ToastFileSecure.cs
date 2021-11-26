#if UNITY_STANDALONE || UNITY_EDITOR
namespace Toast.Internal
{
    using System;

    public static class ToastFileSecure
    {
        private static readonly string EncryptKey = "DE52351AEDF53D44DA6AE1C4B4F7F";

        public static string EncryptProjectKey(string projectKey)
        {
            var encryptedProjectKey = ToastAES.AESEncrypt256(projectKey, EncryptKey);

#if UNITY_2017_3_OR_NEWER
            return UnityEngine.Networking.UnityWebRequest.EscapeURL(encryptedProjectKey);
#else
            return WWW.EscapeURL(encryptedProjectKey);
#endif
        }

        public static string EncryptText(string text)
        {
            return ToastAES.AESEncrypt256(text, EncryptKey);
        }

        public static string DecryptText(string text)
        {
            // 파일 내용이 null인 경우
            if (text == null)
            {
                return "";
            }

            // 파일 내용이 공백인 경우
            text = text.Trim();
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }

            var planeTxt = "";

            try
            {
                planeTxt = ToastAES.AESDecrypt256(text, EncryptKey);
            }
            catch (Exception)
            {
                // 복호화에 실패한 파일은 삭제
            }

            return planeTxt;
        }
    }
}
#endif