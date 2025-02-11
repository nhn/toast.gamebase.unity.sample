#if UNITY_STANDALONE || UNITY_EDITOR

namespace GamePlatform.Logger.Internal
{
    using System;
    using System.IO;
    using System.Text;
    using UnityEngine;

    public static class BackupLogManager
    {
        /// <summary>
        /// 30 days
        /// </summary>
        private static readonly int RETENTION_PERIOD = 2592000;

        public static string GetFirstFile(string projectKey)
        {
            var files = GetFiles(projectKey);
            if (files == null)
            {
                return string.Empty;
            }

            string fileName = string.Empty;
            string[] subString = null;

            foreach (var file in files)
            {
                if (file.LastIndexOf("\\", StringComparison.OrdinalIgnoreCase) != -1)
                {
                    fileName = file.Substring(file.LastIndexOf("\\", StringComparison.OrdinalIgnoreCase) + 1);
                }
                else
                {
                    fileName = file.Substring(file.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1);
                }

                subString = fileName.Split('_');

                long createTime;
                if (long.TryParse(subString[0], out createTime) == true)
                {
                    return file;
                }
            }

            return string.Empty;
        }

        public static int GetProjectFileCount(string projectKey)
        {
            var files = GetFiles(projectKey);
            if (files == null)
            {
                return 0;
            }

            return files.Length;
        }

        public static bool HasFile(string projectKey, long createTime, string transactionId)
        {
            if (HasDirectory(projectKey) == false)
            {
                return false;
            }

            var filePath = GetFilePath(projectKey, createTime, transactionId);
            return File.Exists(filePath);
        }

        public static bool SaveFile(string projectKey, long createTime, string transactionId, string text)
        {
            if (string.IsNullOrEmpty(projectKey) == true)
            {
                return false;
            }

            var folderPath = GetPersistentDataPath(projectKey);
            Directory.CreateDirectory(folderPath);

            var filePath = GetFilePath(projectKey, createTime, transactionId);
            return GpFile.Write(filePath, GpFileSecure.EncryptText(text));
        }

        public static string LoadFile(string projectKey, long createTime, string transactionId)
        {
            if (HasFile(projectKey, createTime, transactionId) == false)
            {
                return string.Empty;
            }

            var filePath = GetFilePath(projectKey, createTime, transactionId);
            var fileData = GpFile.Read(filePath);

            if (string.IsNullOrEmpty(fileData) == true)
            {
                return string.Empty;
            }

            return GpFileSecure.DecryptText(fileData);
        }

        public static void DeleteFile(string projectKey, long createTime, string transactionId)
        {
            if (HasFile(projectKey, createTime, transactionId) == true)
            {
                var filePath = GetFilePath(projectKey, createTime, transactionId);
                GpFile.Delete(filePath);
            }
        }

        public static void RemoveOldFiles(string projectKey)
        {
            var files = GetFiles(projectKey);
            if (files == null)
            {
                return;
            }

            var currentTime = GpUtil.GetEpochMilliSeconds();

            foreach (string file in files)
            {
                var fileName = file.Substring(file.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1);
                string[] subString = fileName.Split('_');
                long createTime;

                if (long.TryParse(subString[0], out createTime) == true)
                {
                    var elapsedTime = (int)(currentTime - createTime) / 1000f;

                    if (elapsedTime >= RETENTION_PERIOD)
                    {
                        GpFile.Delete(file);
                    }
                }
            }
        }

        private static string GetPersistentDataPath(string projectKey)
        {
            return Path.Combine(Application.persistentDataPath, GpFileSecure.EncryptProjectKey(projectKey));
        }

        private static string GetFilePath(string projectKey, long createTime, string transactionId)
        {
            return Path.Combine(GetPersistentDataPath(projectKey), GetFileNameConvention(createTime, transactionId));
        }

        private static bool HasDirectory(string projectKey)
        {
            if (string.IsNullOrEmpty(projectKey) == true)
            {
                return false;
            }

            return Directory.Exists(GetPersistentDataPath(projectKey));
        }

        private static string[] GetFiles(string projectKey)
        {
            if (HasDirectory(projectKey) == false)
            {
                return null;
            }

            var files = Directory.GetFiles(GetPersistentDataPath(projectKey));

            if (files == null || files.Length == 0)
            {
                return null;
            }

            return files;
        }

        private static string GetFileNameConvention(long createTime, string transactionId)
        {
            var sb = new StringBuilder();
            sb.Append(createTime.ToString());
            sb.Append("_");
            sb.Append(transactionId);

            return sb.ToString();
        }
    }
}

#endif