using NhnCloud.GamebaseTools.SettingTool.ThirdParty;
using NhnCloud.GamebaseTools.SettingTool.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;

namespace NhnCloud.GamebaseTools.SettingTool.Data
{
    public class DataLoader : IDisposable
    {
        private const string DOMAIN = "DataLoader";

        private const string MESSAGE_DATA_ALREADY_EXISTS = "The data already exists. key:{0}";
        private const string LOCAL_FILE_INFO_PATH = "NhnCloud/GamebaseTools/SettingTool/Editor/localFileInfo.json";

        private SettingToolResponse.LocalFileInfo localfileInfo;

        public void Dispose()
        {
            EditorCoroutines.StopAllCoroutines(this);
        }

        /// <summary>
        /// Loads all data for SettingTool.
        /// 
        /// 1. <see cref="LoadLocalFileInfo"/> (Local)
        /// VO:<see cref="SettingToolResponse.LocalFileInfo"/>
        /// 
        /// 2. <see cref="LoadCdnUrl"/> (Local)
        /// VO:<see cref="SettingToolResponse.Cdn"/>
        /// 
        /// 3. <see cref="LoadLocalizedString"/> (Local)
        /// VO:<see cref="LocalizedStringVo"/>
        /// 
        /// 4. <see cref="LoadInstalledVersion"/> (Local)
        /// VO:<see cref="SettingToolResponse.InstalledVersion"/>
        /// 
        /// 5. <see cref="LoadAdapterSettings"/> (Local)
        /// VO:<see cref="SettingToolResponse.AdapterSettings"/>
        /// 
        /// 6.<see cref="LoadGamebaseAllDependencies"/> (Local)
        /// XML:<see cref="Assets/NhnCloud/GamebaseTools/SettingTool/Editor/gamebaseAllDependencies.xml"/>
        /// 
        /// 7. <see cref="LoadMasterData"/> (CDN)
        /// VO:<see cref="SettingToolResponse.Master"/>
        /// 
        /// 8. <see cref="LoadLaunchingData"/> (CDN)
        /// VO:<see cref="SettingToolResponse.LaunchingData"/>
        /// 
        /// 9. <see cref="LoadVersionData"/> (CDN)
        /// VO:<see cref="SettingToolResponse.Version"/>
        /// </summary>
        /// <param name="callback"></param>
        public void LoadData(SettingToolCallback.ErrorDelegate callback)
        {
            LoadLocalFileInfo(callback);
        }

        #region Load files and launching data
        private void LoadLocalFileInfo(SettingToolCallback.ErrorDelegate callback)
        {
            var filePath = Path.Combine(Application.dataPath, LOCAL_FILE_INFO_PATH);

            LoadFile(filePath, (jsonString, error) =>
            {
                if (SettingTool.IsSuccess(error) == true)
                {
                    try
                    {
                        localfileInfo = JsonMapper.ToObject<SettingToolResponse.LocalFileInfo>(jsonString);
                    }
                    catch (Exception e)
                    {
                        callback(new SettingToolError(SettingToolErrorCode.LIT_JSON_EXCEPTION, DOMAIN, e.Message));
                        return;
                    }

                    localfileInfo.cdn.path = Path.Combine(Application.dataPath, localfileInfo.cdn.path);
                    localfileInfo.localizedString.path = Path.Combine(Application.dataPath, localfileInfo.localizedString.path);
                    localfileInfo.installedVersion.path = Path.Combine(Application.dataPath, localfileInfo.installedVersion.path);
                    localfileInfo.adapterSettings.path = Path.Combine(Application.dataPath, localfileInfo.adapterSettings.path);
                    localfileInfo.gamebaseAllDependencies.path = Path.Combine(Application.dataPath, localfileInfo.gamebaseAllDependencies.path);
                    localfileInfo.gamebaseSdk.path = Application.dataPath.Replace("Assets", "GamebaseSDK");
                    localfileInfo.ad.downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), localfileInfo.ad.downloadPath);

                    DataManager.SetData(DataKey.LOCAL_FILE_INFO, localfileInfo);

                    LoadCdnUrl(callback);
                }
                else
                {
                    callback(new SettingToolError(SettingToolErrorCode.FAILED_TO_LOAD_LOCAL_FILE_INFO_FILE, DOMAIN, string.Empty, error));
                }
            });
        }

        private void LoadCdnUrl(SettingToolCallback.ErrorDelegate callback)
        {
            LoadFile(localfileInfo.cdn.path, (jsonString, error) =>
            {
                if (SettingTool.IsSuccess(error) == true)
                {
                    SettingToolResponse.Cdn data;

                    try
                    {
                        data = JsonMapper.ToObject<SettingToolResponse.Cdn>(jsonString);
                    }
                    catch (Exception e)
                    {
                        callback(new SettingToolError(SettingToolErrorCode.LIT_JSON_EXCEPTION, DOMAIN, e.Message));
                        return;
                    }

                    DataManager.SetData(DataKey.CDN, data);

                    LoadLocalizedString(callback);
                }
                else
                {
                    callback(new SettingToolError(SettingToolErrorCode.FAILED_TO_LOAD_CDN_FILE, DOMAIN, string.Empty, error));
                }
            });
        }

        private void LoadLocalizedString(SettingToolCallback.ErrorDelegate callback)
        {
            LoadFile(localfileInfo.localizedString.path, (jsonString, error) =>
            {
                if (SettingTool.IsSuccess(error) == true)
                {
                    Dictionary<string, LocalizedStringVo> data;

                    try
                    {
                        data = JsonMapper.ToObject<Dictionary<string, LocalizedStringVo>>(jsonString);
                    }
                    catch (Exception e)
                    {
                        callback(new SettingToolError(SettingToolErrorCode.LIT_JSON_EXCEPTION, DOMAIN, e.Message));
                        return;
                    }

                    DataManager.SetData(DataKey.LOCALIZED_STRING, data);

                    LoadInstalledVersion(callback);
                }
                else
                {
                    callback(new SettingToolError(SettingToolErrorCode.FAILED_TO_LOAD_LOCALIZED_STRING_FILE, DOMAIN, string.Empty, error));
                }
            });
        }

        private void LoadInstalledVersion(SettingToolCallback.ErrorDelegate callback)
        {
            LoadFile(localfileInfo.installedVersion.path, (jsonString, error) =>
            {
                if (SettingTool.IsSuccess(error) == true)
                {
                    SettingToolResponse.InstalledVersion data;

                    try
                    {
                        data = JsonMapper.ToObject<SettingToolResponse.InstalledVersion>(jsonString);
                    }
                    catch (Exception e)
                    {
                        callback(new SettingToolError(SettingToolErrorCode.LIT_JSON_EXCEPTION, DOMAIN, e.Message));
                        return;
                    }

                    DataManager.SetData(DataKey.INSTALLED_VERSION, data);

                    LoadAdapterSettings(callback);
                }
                else
                {
                    callback(new SettingToolError(SettingToolErrorCode.FAILED_TO_LOAD_INSTALLED_VERSION_FILE, DOMAIN, string.Empty, error));
                }
            });
        }

        private void LoadAdapterSettings(SettingToolCallback.ErrorDelegate callback)
        {
            LoadFile(localfileInfo.adapterSettings.path, (jsonString, error) =>
            {
                if (SettingTool.IsSuccess(error) == true)
                {
                    SettingToolResponse.AdapterSettings data;

                    try
                    {
                        data = JsonMapper.ToObject<SettingToolResponse.AdapterSettings>(jsonString);
                    }
                    catch (Exception e)
                    {
                        callback(new SettingToolError(SettingToolErrorCode.LIT_JSON_EXCEPTION, DOMAIN, e.Message));
                        return;
                    }

                    DataManager.SetData(DataKey.ADAPTER_SETTINGS, data);

                    LoadGamebaseAllDependencies(callback);
                }
                else
                {
                    callback(new SettingToolError(SettingToolErrorCode.FAILED_TO_LOAD_ADAPTER_SETTINGS_FILE, DOMAIN, string.Empty, error));
                }
            });
        }

        private void LoadGamebaseAllDependencies(SettingToolCallback.ErrorDelegate callback)
        {
            LoadFile(localfileInfo.gamebaseAllDependencies.path, (data, error) =>
            {
                if (SettingTool.IsSuccess(error) == true)
                {
                    XmlDocument xmlDocument;

                    try
                    {
                        xmlDocument = new XmlDocument();
                        xmlDocument.LoadXml(data);
                        
                    }
                    catch (Exception e)
                    {
                        callback(new SettingToolError(SettingToolErrorCode.UNITY_INTERNAL_ERROR, DOMAIN, e.Message));
                        return;
                    }

                    DataManager.SetData(DataKey.GAMEBASE_ALL_DEPENDENCIES, xmlDocument);

                    LoadMasterData(callback);
                }
                else
                {
                    callback(new SettingToolError(SettingToolErrorCode.FAILED_TO_LOAD_ADAPTER_SETTINGS_FILE, DOMAIN, string.Empty, error));
                }
            });
        }

        private void LoadMasterData(SettingToolCallback.ErrorDelegate callback)
        {
            LoadData<SettingToolResponse.Master>(DataKey.MASTER, (data, error) =>
            {
                if (SettingTool.IsSuccess(error) == true)
                {
                    DecodingAppKey();
                    LoadLaunchingData(callback);
                }
                else
                {
                    callback(new SettingToolError(SettingToolErrorCode.FAILED_TO_LOAD_MASTER_FILE, DOMAIN, string.Empty, error));
                }
            });
        }

        private void DecodingAppKey()
        {
            var masterData = DataManager.GetData<SettingToolResponse.Master>(DataKey.MASTER);
            if (masterData.launching.isEncoding == false)
            {
                return;
            }

            masterData.launching.appKey = DecodingString(masterData.launching.appKey);
            DataManager.SetData(DataKey.MASTER, masterData);
        }

        private string DecodingString(string str)
        {
            if (string.IsNullOrEmpty(str) == true)
            {
                return string.Empty;
            }

            var bytes = Convert.FromBase64String(str);
            return Encoding.UTF8.GetString(bytes);
        }

        private void LoadLaunchingData(SettingToolCallback.ErrorDelegate callback)
        {
            LoadData<SettingToolResponse.LaunchingData>(DataKey.LAUNCHING, (data, error) =>
            {
                if (SettingTool.IsSuccess(error) == true)
                {
                    LoadVersionData(callback);
                }
                else
                {
                    callback(new SettingToolError(SettingToolErrorCode.FAILED_TO_LOAD_LAUNCHING_DATA, DOMAIN, string.Empty, error));
                }
            });
        }

        private void LoadVersionData(SettingToolCallback.ErrorDelegate callback)
        {
            LoadData<SettingToolResponse.Version>(DataKey.VERSION, (data, error) =>
            {
                if (SettingTool.IsSuccess(error) == true)
                {
                    callback(null);
                }
                else
                {
                    callback(new SettingToolError(SettingToolErrorCode.FAILED_TO_LOAD_VERSION_FILE, DOMAIN, string.Empty, error));
                }
            });
        }
        #endregion

        private void LoadFile(string filePath, SettingToolCallback.SettingToolDelegate<string> callback)
        {
            if (File.Exists(filePath) == false)
            {
                callback(string.Empty, new SettingToolError(SettingToolErrorCode.FILE_NOT_FOUND, DOMAIN));
                return;
            }

            string data = string.Empty;

            try
            {
                data = File.ReadAllText(filePath);

                if (string.IsNullOrEmpty(data) == true)
                {
                    callback(string.Empty, new SettingToolError(SettingToolErrorCode.FILE_DATA_EMPTY, DOMAIN));
                    return;
                }

                callback(data, null);
            }
            catch (Exception e)
            {
                callback(string.Empty, new SettingToolError(SettingToolErrorCode.UNITY_INTERNAL_ERROR, DOMAIN, e.Message));
                return;
            }
        }

        private void LoadData<T>(string key, SettingToolCallback.SettingToolDelegate<T> callback)
        {
            var data = DataManager.GetData<T>(key);

            if (data != null)
            {
                SettingToolLog.Debug(string.Format(MESSAGE_DATA_ALREADY_EXISTS, key), typeof(DataLoader), "LoadData");
                callback(data, null);
                return;
            }

            var request = GetRequest(key);
            var helper = new UnityWebRequestHelper(request);

            EditorCoroutines.StartCoroutine(helper.SendWebRequest(() =>
            {
                if (string.IsNullOrEmpty(helper.GetData()) == false)
                {
                    try
                    {
                        data = JsonMapper.ToObject<T>(helper.GetData());                        
                    }
                    catch (Exception e)
                    {
                        callback(default(T), new SettingToolError(SettingToolErrorCode.LIT_JSON_EXCEPTION, DOMAIN, e.Message));
                        return;
                    }

                    DataManager.SetData(key, data);
                    callback(data, null);
                }
                else
                {
                    callback(default(T), new SettingToolError(SettingToolErrorCode.FAILED_TO_LOAD_FILE, DOMAIN));
                }
            }), this);
        }

        private UnityWebRequest GetRequest(string key)
        {
            switch (key)
            {
                case DataKey.LAUNCHING:
                    {
                        var data = DataManager.GetData<SettingToolResponse.Master>(DataKey.MASTER);
                        return UnityWebRequest.Get(string.Format("{0}/{1}/appkeys/{2}/configurations", data.launching.url, data.launching.version, data.launching.appKey));
                    }
                default:
                    {
                        return UnityWebRequest.Get(string.Format("{0}/{1}", DataManager.GetData<SettingToolResponse.Cdn>(DataKey.CDN).url, GetFileName(key)));
                    }
            }
        }

        private string GetFileName(string key)
        {
            switch (key)
            {
                case DataKey.MASTER:
                    {
                        return "GamebaseSettingTool/master.json";
                    }
                case DataKey.VERSION:
                    {
                        return "GamebaseSettingTool/version.json";
                    }
                default:
                    {
                        return string.Empty;
                    }
            }
        }
    }
}