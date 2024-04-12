using System.Text.RegularExpressions;
using NhnCloud.GamebaseTools.SettingTool.Data;

namespace NhnCloud.GamebaseTools.SettingTool
{
    public class Version
    {
        private SettingToolResponse.Version data;

        public Version()
        {
            data = DataManager.GetData<SettingToolResponse.Version>(DataKey.VERSION);
            CheckSettingToolAndGamebaseSDKVersionStatus();
        }

        public void CheckSettingToolAndGamebaseSDKVersionStatus()
        {
            string currentSettingTool = SettingTool.VERSION;
            string currentUnity       = GamebaseInfo.GetCurrentVersion(SupportPlatform.UNITY);
            string currentAndroid     = GamebaseInfo.GetCurrentVersion(SupportPlatform.ANDROID);
            string currentIos         = GamebaseInfo.GetCurrentVersion(SupportPlatform.IOS);
            bool hasGamebaseSdk       = DataManager.GetData<bool>(DataKey.HAS_GAMEBASE_SDK);

            CheckSettingToolStatus(currentSettingTool, this.data);
            CheckGamebaseSdkStatus(currentUnity, currentAndroid, currentIos, hasGamebaseSdk, this.data);
        }

        private void CheckSettingToolStatus(string current, SettingToolResponse.Version data)
        {
            string newest = data.settingTool.newest;
            string minimum = data.settingTool.minimum;

            var updateStatus = SettingToolUpdateStatus.NONE;

            if ((IsInvalidParameter(current,minimum,newest) == true) ||
                (IsUpdateRequired(current, minimum) == true))
            {
                updateStatus = SettingToolUpdateStatus.MANDATORY;
            }
            else if (IsUpdateRequired(current, newest) == true)
            {
                updateStatus = SettingToolUpdateStatus.OPTIONAL;
            }
            
            SettingToolLog.Debug(string.Format("SettingToolUpdateStatus:{0}", updateStatus), GetType(), "CheckSettingToolStatus");
            DataManager.SetData(DataKey.SETTING_TOOL_UPDATE_STATUS, updateStatus);
        }

        private void CheckGamebaseSdkStatus(string currentUnity, string currentAndroid, string currentIos, bool hasGamebaseSdk,
                                            SettingToolResponse.Version versionData)
        {
            string[] unityPair    = { currentUnity,   versionData.unity.newest };
            string[] androidPair  = { currentAndroid, versionData.android.newest };
            string[] iosPair      = { currentIos,     versionData.ios.newest };

            var updateStatus = GamebaseUpdateStatus.NONE;

            if ((IsInvalidParameter(unityPair[0], unityPair[1], androidPair[0], androidPair[1], iosPair[0], iosPair[1]) == true) ||
                (hasGamebaseSdk == false))
            {
                updateStatus = GamebaseUpdateStatus.DOWNLOAD_REQUIRED;
            }
            else if (IsUpdateRequired(unityPair, androidPair, iosPair) == true)
            {
                updateStatus = GamebaseUpdateStatus.UPDATE;
            }

            DataManager.SetData(DataKey.GAMEBASE_UPDATE_STATUS, updateStatus);
            SettingToolLog.Debug(string.Format("GamebaseUpdateStatus:{0}", updateStatus), GetType(), "CheckGamebaseSdkStatus");
        }

        private bool IsInvalidVersionString(string str)
        {
            if(string.IsNullOrEmpty(str) == true)
            {
                return true;
            }

            // Major.Minor.Patch
            Regex VersionMatcher = new Regex(@"^[0-9]+\.[0-9]+\.[0-9]+");
            return VersionMatcher.IsMatch(str) == false;
        }

        private bool IsUpdateRequired(string currentVersionString, string targetVersionString)
        {
            int[] currentVersionArray = ConvertVersionStringToIntArray(currentVersionString);
            int[] targetVersionArray  = ConvertVersionStringToIntArray(targetVersionString);

            bool isUpdateRequired = false;
            for (int index = 0; index < currentVersionArray.Length; index++)
            {
                if (currentVersionArray[index] != targetVersionArray[index])
                {
                    isUpdateRequired = currentVersionArray[index] < targetVersionArray[index];
                    break;
                }
            }
            return isUpdateRequired;
        }


        private bool IsUpdateRequired(params string[][] versions)
        {
            bool isUpdateRequired = false;
            foreach (string[] versionPair in versions)
            {
                if (versionPair.Length != 2)
                {
                    continue;
                }
                string current = versionPair[0];
                string target  = versionPair[1];
                isUpdateRequired = isUpdateRequired || IsUpdateRequired(current, target);
            }
            return isUpdateRequired;
        }

        private int[] ConvertVersionStringToIntArray(string version)
        {
            // Major.Minor.Patch
            int[] versions = new int[3];

            if (IsInvalidVersionString(version) == true)
            {
                return versions;
            }

            string[] splitedVersionString = version.Split('.');
            int.TryParse(splitedVersionString[0], out versions[0]);
            int.TryParse(splitedVersionString[1], out versions[1]);
            int.TryParse(splitedVersionString[2], out versions[2]);
            return versions;
        }

        private bool IsInvalidParameter(params string[] inputs)
        {
            if (inputs == null)
            {
                return true;
            }

            foreach (string input in inputs)
            {
                if (IsInvalidVersionString(input) == true)
                {
                    return true;
                }
            }

            return false;
        }
    }
}