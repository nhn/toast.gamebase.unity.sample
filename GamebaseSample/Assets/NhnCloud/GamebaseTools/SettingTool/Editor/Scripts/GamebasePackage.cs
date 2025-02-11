using NhnCloud.GamebaseTools.SettingTool.Data;
using NhnCloud.GamebaseTools.SettingTool.ThirdParty;
using NhnCloud.GamebaseTools.SettingTool.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine.Networking;
using UnityEditor;
using UnityEngine;

namespace NhnCloud.GamebaseTools.SettingTool
{
    public class DownloadOption
    {
        public DownloadOption(PackageVersion version, IEnumerable<InstallInfo> installs, bool keep = true, bool force = false)
        {
            this.version = version;
            this.installs = installs;
            this.keep = keep;
            this.force = force;
        }
        
        public PackageVersion version;
        public IEnumerable<InstallInfo> installs;
        public bool keep;
        public bool force;
    }

    public class PackageVersion
    {
        public class Version
        {
            public string name;
            public string version;
        }
        
        public string unityVersion;
        public List<Version> packages = new List<Version>();

        public PackageVersion()
        {

        }

        public PackageVersion(IEnumerable<InstallInfo> updatePackageList, PackageVersion updateVersion)
        {
            this.unityVersion = updateVersion.unityVersion;
            foreach (var updatePackage in updatePackageList)
            {
                string version = updateVersion.GetVersion(updatePackage.name);
                Add(updatePackage.name, version);
            }
        }

        public void Add(string name, string version)
        {
            packages.Add(new Version { name = name, version = version });
        }
        public string GetVersion(string name)
        {
            var package = packages.Find((p => p.name.Equals(name)));
            if(package != null)
            {
                if (string.IsNullOrEmpty(package.version))
                {
                    return unityVersion;
                }
                return package.version;
            }

            return "";
        }

        public bool IsNeedUpdated(InstallInfo updateInfo, PackageVersion updateVersion)
        {
            if (updateInfo.repositories != null)
            {
                if (GetVersion(updateInfo.name).Equals(updateVersion.GetVersion(updateInfo.name)))
                {
                    return false;
                }

                return true;
            }

            return false;
        }
    }

    public class GamebasePackage : IDisposable
    {
        private const string DOMAIN = "GamebasePackage";
       
        private string gamebaseSdkPath;
        private string gamebaseSdkVersionPath;

        private PackageVersion packageVersion;

        private ProcessInfo processInfo;

        public void Initialize(ProcessInfo processInfo)
        {
            this.processInfo = processInfo;

            var sdkPath = DataManager.GetData<SettingToolResponse.LocalFileInfo>(DataKey.LOCAL_FILE_INFO).gamebaseSdk;
            gamebaseSdkPath = sdkPath.path;
            gamebaseSdkVersionPath = sdkPath.versionPath;

            packageVersion = LoadSavedPackageVersion();
        }

        private PackageVersion LoadSavedPackageVersion()
        {
            if (Directory.Exists(gamebaseSdkPath) &&
                File.Exists(gamebaseSdkVersionPath))
            {
                try
                {
                    var jsonString = File.ReadAllText(gamebaseSdkVersionPath);

                    packageVersion = JsonMapper.ToObject<PackageVersion>(jsonString);
                }
                catch
                {
                }
            }

            return packageVersion;
        }

        public void LoadPackageVersion(string version, SettingToolCallback.SettingToolDelegate<PackageVersion> callback)
        {
            var packageInfo = AdapterSettings.GetPackageInfo();
            
            string url = packageInfo.versionPath.Replace("{version}", version);
            LoadJsonData(url, callback);
        }
        
        private void LoadJsonData<T>(string path, SettingToolCallback.SettingToolDelegate<T> callback)
        {
            if (path.Contains("file:///"))
            {
                path = "file:///" + Path.GetFullPath(path);
            }
            
            var request = UnityWebRequest.Get(path);
            var helper = new UnityWebRequestHelper(request);

            EditorCoroutines.StartCoroutine(helper.SendWebRequest(() =>
            {
                if (string.IsNullOrEmpty(helper.GetData()) == false)
                {
                    T data;
                    try
                    {
                        data = JsonMapper.ToObject<T>(helper.GetData());                        
                    }
                    catch (Exception e)
                    {
                        callback(default(T), new SettingToolError(SettingToolErrorCode.LIT_JSON_EXCEPTION, DOMAIN, e.Message));
                        return;
                    }

                    callback(data, null);
                }
                else
                {
                    callback(default(T), new SettingToolError(SettingToolErrorCode.FAILED_TO_LOAD_FILE, DOMAIN));
                }
            }), this);
        }

        public void Dispose()
        {
            EditorCoroutines.StopAllCoroutines(this);
        }

        public static bool IsSuccess(SettingToolError error)
        {
            return error == null;
        }
        
        public void DownloaGamebasePackage(DownloadOption option, SettingToolCallback.ErrorDelegate callback)
        {
            EditorCoroutines.StartCoroutine(PrepareProcess(option, callback), this);
        }

        private IEnumerator PrepareProcess(DownloadOption option, SettingToolCallback.ErrorDelegate callback)
        {
            SettingToolError settingToolError = null;
            try
            {
                var downloadPackageList = (from installPackage in option.installs
                    where IsDownload(installPackage, option)
                    select installPackage);

                if (downloadPackageList.Any())
                {
                    yield return EditorCoroutines.StartCoroutine(DownLoadProcess(downloadPackageList, option, (error) =>
                    {
                        settingToolError = error;
                    }), this);

                    if (settingToolError != null)
                    {
                        yield break;
                    }
                    
                }
                SaveUpdatePackage(option.installs, option.version);
            }
            finally
            {
                callback(settingToolError);
            }
        }
        
        private bool IsDownload(InstallInfo installInfo, DownloadOption option)
        {
            if (option.force)
            {
                return true;
            }
            
            if (installInfo.repositories == null)
            {
                return false;
            }

            if (installInfo.dirs != null)
            {
                foreach (var dir in installInfo.dirs)
                {
                    var unitypackagePath = Path.Combine(gamebaseSdkPath, dir);
                    if (Directory.Exists(unitypackagePath) == false)
                        return true;
                }
            }
            
            if (packageVersion != null)
            {
                return packageVersion.IsNeedUpdated(installInfo, option.version);
            }
            
            return true;
        }
        
        private readonly string[] packagePlatformList = new []
        {
            SettingToolStrings.TEXT_UNITY,
            SettingToolStrings.TEXT_WINDOWS,
            SettingToolStrings.TEXT_MACOS
        };
        public IEnumerable<InstallInfo> GetInstallPackagesList(SettingOption settingOption)
        {
            foreach (var package in AdapterSettings.GetAllPackages())
            {
                if (package.install != null)
                {
                    yield return package.install;
                }
            }

            foreach (var platformInfo in settingOption.selection.GetInstallPlatformInfo())
            {
                if (platformInfo.install != null)
                {
                    if(packagePlatformList.Contains(platformInfo.name))
                    {
                        yield return platformInfo.install;
                    }
                }
            }
        }
        
        public void ClearPackage(IEnumerable<InstallInfo> updatePackageList, DownloadOption option)
        {
            foreach (var installInfo in updatePackageList)
            {
                if (IsDownload(installInfo, option))
                {
                    if (installInfo.dirs != null)
                    {
                        foreach (var dir in installInfo.dirs)
                        {
                            var unitypackagePath = Path.Combine(gamebaseSdkPath, dir);
                            if (Directory.Exists(unitypackagePath))
                            {
                                Directory.Delete(unitypackagePath, true);
                            }
                        }
                    }
                }
            }
        }

        private IEnumerable<string> GetVersionRepository(InstallInfo updatePackage, DownloadOption option)
        {
            var version = option.version.GetVersion(updatePackage.name);
            foreach (var repository in updatePackage.repositories)
            {
                if (string.IsNullOrEmpty(repository) == false)
                {
                    string url = repository;
                    
                    if (string.IsNullOrEmpty(version) == false)
                    {
                        url = url.Replace("{version}", version);
                    }

                    yield return url;
                }
            }
        }
            
        private IEnumerator DownLoadProcess(IEnumerable<InstallInfo> updatePackageList, DownloadOption option, SettingToolCallback.ErrorDelegate callback)
        {
            var repositoryList = from updatePackage in updatePackageList
                                 where IsDownload(updatePackage, option)
                                 from repository in GetVersionRepository(updatePackage, option)
                                 where string.IsNullOrEmpty(repository) == false
                                 group repository by repository into g
                                 select g.Key ;

            processInfo.count = 0;
            processInfo.maxCount = repositoryList.Count();
            List<string> downloaded = new List<string>();
            foreach (var repository in repositoryList)
            {
                var path = repository;

                if (path.Contains("file:///"))
                {
                    path = "file:///" + Path.GetFullPath(path);
                }
                
                string fileName = Path.GetFileName(path);

                processInfo.title = Multilanguage.GetString("UI_TEXT_DOWNLOADING");
                processInfo.desc = string.Format("{0}:{1}",
                    Multilanguage.GetString("UI_TEXT_DOWNLOAD_FILE"),
                    Path.GetFileName(fileName));
                processInfo.SetCount(processInfo.count, processInfo.maxCount);

                SettingToolError settingToolError = null;
                yield return DownloadFile(
                    gamebaseSdkPath,
                    path,
                    Path.Combine(gamebaseSdkPath, fileName),
                    (error) =>
                    {
                        if (IsSuccess(error) == true)
                        {
                            downloaded.Add(fileName);
                        }
                        else
                        {
                            settingToolError = error; 
                        }
                    }, (progressValue) =>
                    {
                        processInfo.SetProgress(progressValue);
                    });

                if (settingToolError != null)
                {
                    callback(settingToolError);
                    yield break;
                }

                processInfo.count++;
            }
            
            ClearPackage(updatePackageList, option);
            
            yield return EditorCoroutines.StartCoroutine(UnPackProcess(downloaded), this);
        }

        private IEnumerator UnPackProcess(List<string> downloaded)
        {
            int count = 0;
            foreach (var fileName in downloaded)
            {
                processInfo.title = Multilanguage.GetString("UI_TEXT_EXTRACTING");
                processInfo.desc = string.Format("{0}", Path.GetFileName(fileName));
                processInfo.SetCount(count, downloaded.Count);
                
                bool processing = true;
                string ext = Path.GetExtension(fileName);
                if (ext.Equals(".zip"))
                {
                    ExtractZip(fileName, (extractError) =>
                    {
                        if (IsSuccess(extractError) == false)
                        {
                            SettingToolLog.Error(extractError, GetType(), "UnPackProcess");
                        }

                        processing = false;
                    });
                }
                else
                {
                    processing = false;
                }

                while (processing)
                {
                    yield return null;
                }

                count++;
            }
            
            processInfo.Clear();
        }

        private EditorCoroutines.EditorCoroutine DownloadFile(string root, string remoteFileName, string localFileName, SettingToolCallback.ErrorDelegate callback, Action<float> callbackProgress = null)
        {
            if (Directory.Exists(root) == false)
            {
                Directory.CreateDirectory(root);
            }

            return FileManager.DownloadFileToLocal(remoteFileName, localFileName, (code, message) =>
            {
                switch (code)
                {
                    case FileManager.StateCode.SUCCESS:
                        {
                            callback(null);
                            break;
                        }
                    case FileManager.StateCode.FILE_NOT_FOUND_ERROR:
                        {
                            callback(new SettingToolError(SettingToolErrorCode.FILE_NOT_FOUND, DOMAIN, message));
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
            }, callbackProgress);
        }
        

        private void ExtractZip(string fileName, SettingToolCallback.ErrorDelegate callback)
        {
            EditorCoroutines.StartCoroutine(
                ZipManager.Extract(
                    Path.Combine(gamebaseSdkPath, fileName),
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
                    (progress) =>
                    {
                        processInfo.SetProgress(progress);
                    }, null, true, true
                ), this);
        }

        public void UpdateUnitypackages(SettingOption settingOption, SettingToolCallback.ErrorDelegate callback)
        {
            SettingToolLog.Debug("UpdateSettings STEP2. Import Unitypackages", GetType(), "UpdateUnitypackages");

            var updatePackageList = GetInstallPackagesList(settingOption);

            var pacakgePathList = from updatePackage in updatePackageList
                where updatePackage.packages != null
                from package in updatePackage.packages
                group package by package into g
                select g.Key;

            ImportUnityPackage(pacakgePathList.ToList(), callback);
        }

        private void ImportUnityPackage(List<string> pacakgePathList, SettingToolCallback.ErrorDelegate callback)
        {
            if (pacakgePathList.Count == 0)
            {
                callback(new SettingToolError(SettingToolErrorCode.FILE_NOT_FOUND, DOMAIN, "UnitypackageList is empty."));
                return;
            }

            foreach (var packageDirectoryName in pacakgePathList)
            {
                var unitypackagePath = Path.Combine(gamebaseSdkPath, packageDirectoryName);

                string[] unitypackages = null;

                if (Directory.Exists(unitypackagePath) == true)
                {
                    unitypackages = Directory.GetFiles(unitypackagePath, "*.unitypackage", SearchOption.TopDirectoryOnly);
                    if (unitypackages.Length == 0)
                    {
                        callback(new SettingToolError(SettingToolErrorCode.FILE_NOT_FOUND, DOMAIN, string.Format("Unitypackage is empty in {0}", unitypackagePath)));
                        return;
                    }
                }
                else
                {
                    callback(new SettingToolError(SettingToolErrorCode.UNITY_INTERNAL_ERROR, DOMAIN, string.Format("Directory not found :{0}", unitypackagePath)));
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

        public void SaveUpdatePackage(IEnumerable<InstallInfo> updatePackageList, PackageVersion version)
        {
            File.WriteAllText(gamebaseSdkVersionPath, JsonMapper.ToJson(new PackageVersion(updatePackageList, version)));
        }

        public void Delete()
        {
            if (Directory.Exists(gamebaseSdkPath) == true)
            {
                Directory.Delete(gamebaseSdkPath, true);
            }
        }
    }
}