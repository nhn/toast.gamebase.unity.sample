using NhnCloud.GamebaseTools.SettingTool.Data;
using UnityEditor;
using UnityEngine;

using System.Linq;

namespace NhnCloud.GamebaseTools.SettingTool.Ui
{
    public class PageState : IPage
    {
        private Vector2 scrollPos;

        private AdapterSelection savedSelection;
        
        private SettingOption settingData;
        
        private GamebaseVersion installedVersion;
        private GamebaseVersion lastestVersion;
        
        private RequireSettingUI requireSettingUI = new RequireSettingUI();
  
        public void Initialize()
        {
        }
        
        public void SetSettingData(SettingOption settingData)
        {
            this.settingData = settingData;
            
            var supportVersion = DataManager.GetData<SettingToolResponse.SupportVersion>(DataKey.SUPPOET_VERSION);
            
            installedVersion = GamebaseInfo.GetInstalledVersion();
            
            lastestVersion = new GamebaseVersion();
            lastestVersion.unity = supportVersion.GetUnityLastVersion();
            lastestVersion.android = supportVersion.GetAndroidLastVersion();
            lastestVersion.ios = supportVersion.GetIOSLastVersion();
        }
        
        public SettingOption GetSettingData()
        {
            return settingData;
        }

        public PageType GetPageType()
        {
            return PageType.State;
        }

        public string GetPageName()
        {
            return Multilanguage.GetString("UI_PAGE_STATE");
        }

        public void Draw()
        {
            using (new EditorGUILayout.VerticalScope(ToolStyles.Box))
            {
                using (new EditorGUILayout.VerticalScope(ToolStyles.padding_top_left_10, GUILayout.ExpandHeight(true)))
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        DrawVersion();
                    }

                    GUILayout.Space(8);

                    //if (pageController.IsFirst() == false)
                    {
                        DrawCategoryAdapterAllContainer();
                        
                        GUILayout.Space(8);
                        
                        GUILayout.Label(Multilanguage.GetString("UI_MENU_REQUIRE_SETTING"), ToolStyles.TitleLabel);
            
                        bool needEDM4U = false;
                        foreach (var platform in AdapterSettings.GetAllPlatforms())
                        {
                            if (settingData.IsActivePlatform(platform.name))
                            {
                                if (platform.IsNeedCheck())
                                {
                                    needEDM4U = true;
                                    break;
                                }
                            }
                        }
            
                        using (new EditorGUILayout.VerticalScope(ToolStyles.padding_intent_12))
                        {
                            GUILayout.Label(Multilanguage.GetString("UI_TEXT_CHECK_NEED_INSTALLED"),
                                ToolStyles.DefaultLabel);

                            using (new EditorGUILayout.HorizontalScope())
                            {
                                if (GUILayout.Button(Multilanguage.GetString("UI_TEXT_LINK_SETTING_TOOL_GUIDE"),
                                        ToolStyles.LinkButton) == true)
                                {
                                    Application.OpenURL(
                                        Multilanguage.GetString("UI_TEXT_LINK_SETTING_TOOL_GUIDE_LINK"));
                                }

                                if (needEDM4U)
                                {
                                    GUILayout.Space(4);
                                    
                                    if (GUILayout.Button(Multilanguage.GetString("UI_TEXT_LINK_EDM4U_DOWNLOAD"),
                                            ToolStyles.LinkButton) == true)
                                    {
                                        Application.OpenURL(
                                            Multilanguage.GetString("UI_TEXT_LINK_EDM4U_DOWNLOAD_LINK"));
                                    }
                                }
                    
                                GUILayout.FlexibleSpace();
                            }
                        }
                        
                        requireSettingUI.Draw(settingData);
                    }
                    
                    GUILayout.FlexibleSpace();
                }

                using (new EditorGUILayout.HorizontalScope(ToolStyles.Box))
                {
                    if (GUILayout.Button(Multilanguage.GetString("UI_BUTTON_REMOVE"), ToolStyles.SizeButton))
                    {
                        GamebaseSettingManager.RemoveSetting(error =>
                        {
                            //pageController.Close();    
                        });
                    }
                }    
            }
        }

        public void DrawControlUI()
        {
        }

        void DrawVersion()
        {
            using (new EditorGUILayout.VerticalScope())
            {
                GUILayout.Label(Multilanguage.GetString("UI_MENU_SDK_DOWNLOAD"), ToolStyles.TitleLabel);

                using (new EditorGUILayout.HorizontalScope())
                {
                    using (new EditorGUILayout.VerticalScope(ToolStyles.padding_intent_12))
                    {
                        bool upgradeable = false;
                        if (installedVersion.IsValid())
                        {
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                GUILayout.Label("- Unity", ToolStyles.DefaultLabel, GUILayout.Width(80));
                                GUILayout.Label(installedVersion.GetUnityVersion(), ToolStyles.DefaultLabel, GUILayout.Width(80));

                                if(VersionUtility.CompareVersion(installedVersion.GetUnityVersion(), lastestVersion.GetUnityVersion()) == 1)
                                {
                                    GUILayout.Label("->", ToolStyles.DefaultLabel, GUILayout.Width(80));
                                    GUILayout.Label(lastestVersion.GetUnityVersion(), ToolStyles.DefaultLabelGreen);
                                    upgradeable = true;
                                }
                            }
                            
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                GUILayout.Label("- Andorid", ToolStyles.DefaultLabel, GUILayout.Width(80));
                                GUILayout.Label(installedVersion.GetAndroidVersion(), ToolStyles.DefaultLabel, GUILayout.Width(80));

                                if(VersionUtility.CompareVersion(installedVersion.GetAndroidVersion(), lastestVersion.GetAndroidVersion()) == 1)
                                {
                                    GUILayout.Label("->", ToolStyles.DefaultLabel, GUILayout.Width(80));
                                    GUILayout.Label(lastestVersion.GetAndroidVersion(), ToolStyles.DefaultLabelGreen);
                                    upgradeable = true;
                                }
                            }
                            
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                GUILayout.Label("- iOS", ToolStyles.DefaultLabel, GUILayout.Width(80));
                                GUILayout.Label(installedVersion.GetIOSVersion(), ToolStyles.DefaultLabel, GUILayout.Width(80));

                                if(VersionUtility.CompareVersion(installedVersion.GetIOSVersion(), lastestVersion.GetIOSVersion()) == 1)
                                {
                                    GUILayout.Label("->", ToolStyles.DefaultLabel, GUILayout.Width(80));
                                    GUILayout.Label(lastestVersion.GetIOSVersion(), ToolStyles.DefaultLabelGreen);
                                    upgradeable = true;
                                }
                            }
                        }
                        else
                        {
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                GUILayout.Label("- Unity", ToolStyles.DefaultLabel, GUILayout.Width(80));
                                GUILayout.Label(lastestVersion.GetUnityVersion(), ToolStyles.DefaultLabel, GUILayout.Width(80));
                            }
                            
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                GUILayout.Label("- Andorid", ToolStyles.DefaultLabel, GUILayout.Width(80));
                                GUILayout.Label(lastestVersion.GetAndroidVersion(), ToolStyles.DefaultLabel, GUILayout.Width(80));
                            }
                            
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                GUILayout.Label("- iOS", ToolStyles.DefaultLabel, GUILayout.Width(80));
                                GUILayout.Label(lastestVersion.GetIOSVersion(), ToolStyles.DefaultLabel, GUILayout.Width(80));
                            }
                            
                            upgradeable = true;
                        }
                        
                        if (upgradeable)
                        {
                            GUILayout.Label(Multilanguage.GetString("UI_TEXT_UPDATEABLE"), ToolStyles.DefaultLabelGreen);

                            using (new EditorGUILayout.HorizontalScope())
                            {
                                if (GUILayout.Button(
                                        Multilanguage.GetString("UI_TEXT_LINK_GAMEBASE_UNITY_RELEASE_NOTE"),
                                        ToolStyles.LinkButton) == true)
                                {
                                    Application.OpenURL(
                                        Multilanguage.GetString("UI_TEXT_LINK_GAMEBASE_UNITY_RELEASE_NOTE_LINK"));
                                }

                                if (GUILayout.Button(Multilanguage.GetString("UI_TEXT_LINK_GAMEBASE_UPGRADE_GUIDE"),
                                        ToolStyles.LinkButton) == true)
                                {
                                    Application.OpenURL(
                                        Multilanguage.GetString("UI_TEXT_LINK_GAMEBASE_UPGRADE_GUIDE_LINK"));
                                }
                            }
                        }
                    }
                    
                    
                }
            }
        }
        
        private void DrawCategoryAdapterAllContainer()
        {
            GUILayout.Label(Multilanguage.GetString("UI_MENU_CURRENT_SETTING"), ToolStyles.TitleLabel);

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUI.skin.box);
            {
                EditorGUILayout.BeginVertical();
                {
                    foreach (var category in AdapterSettings.GetAllCategorys())
                    {
                        DrawCategoryAdapterList(category);
                    }


                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.EndScrollView();
            }
        }

        private void DrawCategoryAdapterList(AdapterCategory catogoryData)
        {
            EditorGUILayout.BeginVertical();
            {
                
                var selectedAdapters = from adapter in catogoryData.adapters
                                    where settingData.IsUse(adapter)
                                    select adapter;

                if (selectedAdapters.Count() > 0)
                {
                    EditorGUILayout.BeginHorizontal(ToolStyles.MiniBox);

                    GUILayout.Label(catogoryData.GetDisplayName(), ToolStyles.BoldLabel);

                    EditorGUILayout.EndHorizontal();

                    foreach (var adapter in selectedAdapters)
                    {
                        DrawCategoryAdapter(adapter);
                    }
                }

                EditorGUILayout.EndVertical();
            }
        }

        private void DrawCategoryAdapter(Adapter adapter)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label(adapter.GetDisplayName(), GUILayout.Width(160));
                
                if (adapter.IsUnity() == false)
                {
                    foreach (var platform in settingData.GetActivePlatforms())
                    {
                        if (settingData.IsActiveSelected(adapter, platform.name) == true)
                        {
                            if (settingData.IsNeedUpdate(adapter, platform.name) == false)
                            {
                                GUILayout.Label(platform.name, ToolStyles.DefaultLabel, GUILayout.Width(80));
                            }
                            else
                            {
                                GUILayout.Label(platform.name + " (-)", ToolStyles.DefaultLabelRed, GUILayout.Width(80));
                            }
                        }
                        else
                        {
                            GUILayout.Label("-", ToolStyles.DefaultLabel, GUILayout.Width(80));
                        }
                    }
                }
            }
            DrawExtraInfoUI(adapter);
        }
        
        private void DrawExtraInfoUI(Adapter adapter)
        {
            if (adapter.extras != null && adapter.extras.Count > 0)
            {
                var showExtras = from extra in adapter.extras
                    where IsShowExtraInfo(adapter, extra)
                    select extra;

                if (showExtras.Count() > 0)
                {
                    using (new EditorGUILayout.VerticalScope(ToolStyles.padding_intent_24))
                    {
                        using (new EditorGUILayout.VerticalScope(ToolStyles.MiniBox))
                        {
                            foreach (var extra in showExtras)
                            {
                                switch (extra.type)
                                {
                                    case ExtraInfo.EXTRA_KEY_CHECK_TYPE:
                                        if (string.IsNullOrEmpty(extra.value) == false)
                                        {
                                            if (System.Type.GetType(extra.value) != null)
                                            {
                                                GUILayout.Label(Multilanguage.GetString(extra.text),
                                                    ToolStyles.CheckLabel);
                                            }
                                            else
                                            {
                                                GUILayout.Label(Multilanguage.GetString(extra.text),
                                                    ToolStyles.XLabel);
                                            }
                                        }

                                        break;

                                    case ExtraInfo.EXTRA_KEY_UNITY_VERSION:
                                    case ExtraInfo.EXTRA_KEY_SDK_VERSION:
                                    case ExtraInfo.EXTRA_KEY_ADAPTER:
                                    case ExtraInfo.EXTRA_KEY_REMOVE_TYPE:
                                        string text = Multilanguage.GetString(extra.text);
                                        GUILayout.Label(text, ToolStyles.WarningLabel);
                                        break;
                                    
                                    
                                }
                            }
                        }
                    }
                }
            }
        }
        
        private bool IsShowExtraInfo(Adapter adapter, ExtraInfo extraInfo)
        {
            if (extraInfo.type.Equals(ExtraInfo.EXTRA_KEY_UNITY_VERSION) == false &&
                extraInfo.type.Equals(ExtraInfo.EXTRA_KEY_SDK_VERSION) == false &&
                extraInfo.type.Equals(ExtraInfo.EXTRA_KEY_CHECK_TYPE)  == false &&
                extraInfo.type.Equals(ExtraInfo.EXTRA_KEY_REMOVE_TYPE)  == false &&
                extraInfo.type.Equals(ExtraInfo.EXTRA_KEY_ADAPTER)  == false)
            {
                return false;
            }
            
            
            if (extraInfo.platform != null)
            {
                if (settingData.IsActiveSelected(adapter, extraInfo.platform))
                {
                    return extraInfo.CheckExtra(settingData);
                }
                else
                {
                    return false;
                }
            }
            
            return true;
        }

        private void DrawBlankAdapter(string title)
        {
            EditorGUILayout.BeginVertical();
            {
                GUILayout.Label("No adapter exists.", ToolStyles.AdapterCategory);

                EditorGUILayout.EndVertical();
            }
        }
    }
}