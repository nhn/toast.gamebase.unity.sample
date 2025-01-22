using NhnCloud.GamebaseTools.SettingTool.Data;
using UnityEditor;
using UnityEngine;

using System.Linq;

namespace NhnCloud.GamebaseTools.SettingTool.Ui
{
    public class PageInstall : IPage
    {
        private Vector2 scrollPos;

        private AdapterSelection savedSelection;
        
        private SettingOption settingData;
        
        private RequireSettingUI requireSettingUI = new RequireSettingUI();
  
        public void Initialize()
        {
        }
        
        public void SetSettingData(SettingOption settingData)
        {
            this.settingData = settingData;
        }
        
        public SettingOption GetSettingData()
        {
            return settingData;
        }

        public PageType GetPageType()
        {
            return PageType.Install;
        }

        public string GetPageName()
        {
            return Multilanguage.GetString("UI_PAGE_INSTALL");
        }

        public void Draw()
        {
            using (new EditorGUILayout.VerticalScope(ToolStyles.padding_top_left_10, GUILayout.ExpandHeight(true)))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    DrawDownload();
                }

                GUILayout.Space(8);

                DrawCategoryAdapterAllContainer();
                
                requireSettingUI.Draw(settingData);
            }
        }

        public void DrawControlUI()
        {
            if (GUILayout.Button(new GUIContent(Multilanguage.GetString("UI_BUTTON_INSTALL"), ToolStyles.icon_setting), ToolStyles.Button, GUILayout.Height(30)))
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

        void DrawDownload()
        {
            using (new EditorGUILayout.VerticalScope())
            {
                GUILayout.Label(Multilanguage.GetString("UI_MENU_SDK_DOWNLOAD"), ToolStyles.TitleLabel);

                using (new EditorGUILayout.HorizontalScope())
                {
                    using (new EditorGUILayout.VerticalScope(ToolStyles.padding_intent_12, GUILayout.Width(300)))
                    {
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            GUILayout.Label("Unity : ", ToolStyles.DefaultLabel, GUILayout.Width(80));
                            GUILayout.Label(settingData.GetUnityVersion(), ToolStyles.DefaultLabel);
                        }

                        using (new EditorGUILayout.HorizontalScope())
                        {
                            GUILayout.Label("Android : ", ToolStyles.DefaultLabel, GUILayout.Width(80));
                            GUILayout.Label(settingData.GetAndroidVersion(), ToolStyles.DefaultLabel);
                        }

                        using (new EditorGUILayout.HorizontalScope())
                        {
                            GUILayout.Label("iOS : ", ToolStyles.DefaultLabel, GUILayout.Width(80));
                            GUILayout.Label(settingData.GetIOSVersion(), ToolStyles.DefaultLabel);
                        }
                    }

                    GUILayout.FlexibleSpace();
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