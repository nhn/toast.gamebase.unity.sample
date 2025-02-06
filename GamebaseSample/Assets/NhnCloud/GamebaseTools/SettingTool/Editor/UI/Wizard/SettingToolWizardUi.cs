using UnityEditor;
using UnityEngine;

using NhnCloud.GamebaseTools.SettingTool.Util.Ad;

namespace NhnCloud.GamebaseTools.SettingTool.Ui.Wizard
{
    public class SettingToolWizardUi : ISettingToolUI
    {
        private Header header;
        private WizardPageUi wizardPageUi;
        private Copyright copyright;

        private Rect settingToolArea;
        private Rect headerArea;
        private Rect copyrightArea;
        private Rect leftArea;
        private Rect sdkSettingArea;
        
        private bool isInitialized;
        
        public SettingToolWizardUi()
        {
            //------------------------------
            // settingTool
            //------------------------------
            settingToolArea = new Rect(0, 0, 768, 768);

            //------------------------------
            // header
            //------------------------------
            headerArea = new Rect(0, 0, 600, 100);

            //------------------------------
            // copyright
            //------------------------------
            var copyrightHeight = 24;
            copyrightArea = new Rect(0, settingToolArea.height - copyrightHeight, settingToolArea.width, copyrightHeight);
            copyright = new Copyright(copyrightArea);

            //------------------------------
            // SdkSetting
            //------------------------------
            sdkSettingArea = new Rect(0, headerArea.height, 768, settingToolArea.height - headerArea.height - copyrightArea.height);
        }

        public void Dispose()
        {
            
        }

        public void Initialize()
        {
            ToolStyles.LoadStyle();
            
            header = new Header(headerArea);
            header.Initialize(this);

            wizardPageUi = new WizardPageUi();
            wizardPageUi.Initialize();
            
            isInitialized = true;
        }
        
        public string GetToolName()
        {
            return "Gamebase SettingTool";
        }
        
        public string GetName()
        {
            return "Setup Wizard";
        }

        public bool IsInitialized()
        {
            return isInitialized;
        }
        
        

        /// <summary>
        /// Called from OnGUI.
        /// </summary>
        public void Draw()
        {
            if (isInitialized == false)
            {
                return;
            }

            if (GamebaseSettingManager.IsProcess())
            {
                GUI.enabled = false;
            }
            
            GUILayout.BeginArea(settingToolArea);
            {
                EditorGUILayout.BeginVertical();
                {
                    header.Draw(GetName());
                    
                    GUILayout.BeginArea(sdkSettingArea, ToolStyles.padding_top_left_right_10);
                    {
                        wizardPageUi.Draw();
                        
                        GUILayout.EndArea();
                    }
                    Advertisement.Draw(new Rect(564, 4, 192, 108));

                    copyright.Draw();
                    
                    EditorGUILayout.EndVertical();
                }

                GUILayout.EndArea();
            }
            
            GUI.enabled = true;
        }
    }
}