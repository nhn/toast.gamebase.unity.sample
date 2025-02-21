using NhnCloud.GamebaseTools.SettingTool.Data;
using UnityEditor;
using UnityEngine;

using System.Linq;

namespace NhnCloud.GamebaseTools.SettingTool.Ui
{
    public class PageEdit : IPage, CategoryAdapterUI.IControl
    {
        private SettingOption settingData;

        private Vector2 scrollPos;
        
        private VersionSelectorUI versionSelector;
        
        private RequireSettingUI requireSettingUI = new RequireSettingUI();

        public void Initialize()
        {
            versionSelector = new VersionSelectorUI();
            settingData = new SettingOption(versionSelector, new AdapterSelection(AdapterSettings.updatedSelection));
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
            return PageType.Edit;
        }
        
        public string GetPageName()
        {
            return Multilanguage.GetString("UI_PAGE_EDIT");
        }

        public void Draw()
        {
            DrawCategoryAdapterAllContainer();
        }

        public void DrawControlUI()
        {
        }

        private void DrawCategoryAdapterAllContainer()
        {
            using (new EditorGUILayout.VerticalScope(ToolStyles.padding_top_left_10, GUILayout.ExpandHeight(true)))
            {
                GUILayout.Label(Multilanguage.GetString("UI_MENU_SDK_DOWNLOAD"), ToolStyles.TitleLabel);

                using (new EditorGUILayout.VerticalScope(ToolStyles.padding_intent_12,
                           GUILayout.Width(200)))
                {
                    versionSelector.OnGUI();
                }
            }

            if (versionSelector.Type == VersionSelectorUI.UpdateType.REMOVE)
            {
                GUI.enabled = false;
            }
            
            using (new EditorGUILayout.VerticalScope(ToolStyles.padding_top_left_10, GUILayout.ExpandHeight(true)))
            {
                GUILayout.Label(Multilanguage.GetString("UI_MENU_USE_PLATFORM"), ToolStyles.TitleLabel);

                var selection = settingData.GetSelection();
                using (new EditorGUILayout.HorizontalScope(ToolStyles.padding_intent_12))
                {
                    foreach (var platform in AdapterSettings.GetAllPlatforms())
                    {
                        bool active = selection.IsActivePlatform(platform.name);
                        if (GUILayout.Toggle(active, platform.name, ToolStyles.CheckBox, GUILayout.Width(90)) != active)
                        {
                            selection.SetActivePlatform(platform.name, !active);
                        }
                    }
                }

                GUILayout.Space(8);
                
                GUILayout.Label(Multilanguage.GetString("UI_MENU_GAMEBASE_EDIT"), ToolStyles.TitleLabel);

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
                
                requireSettingUI.Draw(settingData);
            }
            
            if (versionSelector.Type == VersionSelectorUI.UpdateType.REMOVE)
            {
                GUI.enabled = true;
            }
            
            Rect rect = EditorGUILayout.GetControlRect(false, 1 );
            rect.height = 1;
            EditorGUI.DrawRect(rect, new Color ( 0.5f,0.5f,0.5f, 1 ) );
            
            using (new EditorGUILayout.HorizontalScope())
            {
                bool needForceUpdate = false;
                
                using (var scope = new EditorGUILayout.HorizontalScope())
                {
                    if (settingData.IsNeedUpdate())
                    {
                        using (new EditorGUILayout.VerticalScope(ToolStyles.padding_intent_12))
                        {
                            GUILayout.FlexibleSpace();
                            GUILayout.Label(Multilanguage.GetString("UI_HAS_NEED_UPDATE"), ToolStyles.SmallLabel);
                            GUILayout.FlexibleSpace();
                        }

                        needForceUpdate = true;
                    }
                    
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

                var doneContent = new GUIContent(Multilanguage.GetString("UI_BUTTON_SETTINGS"), ToolStyles.icon_setting);
                if (versionSelector.Type == VersionSelectorUI.UpdateType.REMOVE)
                {
                    doneContent = new GUIContent(Multilanguage.GetString("UI_BUTTON_REMOVE"), ToolStyles.icon_remove);
                }
                if (GUILayout.Button(doneContent, ToolStyles.Button, GUILayout.Height(30)))
                {
                    if (versionSelector.Type == VersionSelectorUI.UpdateType.REMOVE)
                    {
                        GamebaseSettingManager.RemoveSetting(error =>
                        {
                            SettingToolWindowManager.CloseAll();
                        });
                    }
                    else
                    {
                        if (needForceUpdate == false)
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
                        else
                        {
                            if (EditorUtility.DisplayDialog(
                                    Multilanguage.GetString("POPUP_SETTING_TITLE"),
                                    Multilanguage.GetString("POPUP_GAMEBASE_NEED_UPDATE_SETTING_MESSAGE",
                                        settingData.GetNeedUpdateNameList()),
                                    Multilanguage.GetString("POPUP_OK")) == true)
                            {
                            }
                        }
                    }
                }
                
                GUI.enabled = true;
            }
        }

        private void DrawCategoryAdapterList(AdapterCategory catogoryData)
        {
            using (new EditorGUILayout.VerticalScope())
            {
                var selectedAdapterUIs = from adapter in catogoryData.adapters
                                        select new CategoryAdapterUI(this, adapter);
                if(selectedAdapterUIs.Count() > 0)
                {
                    EditorGUILayout.BeginHorizontal(ToolStyles.MiniBox);
                    {
                        GUILayout.Label(catogoryData.GetDisplayName(), ToolStyles.BoldLabel);

                        EditorGUILayout.EndHorizontal();
                    }
                    
                    foreach (var adapterUI in selectedAdapterUIs)
                    {
                        adapterUI.Draw();
                    }
                }
            }
        }

        public bool HasAdapter(Adapter adapter)
        {
            return true;
        }
        
        public void MoveShortCut(Adapter adapter)
        {
            
        }
    }
}