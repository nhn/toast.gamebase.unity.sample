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
                AddXmlChild(document, androidPackages, adapterSettings.android.fileName, "androidPackage", "spec", GamebaseInfo.GetCurrentVersion(EditorPrefsKey.ANDROID_CURRENT_VERSION));
            }

            if (adapterSettings.useIOS == true)
            {
                AddXmlChild(document, iosPods, adapterSettings.ios.fileName, "iosPod", "name", GamebaseInfo.GetCurrentVersion(EditorPrefsKey.IOS_CURRENT_VERSION));
            }

            AppendChildren(adapterSettings.android, document, androidPackages, "androidPackage", "spec", GamebaseInfo.GetCurrentVersion(EditorPrefsKey.ANDROID_CURRENT_VERSION));
            AppendChildren(adapterSettings.ios, document, iosPods, "iosPod", "name", GamebaseInfo.GetCurrentVersion(EditorPrefsKey.IOS_CURRENT_VERSION));

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
            var fileNameList = GetFileListByPlatform(platform);

            foreach (var fileName in fileNameList)
            {
                AddXmlChild(document, parentNode, fileName, newNodeName, newAttributeName, version);
            }
        }

        private void AddXmlChild(
            XmlDocument document,
            XmlNode parentNode,
            string fileName,
            string newNodeName,
            string newAttributeName,
            string version)
        {
            var newNode = document.CreateElement(newNodeName);
            var attribute = document.CreateAttribute(newAttributeName);
            attribute.Value = fileName;
            newNode.Attributes.Append(attribute);

            // Platform-specific version information must be written in a different way.
            switch (newNodeName)
            {
                case "androidPackage":
                    {
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

        private List<string> GetFileListByPlatform(SettingToolResponse.AdapterSettings.Platform platform)
        {
            List<string> fileNameList = new List<string>();

            fileNameList = fileNameList.Union(GetFileListByAdapterList(platform.authentication.adapters)).ToList();
            fileNameList = fileNameList.Union(GetFileListByAdapterList(platform.purchase.adapters)).ToList();
            fileNameList = fileNameList.Union(GetFileListByAdapterList(platform.push.adapters)).ToList();
            fileNameList = fileNameList.Union(GetFileListByAdapterList(platform.etc.adapters)).ToList();

            return fileNameList;
        }

        private List<string> GetFileListByAdapterList(List<SettingToolResponse.AdapterSettings.Platform.Category.Adapter> adapterList)
        {
            if (adapterList == null)
            {
                return new List<string>();
            }

            return adapterList
                .OfType<SettingToolResponse.AdapterSettings.Platform.Category.Adapter>()
                .Where(adapter => adapter.used == true)
                .Select(adapter => adapter.fileName).ToList();
        }
    }
}