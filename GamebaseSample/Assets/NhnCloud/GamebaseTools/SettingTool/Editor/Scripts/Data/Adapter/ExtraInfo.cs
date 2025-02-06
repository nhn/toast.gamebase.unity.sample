using System.Xml.Schema;

namespace NhnCloud.GamebaseTools.SettingTool.Data
{
    using UnityEditor;
    using UnityEngine;
    
    public class ExtraInfo
    {
        public const string EXTRA_KEY_TEXT = "text";
        public const string EXTRA_KEY_LINK = "link";
        public const string EXTRA_KEY_CHECK_TYPE = "check_type";
        public const string EXTRA_KEY_REMOVE_TYPE = "remove_type";
        public const string EXTRA_KEY_UNITY_VERSION = "check_unity_version";
        public const string EXTRA_KEY_SDK_VERSION = "check_sdk_version";
        public const string EXTRA_KEY_ADAPTER = "check_adapter";
        public const string EXTRA_KEY_ANDROID_MANIFEST = "check_android_manifest_meta_data_name";
      
        public string platform;
        public string type;
        public string text;
        public string value;

        public bool CheckExtra(SettingOption option)
        {
            if (type.Equals(ExtraInfo.EXTRA_KEY_UNITY_VERSION))
            {
                return VersionUtility.CompareVersion(Application.unityVersion, value) == 1;
            }
            else if (type.Equals(ExtraInfo.EXTRA_KEY_SDK_VERSION))
            {
                if (string.IsNullOrEmpty(value) == false)
                {
                    string settingVersion = null;
                    if (platform == SettingToolStrings.TEXT_ANDROID)
                    {
                        int value = (int)PlayerSettings.Android.minSdkVersion;
                        settingVersion = value.ToString();
                    }
                    else if (platform == SettingToolStrings.TEXT_IOS)
                    {
                        settingVersion = PlayerSettings.iOS.targetOSVersionString;
                    }

                    if (string.IsNullOrEmpty(settingVersion) == false)
                    {
                        return VersionUtility.CompareVersion(settingVersion, value) == 1;
                    }
                }
            }
            else if (type.Equals(ExtraInfo.EXTRA_KEY_ANDROID_MANIFEST))
            {
                if (string.IsNullOrEmpty(value) == false)
                {
                    string filePath = System.IO.Path.Combine(Application.dataPath, "Plugins/Android/AndroidManifest.xml");
                    if (System.IO.File.Exists(filePath) == true)
                    {
                        System.Xml.XmlDocument xmlDocument = null;
                        try
                        {
                            string data = System.IO.File.ReadAllText(filePath);
                            if (string.IsNullOrEmpty(data) == false)
                            {
                                xmlDocument = new System.Xml.XmlDocument();
                                xmlDocument.LoadXml(data);
                            }
                        }
                        catch (System.Exception e)
                        {
                            Debug.LogException(e);
                        }

                        if (xmlDocument != null)
                        {
                            try
                            {
                                var selectNodeList = xmlDocument.SelectNodes("/manifest/application/meta-data");
                                
                                for (int i = 0; i < selectNodeList.Count; i++)
                                {
                                    System.Xml.XmlElement selectNode = (System.Xml.XmlElement)selectNodeList[i];
                                    string attribute = selectNode.GetAttribute("android:name");
                                    if (value.Equals(attribute))
                                    {
                                        return false;
                                    }
                                }
                            }
                            catch (System.Exception e)
                            {
                                Debug.LogException(e);
                            }
                        }

                        return true;
                    }
                }
                return false;
            }
            else if (type.Equals(ExtraInfo.EXTRA_KEY_ADAPTER))
            {
                return option.IsUse(AdapterSettings.GetAdapter(value), platform) == false;
            }
            else if (type.Equals(ExtraInfo.EXTRA_KEY_REMOVE_TYPE))
            {
                return System.Type.GetType(value) != null;
            }
            else if (type.Equals(ExtraInfo.EXTRA_KEY_TEXT))
            {
                return true;
            }
            else if (type.Equals(ExtraInfo.EXTRA_KEY_LINK))
            {
                return true;
            }
            return false;
        }
    }
}