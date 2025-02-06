using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using NhnCloud.GamebaseTools.SettingTool.Util.Ad;

namespace NhnCloud.GamebaseTools.SettingTool.Ui.Wizard
{
    using Data;
    
    public class UpdaterUi : ISettingToolUI
    {
        private Header header;
        private Copyright copyright;

        private Rect settingToolArea;
        private Rect headerArea;
        private Rect copyrightArea;
        private Rect sdkSettingArea;

        private IPage updaterPage;
        
        private bool isInitialized;
        
        public UpdaterUi()
        {
            int width = 550;
            int height = 320;
            
            //------------------------------
            // settingTool
            //------------------------------
            settingToolArea = new Rect(0, 0, width, height);

            //------------------------------
            // header
            //------------------------------
            headerArea = new Rect(0, 0, width, 104);
            
            var copyrightHeight = 24;
            var contentsHeight = settingToolArea.height - headerArea.height - copyrightHeight;
            
            //------------------------------
            // copyright
            //------------------------------
            copyrightArea = new Rect(0, settingToolArea.height - copyrightHeight, settingToolArea.width, copyrightHeight);
            copyright = new Copyright(copyrightArea);
            
            //------------------------------
            // SdkSetting
            //------------------------------
            sdkSettingArea = new Rect(0, headerArea.height, width, contentsHeight);
        }

        public void Dispose()
        {
            
        }

        public void Initialize()
        {
            ToolStyles.LoadStyle();
            
            header = new Header(headerArea);
            header.Initialize(this);
            
            updaterPage = new PageUpdate();
            updaterPage.Initialize();
        
            isInitialized = true;
        }

        public string GetToolName()
        {
            return "Gamebase Updater";
        }
        
        public string GetName()
        {
            return "Latest Version Update";
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
                    using (new EditorGUILayout.VerticalScope(ToolStyles.padding_top_left_10))
                    {
                        using (new EditorGUILayout.VerticalScope(ToolStyles.padding_left_right_10))
                        {
                            GUILayout.Label(GetToolName(), ToolStyles.SettingToolName);

                            using (new EditorGUILayout.HorizontalScope(ToolStyles.padding_left_right_20))
                            {
                                GUILayout.Label(GetName(), ToolStyles.WindowsName);
                            }
                        }
                    }

                    GUILayout.BeginArea(sdkSettingArea, ToolStyles.padding_top_left_right_10);
                    {
                        
                        using (new EditorGUILayout.VerticalScope(ToolStyles.Box))
                        {
                            updaterPage.Draw();
                        }

                        GUILayout.EndArea();
                    }
                    
                    Advertisement.Draw(new Rect(344, 4, 192, 108));

                    copyright.Draw();

                    EditorGUILayout.EndVertical();
                }

                GUILayout.EndArea();
            }
            
            GUI.enabled = true;
        }
    }
}