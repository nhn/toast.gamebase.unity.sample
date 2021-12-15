using UnityEditor;
using UnityEngine;

namespace NhnCloud.GamebaseTools.SettingTool.Ui
{
    public class Copyright
    {
        private const string COPYRIGHT_TEXT = "ⓒ NHN Corp. All rights reserved.";

        private Rect copyrightArea;

        public Copyright(Rect copyrightArea)
        {
            this.copyrightArea = copyrightArea;
        }

        public void Draw()
        {            
            GUILayout.BeginArea(copyrightArea, ToolStyles.padding_left_right_20);
            {
                EditorGUILayout.BeginHorizontal();
                {
                    // As EditorGUILayout.Popup UI is added and COPYRIGHT_TEXT moves to the left, a 60 margin is added to compensate for this.
                    GUILayout.Space(60);
                    GUILayout.Label(COPYRIGHT_TEXT, ToolStyles.CopyrightLabel);
                    EditorGUI.BeginChangeCheck();
                    {
                        Multilanguage.SelectedLanguageIndex = EditorGUILayout.Popup(
                            Multilanguage.SelectedLanguageIndex, 
                            Multilanguage.GetSupportNativeLanguages(),
                            GUILayout.Width(80));

                        if (EditorGUI.EndChangeCheck() == true)
                        {
                            
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }

                GUILayout.EndArea();
            }
        }
    }
}