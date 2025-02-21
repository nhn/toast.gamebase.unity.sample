using System.Linq;
using System.Text.RegularExpressions;
using NhnCloud.GamebaseTools.SettingTool.Data;

namespace NhnCloud.GamebaseTools.SettingTool
{
    public class VersionStatus
    {
        private SettingToolResponse.SupportVersion supportVersion;
     
        public VersionStatus()
        {
            supportVersion = DataManager.GetData<SettingToolResponse.SupportVersion>(DataKey.SUPPOET_VERSION);

            CheckSettingToolStatus();
        }
        
        private void CheckSettingToolStatus()
        {
            string currentSettingTool = SettingTool.VERSION;
            
            string newest = supportVersion.settingTool.newest;
            string minimum = supportVersion.settingTool.minimum;

            var toolUpdateStatus = SettingToolUpdateStatus.NONE;

            if ((VersionUtility.IsInvalidParameter(currentSettingTool,minimum,newest) == true) ||
                (VersionUtility.IsUpdateRequired(currentSettingTool, minimum) == true))
            {
                toolUpdateStatus = SettingToolUpdateStatus.MANDATORY;
            }
            else if (VersionUtility.IsUpdateRequired(currentSettingTool, newest) == true)
            {
                toolUpdateStatus = SettingToolUpdateStatus.OPTIONAL;
            }
            
            SettingToolLog.Debug(string.Format("SettingToolUpdateStatus:{0}", toolUpdateStatus), GetType(), "CheckSettingToolStatus");
            DataManager.SetData(DataKey.SETTING_TOOL_UPDATE_STATUS, toolUpdateStatus);
        }

        public static bool IsUpdate(IGamebaseVersion version)
        {
            var installedVersion = GamebaseInfo.GetInstalledVersion();

            if (installedVersion.IsValid() == false ||
                installedVersion.Equals(version))
            {
                return true;
            }

            return false;
        }

        public static bool IsSupportVersion(IGamebaseVersion version)
        {
            var supportVersion = DataManager.GetData<SettingToolResponse.SupportVersion>(DataKey.SUPPOET_VERSION);

            if (supportVersion?.unity?.Contains(version.GetUnityVersion())== false)
            {
                return false;
            }
            
            if (supportVersion?.android?.Contains(version.GetAndroidVersion())== false)
            {
                return false;
            }
            
            if (supportVersion?.ios?.Contains(version.GetIOSVersion())== false)
            {
                return false;
            }

            return true;
        }
    }
}