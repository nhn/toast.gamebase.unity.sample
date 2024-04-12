#if UNITY_EDITOR || UNITY_STANDALONE
using System.IO;
using UnityEngine;

namespace GamePlatform.Logger.Internal
{
    public static class LocalFileManager
    {
        private const string SETTING_FOLDER_SUFFIX = "_settings";
        private const string SETTING_FILE_NAME = "settings";

        public static bool FileSave(string projectKey, string text)
        {
            var encryptProjectKey = GpFileSecure.EncryptProjectKey(projectKey);
            var folderPath = Path.Combine(Application.persistentDataPath, encryptProjectKey + SETTING_FOLDER_SUFFIX);
            var fullPath = Path.Combine(folderPath, SETTING_FILE_NAME);

            Directory.CreateDirectory(folderPath);

            return GpFile.Write(fullPath, GpFileSecure.EncryptText(text));
        }

        public static string FileLoad(string projectKey)
        {
            var encryptProjectKey = GpFileSecure.EncryptProjectKey(projectKey);
            var folderPath = Path.Combine(Application.persistentDataPath, encryptProjectKey + SETTING_FOLDER_SUFFIX);
            var fullPath = Path.Combine(folderPath, SETTING_FILE_NAME);

            Directory.CreateDirectory(folderPath);

            if (GpFile.Exists(fullPath) == true)
            {
                var result = GpFile.Read(fullPath);
                return GpFileSecure.DecryptText(result);
            }

            return string.Empty;
        }
    }
}
#endif