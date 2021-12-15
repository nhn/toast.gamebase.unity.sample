using System.IO;
using System.Collections;
using ICSharpCode.SharpZipLib.Zip;
using System;
using UnityEditor;

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

        public static IEnumerator Extract(string zipFilePath, string unZipTargetFolderPath, Action<StateCode, string> callback, Action<FileStream> callbackFileStream = null, Action<float> progressCallback = null, string password = null, bool isDeleteZipFile = false)
        {
#if UNITY_EDITOR_WIN
            string separatorPath = ReplaceDirectorySeparator(unZipTargetFolderPath);
            string[] directorys = separatorPath.Split(Path.DirectorySeparatorChar);
            string newPath = null;
            string newPathRoot = null;
            if (null != directorys && 0 < directorys.Length)
            {
                newPathRoot = directorys[0] + Path.DirectorySeparatorChar + "GamebaseSettingsToolTemp";
                newPath = newPathRoot + Path.DirectorySeparatorChar + directorys[directorys.Length - 1];
            }
#elif UNITY_EDITOR_OSX
		string newPath = unZipTargetFolderPath;
#endif

            if (true == string.IsNullOrEmpty(zipFilePath))
            {
                callback(StateCode.FILE_PATH_NULL, zipFilePath);
                yield break;
            }

            if (true == string.IsNullOrEmpty(newPath))
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
            long nowCount = 0;
            long totalCount = 0;
            try
            {
                zipInputStreamCount = new ZipInputStream(File.OpenRead(zipFilePath));
                while (null != zipInputStreamCount.GetNextEntry())
                {
                    totalCount++;
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

                Directory.CreateDirectory(newPath + "/" + directoryName);

                if (false == string.IsNullOrEmpty(fileName))
                {
                    FileStream streamWriter = null;
                    try
                    {
                        string filePath = Path.Combine(newPath, theEntry.Name);
                        streamWriter = File.Create(filePath);
                    }
                    catch (Exception e)
                    {
                        zipInputStream.Close();
#if UNITY_EDITOR_WIN
                        FileUtil.DeleteFileOrDirectory(newPathRoot);
#endif
                        callback(StateCode.UNKNOWN_ERROR, e.Message);
                        yield break;
                    }

                    if (null == streamWriter)
                    {
                        continue;
                    }

                    int size = 2048;
                    byte[] data = new byte[2048];

                    while (true)
                    {
                        try
                        {
                            size = zipInputStream.Read(data, 0, data.Length);

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
                    }

                    streamWriter.Close();
                }
                nowCount++;
                float progress = (float)nowCount / (float)totalCount;
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
#if UNITY_EDITOR_WIN
            try
            {
                if (true == Directory.Exists(unZipTargetFolderPath))
                {
                    FileUtil.DeleteFileOrDirectory(unZipTargetFolderPath);
                }
                FileUtil.MoveFileOrDirectory(newPath, unZipTargetFolderPath);
                FileUtil.DeleteFileOrDirectory(newPathRoot);
            }
            catch (Exception e)
            {
                callback(StateCode.UNKNOWN_ERROR, e.Message);
            }
#endif
            callback(StateCode.SUCCESS, null);

        }

        public static string ReplaceDirectorySeparator(string path)
        {
            return path.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
        }
    }
}