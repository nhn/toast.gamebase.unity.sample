using NhnCloud.GamebaseTools.SettingTool.Data;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace NhnCloud.GamebaseTools.SettingTool.Ui
{
    public class PageUpdate : IPage
    {
        private SettingOption settingData;
        private GamebaseVersion installedVersion;
        private GamebaseVersion lastestVersion;
        
        private SettingToolResponse.SupportVersion supportVersion;
        
        public void Initialize()
        {
            installedVersion = GamebaseInfo.GetInstalledVersion();
            var supportVersion = DataManager.GetData<SettingToolResponse.SupportVersion>(DataKey.SUPPOET_VERSION);
            
            lastestVersion = new GamebaseVersion();
            lastestVersion.unity = supportVersion.GetUnityLastVersion();
            lastestVersion.android = supportVersion.GetAndroidLastVersion();
            lastestVersion.ios = supportVersion.GetIOSLastVersion();

            if (installedVersion.IsValid())
            {
                var savedSelection = new AdapterSelection(AdapterSettings.savedSelection);
                settingData = new SettingOption(lastestVersion, savedSelection);    
            }
        }
        
        public void SetSettingData(SettingOption settingData)
        {
        }
        
        public SettingOption GetSettingData()
        {
            return settingData;
        }
        
        public PageType GetPageType()
        {
            return PageType.Update;
        }

        public string GetPageName()
        {
            return Multilanguage.GetString("UI_PAGE_UPDATE");
        }

        private Vector2 scrollPos;
        public void Draw()
        {
            bool upgradeable = false;
            using (new EditorGUILayout.VerticalScope(ToolStyles.padding_top_left_10, GUILayout.ExpandHeight(true)))
            {
                if (installedVersion.IsValid())
                {
                    upgradeable = DrawDownload();
                }
                else
                {
                    GUILayout.FlexibleSpace();
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.FlexibleSpace();
                        GUILayout.Label(Multilanguage.GetString("UI_DONT_INSTALL"), ToolStyles.WindowsName);
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.FlexibleSpace();
                }
            }
            
            Rect rect = EditorGUILayout.GetControlRect(false, 1 );
            rect.height = 1;
            EditorGUI.DrawRect(rect, new Color ( 0.5f,0.5f,0.5f, 1 ) );

            using (new EditorGUILayout.HorizontalScope())
            {
                using (var scope = new EditorGUILayout.HorizontalScope())
                {
                    var info = GamebaseSettingManager.GetProcessInfo();
                    if (info != null)
                    {
                        Rect progressRect = new Rect(scope.rect.x, scope.rect.y, scope.rect.width, 26);

                        EditorGUI.ProgressBar(progressRect, info.GetTotalProgress(), info.desc);
                        if (info.progress > 0)
                        {
                            progressRect.y += progressRect.height + 1;
                            progressRect.height = 4;
                            EditorGUI.ProgressBar(progressRect, info.progress, "");
                        }
                    }
                    
                    GUILayout.FlexibleSpace();
                }
              
                if (installedVersion.IsValid())
                {
                    if (upgradeable == false)
                    {
                        GUI.enabled = false;
                    }
                    using (new EditorGUILayout.VerticalScope())
                    {
                        if (GUILayout.Button(new GUIContent(Multilanguage.GetString("UI_BUTTON_LAST_UPDATE"), ToolStyles.icon_download),
                                ToolStyles.Button, GUILayout.Height(30)))
                        {
                            GamebaseSettingManager.ApplySetting(settingData.GetSettingOption(), error =>
                            {
                                if (SettingTool.IsSuccess(error) == true)
                                {
                                    SettingToolWindowManager.CloseAll();
                                }
                                else
                                {
                                    if (EditorUtility.DisplayDialog(
                                            Multilanguage.GetString("POPUP_SETTING_TITLE"),
                                            error.message,
                                            Multilanguage.GetString("POPUP_OK")) == true)
                                    {
                                    }
                                }
                            });
                        }
                    }
                    
                    if (upgradeable == false)
                    {
                        GUI.enabled = true;
                    }
                }
                else
                {
                    if (GUILayout.Button(Multilanguage.GetString("UI_BUTTON_GO_INSTALL"), ToolStyles.Button, GUILayout.Height(30)))
                    {
                        SettingToolWindowManager.ShowWizard();
                    }
                }
            }
        }

        public void DrawControlUI()
        {
            
        }

        private const int PLATFORM_WIDTH = 50;
        private const int VERSION_WIDTH = 110; 
        private bool DrawDownload()
        {
            bool upgradeable = false;
            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUILayout.VerticalScope())
                {
                    GUILayout.Label(Multilanguage.GetString("UI_MENU_SDK_DOWNLOAD"), ToolStyles.TitleLabel);

                    using (new EditorGUILayout.HorizontalScope())
                    {
                        using (new EditorGUILayout.VerticalScope(ToolStyles.padding_intent_12))
                        {
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                GUILayout.Space(PLATFORM_WIDTH);
                                if (installedVersion.IsValid())
                                {
                                    GUILayout.Label(Multilanguage.GetString("UI_CURRENT_VERSION"), ToolStyles.DefaultLabel, GUILayout.Width(VERSION_WIDTH));    
                                }
                                
                                if (installedVersion.Equals(lastestVersion) == false)
                                {
                                    GUILayout.Label(Multilanguage.GetString("UI_LASTEST_VERSION"), ToolStyles.DefaultLabel, GUILayout.Width(VERSION_WIDTH));
                                    
                                    upgradeable = true;
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

                                if (installedVersion.IsValid())
                                {
                                    using (new EditorGUILayout.VerticalScope(GUILayout.Width(VERSION_WIDTH)))
                                    {
                                        GUILayout.Label(installedVersion.GetUnityVersion(),
                                            ToolStyles.DefaultLabel);
                                        GUILayout.Label(installedVersion.GetAndroidVersion(),
                                            ToolStyles.DefaultLabel);
                                        GUILayout.Label(installedVersion.GetIOSVersion(), ToolStyles.DefaultLabel);
                                    }
                                }

                                if (upgradeable)
                                {
                                    using (new EditorGUILayout.VerticalScope(GUILayout.Width(VERSION_WIDTH)))
                                    {
                                        UpdateLabel(installedVersion.GetUnityVersion(),
                                            lastestVersion.GetUnityVersion());
                                        
                                        UpdateLabel(installedVersion.GetAndroidVersion(),
                                            lastestVersion.GetAndroidVersion());
                                        
                                        UpdateLabel(installedVersion.GetIOSVersion(),
                                            lastestVersion.GetIOSVersion());
                                    }
                                }
                            }
                            
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                if (GUILayout.Button(Multilanguage.GetString("UI_TEXT_LINK_GAMEBASE_UNITY_RELEASE_NOTE"),
                                        ToolStyles.LinkButton) == true)
                                {
                                    Application.OpenURL(Multilanguage.GetString("UI_TEXT_LINK_GAMEBASE_UNITY_RELEASE_NOTE_LINK"));
                                }

                                GUILayout.Space(4);
                                
                                if (GUILayout.Button(Multilanguage.GetString("UI_TEXT_LINK_GAMEBASE_UPGRADE_GUIDE"),
                                        ToolStyles.LinkButton) == true)
                                {
                                    Application.OpenURL(Multilanguage.GetString("UI_TEXT_LINK_GAMEBASE_UPGRADE_GUIDE_LINK"));
                                }
                            }
                        }
                    }
                }
                
                if (upgradeable)
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        using (new EditorGUILayout.VerticalScope())
                        {
                            GUILayout.FlexibleSpace();
                            GUILayout.Label(Multilanguage.GetString("UI_TEXT_UPDATEABLE"),
                                ToolStyles.DefaultLabelGreen);
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.FlexibleSpace();
                    }
                }
                else
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.FlexibleSpace();
                        using (new EditorGUILayout.VerticalScope())
                        {
                            GUILayout.FlexibleSpace();
                            GUILayout.Label(Multilanguage.GetString("UI_TEXT_ALREADY_LASTVERSION"),
                                ToolStyles.DefaultLabel);
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.FlexibleSpace();
                    }
                }
            }

            return upgradeable;
        }

        void UpdateLabel(string installedVersion, string lastestVersion)
        {
            GUIStyle labelStyle = ToolStyles.DefaultLabel;
            
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
            GUILayout.Label(lastestVersion, labelStyle);
        }
    }
}