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

        public void Dispose()
        {
            EditorCoroutines.StopAllCoroutines(this);
        }

        public void UpdateGamebaseAllDependenciesFile(SettingToolCallback.ErrorDelegate callback)
        {
            var adapterSettings = DataManager.GetData<SettingToolResponse.AdapterSettings>(DataKey.ADAPTER_SETTINGS);
            var document = DataManager.GetData<XmlDocument>(DataKey.GAMEBASE_ALL_DEPENDENCIES);

            var androidPackages = document.SelectSingleNode("dependencies/androidPackages");
            var iosPods = document.SelectSingleNode("dependencies/iosPods");

            androidPackages.RemoveAll();
            iosPods.RemoveAll();

            if (adapterSettings.useAndroid == true)
            {
                AddXmlChild(document, androidPackages, new SelectedAdapterInfo { name = adapterSettings.android.fileName }, "androidPackage", "spec", GamebaseInfo.GetCurrentVersion(SupportPlatform.ANDROID));
            }

            if (adapterSettings.useIOS == true)
            {
                AddXmlChild(document, iosPods, new SelectedAdapterInfo { name = adapterSettings.ios.fileName }, "iosPod", "name", GamebaseInfo.GetCurrentVersion(SupportPlatform.IOS));
            }

            AppendChildren(adapterSettings.android, document, androidPackages, "androidPackage", "spec", GamebaseInfo.GetCurrentVersion(SupportPlatform.ANDROID));
            AppendChildren(adapterSettings.ios, document, iosPods, "iosPod", "name", GamebaseInfo.GetCurrentVersion(SupportPlatform.IOS));

            EditorCoroutines.StartCoroutine(SaveGamebaseAllDependenciesFile(document, callback), this);
        }

        public IEnumerator SaveGamebaseAllDependenciesFile(XmlDocument document, SettingToolCallback.ErrorDelegate callback)
        {
            try
            {
                document.Save(DataManager.GetData<SettingToolResponse.LocalFileInfo>(DataKey.LOCAL_FILE_INFO).gamebaseAllDependencies.path);
            }
            catch (Exception e)
            {
                callback(new SettingToolError(SettingToolErrorCode.UNITY_INTERNAL_ERROR, DOMAIN, e.Message));
                yield break;
            }

            yield return new WaitForSecondsRealtime(1);
            callback(null);
        }

        public void RemoveGamebaseAllDependencies(SettingToolCallback.ErrorDelegate callback)
        {
            var document = DataManager.GetData<XmlDocument>(DataKey.GAMEBASE_ALL_DEPENDENCIES);
            var androidPackages = document.SelectSingleNode("dependencies/androidPackages");
            var iosPods = document.SelectSingleNode("dependencies/iosPods");

            androidPackages.RemoveAll();
            iosPods.RemoveAll();

            EditorCoroutines.StartCoroutine(SaveGamebaseAllDependenciesFile(document, callback), this);
        }

        private void AppendChildren(
            SettingToolResponse.AdapterSettings.Platform platform,
            XmlDocument document,
            XmlNode parentNode,
            string newNodeName,
            string newAttributeName,
            string version)
        {
            var adapterInfoList = GetSelectedAdapterInfoListByPlatform(platform);

            foreach (var adapterInfo in adapterInfoList)
            {
                AddXmlChild(document, parentNode, adapterInfo, newNodeName, newAttributeName, version);
            }
        }

        public class SelectedAdapterInfo
        {
            public string name;
            public List<string> repositories;
        }

        private void AddXmlChild(
            XmlDocument document,
            XmlNode parentNode,
            SelectedAdapterInfo adapterInfo,
            string newNodeName,
            string newAttributeName,
            string version)
        {
            var newNode = document.CreateElement(newNodeName);
            var attribute = document.CreateAttribute(newAttributeName);
            attribute.Value = adapterInfo.name;
            newNode.Attributes.Append(attribute);

            // Platform-specific version information must be written in a different way.
            switch (newNodeName)
            {
                case "androidPackage":
                    {
                        if (adapterInfo.repositories != null)
                        {
                            var repositoriesNode = document.CreateElement("repositories");
                            newNode.AppendChild(repositoriesNode);

                            XmlElement repositoryNode;

                            foreach (var repository in adapterInfo.repositories)
                            {
                                repositoryNode = document.CreateElement("repository");
                                repositoryNode.InnerText = repository;
                                repositoriesNode.AppendChild(repositoryNode);
                            }                            
                        }

                        attribute.Value = string.Format("{0}:{1}", attribute.Value, version);
                        break;
                    }
                case "iosPod":
                    {
                        var versionAttributes = document.CreateAttribute("version");
                        versionAttributes.Value = version;
                        newNode.Attributes.Append(versionAttributes);
                        break;
                    }
            }

            parentNode.AppendChild(newNode);
        }

        private List<SelectedAdapterInfo> GetSelectedAdapterInfoListByPlatform(SettingToolResponse.AdapterSettings.Platform platform)
        {
            var adapterInfoList = new List<SelectedAdapterInfo>();
            adapterInfoList.AddRange(GetSelectedAdapterInfoListByAdapterList(platform.authentication.adapters));
            adapterInfoList.AddRange(GetSelectedAdapterInfoListByAdapterList(platform.purchase.adapters));
            adapterInfoList.AddRange(GetSelectedAdapterInfoListByAdapterList(platform.push.adapters));
            adapterInfoList.AddRange(GetSelectedAdapterInfoListByAdapterList(platform.etc.adapters));

            return adapterInfoList;
        }

        private List<SelectedAdapterInfo> GetSelectedAdapterInfoListByAdapterList(List<SettingToolResponse.AdapterSettings.Platform.Category.Adapter> adapterList)
        {
            if (adapterList == null)
            {
                return new List<SelectedAdapterInfo>();
            }

            return adapterList
                .OfType<SettingToolResponse.AdapterSettings.Platform.Category.Adapter>()
                .Where(adapter => adapter.used == true)
                .Select(adapter => new SelectedAdapterInfo { name = adapter.fileName, repositories = adapter.repositories } ).ToList();
        }
    }
}