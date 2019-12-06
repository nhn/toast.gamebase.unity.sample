#if UNITY_STANDALONE || UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Toast.Internal
{
    public static class ToastFileManager
    {
        public static void FileDelete(string fileName)
        {
            string filePath = Path.Combine(Application.persistentDataPath, fileName);
            File.Delete(filePath);
        }

        public static void FileDelete(string projectKey, long createTime, string transactionId)
        {
            string fileName = projectKey + "/" + createTime.ToString() + "_" + transactionId;
            FileDelete(fileName);
        }

        public static void FileAppendText(string fileName, string text)
        {
            string filePath = Path.Combine(Application.persistentDataPath, fileName);

            FileStream fileStream = new FileStream(filePath, FileMode.Append, FileAccess.Write);
            fileStream.Close();
            StreamWriter streamWriter = new StreamWriter(filePath);
            streamWriter.WriteLine(text);
            streamWriter.Flush();
            streamWriter.Close();            
        }

        public static void FileSave(string fileName, string text)
        {
            string filePath = Path.Combine(Application.persistentDataPath, fileName);

            FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            fileStream.Close();
            StreamWriter streamWriter = new StreamWriter(filePath);
            streamWriter.WriteLine(text);
            streamWriter.Flush();
            streamWriter.Close();
        }

        public static void FileSave(string projectKey, long createTime, string transactionId, string text)
        {
            string folderPath = Path.Combine(Application.persistentDataPath, projectKey);
            Directory.CreateDirectory(folderPath);

            string fileName = projectKey + "/" + createTime.ToString() + "_" + transactionId;
            FileSave(fileName, text);
        }

        public static void SettingFileSave(string projectKey, string text)
        {
            string folderPath = Path.Combine(Application.persistentDataPath, projectKey + "_settings");
            Directory.CreateDirectory(folderPath);

            string fileName = projectKey + "_settings/" + "settings";            
            FileSave(fileName, text);
        }

        public static string SettingFileLoad(string projectKey)
        {
            string folderPath = Path.Combine(Application.persistentDataPath, projectKey + "_settings");
            Directory.CreateDirectory(folderPath);

            string fileName = projectKey + "_settings/" + "settings";
            if (FileExist(fileName))
            {
                return FileLoad(fileName);
            }
            else
            {
                return "";
            }
        }

        public static string GetFirstFile(string projectKey)
        {
            if (!DirectoryExist(projectKey))
            {
                return "";
            }

            string folderPath = Path.Combine(Application.persistentDataPath, projectKey);
            string[] files = Directory.GetFiles(folderPath);

            if (files.Length > 0)
            {
                return files[0];
            }
            else
            {
                return "";
            }
        }

        public static int GetProjectFileCount(string projectKey)
        {
            if (!DirectoryExist(projectKey))
            {
                return 0;
            }

            string folderPath = Path.Combine(Application.persistentDataPath, projectKey);
            string[] files = Directory.GetFiles(folderPath);

            return files.Length;
        }

        public static string FileLoad(string fileName)
        {
            string filePath = Path.Combine(Application.persistentDataPath, fileName);

            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            StreamReader streamReader = new StreamReader(filePath);

            string text = "";
            while (true)
            {
                string readText = streamReader.ReadLine();
                if (readText == null)
                {
                    break;
                }

                text += readText;
            }

            streamReader.Close();
            fileStream.Close();

            return text;
        }

        public static string FileLoad(string projectKey, long createTime, string transactionId)
        {
            if (!DirectoryExist(projectKey))
            {
                return "";
            }
            
            string fileName = projectKey + "/" + createTime.ToString() + "_" + transactionId;
            return FileLoad(fileName);
        }

        public static bool FileCheck(string projectKey, long createTime, string transactionId)
        {
            string folderPath = Path.Combine(Application.persistentDataPath, projectKey);
            if (!Directory.Exists(folderPath))
            {
                return false;
            }
            
            string fileName = projectKey + "/" + createTime.ToString() + "_" + transactionId;
            return FileExist(fileName);
        }

        public static bool FileExist(string fileName)
        {
            string filePath = Path.Combine(Application.persistentDataPath, fileName);

            return File.Exists(filePath);            
        }

        public static bool DirectoryExist(string projectKey)
        {
            string folderPath = Path.Combine(Application.persistentDataPath, projectKey);

            return Directory.Exists(folderPath);
        }

    }
}

#endif