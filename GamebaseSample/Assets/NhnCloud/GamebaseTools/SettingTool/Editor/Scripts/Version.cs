using NhnCloud.GamebaseTools.SettingTool.Data;

namespace NhnCloud.GamebaseTools.SettingTool
{
    public class Version
    {
        private SettingToolResponse.Version data;

        public Version()
        {
            data = DataManager.GetData<SettingToolResponse.Version>(DataKey.VERSION);
            CheckSettingToolStatus();
            CheckGamebaseSdkStatus();
        }

        private void CheckSettingToolStatus()
        {
            var updateStatus = SettingToolUpdateStatus.NONE;

            int current = ConvertVersionStringToInt(SettingTool.VERSION);
            int minimum = ConvertVersionStringToInt(data.settingTool.minimum);
            int newest = ConvertVersionStringToInt(data.settingTool.newest);

            if (current < minimum)
            {
                updateStatus = SettingToolUpdateStatus.MANDATORY;
            }
            else if (current >= minimum)
            {
                if (current < newest)
                {
                    updateStatus = SettingToolUpdateStatus.OPTIONAL;
                }
            }

            SettingToolLog.Debug(string.Format("SettingToolUpdateStatus:{0}", updateStatus), GetType(), "CheckSettingToolStatus");

            DataManager.SetData(DataKey.SETTING_TOOL_UPDATE_STATUS, updateStatus);
        }

        /// <summary>
        /// Check if Gamebase SDK needs to be updated.
        /// </summary>
        public void CheckGamebaseSdkStatus()
        {
            var updateStatus = GamebaseUpdateStatus.NONE;

            if ((CheckUpdateRequired(GamebaseInfo.GetCurrentVersion(EditorPrefsKey.UNITY_CURRENT_VERSION), data.unity.newest) == true) ||
                (CheckUpdateRequired(GamebaseInfo.GetCurrentVersion(EditorPrefsKey.ANDROID_CURRENT_VERSION), data.android.newest) == true) ||
                (CheckUpdateRequired(GamebaseInfo.GetCurrentVersion(EditorPrefsKey.IOS_CURRENT_VERSION), data.ios.newest) == true))
            {
                if (DataManager.GetData<bool>(DataKey.HAS_GAMEBASE_SDK) == true)
                {
                    updateStatus = GamebaseUpdateStatus.UPDATE;
                }
                else
                {
                    updateStatus = GamebaseUpdateStatus.DOWNLOAD_REQUIRED;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(GamebaseInfo.GetCurrentVersion(EditorPrefsKey.UNITY_CURRENT_VERSION)) == false)
                {
                    if (DataManager.GetData<bool>(DataKey.HAS_GAMEBASE_SDK) == true)
                    {
                        updateStatus = GamebaseUpdateStatus.NONE;
                    }
                    else
                    {
                        // Gamebase is installed, but there is no GamebsaeSDK directory.
                        SettingToolLog.Warn("User deleted GamebaseSDK directory when SettingTool is closed.", GetType(), "CheckGamebaseSdkStatus");
                        updateStatus = GamebaseUpdateStatus.DOWNLOAD_REQUIRED;
                    }
                }
            }

            DataManager.SetData(DataKey.GAMEBASE_UPDATE_STATUS, updateStatus);
            SettingToolLog.Debug(string.Format("GamebaseUpdateStatus:{0}", updateStatus), GetType(), "CheckGamebaseSdkStatus");
        }

        private bool CheckUpdateRequired(string current, string newest)
        {
            int intCurrent = ConvertVersionStringToInt(current);
            int intNewest = ConvertVersionStringToInt(newest);

            return (intCurrent < intNewest);
        }

        private int ConvertVersionStringToInt(string version)
        {
            if (string.IsNullOrEmpty(version) == true)
            {
                return 0;
            }

            return int.Parse(string.Join("", version.Split('.')));
        }
    }
}