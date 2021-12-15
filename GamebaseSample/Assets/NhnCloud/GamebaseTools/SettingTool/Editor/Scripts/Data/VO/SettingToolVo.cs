using System.Collections.Generic;
using System.Xml.Serialization;

namespace NhnCloud.GamebaseTools.SettingTool.Data
{
    public class SettingToolVo
    {
        public class BasePlatFormInfo
        {
            [XmlArray("directories")]
            [XmlArrayItem("directory")]
            public List<string> directories;

            [XmlElement("isSettings")]
            public bool isSettings;
        }

        public class BaseInfo
        {
            [XmlElement("unity")]
            public BasePlatFormInfo unity;
            [XmlElement("ios")]
            public BasePlatFormInfo ios;
            [XmlElement("android")]
            public BasePlatFormInfo android;
        }

        public class PlatformInfo
        {
            [XmlElement("directory")]
            public string directory;
            [XmlElement("externals")]
            public string externals;
            [XmlElement("useAdapter")]
            public bool useAdapter;
            [XmlElement("title")]
            public string title;
            [XmlElement("desc")]
            public string desc;
            [XmlElement("button_ok")]
            public string button_ok;
            [XmlElement("button_close")]
            public string button_close;
            [XmlElement("link")]
            public string link;
        }

        public class AdapterInfo
        {
            [XmlElement("category")]
            public string category;
            [XmlElement("name")]
            public string name;

            [XmlElement("unity")]
            public PlatformInfo unity;
            [XmlElement("ios")]
            public PlatformInfo ios;
            [XmlElement("android")]
            public PlatformInfo android;

            [XmlElement("groupBySelectOnlyOneForCategory")]
            public bool groupBySelectOnlyOneForCategory;
            [XmlElement("isUnityPriority")]
            public bool isUnityPriority;
            [XmlElement("unityOrNativeParallelUseDisable")]
            public bool unityOrNativeParallelUseDisable;
            [XmlElement("isSettings")]
            public bool isSettings;
        }

        [XmlRoot("Gamebase"), XmlType("Gamebase")]
        public class GamebaseSDKInfo
        {
            [XmlElement("useUnityAdapter")]
            public bool useUnityAdapter;
            [XmlElement("useAndroidPlatform")]
            public bool useAndroidPlatform;
            [XmlElement("useIOSPlatform")]
            public bool useIOSPlatform;
            [XmlElement("useHangameMixPlatform")]
            public bool useHangameMixPlatform;

            [XmlElement("base")]
            public BaseInfo baseInfo;

            [XmlArray("adapters")]
            [XmlArrayItem("adapter")]
            public List<AdapterInfo> adapters;
        }

        [XmlRoot("SettingToolVersion"), XmlType("SettingToolVersion")]
        public class SupportedSettingToolVersion
        {
            [XmlElement("minimumVersion")]
            public string compatibleVersion;

            [XmlElement("newestVersion")]
            public string newestVersion;
        }
    }
}