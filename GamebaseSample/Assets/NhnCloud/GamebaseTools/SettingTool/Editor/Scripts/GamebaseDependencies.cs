using NhnCloud.GamebaseTools.SettingTool.Data;
using NhnCloud.GamebaseTools.SettingTool.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

namespace NhnCloud.GamebaseTools.SettingTool
{
    public class GamebaseDependencies : IDisposable
    {
        private const string DOMAIN = "GamebaseDependencies";

        static public XmlDocument NewXmlDocument()
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDocument.AppendChild(xmlDeclaration);
                    
            var dependencies = xmlDocument.CreateElement("dependencies");
            xmlDocument.AppendChild(dependencies);

            dependencies.AppendChild(xmlDocument.CreateElement("androidPackages"));
            dependencies.AppendChild(xmlDocument.CreateElement("iosPods"));

            return xmlDocument;
        }
            
        public void Dispose()
        {
            EditorCoroutines.StopAllCoroutines(this);
        }

        public void UpdateGamebaseAllDependenciesFile(SettingOption settingOption, SettingToolCallback.ErrorDelegate callback)
        {
            var document = DataManager.GetData<XmlDocument>(DataKey.GAMEBASE_ALL_DEPENDENCIES);

            var androidPackages = document.SelectSingleNode("dependencies/androidPackages");
            var iosPods = document.SelectSingleNode("dependencies/iosPods");

            androidPackages.RemoveAll();
            iosPods.RemoveAll();

            if (settingOption.IsActivePlatform(SettingToolStrings.TEXT_ANDROID) == true)
            {
                AddXmlChild(document, androidPackages, settingOption.GetPlatform(SettingToolStrings.TEXT_ANDROID).install, "androidPackage", "spec", settingOption.GetAndroidVersion());
            }

            if (settingOption.IsActivePlatform(SettingToolStrings.TEXT_IOS) == true)
            {
                AddXmlChild(document, iosPods, settingOption.GetPlatform(SettingToolStrings.TEXT_IOS).install, "iosPod", "name", settingOption.GetIOSVersion());
            }

            AppendChildren(settingOption, SettingToolStrings.TEXT_ANDROID, document, androidPackages, "androidPackage", "spec", settingOption.GetAndroidVersion());
            AppendChildren(settingOption, SettingToolStrings.TEXT_IOS, document, iosPods, "iosPod", "name", settingOption.GetIOSVersion());

            SaveGamebaseAllDependenciesFile(document, callback);
        }

        public void SaveGamebaseAllDependenciesFile(XmlDocument document, SettingToolCallback.ErrorDelegate callback)
        {
            try
            {
                document.Save(DataManager.GetData<SettingToolResponse.LocalFileInfo>(DataKey.LOCAL_FILE_INFO)
                    .gamebaseAllDependencies.path);
            }
            catch (Exception e)
            {
                callback(new SettingToolError(SettingToolErrorCode.UNITY_INTERNAL_ERROR, DOMAIN, e.Message));
                return;
            }

            callback(null);
        }

        public void RemoveGamebaseAllDependencies(SettingToolCallback.ErrorDelegate callback)
        {
            var document = DataManager.GetData<XmlDocument>(DataKey.GAMEBASE_ALL_DEPENDENCIES);
            var androidPackages = document.SelectSingleNode("dependencies/androidPackages");
            var iosPods = document.SelectSingleNode("dependencies/iosPods");

            androidPackages.RemoveAll();
            iosPods.RemoveAll();

            SaveGamebaseAllDependenciesFile(document, callback);
        }

        private void AppendChildren(
            SettingOption settingOption,
            string platform,
            XmlDocument document,
            XmlNode parentNode,
            string newNodeName,
            string newAttributeName,
            string version)
        {
            foreach (var installInfo in settingOption.GetInstallInfos(platform))
            {
                AddXmlChild(document, parentNode, installInfo, newNodeName, newAttributeName, version);
            }
        }

        private void AddXmlChild(
            XmlDocument document,
            XmlNode parentNode,
            InstallInfo installInfo,
            string newNodeName,
            string newAttributeName,
            string version)
        {
            if (installInfo != null &&
                string.IsNullOrEmpty(installInfo.name) == false)
            {
                string value = installInfo.name;
                if (newNodeName == "androidPackage")
                {
                    value = string.Format("{0}:{1}", value, version);
                }
                
                XmlNode node = null;
                XmlAttribute attribute = null;
                foreach (XmlNode item in parentNode.ChildNodes)
                {
                    if (item.Attributes[newAttributeName].Value == value)
                    {
                        node = item;
                        break;
                    }
                }

                if (node == null)
                {
                    node = document.CreateElement(newNodeName);

                    attribute = document.CreateAttribute(newAttributeName);
                    attribute.Value = value;
                    
                    node.Attributes.Append(attribute);
                    parentNode.AppendChild(node);
                }
                

                if (string.IsNullOrEmpty(installInfo.version) == false)
                {
                    version = installInfo.version;
                }

                // Platform-specific version information must be written in a different way.
                switch (newNodeName)
                {
                    case "androidPackage":
                    {
                        if (installInfo.repositories != null)
                        {
                            var repositoriesNode = document.CreateElement("repositories");
                            node.AppendChild(repositoriesNode);

                            XmlElement repositoryNode;
                            foreach (var repository in installInfo.repositories)
                            {
                                repositoryNode = document.CreateElement("repository");
                                repositoryNode.InnerText = repository;
                                repositoriesNode.AppendChild(repositoryNode);
                            }
                        }
                        break;
                    }
                    case "iosPod":
                    {
                        var versionAttributes = document.CreateAttribute("version");
                        versionAttributes.Value = version;
                        node.Attributes.Append(versionAttributes);

                        if (installInfo.repositories != null)
                        {
                            var sourcesNode = document.CreateElement("sources");
                            node.AppendChild(sourcesNode);
                            XmlElement sourceNode;
                            foreach (var repository in installInfo.repositories)
                            {
                                sourceNode = document.CreateElement("source");
                                sourceNode.InnerText = repository;
                                sourcesNode.AppendChild(sourceNode);
                            }
                        }

                        break;
                    }
                }

                
            }
        }
    }
}