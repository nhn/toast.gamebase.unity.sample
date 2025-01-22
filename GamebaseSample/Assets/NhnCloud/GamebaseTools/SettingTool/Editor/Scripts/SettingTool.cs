using NhnCloud.GamebaseTools.SettingTool.Data;
using NhnCloud.GamebaseTools.SettingTool.ThirdParty;
using NhnCloud.GamebaseTools.SettingTool.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace NhnCloud.GamebaseTools.SettingTool
{
    public class SettingTool : IDisposable
    {
        public const string VERSION = "3.0.0";
        private const string DOMAIN = "SettingTool";

        private VersionStatus versionStatus;
        private GamebaseInfo gamebaseInfo;
        private GamebasePackage gamebasePackacge;

        private GamebaseDependencies gamebaseDependencies;
        
        private ProcessInfo processInfo;

        public static void SetDebugMode(bool isDebug)
        {
            SettingToolLog.DebugLogEnabled = isDebug;
        }

        public static bool IsSuccess(SettingToolError error)
        {
            return error == null;
        }

        public void Dispose()
        {
            EditorCoroutines.StopAllCoroutines(this);

            if (gamebaseInfo != null)
            {
                gamebaseInfo.Dispose();
                gamebaseInfo = null;
            }

            if (gamebasePackacge != null)
            {
                gamebasePackacge.Dispose();
                gamebasePackacge = null;
            }
            
            if (gamebaseDependencies != null)
            {
                gamebaseDependencies.Dispose();
                gamebaseDependencies = null;
            }
            FileManager.Dispose();
        }

        public void Initialize(
            ProcessInfo processInfo, 
            SettingToolCallback.ErrorDelegate callback)
        {
            this.processInfo = processInfo;
            var adapterData = DataManager.GetData<AdapterData>(DataKey.ADAPTER_DATA);
            var savedSelection = DataManager.GetData<AdapterSelection>(DataKey.ADAPTER_SELECTION);
            AdapterSettings.Initialize(adapterData, savedSelection);

            gamebaseInfo = new GamebaseInfo();
            gamebaseInfo.Initialize();

            gamebasePackacge = new GamebasePackage();
            gamebasePackacge.Initialize(processInfo);

            gamebaseDependencies = new GamebaseDependencies();

            Multilanguage.Initialize();

            versionStatus = new VersionStatus();

            DataManager.SetData(DataKey.SETTING_TOOL, this);

            callback(null);
        }
        
        public void RemoveGamebase(SettingToolCallback.ErrorDelegate callback)
        {
            DeleteDirectories(error =>
            {
                DeleteGamebaseSetting(callback);
            });
        }

        public void DeleteGamebaseSetting(SettingToolCallback.ErrorDelegate callback)
        {
            gamebaseInfo.ClearInstallVersion();
            gamebasePackacge.Delete();

            gamebaseDependencies.RemoveGamebaseAllDependencies(callback);

        }
        
        delegate void InstallFunc(SettingOption settingOption, SettingToolCallback.ErrorDelegate callback);

        public void InstallGamebase(SettingOption settingOption, SettingToolCallback.ErrorDelegate callback)
        {
            InstallFunc[] installFuncs =
            {
                DownloadGamebasePackage,
                ResetAllSettings,
                UpdateAllSettings,
            };
            
            EditorCoroutines.StartCoroutine(_InstallAll(installFuncs, settingOption, callback), this);
        }
        
        private IEnumerator _InstallAll(InstallFunc[] installFuncs, SettingOption settingOption, SettingToolCallback.ErrorDelegate callback)
        {
            int count = 0;
            int max = installFuncs.Length;

            //progress = 0;
            
            SettingToolError lastError = null;
            foreach (var installFunc in installFuncs)
            {
                bool waitLoad = true;
                installFunc.Invoke(settingOption, (error) =>
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

                count++;
                processInfo.title = "Install";
                processInfo.desc = installFunc.Method.Name;
                processInfo.SetCount(count, installFuncs.Length);

                if (lastError != null)
                {
                    break;    
                }
            }

            processInfo.Clear();
            
            callback(lastError);
        }
       
        #region Download Gamebase SDK
        
        public void DownloadGamebasePackage(SettingOption settingOption, SettingToolCallback.ErrorDelegate callback)
        {
            gamebasePackacge.LoadPackageVersion(settingOption.GetUnityVersion(), (packageVersion, error) =>
            {
                if (IsSuccess(error))
                {
                    var installPackageList = gamebasePackacge.GetInstallPackagesList(settingOption);
                    
                    var option = new DownloadOption(packageVersion, installPackageList);
                    if (option.force)
                    {
                        gamebasePackacge.Delete();
                    }
            
                    
                    gamebasePackacge.DownloaGamebasePackage(option, (error) =>
                    {
                        if (IsSuccess(error))
                        {
                            callback(null);
                        }
                        else
                        {
                            callback(error);
                        }
                    });
                }
                else
                {
                    callback(error);
                }
            });
        }
        
        
        #endregion

        #region UpdateAllSettings
        
        public void UpdateAllSettings(SettingOption settingOption, SettingToolCallback.ErrorDelegate callback)
        {
            SettingHistory history = DataManager.GetData<SettingHistory>(DataKey.SETTING_HISTORY);

            var installedVersion = DataManager.GetData<GamebaseVersion>(DataKey.INSTALLED_VERSION);
            
            SettingToolLog.Debug("UpdateSettings STEP1. Update gamebaseAllDependencies.xml.", GetType(), "UpdateGamebaseDependencies");
            gamebaseDependencies.UpdateGamebaseAllDependenciesFile(settingOption, (error) =>
            {
                if (IsSuccess(error))
                {
                    gamebasePackacge.UpdateUnitypackages(settingOption, packageError =>
                    {
                        if (IsSuccess(packageError))
                        {
                            if (installedVersion.Equals(settingOption.GetGamebaseVersion()) == false)
                            {
                                gamebaseInfo.SaveInstallVersion(settingOption.GetUnityVersion(), settingOption.GetAndroidVersion(), settingOption.GetIOSVersion());

                                history.Remove(settingOption.GetGamebaseVersion());
                                history.AddSave(installedVersion);
                            }
                            
                            AssetDatabase.ImportAsset(DataManager.GetData<SettingToolResponse.LocalFileInfo>(DataKey.LOCAL_FILE_INFO)
                                .settingTool.path);
                            
                            settingOption.UpdatePlatformSetting();
                            
                            AssetDatabase.Refresh();

                            callback(null);
                        }
                        else
                        {
                            callback(packageError);
                        }
                    });
                }
                else
                {
                    callback(error);
                }
            });
        }

        public void ClearSelection()
        {
            AdapterSettings.Clear();

            string filePath = DataManager.GetData<SettingToolResponse.LocalFileInfo>(DataKey.LOCAL_FILE_INFO).adapterSelection.path;

            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);

                    string metaPath = filePath + ".meta";
                    if (File.Exists(metaPath))
                    {
                        File.Delete(metaPath);
                    }
                }
            }
            catch (Exception e)
            {
                SettingToolLog.Error(new SettingToolError(SettingToolErrorCode.UNITY_INTERNAL_ERROR, DOMAIN, e.Message), GetType(), "OnClickReset");
            }
        }

        public void ClearHistory()
        {
            DataManager.RemoveKey(DataKey.SETTING_HISTORY);
            
            string filePath = DataManager.GetData<SettingToolResponse.LocalFileInfo>(DataKey.LOCAL_FILE_INFO)
                .adapterSelection.historyPath;

            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);

                    string metaPath = filePath + ".meta";
                    if (File.Exists(metaPath))
                    {
                        File.Delete(metaPath);
                    }
                }
            }
            catch (Exception e)
            {
                SettingToolLog.Error(new SettingToolError(SettingToolErrorCode.UNITY_INTERNAL_ERROR, DOMAIN, e.Message), GetType(), "ClearHistory");
            }
        }

        private void SaveAdapterSelectionFile(SettingOption settingOption, SettingToolCallback.ErrorDelegate callback)
        {
            string filePath = DataManager.GetData<SettingToolResponse.LocalFileInfo>(DataKey.LOCAL_FILE_INFO).adapterSelection.path;

            JsonWriter writer = new JsonWriter
            {
                PrettyPrint = true
            };

            settingOption.Nomalize();
            JsonMapper.ToJson(settingOption.selection, writer);

            try
            {
                File.WriteAllText(filePath, writer.ToString());

                RemoveLagacyAdapterFile(callback);
            }
            catch (Exception e)
            {
                callback(new SettingToolError(SettingToolErrorCode.UNITY_INTERNAL_ERROR, DOMAIN, e.Message));
            }
        }

        private void RemoveLagacyAdapterFile(SettingToolCallback.ErrorDelegate callback)
        {
            string filePath = DataManager.GetData<SettingToolResponse.LocalFileInfo>(DataKey.LOCAL_FILE_INFO).legacyAdapterSetting.path;

            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    
                    string metaPath = filePath + ".meta";
                    if (File.Exists(metaPath))
                    {
                        File.Delete(metaPath);
                    }
                }
                
                callback(null);
            }
            catch (Exception e)
            {
                callback(new SettingToolError(SettingToolErrorCode.UNITY_INTERNAL_ERROR, DOMAIN, e.Message));
            }
        }
        #endregion

        #region ResetAllSettings

        public void ResetAllSettings(SettingOption settingOption, SettingToolCallback.ErrorDelegate callback)
        {
            ResetAdapterSettings(settingOption, callback);
        }

        private void ResetAdapterSettings(SettingOption settingOption, SettingToolCallback.ErrorDelegate callback)
        {
            SettingToolLog.Debug("RemoveSettings STEP1. Reset adapterSettings.json.", GetType(), "ResetAdapterSettings");
            SaveAdapterSelectionFile(settingOption, (error) =>
            {
                if (IsSuccess(error))
                {
                    ResetGamebaseDependencies(callback);
                }
                else
                {
                    callback(error);
                }
            });
        }

        private void ResetGamebaseDependencies(SettingToolCallback.ErrorDelegate callback)
        {
            SettingToolLog.Debug("RemoveSettings STEP2. Reset gamebaseAllDependencies.xml.", GetType(), "ResetGamebaseDependencies");
            gamebaseDependencies.RemoveGamebaseAllDependencies((error) =>
            {
                if (IsSuccess(error))
                {
                    DeleteDirectories(callback);
                }
                else
                {
                    callback(error);
                }
            });
        }

        private void DeleteDirectories(SettingToolCallback.ErrorDelegate callback)
        {
            SettingToolLog.Debug("RemoveSettings STEP3. Remove Gamebase SDK folder.", GetType(), "DeleteDirectories");
            DeleteDicrectoriesForUnityAdapter((error) =>
            {
                if (IsSuccess(error))
                {
                    callback(null);
                }
                else
                {
                    callback(error);
                }
            });
        }

        private void DeleteDicrectoriesForUnityAdapter(SettingToolCallback.ErrorDelegate callback)
        {
            try
            {
                if (Directory.Exists(Path.Combine(Application.dataPath, "Gamebase")))
                {
                    FileUtil.DeleteFileOrDirectory(Path.Combine(Application.dataPath, "Gamebase"));
                    FileUtil.DeleteFileOrDirectory(Path.Combine(Application.dataPath, "Gamebase.meta"));
                }

                if (Directory.Exists(Application.streamingAssetsPath))
                {
                    var files = Directory.GetFiles(Application.streamingAssetsPath);
                    if (files.Length == 1 && (Path.GetFileNameWithoutExtension(files[0]).Equals("Gamebase")))
                    {
                        FileUtil.DeleteFileOrDirectory(Application.streamingAssetsPath);
                        FileUtil.DeleteFileOrDirectory(Application.streamingAssetsPath + ".meta");
                    }
                    else
                    {
                        FileUtil.DeleteFileOrDirectory(Path.Combine(Application.streamingAssetsPath, "Gamebase"));
                        FileUtil.DeleteFileOrDirectory(Path.Combine(Application.streamingAssetsPath, "Gamebase.meta"));
                    }
                }

                if (Directory.Exists(Path.Combine(Application.dataPath, "NCSDK")))
                {
                    FileUtil.DeleteFileOrDirectory(Path.Combine(Application.dataPath, "NCSDK"));
                    FileUtil.DeleteFileOrDirectory(Path.Combine(Application.dataPath, "NCSDK.meta"));
                }
            }
            catch (Exception e)
            {
                callback(new SettingToolError(SettingToolErrorCode.UNITY_INTERNAL_ERROR, DOMAIN, e.Message));
                return;
            }

            callback(null);
        }
        #endregion
    }
}
