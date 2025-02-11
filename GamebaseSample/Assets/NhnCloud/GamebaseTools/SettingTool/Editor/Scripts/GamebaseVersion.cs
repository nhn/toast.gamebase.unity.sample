using System;

namespace NhnCloud.GamebaseTools.SettingTool
{
    using Data;
    
    public interface IGamebaseVersion
    {
        string GetUnityVersion();
        string GetAndroidVersion();
        string GetIOSVersion();
    }
    
    public class GamebaseVersion : IEquatable<IGamebaseVersion>, IGamebaseVersion, AdapterSelection.ISelectCondition
    {
        public string unity = String.Empty;
        public string android = String.Empty;
        public string ios = String.Empty;

        public GamebaseVersion()
        {
            
        }
        
        public GamebaseVersion(string unity, string android, string ios)
        {
            this.unity = unity;
            this.android = android;
            this.ios = ios;    
        }
        
        public GamebaseVersion(SettingToolResponse.InstalledVersion installedVersion)
        {
            if (installedVersion != null)
            {
                unity = installedVersion.unity;
                android = installedVersion.android;
                ios = installedVersion.ios;    
            }
        }
        
        public GamebaseVersion(SettingToolResponse.SupportVersion versionData)
        {
            if (versionData != null)
            {
                unity = versionData.GetUnityLastVersion();
                android = versionData.GetAndroidLastVersion();
                ios = versionData.GetIOSLastVersion();
            }
        }

        public bool Equals(IGamebaseVersion other)
        {
            if (other == null)
                return false;
            
            return unity.Equals(other.GetUnityVersion()) && android.Equals(other.GetAndroidVersion()) && ios.Equals(other.GetIOSVersion());
        }

        public string GetUnityVersion()
        {
            return unity;
        }

        public string GetAndroidVersion()
        {
            return android;
        }

        public string GetIOSVersion()
        {
            return ios;
        }

        public bool IsValid()
        {
            return VersionUtility.IsInvalidParameter(unity, android, ios) == false;
        }

        public string GetPlatformVersion(string platform)
        {
            switch (platform)
            {
                case SettingToolStrings.TEXT_UNITY:
                    return unity;
                case SettingToolStrings.TEXT_ANDROID:
                    return android;
                case SettingToolStrings.TEXT_IOS:
                    return ios;
            }

            return unity;
        }
    }
}