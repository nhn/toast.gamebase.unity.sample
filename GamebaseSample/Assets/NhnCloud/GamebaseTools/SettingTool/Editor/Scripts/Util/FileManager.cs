using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Networking;

namespace NhnCloud.GamebaseTools.SettingTool.Util
{
    public static class FileManager
    {
        public enum StateCode
        {
            SUCCESS,
            FILE_NOT_FOUND_ERROR,
            WEB_REQUEST_ERROR,
            UNKNOWN_ERROR,
        }

        public static void Dispose()
        {
        }

        public static void CopyDirectory(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            if (dir.Exists == false)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            if (Directory.Exists(destDirName) == false)
            {
                Directory.CreateDirectory(destDirName);
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }

            if (copySubDirs == true)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    CopyDirectory(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        public static EditorCoroutines.EditorCoroutine DownloadFileToLocal(string remoteFilename, string localFilename, Action<StateCode, string> callback, Action<float> callbackProgress = null)
        {
            return DownloadFile(remoteFilename, (stateCode, message, data) =>
            {
                if (StateCode.SUCCESS == stateCode)
                {
                    try
                    {
                        File.WriteAllBytes(localFilename, data);
                        callback(StateCode.SUCCESS, null);
                    }
                    catch (Exception e)
                    {
                        callback(StateCode.UNKNOWN_ERROR, e.Message);
                    }
                }
                else
                {
                    callback(stateCode, message);
                }
            }, callbackProgress);
        }

        public static EditorCoroutines.EditorCoroutine DownloadFileToString(string remoteFilename, Action<StateCode, string, string> callback, Action<float> callbackProgress = null)
        {
            return DownloadFile(remoteFilename, (stateCode, message, data) =>
            {
                string encoding = null;
                if (data != null)
                {
                    encoding = System.Text.Encoding.Default.GetString(data);
                }

                callback(stateCode, message, encoding);
            }, callbackProgress);
        }

        public static void StopDownloadFile()
        {
            Dispose();
        }

        class CustomCertificateHandler : CertificateHandler
        {
            protected override bool ValidateCertificate(byte[] certificateData)
            {
                return true;
            }
        }

        private static EditorCoroutines.EditorCoroutine DownloadFile(string remoteFilename, Action<StateCode, string, byte[]> callback, Action<float> callbackProgress = null)
        {
            UnityWebRequest request = UnityWebRequest.Get(remoteFilename);
            request.certificateHandler = new CustomCertificateHandler();
            var helper = new UnityWebRequestHelper(request);

            return EditorCoroutines.StartCoroutine(helper.SendWebRequest(() =>
            {
                if (request.result == UnityWebRequest.Result.Success)
                {
                    if (request.downloadHandler.data != null)
                    {
                        callback(StateCode.SUCCESS, null, request.downloadHandler.data);
                    }
                    else
                    {
                        callback(StateCode.WEB_REQUEST_ERROR, "downloadHandler.data == null", null);
                    }
                }
                else if (request.result == UnityWebRequest.Result.ConnectionError)
                {
                    callback(StateCode.WEB_REQUEST_ERROR, request.error, null);
                }
                else if (request.result == UnityWebRequest.Result.ProtocolError)
                {
                    if (request.responseCode == 404)
                    {
                        string message = string.Format("Not Found Error\n{0}", remoteFilename);
                        callback(StateCode.FILE_NOT_FOUND_ERROR, message, null);
                    }
                    else
                    {
                        callback(StateCode.WEB_REQUEST_ERROR, request.error, null);
                    }
                    
                }
            }, callbackProgress), helper);
        }
    }
}