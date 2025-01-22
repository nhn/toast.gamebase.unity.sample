using System;
using System.Xml;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace NhnCloud.GamebaseTools.SettingTool.Data
{
    public  class InstallCheckValue
    {
        private const string CHECK_TYPE = "checkType";
        private const string CHECK_XML_FILE = "checkXmlFile";
        private const string CHECK_HAS_FILE = "fileCheck";
        private const string CHECK_SETTING_ACTIVITY = "checkActivity";
        private const string CHECK_SETTING_GAMEACTIVITY = "checkGameActivity";
        private const string CHECK_SDK_VERSION = "checkSdkVersion";
        
        private const string XML_FIND_NODE = "findNode";
        private const string XML_HAS_NODE = "nodeValue";
        private const string XML_CHECK_NODE_VALUE = "checkValue";
        
        public string type;
        public string platform;
        public string path;
        public string key;
        public string value;
        public string defaultValue;
        public string message;

        public List<InstallCheckValue> childs;

        public bool IsCheck()
        {
            if (type == CHECK_TYPE)
            {
                return Type.GetType(value) != null;
            }
            else if (type == CHECK_HAS_FILE)
            {
                string filePath = Path.Combine(Application.dataPath, path);
                return File.Exists(filePath);
            }
            else if(type == CHECK_SETTING_ACTIVITY)
            {
#if UNITY_2023_1_OR_NEWER
                return UnityEditor.PlayerSettings.Android.applicationEntry.HasFlag(UnityEditor.AndroidApplicationEntry.Activity);
#else
                return true;
#endif
            }
            else if(type == CHECK_SETTING_GAMEACTIVITY)
            {
#if UNITY_2023_1_OR_NEWER
                return UnityEditor.PlayerSettings.Android.applicationEntry.HasFlag(UnityEditor.AndroidApplicationEntry.GameActivity);
#else
                return false;
#endif              
            }
            else if (type.Equals(CHECK_SDK_VERSION))
            {
                if (string.IsNullOrEmpty(value) == false)
                {
                    string settingVersion = null;
                    if (platform == SettingToolStrings.TEXT_ANDROID)
                    {
                        int value = (int)UnityEditor.PlayerSettings.Android.minSdkVersion;
                        settingVersion = value.ToString();
                    }
                    else if (platform == SettingToolStrings.TEXT_IOS)
                    {
                        settingVersion = UnityEditor.PlayerSettings.iOS.targetOSVersionString;
                    }

                    if (string.IsNullOrEmpty(settingVersion) == false)
                    {
                        return VersionUtility.CompareVersion(settingVersion, value) <= 0;
                    }
                }
            }

            return true;
        }
        
        public void Check(ref List<string> status)
        {
            if (type == CHECK_TYPE)
            {
                if (IsCheck())
                {
                    try
                    {
                        if (childs != null)
                        {
                            foreach (var child in childs)
                            {
                                child.Check(ref status);    
                            }    
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
                else
                {
                    status.Add(message);
                }
            }
            else if (type == CHECK_XML_FILE)
            {
                string filePath = Path.Combine(Application.dataPath, path);
                if (File.Exists(filePath) == false)
                {
                    status.Add(message);
                }
                else
                {
                    XmlDocument xmlDocument = null;
                    try
                    {
                        string data = File.ReadAllText(filePath);
                        if (string.IsNullOrEmpty(data))
                        {
                            status.Add(message);
                        }
                        else
                        {
                            xmlDocument = new XmlDocument();
                            xmlDocument.LoadXml(data);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }

                    if (xmlDocument != null)
                    {
                        try
                        {
                            if (childs != null)
                            {
                                foreach (var child in childs)
                                {
                                    child.CheckXML(xmlDocument, ref status);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }
                    }
                }
            }
            else if (type == CHECK_HAS_FILE)
            {
                if (IsCheck())
                {
                    try
                    {
                        if (childs != null)
                        {
                            foreach (var child in childs)
                            {
                                child.Check(ref status);    
                            }    
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
                else
                {
                    status.Add(message);
                }
            }
            else if(type.Equals(CHECK_SETTING_ACTIVITY) ||
                    type.Equals(CHECK_SETTING_GAMEACTIVITY) )
            {
                if (IsCheck() == true)
                {
                    try
                    {
                        if (childs != null)
                        {
                            foreach (var child in childs)
                            {
                                child.Check(ref status);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }
            else if (type.Equals(CHECK_SDK_VERSION))
            {
                if (IsCheck() == true)
                {
                    try
                    {
                        if (childs != null)
                        {
                            foreach (var child in childs)
                            {
                                child.Check(ref status);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
                else
                {
                    status.Add(message);
                }
            }
        }
        public void CheckXML(XmlNode element, ref List<string> status)
        {
            if (type.Equals(XML_FIND_NODE))
            {
                var selectNodeList = element.SelectNodes(path);

                bool find = false;
                for (int i = 0; i < selectNodeList.Count; i++)
                {
                    XmlElement selectNode = (XmlElement)selectNodeList[i];
                    string attribute = selectNode.GetAttribute(key);
                    if (string.IsNullOrEmpty(value) == false &&
                        value.Equals(attribute))
                    {
                        find = true;

                        if (childs != null)
                        {
                            foreach (var child in childs)
                            {
                                child.CheckXML(selectNode, ref status);
                            }
                        }
                            
                        break;
                    }
                }

                if (find == false)
                {
                    status.Add(message);
                }
            }
            else if (type.Equals(XML_HAS_NODE))
            {
                string attribute = ((XmlElement)element).GetAttribute(key);
                if (string.IsNullOrEmpty(value) == false &&
                    value.Equals(attribute) == false)
                {
                    status.Add(message);
                }
            }
            else if (type.Equals(XML_CHECK_NODE_VALUE))
            {
                var selectNodeList = element.SelectNodes(path);

                bool find = false;
                for (int i = 0; i < selectNodeList.Count; i++)
                {
                    
                    XmlElement selectNode = (XmlElement)selectNodeList[i];
                    string name = selectNode.GetAttribute("name");
                    
                    if (string.IsNullOrEmpty(name) == false &&
                        name.Equals(key))
                    {
                        find = true;

                        string valueAttribute = selectNode.GetAttribute("value");
                        if (string.IsNullOrEmpty((valueAttribute)))
                        {
                            valueAttribute = defaultValue;
                        }
                        if (value != valueAttribute)
                        {
                            status.Add(message);
                        }
                        
                        break;
                    }
                }

                if (find == false)
                {
                    if (defaultValue != null &&
                        value != defaultValue)
                    {
                        status.Add(message);  
                    }
                }
            }
        }
    }

    public class PlatformInstall
    {
        public string platform;
        public List<InstallCheckValue> checkList = new List<InstallCheckValue>();

        public List<string> Check()
        {
            List<string> status = new List<string>();
            foreach (var checkInfo in checkList)
            {
                checkInfo.Check(ref status);
            }

            return status;
        }
        
        
    }

    public class InstallCheckInfo
    {
        public List<PlatformInstall> platformInstall = new List<PlatformInstall>();
        
        public List<string> CheckInstall(string platform)
        {
            List<string> status = new List<string>();
            foreach (var platformInfo in platformInstall)
            {
                if (platformInfo.platform == platform)
                {
                    status.AddRange(platformInfo.Check());
                }
            }
            
            return status;
        }
    }
}