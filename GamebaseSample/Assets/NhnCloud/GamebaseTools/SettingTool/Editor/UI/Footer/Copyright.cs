using UnityEditor;
using UnityEngine;

namespace NhnCloud.GamebaseTools.SettingTool.Ui
{
    using Util.Ad;
    
    public class Copyright
    {
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
                    GUILayout.Label(Multilanguage.GetString("COPYRIGHT_TEXT"), ToolStyles.CopyrightLabel);
                    EditorGUI.BeginChangeCheck();
                    {
                        Multilanguage.SelectedLanguageIndex = EditorGUILayout.Popup(
                            Multilanguage.SelectedLanguageIndex, 
                            Multilanguage.GetSupportNativeLanguages(),
                            GUILayout.Width(80));

                        if (EditorGUI.EndChangeCheck() == true)
                        {
                            Advertisement.SetLanguageCode(Multilanguage.GetSupportLanguages()[Multilanguage.SelectedLanguageIndex]);
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }

                GUILayout.EndArea();
            }
        }
    }
}