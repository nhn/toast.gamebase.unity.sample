using NhnCloud.GamebaseTools.SettingTool.Data;
using NhnCloud.GamebaseTools.SettingTool.Util;
using System;
using System.Collections;
using System.IO;
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
            if (EditorPrefs.HasKey(platform) == true)
            {
                return EditorPrefs.GetString(platform);
            }
            else
            {
                return string.Empty;
            }
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
            var gamebaseSdkPath = DataManager.GetData<SettingToolResponse.LocalFileInfo>(DataKey.LOCAL_FILE_INFO).gamebaseSdk.path;
            if (string.IsNullOrEmpty(gamebaseSdkPath) == true)
            {
                throw new Exception("FileInfo data of Gamebase SDK is null.");
            }

            SettingToolLog.Debug(string.Format("Directory.Exists:{0}", Directory.Exists(gamebaseSdkPath)), GetType(), "CheckHasGamebaseSdk");

            return Directory.Exists(gamebaseSdkPath);
        }

        /// <summary>
        /// 1. User deleted GamebaseSDK directory when SettingTool is opened.
        /// 2. User deleted GamebaseSDK directory when SettingTool is closed.
        /// 3. Click RemoveButton.
        /// </summary>
        public void ClearCurrentVersion()
        {
            ClearDataInEditorPrefs(EditorPrefsKey.UNITY_CURRENT_VERSION);
            ClearDataInEditorPrefs(EditorPrefsKey.ANDROID_CURRENT_VERSION);
            ClearDataInEditorPrefs(EditorPrefsKey.IOS_CURRENT_VERSION);
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

            AddDataInEditorPrefs(EditorPrefsKey.UNITY_CURRENT_VERSION, data.unity.newest);            
            AddDataInEditorPrefs(EditorPrefsKey.ANDROID_CURRENT_VERSION, data.android.newest);
            AddDataInEditorPrefs(EditorPrefsKey.IOS_CURRENT_VERSION, data.ios.newest);
        }

        private IEnumerator CheckGamebaseSdkExists()
        {
            if (HasGamebaseSdk != CheckHasGamebaseSdk())
            {
                HasGamebaseSdk = CheckHasGamebaseSdk();
                changeGamebaseSdkDownloadStatus(HasGamebaseSdk);
            }

            yield return new WaitForSecondsRealtime(1);

            EditorCoroutines.StartCoroutine(CheckGamebaseSdkExists(), this);
        }

        private void AddDataInEditorPrefs(string key, string data)
        {
            if (EditorPrefs.HasKey(key) == false)
            {
                EditorPrefs.SetString(key, data);
            }
        }

        private void ClearDataInEditorPrefs(string key)
        {
            if (EditorPrefs.HasKey(key) == true)
            {
                EditorPrefs.DeleteKey(key);
            }
        }
    }
}
