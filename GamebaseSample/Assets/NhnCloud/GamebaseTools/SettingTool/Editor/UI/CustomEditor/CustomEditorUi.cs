using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

using NhnCloud.GamebaseTools.SettingTool.Util.Ad;

namespace NhnCloud.GamebaseTools.SettingTool.Ui.CustomEditor
{
    using Data;
    
    public class CustomEditorUi : ISettingToolUI
    {
        private Header header;
        private Copyright copyright;

        private Rect settingToolArea;
        private Rect headerArea;
        private Rect copyrightArea;
        private Rect sdkSettingArea;
        
        private IPage pageEdit;
        
        private bool isInitialized;
        
        public CustomEditorUi()
        {
            //------------------------------
            // settingTool
            //------------------------------
            settingToolArea = new Rect(0, 0, 768, 768);

            //------------------------------
            // header
            //------------------------------
            headerArea = new Rect(0, 0, 600, 114);

            //------------------------------
            // copyright
            //------------------------------
            var copyrightHeight = 24;
            copyrightArea = new Rect(0, settingToolArea.height - copyrightHeight, settingToolArea.width, copyrightHeight);
            copyright = new Copyright(copyrightArea);

            //------------------------------
            // SdkSetting
            //------------------------------
            sdkSettingArea = new Rect(0, headerArea.height, settingToolArea.width, settingToolArea.height - headerArea.height - copyrightArea.height);
        }

        public void Dispose()
        {
            
        }

        public void Initialize()
        {
            ToolStyles.LoadStyle();
       
            header = new Header(headerArea);
            header.Initialize(this);
 
            pageEdit = new PageEdit();
            pageEdit.Initialize();
  
            isInitialized = true;
        }
        
        public string GetToolName()
        {
            return "Gamebase SettingTool";
        }
        
        public string GetName()
        {
            return "Customize";
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
            
            EditorGUILayout.BeginVertical();
            {
                header.Draw(GetName());

                GUILayout.BeginArea(sdkSettingArea, ToolStyles.padding_left_right_10);
                {
                    using (new EditorGUILayout.VerticalScope(ToolStyles.Box))
                    {
                        pageEdit.Draw();
                    }

                    GUILayout.EndArea();
                }
                            
                Advertisement.Draw(new Rect(566, 4, 192, 108));

                copyright.Draw();

                EditorGUILayout.EndVertical();
            }
            
            GUI.enabled = true;
        }
    }
}