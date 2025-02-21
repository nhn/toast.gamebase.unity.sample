using NhnCloud.GamebaseTools.SettingTool.Data;
using NhnCloud.GamebaseTools.SettingTool.ThirdParty;
using NhnCloud.GamebaseTools.SettingTool.Util;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace NhnCloud.GamebaseTools.SettingTool
{
    public class GamebaseInfo : IDisposable
    {
        public static GamebaseVersion GetInstalledVersion()
        {
            return DataManager.GetData<GamebaseVersion>(DataKey.INSTALLED_VERSION);
        }
        
        public static string GetCurrentVersion(string platform)
        {
            var installedVersion = GetInstalledVersion();
            switch (platform)
            {
                case SettingToolStrings.TEXT_UNITY:
                    return installedVersion.unity;
                
                case SettingToolStrings.TEXT_ANDROID:
                    return installedVersion.android;
                
                case SettingToolStrings.TEXT_IOS:
                    return installedVersion.ios;
            }
            
            return installedVersion.unity;
        }
        
        public void Dispose()
        {
            EditorCoroutines.StopAllCoroutines(this);
        }

        public void Initialize()
        {
        }

        public bool CheckInstalledGamebaseSdk()
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
        public void ClearInstallVersion()
        {
            SaveInstallVersion(string.Empty, string.Empty, string.Empty);
        }

        public void SaveInstallVersion(string unityVersion, string androidVersion, string iosVersion)
        {
            var data = new SettingToolResponse.InstalledVersion();
            data.unity = unityVersion;
            data.android = androidVersion;
            data.ios = iosVersion;
            
            string filePath = DataManager.GetData<SettingToolResponse.LocalFileInfo>(DataKey.LOCAL_FILE_INFO).installedVersion.path;
            File.WriteAllText(filePath, JsonMapper.ToJson(data));
        }
    }
}
