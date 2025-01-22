using NhnCloud.GamebaseTools.SettingTool.Data;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

using NhnCloud.GamebaseTools.SettingTool.Util.Ad;

namespace NhnCloud.GamebaseTools.SettingTool
{
    public class ProcessInfo
    {
        public int count;
        public int maxCount;
        
        public string title;
        public string desc;
        public float progress;

        public float GetTotalProgress()
        {
            return ((float)count  / maxCount) + (progress / maxCount);
        }
        
        public void SetCount(int count, int maxCount)
        {
            this.count = count;
            this.maxCount = maxCount;
            this.progress = 0;
            
            EditorUtility.DisplayProgressBar(title, desc, GetTotalProgress());
        }
        
        public void SetProgress(float value)
        {
            progress = value;
            
            EditorUtility.DisplayProgressBar(title, desc, GetTotalProgress());
        }

        public void Clear()
        {
            title = "";
            desc = "";
            count = 0;
            maxCount = 0;
            progress = 0;
            EditorUtility.ClearProgressBar();    
        }
        
        
    }
    
    public static class GamebaseSettingManager
    {
        private static ProcessInfo processInfo;
        private static DataLoader loader;
        private static SettingTool settingTool;
        private static Indicator indicator;

        private enum State
        {
            NONE = 0,
            INITIALING,
            INITIALIZED,
            PROCESSING,
        }

        private static State state = State.NONE;
        
        public static void Initialize(SettingToolCallback.ErrorDelegate callback)
        {
            if (state >= State.INITIALIZED)
            {
                callback(null);
                return;
            }

            if (settingTool == null)
            {
                processInfo = new ProcessInfo();
            }

            state = State.INITIALING;
            
            DataManager.Initialize();
            
            loader = new DataLoader();
            loader.LoadData(processInfo, (loadError) =>
            {
                if (IsSuccess(loadError))
                {
                    SettingToolLog.Debug("Data load was successful.", typeof(GamebaseSettingManager), "Initialize");
                    
                    if (settingTool == null)
                    {
                        settingTool = new SettingTool();
                    }
                    settingTool.Initialize(processInfo, (error)=>
                    {
                        if (IsSuccess(error))
                        {
                            if (indicator == null)
                            {
                                indicator = new Indicator();
                                indicator.Initialize();
                            }

                            AdInitialize();
                            state = State.INITIALIZED;
                        }
                        else
                        {
                            state = State.NONE;
                        }
                        callback(error);
                    });
                }
                else
                {
                    state = State.NONE;
                    callback(new SettingToolError(SettingToolErrorCode.NOT_INITIALIZED, "GamebaseSettingManager", string.Empty, loadError));
                }
            });
        }
        
        private static void AdInitialize()
        {
            const string ADVERTISEMENT_XML_NAME = "Advertisement.xml";
            var imageDownloadPath = DataManager.GetData<SettingToolResponse.LocalFileInfo>(DataKey.LOCAL_FILE_INFO).ad.downloadPath;
            var remoteUrl = Path.Combine(DataManager.GetData<SettingToolResponse.Cdn>(DataKey.CDN).url, "GamebaseSettingTool/Ad/");

            Advertisement.Initialize(
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
        
        private static void OnClickAd(string adName, string link)
        {
            var sendData = new Dictionary<string, string>()
            {
                { "ACTION", "Ad" },
                { "ACTION_DETAIL_1", adName },
                { "ACTION_DETAIL_2", link },
            };

            SendIndicator(sendData);
        }
        
        public static void Destroy()
        {
            if (settingTool != null)
            {
                settingTool.Dispose();
                settingTool = null;
            }
            
            if (indicator != null)
            {
                indicator.Dispose();
                indicator = null;
            }
            
            if (loader != null)
            {
                loader.Dispose();
                loader = null;
            }
            
            Multilanguage.Destroy();
            DataManager.Destroy();
            Advertisement.Destroy();

            state = State.NONE;
        }
        
        public static void SendIndicator(Dictionary<string, string> sendData)
        {
            indicator.Send(sendData);
        }
        
        public static void SetDebugMode(bool isDebug)
        {
            SettingToolLog.DebugLogEnabled = isDebug;
        }

        public static bool IsSuccess(SettingToolError error)
        {
            return error == null;
        }

        public static bool IsProcess()
        {
            return state == State.INITIALING || state == State.PROCESSING;
        }
        
        public static ProcessInfo GetProcessInfo()
        {
            if (IsProcess())
            {
                if(processInfo != null)
                    return processInfo;
            }

            return null;
        }

        public static void ApplySetting(SettingOption settingOption, SettingToolCallback.ErrorDelegate callback)
        {
            if(IsProcess())
                return;
            
            AssetDatabase.SaveAssets();
            
            if(settingOption.CheckPlatform() == false)
            {
                if (EditorUtility.DisplayDialog(
                        Multilanguage.GetString("POPUP_SETTING_TITLE"),
                        Multilanguage.GetString("POPUP_GAMEBASE_SETTING_DONT_HAVE_EDM4U_MESSAGE"),
                        Multilanguage.GetString("POPUP_OK"),
                        Multilanguage.GetString("POPUP_CANCEL")) == false)
                {
                    return;
                }
            }

            state = State.PROCESSING;
            settingTool.InstallGamebase(settingOption, error =>
            {
                if (SettingTool.IsSuccess(error) == true)
                {
                    if (EditorUtility.DisplayDialog(
                            Multilanguage.GetString("POPUP_SETTING_TITLE"),
                            Multilanguage.GetString("POPUP_GAMEBASE_SETTING_COMPLETE_MESSAGE"),
                            Multilanguage.GetString("POPUP_OK")) == true)
                    {
                        callback(null);
                    }
                }
                else
                {
                        
                    SettingToolLog.Error(error, typeof(GamebaseSettingManager), "ApplySetting");
                    callback(error);
                }

                state = State.NONE;
            });
        }

        public static void RemoveSetting(SettingToolCallback.ErrorDelegate callback)
        {
            if(IsProcess())
                return;
            
            if (EditorUtility.DisplayDialog(
                    Multilanguage.GetString("POPUP_SETTING_TITLE"),
                    Multilanguage.GetString("POPUP_GAMEBASE_REMOVE_MESSAGE"),
                    Multilanguage.GetString("POPUP_OK"),
                    Multilanguage.GetString("POPUP_CANCEL")))
            {
                state = State.PROCESSING;
                settingTool.RemoveGamebase(error =>
                {
                    if (error == null)
                    {
                        settingTool.ClearSelection();
                        
                        settingTool.ClearHistory();
                        
                        AssetDatabase.Refresh();
                    }
                    
                    if (SettingTool.IsSuccess(error) == true)
                    {
                        if (EditorUtility.DisplayDialog(
                                Multilanguage.GetString("POPUP_SETTING_TITLE"),
                                Multilanguage.GetString("POPUP_GAMEBASE_REMOVE_COMPLETE_MESSAGE"),
                                Multilanguage.GetString("POPUP_OK")) == true)
                        {
                            callback(null);
                        }
                    }
                    else
                    {
                        SettingToolLog.Error(error, typeof(GamebaseSettingManager), "RemoveSetting");
                        callback(error);
                    }
                    state = State.NONE;
                });
            }
        }
    }
}