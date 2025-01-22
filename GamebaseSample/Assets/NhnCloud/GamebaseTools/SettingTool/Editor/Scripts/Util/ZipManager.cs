using System.IO;
using System.Collections;
using ICSharpCode.SharpZipLib.Zip;
using System;
using UnityEditor;
using System.Text;
using UnityEngine;

namespace NhnCloud.GamebaseTools.SettingTool.Util
{
    public static class ZipManager
    {
        public enum StateCode
        {
            SUCCESS,
            FILE_NOT_FOUND_ERROR,
            FILE_PATH_NULL,
            FOLDER_PATH_NULL,
            UNKNOWN_ERROR,
        }

        public static IEnumerator Extract(string zipFilePath, string unZipTargetFolderPath, Action<StateCode, string> callback, Action<FileStream> callbackFileStream = null, Action<float> progressCallback = null, string password = null, bool isDeleteZipFile = false, bool isOverwrite = false)
        {
            string separatorPath = ReplaceDirectorySeparator(unZipTargetFolderPath);
            string[] directories = separatorPath.Split(Path.DirectorySeparatorChar);
            string tempPath = null;
            string tempPathRoot = null;
            
#if UNITY_EDITOR_WIN
            if (null != directories && 0 < directories.Length)
            {
                tempPathRoot = directories[0] + Path.DirectorySeparatorChar + "GamebaseSettingsToolTemp";
                tempPath = tempPathRoot + Path.DirectorySeparatorChar + directories[directories.Length - 1];
            }
#elif UNITY_EDITOR_OSX
            var sb = new StringBuilder();
            if (null != directories && 0 < directories.Length)
            {
                for (int pathIndex = 0; pathIndex < directories.Length - 1; pathIndex++)
                { 
                    sb.Append(directories[pathIndex]);
                    sb.Append(Path.DirectorySeparatorChar);
                }

                sb.Append("GamebaseSettingsToolTemp");
                tempPathRoot = sb.ToString();

                sb.Append(Path.DirectorySeparatorChar);
                sb.Append(directories[directories.Length - 1]);
                tempPath = sb.ToString();
            }
#endif

            if (true == string.IsNullOrEmpty(zipFilePath))
            {
                callback(StateCode.FILE_PATH_NULL, zipFilePath);
                yield break;
            }

            if (true == string.IsNullOrEmpty(tempPath))
            {
                callback(StateCode.FOLDER_PATH_NULL, unZipTargetFolderPath);
                yield break;
            }

            if (false == File.Exists(zipFilePath))
            {
                callback(StateCode.FILE_NOT_FOUND_ERROR, zipFilePath);
                yield break;
            }

            ZipInputStream zipInputStreamCount;
            FileStream fs;
            ZipInputStream zipInputStream;
            ZipEntry theEntry;
            
            long nowSize = 0;
            long totalSize = 0;
            try
            {
                zipInputStreamCount = new ZipInputStream(File.OpenRead(zipFilePath));
                
                ZipEntry countEntry;
                while ((countEntry = zipInputStreamCount.GetNextEntry()) != null)
                {
                    totalSize += countEntry.Size;
                }
                zipInputStreamCount.Close();

                fs = File.Open(zipFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
                if (null != callbackFileStream)
                {
                    callbackFileStream(fs);
                }
                zipInputStream = new ZipInputStream(fs);

                if (false == string.IsNullOrEmpty(password))
                {
                    zipInputStream.Password = password;
                }
            }
            catch (Exception e)
            {
                callback(StateCode.UNKNOWN_ERROR, e.Message);
                yield break;
            }

            while ((theEntry = zipInputStream.GetNextEntry()) != null)
            {
                string directoryName = Path.GetDirectoryName(theEntry.Name);
                string fileName = Path.GetFileName(theEntry.Name);

                Directory.CreateDirectory(tempPath + "/" + directoryName);

                if (false == string.IsNullOrEmpty(fileName))
                {
                    FileStream streamWriter = null;
                    try
                    {
                        string filePath = Path.Combine(tempPath, theEntry.Name);
                        streamWriter = File.Create(filePath);
                    }
                    catch (Exception e)
                    {
                        zipInputStream.Close();
                        FileUtil.DeleteFileOrDirectory(tempPathRoot);
                        callback(StateCode.UNKNOWN_ERROR, e.Message);
                        yield break;
                    }

                    if (null == streamWriter)
                    {
                        continue;
                    }

                    int size = 2048;
                    byte[] data = new byte[2048];

                    DateTime time = DateTime.Now;
                    while (true)
                    {
                        try
                        {
                            size = zipInputStream.Read(data, 0, data.Length);
                            nowSize += size;
                            if (0 < size)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }
                        catch (Exception e)
                        {
                            zipInputStream.Close();
                            streamWriter.Close();
                            callback(StateCode.UNKNOWN_ERROR, e.Message);
                            yield break;
                        }
                        
                        if ((DateTime.Now - time).TotalSeconds > 0.25f)
                        {
                            time = DateTime.Now;

                            float progressStream = (float)nowSize / (float)totalSize;
                            progressCallback(progressStream);
                                
                            yield return null;
                        }
                    }

                    streamWriter.Close();
                }
                float progress = (float)nowSize / (float)totalSize;
                progressCallback(progress);
                yield return null;
            }

            zipInputStream.Close();

            if (true == isDeleteZipFile)
            {
                try
                {
                    File.Delete(zipFilePath);
                }
                catch (Exception e)
                {
                    callback(StateCode.UNKNOWN_ERROR, e.Message);
                }
            }
            try
            {
                if(true == isOverwrite)
                {
                    FileManager.CopyDirectory(tempPath, unZipTargetFolderPath, true);
                }
                else
                {
                    if (true == Directory.Exists(unZipTargetFolderPath))
                    {
                        FileUtil.DeleteFileOrDirectory(unZipTargetFolderPath);
                    }
                    FileUtil.MoveFileOrDirectory(tempPath, unZipTargetFolderPath);
                }

                FileUtil.DeleteFileOrDirectory(tempPathRoot);
            }
            catch (Exception e)
            {
                callback(StateCode.UNKNOWN_ERROR, e.Message);
            }
            callback(StateCode.SUCCESS, null);

        }

        public static string ReplaceDirectorySeparator(string path)
        {
            return path.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
        }
    }
}