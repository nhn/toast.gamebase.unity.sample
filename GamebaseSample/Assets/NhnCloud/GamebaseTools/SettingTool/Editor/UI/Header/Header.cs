using NhnCloud.GamebaseTools.SettingTool.Data;
using UnityEditor;
using UnityEngine;

namespace NhnCloud.GamebaseTools.SettingTool.Ui
{
    public class Header
    {
        /// <summary> 
        /// title(SettingTool)과 Align을 맞추기 위해 높이 값을 지정한다.
        /// </summary>
        private const float HEIGHT_VERSION_LABEL = 38;

        private Rect headerArea;

        private ISettingToolUI targetUI;
        
        private Texture2D settingToolUpdateIcon;
        private Texture2D downloadIcon;
                
        public Header(Rect headerArea)
        {
            this.headerArea = headerArea;
        }

        public void Initialize(ISettingToolUI targetUI)
        {
            this.targetUI = targetUI;
        }

        public void Draw(string name)
        {
            GUILayout.BeginArea(headerArea, ToolStyles.padding_top_left_10);
            {
                using (new EditorGUILayout.VerticalScope())
                {
                    using (new EditorGUILayout.VerticalScope(ToolStyles.padding_left_right_10))
                    {
                        DrawVersion();

                        using (new EditorGUILayout.HorizontalScope(ToolStyles.padding_left_right_20))
                        {
                            GUILayout.Label(name, ToolStyles.WindowsName);
                        }
                    }
                }

                GUILayout.EndArea();
            }
        }

        private void DrawVersion()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label(targetUI.GetToolName(), ToolStyles.SettingToolName);
                GUILayout.Label(SettingTool.VERSION, ToolStyles.SettingToolVersion,
                    GUILayout.Height(HEIGHT_VERSION_LABEL));
            }
        }
    }
}