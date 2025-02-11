using NhnCloud.GamebaseTools.SettingTool.Data;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace NhnCloud.GamebaseTools.SettingTool.Ui
{
    public class VersionSelectorUI : IGamebaseVersion, AdapterSelection.ISelectCondition
    {
        private class HistoryVersionUI
        {
            private SettingHistory history;
            private int selectHistory = 0;

            private string[] historyNames = null;
            private List<GamebaseVersion> historyVersionList = new List<GamebaseVersion>();
            
            public HistoryVersionUI()
            {
                selectHistory = 0;
                historyNames = null;
                historyVersionList.Clear();
                
                history = DataManager.GetData<SettingHistory>(DataKey.SETTING_HISTORY);
                if (history != null)
                {
                    List<string> nameList = new List<string>();
                        
                    foreach (var historDyata in history.histories)
                    {
                        var gamebaseVersion = historDyata.gamebaseVersion;
                        
                        if (VersionStatus.IsSupportVersion(gamebaseVersion))
                        {
                            string name = string.Format("History {3} - Unity : {0}, Android : {1}, iOS : {2}",
                                gamebaseVersion.unity,
                                gamebaseVersion.android,
                                gamebaseVersion.ios,
                                historyVersionList.Count);
                                
                            nameList.Add(name);
                            historyVersionList.Add(gamebaseVersion);
                        }
                    }

                    historyNames = nameList.ToArray();
                }
            }

            public bool HasHistory()
            {
                if (historyNames != null &&
                    historyNames.Length > 0)
                {
                    return true;
                }

                return false;
            }
            public GamebaseVersion GetVersion()
            {
                return historyVersionList[selectHistory];
            }

            public void OnGUI()
            {
                if (HasHistory())
                {
                    selectHistory = EditorGUILayout.Popup(selectHistory, historyNames, ToolStyles.Popup);    
                }
            }
        }
        
        public enum UpdateType
        {
            CURRENT,
            LASTEST,
            HISTORY,
            SELECT_VERSION,
            REMOVE,
        }

        public UpdateType Type = UpdateType.CURRENT;
        
        public int selectUnity = 0;
        public int selectAndroid = 0;
        public int selectiOS = 0;

        private GamebaseVersion installedVersion;
        private SettingToolResponse.SupportVersion supportVersion;
        private HistoryVersionUI historyVersionUI;

        public VersionSelectorUI()
        {
            installedVersion = GamebaseInfo.GetInstalledVersion();
            supportVersion = DataManager.GetData<SettingToolResponse.SupportVersion>(DataKey.SUPPOET_VERSION);

            historyVersionUI = new HistoryVersionUI();

            if (HasCurrentVersion() == false)
            {
                Type = UpdateType.LASTEST;
            }
        }

        public GamebaseVersion GetCurrentVersion()
        {
            return installedVersion;
        }

        public bool HasCurrentVersion()
        {
            return installedVersion.IsValid();
        }
        
        public string GetUnityVersion()
        {
            if (Type == UpdateType.SELECT_VERSION)
            {
                return supportVersion.unity[selectUnity];
            }
            else if (Type == UpdateType.LASTEST)
            {
                return supportVersion.GetUnityLastVersion();
            }
            else if (Type == UpdateType.HISTORY &&
                     historyVersionUI.HasHistory())
            {
                return historyVersionUI.GetVersion().GetUnityVersion();
            }
            else if (HasCurrentVersion())
            {
                return installedVersion.unity;
            } 
            else
            {
                return supportVersion.GetUnityLastVersion();
            }
        }

        public string GetAndroidVersion()
        {
            if (Type == UpdateType.SELECT_VERSION)
            {
                return supportVersion.android[selectAndroid];
            }
            else if (Type == UpdateType.LASTEST)
            {
                return supportVersion.GetAndroidLastVersion();
            }
            else if (Type == UpdateType.HISTORY &&
                     historyVersionUI.HasHistory())
            {
                return historyVersionUI.GetVersion().GetAndroidVersion();
            }
            else// if (versionType == UpdateVersionType.CURRENT_VERSION)
            {
                if (HasCurrentVersion())
                {
                    return installedVersion.android;
                }
                else
                {
                    return supportVersion.GetAndroidLastVersion();
                }
            }
        }

        public string GetIOSVersion()
        {
            if (Type == UpdateType.SELECT_VERSION)
            {
                return supportVersion.ios[selectiOS];
            }
            else if (Type == UpdateType.LASTEST)
            {
                return supportVersion.GetIOSLastVersion();
            }
            else if (Type == UpdateType.HISTORY &&
                     historyVersionUI.HasHistory())
            {
                return historyVersionUI.GetVersion().GetIOSVersion();
            }
            else// if (versionType == UpdateVersionType.CURRENT_VERSION)
            {
                if (HasCurrentVersion())
                {
                    return installedVersion.ios;
                }
                else
                {
                    return supportVersion.GetIOSLastVersion();
                }
            }
        }

        private const int PLATFORM_WIDTH = 80;
        private const int VERSION_WIDTH = 110; 
        
        public void OnGUI()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUILayout.VerticalScope(ToolStyles.padding_intent_12))
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.Space(PLATFORM_WIDTH);

                        if (HasCurrentVersion())
                        {
                            GUILayout.Label(Multilanguage.GetString("UI_CURRENT_VERSION"), ToolStyles.DefaultLabel,
                                GUILayout.Width(VERSION_WIDTH));
                        }

                        if (Type == UpdateType.LASTEST)
                        {
                            GUILayout.Label(Multilanguage.GetString("UI_LASTEST_VERSION"), ToolStyles.DefaultLabel, GUILayout.Width(VERSION_WIDTH));
                        }
                        else if (Type == UpdateType.HISTORY)
                        {
                            GUILayout.Label(Multilanguage.GetString("UI_HISTORY_VERSION"), ToolStyles.DefaultLabel, GUILayout.Width(VERSION_WIDTH));
                        }
                        else if (Type == UpdateType.SELECT_VERSION)
                        {
                            GUILayout.Label(Multilanguage.GetString("UI_SELECT_VERSION"), ToolStyles.DefaultLabel, GUILayout.Width(VERSION_WIDTH));
                        }
                    }

                    using (new EditorGUILayout.HorizontalScope())
                    {
                        using (new EditorGUILayout.VerticalScope(GUILayout.Width(PLATFORM_WIDTH)))
                        {
                            GUILayout.Label("Unity", ToolStyles.DefaultLabel);
                            GUILayout.Label("Android", ToolStyles.DefaultLabel);
                            GUILayout.Label("iOS", ToolStyles.DefaultLabel);
                        }

                        if (HasCurrentVersion())
                        {
                            using (new EditorGUILayout.VerticalScope(GUILayout.Width(VERSION_WIDTH)))
                            {
                                GUIStyle styles = ToolStyles.DefaultLabel;
                                if (Type == UpdateType.REMOVE)
                                {
                                    styles = ToolStyles.DefaultLabelRed;
                                }
                                GUILayout.Label(installedVersion.GetUnityVersion(), styles);
                                GUILayout.Label(installedVersion.GetAndroidVersion(), styles);
                                GUILayout.Label(installedVersion.GetIOSVersion(), styles);
                            }
                        }

                        using (new EditorGUILayout.VerticalScope(GUILayout.Width(VERSION_WIDTH)))
                        {
                            if (Type == UpdateType.LASTEST)
                            {
                                UpdateLabel(installedVersion.GetUnityVersion(), supportVersion.GetUnityLastVersion());
                                UpdateLabel(installedVersion.GetAndroidVersion(), supportVersion.GetAndroidLastVersion());
                                UpdateLabel(installedVersion.GetIOSVersion(), supportVersion.GetIOSLastVersion());
                            }
                            else if (Type == UpdateType.HISTORY &&
                                     historyVersionUI.HasHistory())
                            {
                                UpdateLabel(installedVersion.GetUnityVersion(), historyVersionUI.GetVersion().unity);
                                UpdateLabel(installedVersion.GetAndroidVersion(), historyVersionUI.GetVersion().android);
                                UpdateLabel(installedVersion.GetIOSVersion(), historyVersionUI.GetVersion().ios);
                            }
                            else if (Type == UpdateType.SELECT_VERSION)
                            {
                                selectUnity = EditorGUILayout.Popup(selectUnity, supportVersion.unity, ToolStyles.Popup, GUILayout.Width(110));
                                selectAndroid = EditorGUILayout.Popup(selectAndroid, supportVersion.android, ToolStyles.Popup, GUILayout.Width(110));
                                selectiOS = EditorGUILayout.Popup(selectiOS, supportVersion.ios, ToolStyles.Popup, GUILayout.Width(110));
                            }
                        }
                    }
                    
                    if (Type == UpdateType.HISTORY &&
                        historyVersionUI.HasHistory())
                    {
                        using (new EditorGUILayout.HorizontalScope(GUILayout.Width(460)))
                        {
                            historyVersionUI.OnGUI();
                        }
                    }
                    
                    if (Type == UpdateType.SELECT_VERSION)
                    {
                        if (selectUnity > 0 ||
                            selectAndroid > 0 ||
                            selectiOS > 0)
                        {
                            GUILayout.Label(Multilanguage.GetString("UI_RECOMMEND_LAST_VERSION"), ToolStyles.DefaultLabelYellow);
                        }
                    }
                    
                    using (new EditorGUILayout.HorizontalScope(GUILayout.Width(700)))
                    {
                        GUILayout.Label("VersionType : ", ToolStyles.DefaultLabel);
                    
                        bool active;
                        if (HasCurrentVersion())
                        {
                            active = Type == UpdateType.CURRENT;
                            if (GUILayout.Toggle(active, Multilanguage.GetString("UI_CURRENT_VERSION"),
                                    ToolStyles.RadioBox) != active)
                            {
                                Type = UpdateType.CURRENT;
                            }
                            GUILayout.Space(10);
                        }
                    
                        active = Type == UpdateType.LASTEST;
                        if (GUILayout.Toggle(active, Multilanguage.GetString("UI_LASTEST_VERSION"),
                                ToolStyles.RadioBox) != active)
                        {
                            Type = UpdateType.LASTEST;
                        }
                        GUILayout.Space(10);
                    
                        if (historyVersionUI.HasHistory())
                        {
                            active = Type == UpdateType.HISTORY;
                            if (GUILayout.Toggle(active, Multilanguage.GetString("UI_HISTORY_VERSION"),
                                    ToolStyles.RadioBox) != active)
                            {
                                Type = UpdateType.HISTORY;
                            }
                            GUILayout.Space(10);
                        }
                    
                        active = Type == UpdateType.SELECT_VERSION;
                        if (GUILayout.Toggle(active, Multilanguage.GetString("UI_SELECT_VERSION"),
                                ToolStyles.RadioBox) != active)
                        {
                            Type = UpdateType.SELECT_VERSION;
                        }
                        GUILayout.Space(10);

                        if (HasCurrentVersion())
                        {
                            active = Type == UpdateType.REMOVE;
                            if (GUILayout.Toggle(active, Multilanguage.GetString("UI_REMOVE"),
                                    ToolStyles.RadioBox) != active)
                            {
                                Type = UpdateType.REMOVE;
                            }
                        }
                        
                        GUILayout.FlexibleSpace();
                    }
                }
            }
        }

        public string GetPlatformVersion(string platform)
        {
            switch (platform)
            {
                case SettingToolStrings.TEXT_UNITY:
                    return GetUnityVersion();
                case SettingToolStrings.TEXT_ANDROID:
                    return GetAndroidVersion();
                case SettingToolStrings.TEXT_IOS:
                    return GetIOSVersion();
            }

            return GetUnityVersion();;
        }

        public GamebaseVersion GetVersion()
        {
            return new GamebaseVersion(GetUnityVersion(), GetAndroidVersion(), GetIOSVersion());
        }
        
        void UpdateLabel(string installedVersion, string lastestVersion)
        {
            GUIStyle labelStyle = ToolStyles.DefaultLabel;

            if (string.IsNullOrEmpty(installedVersion) == false)
            {
                int compareVersion = VersionUtility.CompareVersion(
                    installedVersion,
                    lastestVersion);
                if (compareVersion == 1)
                {
                    labelStyle = ToolStyles.DefaultLabelGreen;
                }
                else if (compareVersion == -1)
                {
                    labelStyle = ToolStyles.DefaultLabelYellow;
                }
                else
                {
                    labelStyle = ToolStyles.DefaultLabel;
                }
            }
            
            GUILayout.Label(lastestVersion, labelStyle);
        }
    }
}