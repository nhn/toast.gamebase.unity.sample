using System.Collections.Generic;

namespace NhnCloud.GamebaseTools.SettingTool.Data
{
    public static class SettingToolResponse
    {
        public class LocalFileInfo
        {
            public class SettingToolRoot
            {
                public string path;
            }
            
            public class Cdn
            {
                public string path;
            }

            public class LocalizedString
            {
                public string path;
            }

            public class LegacyAdapterData
            {
                public string path;
            }

            public class AdditionalAdapterData
            {
                public string path;
                
            }

            public class AdapterSelection
            {
                public string path;
                public string historyPath;
            }

            public class GamebaseAllDependencies
            {
                public string path;
            }

            public class InstalledVersion
            {
                public string path;
            }
            
            public class GamebaseSdk
            {
                public string path;
                public string versionPath;
            }

            public class Ad
            {
                public string downloadPath;
            }

            public SettingToolRoot settingTool;
            public Cdn cdn;
            public LocalizedString localizedString;
            public LegacyAdapterData legacyAdapterSetting;
            public AdditionalAdapterData additionalAdapterData;
            public AdapterSelection adapterSelection;
            public GamebaseAllDependencies gamebaseAllDependencies;
            public InstalledVersion installedVersion;
            public GamebaseSdk gamebaseSdk;
            public Ad ad;
        }

        public class Cdn
        {
            public string url;
        }

        public class Master
        {
            public class LaunchingInfo
            {
                public string url;
                public string version;
                public string appKey;
                public string subKey;
                public bool isEncoding;
            }

            public LaunchingInfo launchingInfo;
        }

        public class SupportVersion
        {
            public class SettingTool
            {
                public string minimum;
                public string newest;
            }
            
            public SettingTool settingTool;
            
            public string[] unity;
            public string[] android;
            public string[] ios;

            public string GetUnityLastVersion()
            {
                if (unity != null &&
                    unity.Length > 0)
                {
                    return unity[0];
                }

                return "";
            }
            
            public string GetAndroidLastVersion()
            {
                if (android != null &&
                    android.Length > 0)
                {
                    return android[0];
                }

                return "";
            }
            
            public string GetIOSLastVersion()
            {
                if (ios != null &&
                    ios.Length > 0)
                {
                    return ios[0];
                }

                return "";
            }
        }

        public class Notice
        {
            public class NoticeMessage
            {
                public string platform;
                public string minVersion;
                public string maxVersion;
                public string message;
                public string linkText;
                public string linkUrl;
                
                public bool CanShow()
                {
                    if (string.IsNullOrEmpty(maxVersion) == false)
                    {
                        if (VersionUtility.CompareVersion(GamebaseInfo.GetCurrentVersion(platform), maxVersion) == -1)
                        {
                            return false;
                        }
                    }
                    
                    if (string.IsNullOrEmpty(minVersion) == false)
                    {
                        if (VersionUtility.CompareVersion(minVersion, GamebaseInfo.GetCurrentVersion(platform)) == -1)
                        {
                            return false;
                        }
                    }
                    
                    return true;
                }
            }

            public List<NoticeMessage> noticeList;
        }

        public class Ad
        {
            public int intervalTime;
            public string imagePath;
            public List<Item> items;

            public class Item
            {
                public string name;
                public string imageName;
                public string link;
                public string description;
                public TimeInfo timeInfo;

                public class TimeInfo
                {
                    public string startTime;
                    public string endTime;
                    public Day day;

                    public class Day
                    {
                        public string start;
                        public string end;
                    }
                }
            }
        }

        public class LaunchingData
        {
            public class Header
            {
                public bool isSuccessful;
                public int resultCode;
                public string resultMessage;
            }

            public class Launching
            {
                public class SettingTool
                {
                    public class Zone
                    {
                        public string logVersion;
                        public string appKey;
                        public string url;
                        public string activation;
                    }

                    public Zone alpha;
                    public Zone real;
                }

                public SettingTool settingTool;
            }

            public Header header;
            public Launching launching;
        }

        

        public class InstalledVersion
        {
            public string unity = "";
            public string android = "";
            public string ios = "";
        }
    }
}