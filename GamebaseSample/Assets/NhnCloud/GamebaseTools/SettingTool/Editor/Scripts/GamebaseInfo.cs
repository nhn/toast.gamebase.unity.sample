using NhnCloud.GamebaseTools.SettingTool.Data;
using NhnCloud.GamebaseTools.SettingTool.ThirdParty;
using NhnCloud.GamebaseTools.SettingTool.Util;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NhnCloud.GamebaseTools.SettingTool
{
    public class GamebaseInfo : IDisposable
    {
        private SettingToolCallback.DataDelegate<bool> changeGamebaseSdkDownloadStatus;

        private bool _hasGamebaseSdk;

        public bool HasGamebaseSdk
        {
            get
            {
                return _hasGamebaseSdk;
            }
            set
            {
                if (_hasGamebaseSdk != value)
                {
                    _hasGamebaseSdk = value;
                    DataManager.SetData(DataKey.HAS_GAMEBASE_SDK, value);
                }
            }
        }

        public static string GetCurrentVersion(string platform)
        {
            var data = DataManager.GetData<SettingToolResponse.InstalledVersion>(DataKey.INSTALLED_VERSION);
            return FindInstanceValueFromObject<string>(platform, data);
        }

        public void Dispose()
        {
            EditorCoroutines.StopAllCoroutines(this);
        }

        public void Initialize(SettingToolCallback.DataDelegate<bool> changeGamebaseSdkDownloadStatus)
        {
            this.changeGamebaseSdkDownloadStatus = changeGamebaseSdkDownloadStatus;
            HasGamebaseSdk = CheckHasGamebaseSdk();

            EditorCoroutines.StartCoroutine(CheckGamebaseSdkExists(), this);
        }

        public bool CheckHasGamebaseSdk()
        {
            var data = DataManager.GetData<SettingToolResponse.LocalFileInfo>(DataKey.LOCAL_FILE_INFO);
            if (data == null)
            {
                return false;
            }

            var gamebaseSdkPath = data.gamebaseSdk.path;

            if (string.IsNullOrEmpty(gamebaseSdkPath) == true)
            {
                throw new Exception("FileInfo data of Gamebase SDK is null.");
            }

            SettingToolLog.Debug(string.Format("Directory.Exists:{0}", Directory.Exists(gamebaseSdkPath)), GetType(), "CheckHasGamebaseSdk");

            return Directory.Exists(gamebaseSdkPath);
        }

        /// <summary>
        /// 1. When the setting tool is initialized.
        /// 2. User deleted GamebaseSDK directory when SettingTool is opened.
        /// </summary>
        public void ClearCurrentVersion()
        {
            UpdateCurrentVersion(string.Empty, string.Empty, string.Empty);
        }

        /// <summary>
        /// Click DownloadButton.
        /// </summary>
        public void SetCurrentVersion()
        {
            var data = DataManager.GetData<SettingToolResponse.Version>(DataKey.VERSION);
            if (data == null)
            {
                SettingToolLog.Warn("Version data of Gamebase SDK is null.", typeof(GamebaseInfo), "SetCurrentVersion");
                return;
            }

            UpdateCurrentVersion(data.unity.newest, data.android.newest, data.ios.newest);
        }

        private void UpdateCurrentVersion(string unityVersion, string androidVersion, string iosVersion)
        {
            var data = DataManager.GetData<SettingToolResponse.InstalledVersion>(DataKey.INSTALLED_VERSION);

            data.unity = unityVersion;
            data.android = androidVersion;
            data.ios = iosVersion;

            DataManager.SetData(DataKey.INSTALLED_VERSION, data);

            string filePath = DataManager.GetData<SettingToolResponse.LocalFileInfo>(DataKey.LOCAL_FILE_INFO).installedVersion.path;
            File.WriteAllText(filePath, JsonMapper.ToJson(data));
        }

        private IEnumerator CheckGamebaseSdkExists()
        {
            if (HasGamebaseSdk != CheckHasGamebaseSdk())
            {
                HasGamebaseSdk = !HasGamebaseSdk;
                changeGamebaseSdkDownloadStatus(HasGamebaseSdk);
            }

            yield return new WaitForSecondsRealtime(1);

            EditorCoroutines.StartCoroutine(CheckGamebaseSdkExists(), this);
        }

        private static T FindInstanceValueFromObject<T>(string name, object obj)
        {
            var memberInfo = obj.GetType().GetMember(name.ToLower(), BindingFlags.Instance | BindingFlags.Public)[0];
            var fieldInfo = (FieldInfo)memberInfo;
            return (T)fieldInfo.GetValue(obj);
        }
    }
}
