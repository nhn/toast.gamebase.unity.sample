#if UNITY_STANDALONE || UNITY_EDITOR

namespace Toast.Internal
{
    using System;
    using System.IO;
    using System.Text;
    using UnityEngine;

    public static class BackupLogManager
    {
        private static readonly int RETENTION_PERIOD = 2592000;

        public static string GetFirstFile(string projectKey)
        {
            if (string.IsNullOrEmpty(projectKey))
            {
                return "";
            }

            var encryptProjectKey = ToastFileSecure.EncryptProjectKey(projectKey);
            var folderPath = Path.Combine(
                Application.persistentDataPath,
                encryptProjectKey);

            if (!Directory.Exists(folderPath))
            {
                return "";
            }

            string[] files = Directory.GetFiles(folderPath);
            if (files != null)
            {
                for(int i = 0; i < files.Length; i++)
                {
                    string fileName = files[i].Substring(files[i].LastIndexOf("\\", StringComparison.OrdinalIgnoreCase) + 1);

                    if (files[i].LastIndexOf("\\", StringComparison.OrdinalIgnoreCase) == -1)
                    {
                        fileName = files[i].Substring(files[i].LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1);
                    }

                    string[] subString = fileName.Split('_');
                    
                    long createTime;
                    if (long.TryParse(subString[0], out createTime))
                    {
                        return files[i];
                    }
                }
            }
            
            return "";
        }

        public static int GetProjectFileCount(string projectKey)
        {
            if (string.IsNullOrEmpty(projectKey))
            {
                return 0;
            }

            var encryptProjectKey = ToastFileSecure.EncryptProjectKey(projectKey);
            var folderPath = Path.Combine(
                Application.persistentDataPath,
                encryptProjectKey);

            if (!Directory.Exists(folderPath))
            {
                return 0;
            }

            string[] files = Directory.GetFiles(folderPath);

            if (files == null)
            {
                return 0;
            }

            return files.Length;
        }

        public static bool DirectoryCheck(string projectKey)
        {
            if (string.IsNullOrEmpty(projectKey))
            {
                return false;
            }

            var encryptProjectKey = ToastFileSecure.EncryptProjectKey(projectKey);
            var folderPath = Path.Combine(
                Application.persistentDataPath,
                encryptProjectKey);

            return Directory.Exists(folderPath);
        }

        public static bool FileCheck(string projectKey, long createTime, string transactionId)
        {
            if (!DirectoryCheck(projectKey))
            {
                return false;
            }

            var folderPath = Path.Combine(
                Application.persistentDataPath,
                ToastFileSecure.EncryptProjectKey(projectKey));

            var sb = new StringBuilder();
            sb.Append(createTime.ToString());
            sb.Append("_");
            sb.Append(transactionId);

            var filePath = Path.Combine(folderPath, sb.ToString());

            return File.Exists(filePath);
        }

        public static bool FileSave(string projectKey, long createTime, string transactionId, string text)
        {
            if (string.IsNullOrEmpty(projectKey))
            {
                return false;
            }

            var folderPath = Path.Combine(
                Application.persistentDataPath,
                ToastFileSecure.EncryptProjectKey(projectKey));

            Directory.CreateDirectory(folderPath);

            var sb = new StringBuilder();
            sb.Append(createTime.ToString());
            sb.Append("_");
            sb.Append(transactionId);

            var filePath = Path.Combine(folderPath, sb.ToString());
            return ToastFile.Write(filePath, ToastFileSecure.EncryptText(text));
        }

        public static string FileLoad(string projectKey, long createTime, string transactionId)
        {
            if (!DirectoryCheck(projectKey))
            {
                return "";
            }

            var folderPath = Path.Combine(
                Application.persistentDataPath,
                ToastFileSecure.EncryptProjectKey(projectKey));

            var sb = new StringBuilder();
            sb.Append(createTime.ToString());
            sb.Append("_");
            sb.Append(transactionId);

            var filePath = Path.Combine(folderPath, sb.ToString());

            var result = ToastFile.Read(filePath);

            if (!string.IsNullOrEmpty(result))
            {
                return ToastFileSecure.DecryptText(result);
            }

            return "";
        }

        public static void FileDelete(string projectKey, long createTime, string transactionId)
        {
            if (string.IsNullOrEmpty(projectKey))
            {
                return;
            }

            var folderPath = Path.Combine(
               Application.persistentDataPath,
               ToastFileSecure.EncryptProjectKey(projectKey));

            var sb = new StringBuilder();
            sb.Append(createTime.ToString());
            sb.Append("_");
            sb.Append(transactionId);

            var filePath = Path.Combine(folderPath, sb.ToString());

            ToastFile.Delete(filePath);
        }

        public static void RemoveOldFiles(string projectKey)
        {
            if (string.IsNullOrEmpty(projectKey))
            {
                return;
            }

            var encryptProjectKey = ToastFileSecure.EncryptProjectKey(projectKey);
            var folderPath = Path.Combine(
                Application.persistentDataPath,
                encryptProjectKey);

            if (!Directory.Exists(folderPath))
            {
                return;
            }

            var currentTime = ToastUtil.GetEpochMilliSeconds();

            string[] files = Directory.GetFiles(folderPath);
            if (files != null && files.Length > 0)
            {
                foreach(string file in files)
                {
                    var fileName = file.Substring(file.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1);
                    string[] subString = fileName.Split('_');
                    long createTime;

                    if (long.TryParse(subString[0], out createTime))
                    {
                        var elapsedTime = (int)(currentTime - createTime) / 1000f;

                        if (elapsedTime >= RETENTION_PERIOD)
                        {
                            ToastFile.Delete(file);
                        }
                    }
                }
            }
        }
    }
}

#endif