#if UNITY_STANDALONE || UNITY_EDITOR
namespace GamePlatform.Logger.Internal
{
    using System.IO;
    using UnityEngine;

    public static class SettingsFileManager
    {
        public static bool SaveFile(string projectKey, string text)
        {
            var encryptProjectKey = GpFileSecure.EncryptProjectKey(projectKey);
            var folderPath = Path.Combine(Application.persistentDataPath, encryptProjectKey + "_settings");
            var fullPath = Path.Combine(folderPath, "settings");

            Directory.CreateDirectory(folderPath);

            return GpFile.Write(fullPath, GpFileSecure.EncryptText(text));
        }

        public static string LoadFile(string projectKey)
        {
            var encryptProjectKey = GpFileSecure.EncryptProjectKey(projectKey);
            var folderPath = Path.Combine(Application.persistentDataPath, encryptProjectKey + "_settings");
            var fullPath = Path.Combine(folderPath, "settings");

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