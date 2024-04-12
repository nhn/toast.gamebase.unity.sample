using NhnCloud.GamebaseTools.SettingTool.Data;
using NhnCloud.GamebaseTools.SettingTool.Ui;
using NhnCloud.GamebaseTools.SettingTool.Util.Ad;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace NhnCloud.GamebaseTools.SettingTool
{
    [InitializeOnLoad]
    public class SettingToolWindow : EditorWindow
    {
        private const string DOMAIN = "SDKSettingToolWindow";
        private const string ADVERTISEMENT_XML_NAME = "Advertisement.xml";

        private readonly object lockObject = new object();

        private static SettingToolWindow window;

        private SettingTool settingTool;
        private SettingToolUi ui;

        [MenuItem("Tools/NhnCloud/Gamebase/SettingTool/Settings")]
        public static void ShowWindow()
        {
            //SettingTool.SetDebugMode(true);
            window = GetWindowWithRect<SettingToolWindow>(new Rect(100, 100, 1024, 768), true, "Gamebase Settings");
        }

        private void OnDestroy()
        {
            if (settingTool != null)
            {
                settingTool.Dispose();
            }

            if (ui != null)
            {
                ui.Dispose();
            }

            Advertisement.Destroy();
            Multilanguage.Destroy();
            DataManager.Destroy();
        }

        private void Awake()
        {
            settingTool = new SettingTool();
            ui = new SettingToolUi();

            Initialize();
        }

        private void OnGUI()
        {
            lock (lockObject)
            {
                if (ui != null)
                {
                    ui.Draw();
                    Advertisement.Draw();
                }
            }
        }

        private void Initialize()
        {
            DataManager.Initialize();

            settingTool.Initialize((error) =>
            {
                if (SettingTool.IsSuccess(error) == true)
                {
                    SettingToolLog.Debug("The SettingTool initialization was successful.", GetType(), "Initialize");

                    ui.Initialize(
                        OnClickGamebaseSdkDownload,
                        OnClickNaverCefePlugDownload,
                        OnClickSetting,
                        OnClickRemove
                    );

                    AdInitialize();
                }
                else
                {
                    SettingToolLog.Error(error, GetType(), "Initialize");
                }
            }, OnDeleteGamebaseSdk, ShowProgressBar);
        }

        private void OnClickGamebaseSdkDownload()
        {
            settingTool.DownloadGamebaseSdk((error) =>
            {
                if (SettingTool.IsSuccess(error) == true)
                {
                    SettingToolLog.Debug("Gamebase SDK download was successful.", GetType(), "OnClickGamebaseSdkDownload");
                }
                else
                {
                    SettingToolLog.Error(error, GetType(), "OnClickGamebaseSdkDownload");
                }
            });
        }

        private void OnClickNaverCefePlugDownload()
        {
            settingTool.DownloadNaverCefePlug((error) =>
            {
                if (SettingTool.IsSuccess(error) == true)
                {
                    SettingToolLog.Debug("Naver Cefe Plug download was successful.", GetType(), "OnClickNaverCefePlugDownload");
                }
                else
                {
                    SettingToolLog.Error(error, GetType(), "OnClickNaverCefePlugDownload");
                }

                CloseEditorWindow();
            });
        }

        private void OnClickSetting()
        {
            settingTool.ResetAllSettings((error) =>
            {
                if (SettingTool.IsSuccess(error) == true)
                {
                    SettingToolLog.Debug("Remove all settings in SettingTool was successful.", GetType(), "OnClickSetting");

                    settingTool.UpdateAllSettings((updateError) =>
                    {
                        if (SettingTool.IsSuccess(updateError) == true)
                        {
                            SettingToolLog.Debug("Gamebase adapter setup was successful.", GetType(), "OnClickSetting");
                        }
                        else
                        {
                            SettingToolLog.Error(updateError, GetType(), "OnClickSetting");
                        }

                        CloseEditorWindow();
                    });
                }
                else
                {
                    SettingToolLog.Error(error, GetType(), "OnClickSetting");
                }
            });
        }

        private void OnClickRemove()
        {
            ui.RemoveSetting();
            settingTool.ResetAllSettings((error) =>
            {
                if (SettingTool.IsSuccess(error) == true)
                {
                    SettingToolLog.Debug("Remove all settings in SettingTool was successful.", GetType(), "OnClickRemove");
                }
                else
                {
                    SettingToolLog.Error(error, GetType(), "OnClickRemove");
                }

                CloseEditorWindow();
            });
        }

        private void OnClickAd(string adName, string link)
        {
            var sendData = new Dictionary<string, string>()
            {
                { "ACTION", "Ad" },
                { "ACTION_DETAIL_1", adName },
                { "ACTION_DETAIL_2", link },
            };

            settingTool.SendIndicator(sendData);
        }

        private void ShowProgressBar(ProgressInfo progressInfo)
        {
            ui.ShowProgressBar(progressInfo.downloadFileName, progressInfo.progress);
            Repaint();
        }

        /// <summary>
        /// User delete GamebaseSDK directory.
        /// </summary>
        private void OnDeleteGamebaseSdk()
        {
            SettingToolLog.Warn("User deleted GamebaseSDK directory when SettingTool is opened.", GetType(), "OnDeleteGamebaseSdk");
            settingTool.OnDeleteGamebaseSdk();
        }

        public void CloseEditorWindow()
        {
            lock (lockObject)
            {
                AssetDatabase.Refresh();

                if (window != null)
                {
                    window.Close();
                }
            }
        }

        private void AdInitialize()
        {
            var imageDownloadPath = DataManager.GetData<SettingToolResponse.LocalFileInfo>(DataKey.LOCAL_FILE_INFO).ad.downloadPath;
            var remoteUrl = Path.Combine(DataManager.GetData<SettingToolResponse.Cdn>(DataKey.CDN).url, "GamebaseSettingTool/Ad/");

            Advertisement.Initialize(
                window, 
                new Rect(790, 61, 210, 118),
                new AdvertisementConfigurations(
                    remoteUrl, 
                    imageDownloadPath, 
                    ADVERTISEMENT_XML_NAME, 
                    Multilanguage.GetSupportLanguages()),
                Multilanguage.GetSupportLanguages()[Multilanguage.SelectedLanguageIndex]);

            Advertisement.SetSelectAdvertisementInfoCallback((adName, link) =>
            {
                OnClickAd(adName, link);
            });
        }
    }
}