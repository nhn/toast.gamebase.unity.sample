using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NhnCloud.GamebaseTools.SettingTool
{
    using Data;
    using Ui;
    using Ui.Wizard;
    using Ui.CustomEditor;
    
    public static class SettingToolWindowManager
    {
        internal static List<SettingToolWindow> settingToolWindows = new List<SettingToolWindow>();
        
        class SettingToolWizardWindow : SettingToolWindow
        {
            protected override ISettingToolUI CreatToolUI()
            {
                return new SettingToolWizardUi();
            }
        }
        
        [MenuItem("Tools/Gamebase/Setup Wizard", false, 1)]
        public static void ShowWizard()
        {
            SettingToolWindow.ShowWindow<SettingToolWizardWindow>(new Rect(100, 100, 768, 768));
        }
        
        class UpdaterUiWindow : SettingToolWindow
        {
            protected override ISettingToolUI CreatToolUI()
            {
                return new UpdaterUi();
            }
        }
        
        [MenuItem("Tools/Gamebase/Update Latest Version...", false, 2)]
        public static void ShowUpdater()
        {
            SettingToolWindow.ShowWindow<UpdaterUiWindow>(new Rect(50, 50, 550, 320));
        }

        class CustomizeWindow : SettingToolWindow
        {
            protected override ISettingToolUI CreatToolUI()
            {
                return new CustomEditorUi();
            }
        }
        
        [MenuItem("Tools/Gamebase/Customize...", false, 3)]
        public static void ShowCustomize()
        {
            SettingToolWindow.ShowWindow<CustomizeWindow>(new Rect(150, 150, 768, 768));
        }

        [MenuItem("Tools/Gamebase/Refresh SettingTool", false, 4)]
        public static void RefreshSettingTool()
        {
            CloseAll();
            GamebaseSettingManager.Destroy();
            GamebaseSettingManager.Initialize((error) =>
            {
                if (SettingTool.IsSuccess(error) == true)
                {
                    SettingToolLog.Debug("The SettingTool initialization was successful.", typeof(GamebaseSettingManager), "Initialize");
                }
                else
                {
                    SettingToolLog.Error(error, typeof(GamebaseSettingManager), "Initialize");
                    if (EditorUtility.DisplayDialog("Initialize",
                            error.message,
                            "Ok") == true)
                    {
                    }
                }
            });
        }
        
        public static void CloseAll()
        {
            while (settingToolWindows.Count != 0)
            {
                settingToolWindows[0].Close();
            }
        }

    }
    
    public abstract class SettingToolWindow : EditorWindow
    {
        private readonly object lockObject = new object();

        protected abstract ISettingToolUI CreatToolUI();
        
        private ISettingToolUI ui;

        public static void ShowWindow<T>(Rect rect) where T : SettingToolWindow
        {
            if (Application.dataPath.Length > 110)
            {
                if (EditorUtility.DisplayDialog("Gamebase SettingTool",
                        "Current path is too long",
                        "Ok") == true)
                {
                    return;
                }
            }
            else
            {
                GamebaseSettingManager.Initialize((error) =>
                {
                    string settingToolUpdateStatus = DataManager.GetData<string>(DataKey.SETTING_TOOL_UPDATE_STATUS);
                    switch (settingToolUpdateStatus)
                    {
                        case SettingToolUpdateStatus.MANDATORY:
                        {
                            if (EditorUtility.DisplayDialog("SettingTool Update",
                                Multilanguage.GetString("POPUP_SETTING_TOOL_MANDATORY_UPDATE_MESSAGE"),
                                "Ok") == true)
                            {
                                Application.OpenURL(Multilanguage.GetString("UI_TEXT_DOWNLOAD_SETTINGTOOL_LINK"));
                            }
                            
                            return;
                        }
                        case SettingToolUpdateStatus.OPTIONAL:
                        {
                            if (EditorUtility.DisplayDialog("SettingTool Update",
                                Multilanguage.GetString("POPUP_SETTING_TOOL_OPTIONAL_UPDATE_MESSAGE"),
                                "Ok", "Cancel") == true)
                            {
                                Application.OpenURL(Multilanguage.GetString("UI_TEXT_DOWNLOAD_SETTINGTOOL_LINK"));
                                return;
                            }
                            
                            break;
                        }
                    }  
                    
                    if (SettingTool.IsSuccess(error) == true)
                    {
                        var window = GetWindowWithRect<T>(rect);
                        window.Show();
                    }
                    else
                    {
                        SettingToolLog.Error(error, typeof(GamebaseSettingManager), "Initialize");
                        if (EditorUtility.DisplayDialog("Initialize",
                                error.message,
                                "Ok") == true)
                        {
                        }
                    }
                });
            }
        }

        private void OnEnable()
        {
            Initialize();
        }

        private void OnDisable()
        {
            Dispose();
        }

        private void Initialize()
        {
            SettingToolWindowManager.settingToolWindows.Add(this);
            
            GamebaseSettingManager.Initialize((error) =>
            {
                if (SettingTool.IsSuccess(error) == true)
                {
                    ui = CreatToolUI();
            
                    ui.Initialize();

                    titleContent = new GUIContent(ui.GetName());
                }
                else
                {
                    SettingToolLog.Error(error, GetType(), "Initialize");
                    if (EditorUtility.DisplayDialog("Initialize",
                            error.message,
                            "Ok") == true)
                    {
                    }
                }
            });
        }
        
        private void Dispose()
        {
            SettingToolWindowManager.settingToolWindows.Remove(this);
            
            if (ui != null)
            {
                ui.Dispose();
                
                ui = null;
            }
        }

        private float currentProgress = 0;
        private bool hasInfo = false;
        private void Update()
        {
            var info = GamebaseSettingManager.GetProcessInfo();
            if (info != null)
            {
                if (currentProgress != info.GetTotalProgress())
                {
                    currentProgress = info.GetTotalProgress();
                    Repaint();
                }

                hasInfo = true;
            }
            else if(hasInfo)
            {
                Repaint();
                hasInfo = false;
            }
        }

        private void OnGUI()
        {
            lock (lockObject)
            {
                var process = GamebaseSettingManager.IsProcess();
                if (process)
                {
                    GUI.enabled = false;
                }
                   
                if (ui != null)
                {
                    ui.Draw();
                }

                if (process)
                {
                    GUI.enabled = true;
                }
            }
        }
        
        public void CloseEditorWindow()
        {
            lock (lockObject)
            {
                Close();

                AssetDatabase.Refresh();
            }
        }
        public void OnClose()
        {
            Debug.Log("OnDisable");
            
            CloseEditorWindow();
        }
    }
}