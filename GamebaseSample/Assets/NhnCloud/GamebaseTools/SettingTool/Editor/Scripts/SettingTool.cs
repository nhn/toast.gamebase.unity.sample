using NhnCloud.GamebaseTools.SettingTool.Data;
using NhnCloud.GamebaseTools.SettingTool.ThirdParty;
using NhnCloud.GamebaseTools.SettingTool.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace NhnCloud.GamebaseTools.SettingTool
{
    public class SettingTool : IDisposable
    {
        public const string VERSION = "2.0.0";
        private const string DOMAIN = "SettingTool";

        private DataLoader loader;
        private Version version;
        private Indicator indicator;
        private GamebaseInfo gamebaseInfo;
        private GamebaseDependencies gamebaseDependencies;
        private string gamebaseSdkPath;

        private SettingToolCallback.VoidDelegate onDeleteGamebaseSdk;
        private SettingToolCallback.DataDelegate<ProgressInfo> showProgressBar;

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

            if (loader != null)
            {
                loader.Dispose();
                loader = null;
            }

            if (indicator != null)
            {
                indicator.Dispose();
                indicator = null;
            }

            if (gamebaseInfo != null)
            {
                gamebaseInfo.Dispose();
                gamebaseInfo = null;
            }

            if (gamebaseDependencies != null)
            {
                gamebaseDependencies.Dispose();
                gamebaseDependencies = null;
            }

            FileManager.Dispose();
        }

        public void Initialize(
            SettingToolCallback.ErrorDelegate callback,
            SettingToolCallback.VoidDelegate onDeleteGamebaseSdk,
            SettingToolCallback.DataDelegate<ProgressInfo> showProgressBar)
        {
            this.onDeleteGamebaseSdk = onDeleteGamebaseSdk;
            this.showProgressBar = showProgressBar;

            loader = new DataLoader();
            loader.LoadData((error) =>
            {
                if (IsSuccess(error) == true)
                {
                    SettingToolLog.Debug("Data load was successful.", GetType(), "Initialize");

                    gamebaseSdkPath = DataManager.GetData<SettingToolResponse.LocalFileInfo>(DataKey.LOCAL_FILE_INFO).gamebaseSdk.path;

                    indicator = new Indicator();
                    indicator.Initialize();

                    gamebaseInfo = new GamebaseInfo();
                    gamebaseInfo.Initialize(ChangeGamebaseSdkDownloadStatus);

                    gamebaseDependencies = new GamebaseDependencies();

                    Multilanguage.Initialize();

                    version = new Version();

                    callback(null);
                }
                else
                {
                    callback(new SettingToolError(SettingToolErrorCode.NOT_INITIALIZED, GetType().Name, string.Empty, error));
                }
            });
        }

        public void OnDeleteGamebaseSdk()
        {
            version.CheckGamebaseSdkStatus();
        }

        #region Download Gamebase SDK
        public void DownloadGamebaseSdk(SettingToolCallback.ErrorDelegate callback)
        {
            DownloadFile(
                gamebaseSdkPath,
                Path.Combine(DataManager.GetData<string>(DataKey.CDN_URL), "GamebaseSDK-Unity.zip"),
                Path.Combine(gamebaseSdkPath, "GamebaseSDK.zip"),
                (error) =>
                {
                    if (IsSuccess(error) == true)
                    {
                        ExtractZip((extractError) =>
                        {
                            if (IsSuccess(extractError) == true)
                            {
                                gamebaseInfo.ClearCurrentVersion();
                                gamebaseInfo.SetCurrentVersion();

                                version.CheckGamebaseSdkStatus();

                                callback(null);
                            }
                            else
                            {
                                callback(extractError);
                            }
                        });
                    }
                    else
                    {
                        callback(error);
                    }
                });
        }

        public void DownloadNaverCefePlug(SettingToolCallback.ErrorDelegate callback)
        {
            SettingToolLog.Debug("Download Naver Cafe Plug.", GetType(), "DownloadNaverCefePlug");

            var naverCafePlugData = DataManager.GetData<SettingToolResponse.LaunchingData>(DataKey.LAUNCHING).launching.settingTool.naverCafePlug;
            var naverCefePlugPath = Application.dataPath.Replace("Assets", naverCafePlugData.installPath);

            DownloadFile(
                naverCefePlugPath,
                naverCafePlugData.sdkUrl,
                Path.Combine(naverCefePlugPath, Path.GetFileName(naverCafePlugData.sdkUrl)),
                (error) =>
                {
                    if (IsSuccess(error) == true)
                    {   
                        SettingToolLog.Debug(string.Format("{0} download was successful.", Path.GetFileName(naverCafePlugData.sdkUrl)), GetType(), "DownloadNaverCefePlug");

                        DownloadFile(
                            naverCefePlugPath,
                            naverCafePlugData.extensionUrl,
                            Path.Combine(naverCefePlugPath, Path.GetFileName(naverCafePlugData.extensionUrl)),
                            (downloadError) =>
                            {
                                if (IsSuccess(downloadError) == true)
                                {
                                    SettingToolLog.Debug(string.Format("{0} download was successful.", Path.GetFileName(naverCafePlugData.extensionUrl)), GetType(), "DownloadNaverCefePlug");

                                    var androidManifest = string.Format("{0}/Plugins/Android/androidManifest.xml", Application.dataPath);

                                    if (File.Exists(androidManifest) == true)
                                    {
                                        var androidManifestBackup = string.Format(
                                            "{0}/Plugins/Android/androidManifest{1}.xml",
                                            Application.dataPath,
                                            string.Format("_{0:yyyy_MM_dd_HH_mm_ss}", DateTime.Now));

                                        FileUtil.CopyFileOrDirectory(androidManifest, androidManifestBackup);
                                    }

                                    ImportUnityPackage(new List<string> { naverCefePlugPath }, callback);
                                }
                                else
                                {
                                    callback(downloadError);
                                }
                            });
                    }
                    else
                    {
                        callback(error);
                    }
                });
        }

        private void DownloadFile(string root, string remoteFileName, string localFileName, SettingToolCallback.ErrorDelegate callback)
        {
            if (Directory.Exists(root) == false)
            {
                Directory.CreateDirectory(root);
            }

            FileManager.DownloadFileToLocal(remoteFileName, localFileName, (code, message) =>
            {
                showProgressBar(new ProgressInfo() { downloadFileName = string.Empty, progress = 0 });

                switch (code)
                {
                    case FileManager.StateCode.SUCCESS:
                        {
                            callback(null);
                            break;
                        }
                    case FileManager.StateCode.FILE_NOT_FOUND_ERROR:
                        {
                            callback(new SettingToolError(SettingToolErrorCode.FILE_NOT_FOUND, DOMAIN));
                            break;
                        }
                    case FileManager.StateCode.WEB_REQUEST_ERROR:
                        {
                            callback(new SettingToolError(SettingToolErrorCode.UNITY_INTERNAL_ERROR, DOMAIN, message));
                            break;
                        }
                    case FileManager.StateCode.UNKNOWN_ERROR:
                        {
                            callback(new SettingToolError(SettingToolErrorCode.UNKNOWN_ERROR, DOMAIN, message));
                            break;
                        }
                }
            }, (progressValue) => {
                showProgressBar(new ProgressInfo() { downloadFileName = localFileName, progress = progressValue });
            });
        }

        private void ExtractZip(SettingToolCallback.ErrorDelegate callback)
        {
            EditorCoroutines.StartCoroutine(
                ZipManager.Extract(
                    Path.Combine(gamebaseSdkPath, "GamebaseSDK.zip"),
                    gamebaseSdkPath,
                    (code, message) =>
                    {
                        if (code == ZipManager.StateCode.SUCCESS)
                        {
                            callback(null);
                        }
                        else
                        {
                            callback(new SettingToolError((int)code, DOMAIN, message));
                        }
                    },
                    (stream) => { },
                    (progress) => { }
                ), this);
        }

        #endregion

        #region UpdateAllSettings
        public void UpdateAllSettings(SettingToolCallback.ErrorDelegate callback)
        {
            UpdateGamebaseDependencies(callback);
        }

        private void UpdateGamebaseDependencies(SettingToolCallback.ErrorDelegate callback)
        {
            SettingToolLog.Debug("UpdateSettings STEP1. Update gamebaseAllDependencies.xml.", GetType(), "UpdateGamebaseDependencies");

            gamebaseDependencies.UpdateGamebaseAllDependenciesFile((error) =>
            {
                if (IsSuccess(error) == true)
                {
                    UpdateUnitypackages(callback);
                }
                else
                {
                    callback(error);
                }
            });
        }

        private void UpdateUnitypackages(SettingToolCallback.ErrorDelegate callback)
        {
            SettingToolLog.Debug("UpdateSettings STEP2. Import Unitypackages", GetType(), "UpdateUnitypackages");

            var data = DataManager.GetData<SettingToolResponse.AdapterSettings>(DataKey.ADAPTER_SETTINGS);
            var unitypackageList = new List<string>()
            {
                Path.Combine(gamebaseSdkPath, data.unity.fileName)
            };

            unitypackageList = unitypackageList.Union(GetFileListByAdapterList(data.unity.authentication.adapters)).ToList();
            unitypackageList = unitypackageList.Union(GetFileListByAdapterList(data.unity.purchase.adapters)).ToList();
            unitypackageList = unitypackageList.Union(GetFileListByAdapterList(data.unity.push.adapters)).ToList();
            unitypackageList = unitypackageList.Union(GetFileListByAdapterList(data.unity.etc.adapters)).ToList();

            ImportUnityPackage(unitypackageList, callback);
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
                .Select(adapter => Path.Combine(gamebaseSdkPath, adapter.fileName)).ToList();
        }

        private void ImportUnityPackage(List<string> unitypackageDirectoryNameList, SettingToolCallback.ErrorDelegate callback)
        {
            if (unitypackageDirectoryNameList.Count == 0)
            {
                callback(new SettingToolError(SettingToolErrorCode.FILE_NOT_FOUND, DOMAIN, "UnitypackageList is empty."));
                return;
            }

            foreach (var unitypackageDirectoryName in unitypackageDirectoryNameList)
            {
                string[] unitypackages = null;

                if (Directory.Exists(unitypackageDirectoryName) == true)
                {
                    unitypackages = Directory.GetFiles(unitypackageDirectoryName, "*.unitypackage", SearchOption.TopDirectoryOnly);
                    if (unitypackages.Length == 0)
                    {
                        callback(new SettingToolError(SettingToolErrorCode.FILE_NOT_FOUND, DOMAIN, string.Format("Unitypackage is empty in {0}", unitypackageDirectoryName)));
                        return;
                    }
                }
                else
                {
                    callback(new SettingToolError(SettingToolErrorCode.UNITY_INTERNAL_ERROR, DOMAIN, string.Format("Directory not found :{0}", unitypackageDirectoryName)));
                    return;
                }
                                
                foreach (var unitypackage in unitypackages)
                {
                    SettingToolLog.Debug(string.Format("unitypackage:{0}", unitypackage), GetType(), "ImportUnityPackage");
                    AssetDatabase.ImportPackage(unitypackage, false);
                }
            }

            callback(null);
        }

        private void SaveAdapterSettingsFile(SettingToolCallback.ErrorDelegate callback)
        {
            var data = DataManager.GetData<SettingToolResponse.AdapterSettings>(DataKey.ADAPTER_SETTINGS);
            string filePath = DataManager.GetData<SettingToolResponse.LocalFileInfo>(DataKey.LOCAL_FILE_INFO).adapterSettings.path;

            if (File.Exists(filePath) == true)
            {
                JsonWriter writer = new JsonWriter
                {
                    PrettyPrint = true
                };

                JsonMapper.ToJson(data, writer);

                try
                {
                    File.WriteAllText(filePath, writer.ToString());
                    callback(null);
                }
                catch (Exception e)
                {
                    callback(new SettingToolError(SettingToolErrorCode.UNITY_INTERNAL_ERROR, DOMAIN, e.Message));
                }
            }
            else
            {
                callback(new SettingToolError(SettingToolErrorCode.FILE_NOT_FOUND, DOMAIN));
            }
        }
        #endregion

        #region ResetAllSettings
        public void ResetAllSettings(SettingToolCallback.ErrorDelegate callback)
        {
            ResetAdapterSettings(callback);
        }

        private void ResetAdapterSettings(SettingToolCallback.ErrorDelegate callback)
        {
            SettingToolLog.Debug("RemoveSettings STEP1. Reset adapterSettings.json.", GetType(), "ResetAdapterSettings");
            SaveAdapterSettingsFile((error) =>
            {
                if (IsSuccess(error) == true)
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
                if (IsSuccess(error) == true)
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
                if (IsSuccess(error) == true)
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
                if (Directory.Exists(Path.Combine(Application.dataPath, "Gamebase")) == true)
                {
                    FileUtil.DeleteFileOrDirectory(Path.Combine(Application.dataPath, "Gamebase"));
                    FileUtil.DeleteFileOrDirectory(Path.Combine(Application.dataPath, "Gamebase.meta"));
                }

                if (Directory.Exists(Application.streamingAssetsPath) == true)
                {
                    var files = Directory.GetFiles(Application.streamingAssetsPath);
                    if (files.Length == 1 && (Path.GetFileNameWithoutExtension(files[0]).Equals("Gamebase") == true))
                    {
                        FileUtil.DeleteFileOrDirectory(Application.streamingAssetsPath);
                        FileUtil.DeleteFileOrDirectory(Path.Combine(Application.streamingAssetsPath, ".meta"));
                    }
                    else
                    {
                        FileUtil.DeleteFileOrDirectory(Path.Combine(Application.streamingAssetsPath, "Gamebase"));
                        FileUtil.DeleteFileOrDirectory(Path.Combine(Application.streamingAssetsPath, "Gamebase.meta"));
                    }
                }

                if (Directory.Exists(Path.Combine(Application.dataPath, "NCSDK")) == true)
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

            gamebaseInfo.HasGamebaseSdk = false;

            callback(null);
        }
        #endregion

        public void SendIndicator(Dictionary<string, string> sendData)
        {
            indicator.Send(sendData);
        }

        private void ChangeGamebaseSdkDownloadStatus(bool isDownload)
        {
            SettingToolLog.Debug(string.Format("HAS_GAMEBASE_SDK:{0}", isDownload), GetType(), "ChangeGamebaseSdkDownloadStatus");
            if (isDownload == false)
            {
                if (onDeleteGamebaseSdk != null)
                {
                    onDeleteGamebaseSdk();
                }
            }
        }
    }
}
