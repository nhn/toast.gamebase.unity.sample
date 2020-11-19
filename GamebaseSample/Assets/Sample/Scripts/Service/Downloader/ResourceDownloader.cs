using System;
using System.IO;
using Toast.SmartDownloader;
using UnityEngine;

namespace GamebaseSample
{
    public class ResourceDownloader
    {
        private static ResourceDownloader instance;

        public static ResourceDownloader Instance
        {
            get { return instance ?? (instance = new ResourceDownloader()); }
        }

        private readonly DownloadConfig config = DownloadConfig.Default;

        private const string LEAF_DIRECTORY = "SmartDLDownloads";
        private static readonly string Seperator = Path.DirectorySeparatorChar.ToString();

        private static string DownloadPath
        {
            get
            {
                return
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Seperator + LEAF_DIRECTORY;
#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
                    // MacOS Path : /Users/{USER_NAME}/Documents/SmartDLDownloads
                    Environment.GetFolderPath(Environment.SpecialFolder.Personal) + Seperator + "Documents" + Seperator + LEAF_DIRECTORY;
#else
                    Application.persistentDataPath + Seperator + LEAF_DIRECTORY;
#endif
            }
        }

        public ProgressInfo Progress
        {
            get { return SmartDl.Progress; }
        }

        public ResourceDownloader()
        {
            SetupSmartDl();
        }

        public void StartDownload(Action<bool> callback, params string[] downloadPaths)
        {
            config.FixedDownloadThreadCount = DataManager.SmartDlThreadCount;

            config.ClearSpecifyPath();

            for (int i = 0; i < downloadPaths.Length; i++)
            {
                config.AddSpecifyPath(downloadPaths[i]);
            }

            SmartDl.StartDownload(
                DataManager.Launching.smartdl.appkey,
                DataManager.Launching.smartdl.service_android,
                DownloadPath,
                result => { callback(result.IsSuccessful); });
        }

        public void StopDownload()
        {
            SmartDl.StopDownload();
        }

        public bool DeleteCaches()
        {
            try
            {
                var directories = Directory.GetDirectories(DownloadPath);
                foreach (var dir in directories)
                {
                    Directory.Delete(dir, true);
                }

                var files = Directory.GetFiles(DownloadPath, "*");
                foreach (var file in files)
                {
                    File.Delete(file);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void SetupSmartDl()
        {
            SmartDlLogger.CurrentLevel = SmartDlLogger.LogLevel.Trace;
            SmartDlLogger.OnLog += (type, log) =>
            {
                switch (type)
                {
                    case SmartDlLogger.LogLevel.Warn:
                        {
                            Debug.LogWarning(log);
                            break;
                        }

                    case SmartDlLogger.LogLevel.Error:
                        {
                            Debug.LogError(log);
                            break;
                        }

                    default:
                        {
                            Debug.Log(log);
                            break;
                        }
                }
            };
        }
    }
}