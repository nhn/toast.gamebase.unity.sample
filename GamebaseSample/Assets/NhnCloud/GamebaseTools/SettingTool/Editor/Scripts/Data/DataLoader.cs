using NhnCloud.GamebaseTools.SettingTool.ThirdParty;
using NhnCloud.GamebaseTools.SettingTool.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Reflection;
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
        /// VO:<see cref="LocalizedString"/>
        /// 
        /// 4. <see cref="LoadInstalledVersion"/> (Local)
        /// VO:<see cref="SettingToolResponse.InstalledVersion"/>
        /// 
        /// 5. <see cref="LoadLegacyAdapterSettings"/> (Local)
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

        delegate void LoadFunc(SettingToolCallback.ErrorDelegate callback);
        public void LoadData(ProcessInfo processInfo, SettingToolCallback.ErrorDelegate callback)
        {
            LoadFunc[] loadFuncs =
            {
                // core
                LoadLocalFileInfo,
                LoadCdnUrl,
                LoadLocalizedString,
                
                // base
                LoadMasterData,
                LoadVersionData,
                
                // ToolData
                LoadAdapterData,
                
                // settingData
                LoadInstalledVersion,
                LoadAdapterSelection,
                LoadGamebaseAllDependencies,
                
                LoadSettingHistory,
                
                // state
                LoadInstallCheckInfo,
                
                // additional
                LoadAddLocalizedString,
                
                // log
                LoadLaunchingData,
            };
            
            EditorCoroutines.StartCoroutine(_LoadAll(processInfo, loadFuncs, callback), this);
        }

        private float progress = 0;
        public float GetProgress()
        {
            return progress;
        }
        
        private IEnumerator _LoadAll(ProcessInfo processInfo, LoadFunc[] loadFuncs, SettingToolCallback.ErrorDelegate callback)
        {
            processInfo.count = 0;
            processInfo.maxCount = loadFuncs.Length;

            progress = 0;
            
            SettingToolError lastError = null;
            foreach (var loadFunc in loadFuncs)
            {
                processInfo.title = "Loading";
                processInfo.desc = string.Format("{0} ({1}/{2})", loadFunc.Method.Name, processInfo.count,
                    processInfo.maxCount);
                
                bool waitLoad = true;
                loadFunc.Invoke((error) =>
                {
                    waitLoad = false;
                    if (error != null)
                    {
                        lastError = error;
                    }
                });

                while (waitLoad)
                {
                    yield return null;
                }

                processInfo.count++;
                processInfo.SetCount(processInfo.count, processInfo.maxCount);

                if (lastError != null)
                {
                    break;    
                }
            }
            
            progress = 1;
            
            processInfo.Clear();

            callback(lastError);
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

                    localfileInfo.settingTool.path = Path.Combine("Assets", localfileInfo.settingTool.path);
                    localfileInfo.cdn.path = Path.Combine(Application.dataPath, localfileInfo.cdn.path);
                    localfileInfo.localizedString.path = Path.Combine(Application.dataPath, localfileInfo.localizedString.path);
                    localfileInfo.installedVersion.path = Path.Combine(Application.dataPath, localfileInfo.installedVersion.path);
                    localfileInfo.legacyAdapterSetting.path = Path.Combine(Application.dataPath, localfileInfo.legacyAdapterSetting.path);                    
                    localfileInfo.additionalAdapterData.path = Path.Combine(Application.dataPath, localfileInfo.additionalAdapterData.path);
                    localfileInfo.adapterSelection.path = Path.Combine(Application.dataPath, localfileInfo.adapterSelection.path);
                    localfileInfo.adapterSelection.historyPath = Path.Combine(Application.dataPath, localfileInfo.adapterSelection.historyPath);
                    localfileInfo.gamebaseAllDependencies.path = Path.Combine(Application.dataPath, localfileInfo.gamebaseAllDependencies.path);
                    localfileInfo.gamebaseSdk.path = Application.dataPath.Replace("Assets", localfileInfo.gamebaseSdk.path);
                    localfileInfo.gamebaseSdk.versionPath = Application.dataPath.Replace("Assets", localfileInfo.gamebaseSdk.versionPath);
                    localfileInfo.ad.downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), localfileInfo.ad.downloadPath);

                    DataManager.SetData(DataKey.LOCAL_FILE_INFO, localfileInfo);

                    callback(null);
                }
                else
                {
                    callback(new SettingToolError(SettingToolErrorCode.FAILED_TO_LOAD_LOCAL_FILE_INFO_FILE, DOMAIN, MethodBase.GetCurrentMethod().Name, error));
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

                    callback(null);
                }
                else
                {
                    callback(new SettingToolError(SettingToolErrorCode.FAILED_TO_LOAD_CDN_FILE, DOMAIN, MethodBase.GetCurrentMethod().Name, error));
                }
            });
        }
        private void LoadLocalizedString(SettingToolCallback.ErrorDelegate callback)
        {
            LoadFile(localfileInfo.localizedString.path, (jsonString, error) =>
            {
                if (SettingTool.IsSuccess(error) == true)
                {
                    Dictionary<string, LocalizedString> data;

                    try
                    {
                        data = JsonMapper.ToObject<Dictionary<string, LocalizedString>>(jsonString);
                    }
                    catch (Exception e)
                    {
                        callback(new SettingToolError(SettingToolErrorCode.LIT_JSON_EXCEPTION, DOMAIN, e.Message));
                        return;
                    }

                    Multilanguage.SetLocalizedStrings(data);

                    callback(null);
                }
                else
                {
                    callback(new SettingToolError(SettingToolErrorCode.FAILED_TO_LOAD_LOCALIZED_STRING_FILE, DOMAIN, MethodBase.GetCurrentMethod().Name, error));
                }
            });
        }

        private void LoadInstalledVersion(SettingToolCallback.ErrorDelegate callback)
        {
            LoadFile(localfileInfo.installedVersion.path, (jsonString, error) =>
            {
                GamebaseVersion installedVersion = null;
                if (SettingTool.IsSuccess(error) == true)
                {
                    try
                    {
                        SettingToolResponse.InstalledVersion data = JsonMapper.ToObject<SettingToolResponse.InstalledVersion>(jsonString);
                        installedVersion = new GamebaseVersion(data);
                    }
                    catch
                    {
                        
                    }
                }

                if (installedVersion == null)
                {
                    installedVersion = new GamebaseVersion();
                }

                DataManager.SetData(DataKey.INSTALLED_VERSION, installedVersion);

                callback(null);
            });
        }

        private void LoadAdapterData(SettingToolCallback.ErrorDelegate callback)
        {
            LoadData<AdapterData>(DataKey.ADAPTER_DATA, (data, error) =>
            {
                if (SettingTool.IsSuccess(error) == true)
                {
                    LoadAdditionalAdapterData();
                    callback(null);
                }
                else
                {
                    callback(new SettingToolError(SettingToolErrorCode.FAILED_TO_LOAD_ADAPTER_SETTINGS_FILE, DOMAIN, MethodBase.GetCurrentMethod().Name, error));
                }
            });
        }

        private void LoadAdditionalAdapterData()
        {
            AdapterData data = DataManager.GetData<AdapterData>(DataKey.ADAPTER_DATA);

            try
            {
                if (string.IsNullOrEmpty(localfileInfo.additionalAdapterData.path) == false &&
                Directory.Exists(localfileInfo.additionalAdapterData.path) == true)
                {
                    var adapterPaths = Directory.GetFiles(localfileInfo.additionalAdapterData.path, "*.json", SearchOption.TopDirectoryOnly);
                    foreach (var adapterPath in adapterPaths)
                    {
                        LoadFile(adapterPath, (jsonString, error) =>
                        {
                            if (SettingTool.IsSuccess(error) == true)
                            {
                                try
                                {
                                    AdapterData additional = JsonMapper.ToObject<AdapterData>(jsonString);
                                    if (additional != null)
                                    {
                                        data.Add(additional);
                                    }

                                }
                                catch (Exception e)
                                {
                                    Debug.LogError(e);
                                }
                            }
                        });
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private void LoadAdapterSelection(SettingToolCallback.ErrorDelegate callback)
        {
            AdapterSelection selection = null;

            LoadFile(localfileInfo.adapterSelection.path, (jsonString, error) =>
            {
                if (SettingTool.IsSuccess(error) == true)
                {
                    try
                    {
                        selection = JsonMapper.ToObject<AdapterSelection>(jsonString);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
            });

            if (selection == null)
            {
                LoadFile(localfileInfo.legacyAdapterSetting.path, (jsonString, error) =>
                {
                    if (SettingTool.IsSuccess(error) == true)
                    {
                        try
                        {
                            LegacyAdapterSettings legacySetting = JsonMapper.ToObject<LegacyAdapterSettings>(jsonString);
                            selection = legacySetting.Convert();
                        }
                        catch (Exception e)
                        {
                            Debug.LogError(e);
                        }
                    }
                });
            }
            
            DataManager.SetData(DataKey.ADAPTER_SELECTION, selection);

            callback(null);
        }

        private void LoadGamebaseAllDependencies(SettingToolCallback.ErrorDelegate callback)
        {
            LoadFile(localfileInfo.gamebaseAllDependencies.path, (data, error) =>
            {
                XmlDocument xmlDocument;
                if (SettingTool.IsSuccess(error) == true)
                {
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
                }
                else
                {
                    xmlDocument = GamebaseDependencies.NewXmlDocument();
                }
                
                DataManager.SetData(DataKey.GAMEBASE_ALL_DEPENDENCIES, xmlDocument);

                callback(null);
            });
        }

        private void LoadSettingHistory(SettingToolCallback.ErrorDelegate callback)
        {
            SettingHistory history = null;

            LoadFile(localfileInfo.adapterSelection.historyPath, (jsonString, error) =>
            {
                if (SettingTool.IsSuccess(error) == true)
                {
                    try
                    {
                        history = JsonMapper.ToObject<SettingHistory>(jsonString);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
                else
                {
                    history = new SettingHistory();
                }
                
                DataManager.SetData(DataKey.SETTING_HISTORY, history);

                callback(null);
            });
        }

        private void LoadInstallCheckInfo(SettingToolCallback.ErrorDelegate callback)
        {
            LoadData<InstallCheckInfo>(DataKey.INSTALL_CHECK, (data, error) =>
            {
                if (SettingTool.IsSuccess(error) == true)
                {
                    callback(null);
                }
                else
                {
                    callback(new SettingToolError(SettingToolErrorCode.FAILED_TO_LOAD_INSTALL_CHECK_FILE, DOMAIN, MethodBase.GetCurrentMethod().Name, error));
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
                    callback(null);
                }
                else
                {
                    callback(new SettingToolError(SettingToolErrorCode.FAILED_TO_LOAD_MASTER_FILE, DOMAIN, MethodBase.GetCurrentMethod().Name, error));
                }
            });
        }

        private void DecodingAppKey()
        {
            var masterData = DataManager.GetData<SettingToolResponse.Master>(DataKey.MASTER);
            if (masterData.launchingInfo.isEncoding == false)
            {
                return;
            }

            masterData.launchingInfo.appKey = DecodingString(masterData.launchingInfo.appKey);
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
                    callback(null);
                }
                else
                {
                    callback(new SettingToolError(SettingToolErrorCode.FAILED_TO_LOAD_LAUNCHING_DATA, DOMAIN, MethodBase.GetCurrentMethod().Name, error));
                }
            });
        }

        private void LoadVersionData(SettingToolCallback.ErrorDelegate callback)
        {
            LoadData<SettingToolResponse.SupportVersion>(DataKey.SUPPOET_VERSION, (data, error) =>
            {
                if (SettingTool.IsSuccess(error) == true)
                {
                    callback(null);
                }
                else
                {
                    callback(new SettingToolError(SettingToolErrorCode.FAILED_TO_LOAD_VERSION_FILE, DOMAIN, MethodBase.GetCurrentMethod().Name, error));
                }
            });
        }
        
        private void LoadAddLocalizedString(SettingToolCallback.ErrorDelegate callback)
        {
            LoadData<Dictionary<string, LocalizedString>>(DataKey.LOCALIZED_STRING, (jsonString, error) =>
            {
                if (SettingTool.IsSuccess(error) == true)
                {
                    Dictionary<string, LocalizedString> data = DataManager.GetData<Dictionary<string, LocalizedString>>(DataKey.LOCALIZED_STRING);
                    
                    if(data != null)
                    {
                        Multilanguage.AddLocalizedStrings(data);
                    }

                    callback(null);
                }
                else
                {
                    callback(new SettingToolError(SettingToolErrorCode.FAILED_TO_LOAD_LOCALIZED_STRING_FILE, DOMAIN, MethodBase.GetCurrentMethod().Name, error));
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
            request.useHttpContinue = false;
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

                        return UnityWebRequest.Get(
                            string.Format(
                                "{0}/{1}/appkeys/{2}/configurations?subKey={3}",
                                data.launchingInfo.url,
                                data.launchingInfo.version,
                                data.launchingInfo.appKey,
                                data.launchingInfo.subKey));
                    }
                default:
                    {
                        var path = DataManager.GetData<SettingToolResponse.Cdn>(DataKey.CDN).url;

                        if (path.Contains("file:///"))
                        {
                            path = "file:///" + Path.GetFullPath(path);
                        }
                        return UnityWebRequest.Get(string.Format("{0}/{1}", path, GetFileName(key)));
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
                case DataKey.SUPPOET_VERSION:
                    {
                        return "GamebaseSettingTool/supportVersion.json";
                    }
                case DataKey.ADAPTER_DATA:
                    {
                        return "GamebaseSettingTool/adapterData.json";
                    }
                case DataKey.INSTALL_CHECK:
                    {
                        return "GamebaseSettingTool/installCheck.json";
                    }
                case DataKey.LOCALIZED_STRING:
                    {
                        return "GamebaseSettingTool/localizedstring.json";
                    }
                default:
                    {
                        return string.Empty;
                    }
            }
        }
    }
}