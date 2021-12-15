using System.Collections.Generic;

namespace NhnCloud.GamebaseTools.SettingTool.Data
{
    public static class SettingToolResponse
    {
        public class LocalFileInfo
        {
            public class Cdn
            {
                public string path;
            }

            public class LocalizedString
            {
                public string path;
            }

            public class AdapterSettings
            {
                public string path;
            }

            public class GamebaseAllDependencies
            {
                public string path;
            }

            public class GamebaseSdk
            {
                public string path;
            }

            public class Ad
            {
                public string downloadPath;
            }

            public Cdn cdn;
            public LocalizedString localizedString;
            public AdapterSettings adapterSettings;
            public GamebaseAllDependencies gamebaseAllDependencies;
            public GamebaseSdk gamebaseSdk;
            public Ad ad;
        }

        public class Cdn
        {
            public string url;
        }

        public class Master
        {
            public class Version
            {
                public string path;
            }

            public class Ad
            {
                public string path;
            }

            public class Launching
            {
                public string url;
                public string version;
                public string appKey;
            }

            public Version version;
            public Ad ad;
            public Launching launching;
        }

        public class Version
        {
            public class SettingTool
            {
                public string minimum;
                public string newest;
            }

            public class Unity
            {
                public string newest;
            }

            public class Android
            {
                public string newest;
            }

            public class IOS
            {
                public string newest;
            }

            public SettingTool settingTool;
            public Unity unity;
            public Android android;
            public IOS ios;
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

                    public class NaverCafePlug
                    {
                        public string sdkUrl;
                        public string installPath;
                        public string extensionUrl;
                    }

                    public Zone alpha;
                    public Zone real;
                    public NaverCafePlug naverCafePlug;
                }

                public SettingTool settingTool;
            }

            public Header header;
            public Launching launching;
        }

        public class AdapterSettings
        {
            public bool useAndroid;
            public bool useIOS;

            public Platform unity;
            public Platform android;
            public Platform ios;

            public class Platform
            {
                /// <summary>
                /// Android, iOS, Unity Core SDK's file name.
                /// </summary>
                public string fileName;
                public Category authentication;
                public Category purchase;
                public Category push;
                public Category etc;

                public class Category
                {
                    public string name;
                    public bool onlyOneCanBeSelectedFromCategory;
                    public List<Adapter> adapters;

                    public class Adapter
                    {
                        public string name;
                        public bool used;
                        public bool canOnlyChooseEitherUnityOrNative;
                        public string fileName;
                    }
                }
            }
        }
    }
}