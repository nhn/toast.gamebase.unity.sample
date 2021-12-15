#if UNITY_STANDALONE || UNITY_EDITOR
namespace Toast.Internal
{
    using System.IO;
    using UnityEngine;

    public static class SettingsFileManager
    {
        public static bool FileSave(string projectKey, string text)
        {
            var encryptProjectKey = ToastFileSecure.EncryptProjectKey(projectKey);
            var folderPath = Path.Combine(Application.persistentDataPath, encryptProjectKey + "_settings");
            var fullPath = Path.Combine(folderPath, "settings");

            Directory.CreateDirectory(folderPath);

            return ToastFile.Write(fullPath, ToastFileSecure.EncryptText(text));
        }

        public static string FileLoad(string projectKey)
        {
            var encryptProjectKey = ToastFileSecure.EncryptProjectKey(projectKey);
            var folderPath = Path.Combine(Application.persistentDataPath, encryptProjectKey + "_settings");
            var fullPath = Path.Combine(folderPath, "settings");

            Directory.CreateDirectory(folderPath);

            if (ToastFile.Exists(fullPath))
            {
                var result =  ToastFile.Read(fullPath);
                return ToastFileSecure.DecryptText(result);
            }

            return "";
        }
    }
}
#endif