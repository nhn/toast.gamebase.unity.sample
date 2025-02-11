using NhnCloud.GamebaseTools.SettingTool.Data;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace NhnCloud.GamebaseTools.SettingTool.Ui
{
    public class WizardPageUi : ISettingToolUI, CategoryAdapterUI.IControl
    {
        public int currentPage = 0;

        public SettingOption updateSettingOption;
        
        public List<IPage> pageList = new List<IPage>();
        
        public WizardPageUi()
        {
        }
        
        public void Dispose()
        {
            
        }

        public void Initialize()
        {
            var supportVersion = DataManager.GetData<SettingToolResponse.SupportVersion>(DataKey.SUPPOET_VERSION);
            var lastestVersion = new GamebaseVersion(supportVersion);
            
            updateSettingOption = new SettingOption(lastestVersion, new AdapterSelection(AdapterSettings.updatedSelection));
            
            IPage page = new PageInstallSetting();
            page.Initialize();
            page.SetSettingData(updateSettingOption);
            
            pageList.Add(page);

            foreach (var category in AdapterSettings.GetAllCategorys())
            {
                page = new PageCategory(this, category);
                page.Initialize();
                page.SetSettingData(updateSettingOption);

                pageList.Add(page);
            }
            page = new PageInstall();
            page.Initialize();
            page.SetSettingData(updateSettingOption);
            pageList.Add(page);
        }
        
        public string GetToolName()
        {
            return "Gamebase SettingTool";
        }

        public string GetName()
        {
            return "Setup Wizard";
        }

        public bool HasAdapter(Adapter adapter)
        {
            for (int i = 0; i < pageList.Count; i++)
            {
                if (pageList[i] is CategoryAdapterUI.IControl control)
                {
                    if (control.HasAdapter(adapter))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public SettingOption GetSettingData()
        {
            return updateSettingOption;
        }
        
        public void MoveShortCut(Adapter adapter)
        {
            for (int i = 0; i < pageList.Count; i++)
            {
                if (pageList[i] is CategoryAdapterUI.IControl control)
                {
                    if (control.HasAdapter(adapter))
                    {
                        currentPage = i;
                        break;
                    }
                }
            }
        }

        public void BackPage()
        {
            currentPage--;
        }

        public void NextPage()
        {
            currentPage++;
        }

        public void OnDrawControlUI(IPage current)
        {
            int index = pageList.IndexOf(current);
            if (index >= 0)
            {
                if (index > 0)
                {
                    if (GUILayout.Button(new GUIContent(Multilanguage.GetString("UI_BUTTON_BACK"), ToolStyles.icon_back), ToolStyles.SizeButton))
                    {
                        BackPage();
                    }
                }

                if (index < pageList.Count - 1)
                {
                    if (GUILayout.Button(new GUIContent(Multilanguage.GetString("UI_BUTTON_NEXT"), ToolStyles.icon_next), ToolStyles.SizeButton))
                    {
                        NextPage();
                    }
                }

                current.DrawControlUI();
            }
        }
        
        public void Draw()
        {
            int pageIndex = (int)currentPage;

            using (new GUILayout.HorizontalScope())
            {
                using (new GUILayout.VerticalScope(ToolStyles.padding_top_left_right_10))
                {
                    using (new EditorGUILayout.VerticalScope(GUILayout.Width(121)))
                    {
                        for (int i = 0; i < pageList.Count(); i++)
                        {
                            bool before = GUI.enabled;

                            var page = pageList[i];

                            if (i > pageIndex)
                            {
                                GUI.enabled = false;
                                GUILayout.Label(new GUIContent(page.GetPageName(), ToolStyles.icon_empty), ToolStyles.PageLabel);
                            }
                            else
                            {
                                GUILayout.Label(new GUIContent(page.GetPageName(), ToolStyles.icon_check), ToolStyles.PageLabel);
                            }
                            
                            

                            GUI.enabled = before;
                        }
                    }
                }

                using (new EditorGUILayout.VerticalScope(ToolStyles.Box))
                {
                    pageList[pageIndex].Draw();

                    GUILayout.FlexibleSpace();

                    Rect rect = EditorGUILayout.GetControlRect(false, 1 );
                    rect.height = 1;
                    EditorGUI.DrawRect(rect, new Color ( 0.5f,0.5f,0.5f, 1 ) );

                    using (new EditorGUILayout.HorizontalScope())
                    {
                        using (var scope = new EditorGUILayout.HorizontalScope())
                        {
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

                        OnDrawControlUI(pageList[pageIndex]);
                    }
                }
            }
        }
    }
}