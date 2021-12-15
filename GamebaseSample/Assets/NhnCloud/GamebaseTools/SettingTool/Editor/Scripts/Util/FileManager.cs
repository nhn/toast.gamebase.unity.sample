using System;
using System.IO;
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

        public static void DownloadFileToLocal(string remoteFilename, string localFilename, Action<StateCode, string> callback, Action<float> callbackProgress = null)
        {
            DownloadFile(remoteFilename, (stateCode, message, data) =>
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

        public static void DownloadFileToString(string remoteFilename, Action<StateCode, string, string> callback, Action<float> callbackProgress = null)
        {
            DownloadFile(remoteFilename, (stateCode, message, data) =>
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

        private static void DownloadFile(string remoteFilename, Action<StateCode, string, byte[]> callback, Action<float> callbackProgress = null)
        {
            UnityWebRequest request = UnityWebRequest.Get(remoteFilename);
            var helper = new UnityWebRequestHelper(request);

            EditorCoroutine.Start(helper.SendWebRequest(() =>
            {
                if (request.downloadHandler.data != null)
                {
                    callback(StateCode.SUCCESS, null, request.downloadHandler.data);
                }
                else
                {
                    callback(StateCode.WEB_REQUEST_ERROR, request.error, null);
                }
            }, callbackProgress));
        }
    }
}