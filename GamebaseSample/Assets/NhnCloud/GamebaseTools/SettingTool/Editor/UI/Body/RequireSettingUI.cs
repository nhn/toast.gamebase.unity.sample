using NhnCloud.GamebaseTools.SettingTool.Data;
using UnityEditor;
using UnityEngine;

namespace NhnCloud.GamebaseTools.SettingTool.Ui
{
    public class RequireSettingUI
    {
        private Vector2 scrollPos;

        public void Draw(SettingOption settingData)
        {
            InstallCheckInfo installCheckInfo = DataManager.GetData<InstallCheckInfo>(DataKey.INSTALL_CHECK);

            bool hasStatus = false;
            foreach (var platform in AdapterSettings.GetAllPlatforms())
            {
                if (settingData.IsActivePlatform(platform.name) && platform.IsNeedCheck())
                {
                    var installStatusList = installCheckInfo.CheckInstall(platform.name);
                    if (installStatusList.Count > 0)
                    {
                        hasStatus = true;
                        break;
                    }
                }
            }

            if (hasStatus == false)
            {
                return;
            }

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
            
            GUILayout.Space(8);

            GUILayout.Label(Multilanguage.GetString("UI_MENU_REQUIRE_SETTING"), ToolStyles.TitleLabel);
        
            using (new EditorGUILayout.VerticalScope(ToolStyles.padding_intent_12))
            {
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

            if (hasStatus)
            {
                using (new EditorGUILayout.VerticalScope(ToolStyles.Box))
                {
                    using (var scope = new EditorGUILayout.ScrollViewScope(scrollPos))
                    {
                        scrollPos = scope.scrollPosition;
                        foreach (var platform in AdapterSettings.GetAllPlatforms())
                        {
                            if (settingData.IsActivePlatform(platform.name) && platform.IsNeedCheck())
                            {
                                var installStatusList = installCheckInfo.CheckInstall(platform.name);
                                if (installStatusList.Count > 0)
                                {
                                    foreach (var status in installStatusList)
                                    {
                                        string text = Multilanguage.GetString(status);
                                        GUILayout.Label(text, ToolStyles.WarningLabel);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}