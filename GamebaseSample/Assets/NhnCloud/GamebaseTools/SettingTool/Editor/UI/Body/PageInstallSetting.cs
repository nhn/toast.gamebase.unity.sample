using NhnCloud.GamebaseTools.SettingTool.Data;
using UnityEditor;
using UnityEngine;

namespace NhnCloud.GamebaseTools.SettingTool.Ui
{
    public class PageInstallSetting : IPage
    {
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
            return Multilanguage.GetString("UI_PAGE_INSTALL_SETTING");
        }

        private Vector2 scrollPos;
        public void Draw()
        {
            
            using (new EditorGUILayout.VerticalScope(ToolStyles.padding_top_left_10, GUILayout.ExpandHeight(true)))
            {
                GUILayout.Label(Multilanguage.GetString("UI_MENU_GAMEBASE"), ToolStyles.TitleLabel);

                using (new EditorGUILayout.VerticalScope(ToolStyles.padding_intent_12))
                {
                    GUILayout.Label(Multilanguage.GetString("UI_TEXT_TOOL_INTRODUCE"), ToolStyles.DefaultLabel, GUILayout.Width(500));
                    
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        if (GUILayout.Button(Multilanguage.GetString("UI_TEXT_LINK_GAMEBASE"), ToolStyles.LinkButton) == true)
                        {
                            Application.OpenURL(Multilanguage.GetString("UI_TEXT_LINK_GAMEBASE_LINK"));
                        }
                        
                        GUILayout.Space(4);

                        if (GUILayout.Button(Multilanguage.GetString("UI_TEXT_LINK_GAMEBASE_SAMPLE"), ToolStyles.LinkButton) == true)
                        {
                            Application.OpenURL(Multilanguage.GetString("UI_TEXT_LINK_GAMEBASE_SAMPLE_LINK"));
                        }
                    }
                }

                GUILayout.Space(8);
                
                var selection = settingData.GetSelection();
        
                GUILayout.Label(Multilanguage.GetString("UI_MENU_PLATFORM"), ToolStyles.TitleLabel);
                using (new EditorGUILayout.VerticalScope(ToolStyles.padding_intent_12))
                {
                    using (new EditorGUILayout.VerticalScope(GUILayout.ExpandHeight(true)))
                    {
                        GUILayout.Label(Multilanguage.GetString("UI_TEXT_SELECT_ADDITIONAL_PLATFORMS"), ToolStyles.DefaultLabel);
                        foreach (var platform in AdapterSettings.GetAllPlatforms())
                        {
                            bool active = selection.IsActivePlatform(platform.name);
                            if (GUILayout.Toggle(active, ToolStyles.GetPlatformContent(platform.name), ToolStyles.CheckBox) != active)
                            {
                                selection.SetActivePlatform(platform.name, !active);
                            }
                        }
                    }
                }
                
                requireSettingUI.Draw(settingData);
            }
        }

        public void DrawControlUI()
        {
        }
    }
}