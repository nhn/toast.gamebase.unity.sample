using UnityEditor;
using UnityEngine;
using System.Linq;

namespace NhnCloud.GamebaseTools.SettingTool.Ui
{
    using Data;

    public class CategoryAdapterUI
    {
        public interface IControl
        {
            SettingOption GetSettingData();

            bool HasAdapter(Adapter adapter);
            
            void MoveShortCut(Adapter adapter);
        }
        
        private IControl control;
        private SettingOption selector;
        private Adapter adapter;

        private Adapter checkdAdapter = null;

        private bool needUpdate = false;

        public CategoryAdapterUI(IControl control, Adapter adapter)
        {
            this.control = control;
            this.selector = control.GetSettingData();
            this.adapter = adapter;

            var selection = selector.GetSelection();
            if (selection.IsSelected(adapter) == false)
            {
                checkdAdapter = selection.GetTypeAdapter(adapter);
            }
        }

        public string GetName()
        {
            return adapter.name;
        }

        public void Draw()
        {
            var selection = selector.GetSelection();

            using (new EditorGUILayout.VerticalScope())
            {
                var state = selector.GetSelectedState(adapter);
                    
                if (state != SelectedState.DISABLE)
                {
                    bool selected = false;
                    GUIStyle style = ToolStyles.CheckBox;

                    if (state == SelectedState.ALL)
                    {
                        style = ToolStyles.CheckBox;

                        selected = true;
                    }
                    else if (state == SelectedState.ANY)
                    {
                        style = ToolStyles.CheckPartBox;
                        selected = true;
                    }
                    else if (state == SelectedState.UPGRADEABLE)
                    {
                        style = ToolStyles.CheckUpgradeBox;
                        selected = false;
                    }
                    else if (state == SelectedState.UPGRADE)
                    {
                        style = ToolStyles.CheckUpgradeBox;
                        selected = true;
                        needUpdate = true;
                    }

                    using (new EditorGUILayout.HorizontalScope())
                    {
                        bool used = GUILayout.Toggle(selected, adapter.GetDisplayName(), style, GUILayout.Width(180));
                        if (used != selected)
                        {
                            SelectAdapter(used, adapter);
                            selected = used;
                        }

                        DrawPlatformSelectUI();
                    }
                
                    if (selected == true)
                    {
                        if (adapter.HasType() == true)
                        {
                            foreach (var typeAdapter in adapter.types)
                            {
                                using (new EditorGUILayout.VerticalScope(ToolStyles.padding_intent_24))
                                {
                                    selected = selection.IsSelected(typeAdapter);
                                    bool used = GUILayout.Toggle(selected, typeAdapter.GetDisplayName(), ToolStyles.RadioBox);

                                    if (used == true)
                                    {
                                        if (selected == false)
                                        {
                                            selection.UnSelect(adapter);
                                            SelectAdapter(true, typeAdapter);
                                        }
                                        
                                        DrawExtraInfoUI(typeAdapter);
                                    }
                                }
                            }
                        }
                        else
                        {
                            DrawExtraInfoUI(adapter);
                        }
                    }
                }
                else
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        using (new EditorGUI.DisabledGroupScope(true))
                        {
                            GUILayout.Toggle(false, adapter.GetDisplayName(), ToolStyles.CheckBox,
                                GUILayout.Width(180));

                            DrawPlatformSelectUI();
                        }
                    }
                }
            }
        }

        private bool IsShowExtraInfo(SettingOption settingOption, Adapter adapter, ExtraInfo extraInfo)
        {
            if (extraInfo.platform != null)
            {
                if (settingOption.IsActiveSelected(adapter, extraInfo.platform))
                {
                    return extraInfo.CheckExtra(settingOption);
                }
                else
                {
                    return false;
                }
            }
            
            return true;
        }
        

        private void DrawExtraInfoUI(Adapter adapter)
        {
            if (needUpdate)
            {
                using (new EditorGUILayout.VerticalScope(ToolStyles.padding_intent_24))
                {
                    GUILayout.Label(Multilanguage.GetString("UI_NEED_UPDATE"), ToolStyles.SmallLabel);
                }
            }
            
            if (adapter != checkdAdapter)
            {
                if (adapter.extras != null && adapter.extras.Count > 0)
                {
                    var showExtras = from extra in adapter.extras
                        where IsShowExtraInfo(selector.GetSettingOption(), adapter, extra)
                        select extra;

                    if (showExtras.Count() > 0)
                    {
                        using (new EditorGUILayout.VerticalScope(ToolStyles.padding_intent_24))
                        {
                            using (new EditorGUILayout.VerticalScope(ToolStyles.MiniBox))
                            {
                                foreach (var extra in showExtras)
                                {
                                    switch (extra.type)
                                    {
                                        case ExtraInfo.EXTRA_KEY_TEXT:
                                            GUILayout.Label(Multilanguage.GetString(extra.text));
                                            break;

                                        case ExtraInfo.EXTRA_KEY_LINK:
                                            if (GUILayout.Button(Multilanguage.GetString(extra.text),
                                                    ToolStyles.LinkButton))
                                            {
                                                Application.OpenURL(extra.value);
                                            }

                                            break;

                                        case ExtraInfo.EXTRA_KEY_CHECK_TYPE:
                                            if (string.IsNullOrEmpty(extra.value) == false)
                                            {
                                                if (System.Type.GetType(extra.value) != null)
                                                {
                                                    GUILayout.Label(Multilanguage.GetString(extra.text),
                                                        ToolStyles.CheckLabel);
                                                }
                                                else
                                                {
                                                    GUILayout.Label(Multilanguage.GetString(extra.text),
                                                        ToolStyles.XLabel);
                                                }
                                            }

                                            break;

                                        case ExtraInfo.EXTRA_KEY_UNITY_VERSION:
                                        case ExtraInfo.EXTRA_KEY_SDK_VERSION:
                                        case ExtraInfo.EXTRA_KEY_ADAPTER:
                                        case ExtraInfo.EXTRA_KEY_REMOVE_TYPE:
                                            string text = Multilanguage.GetString(extra.text);
                                            GUILayout.Label(text, ToolStyles.WarningLabel);
                                            break;
                                        
                                        case ExtraInfo.EXTRA_KEY_ANDROID_MANIFEST:
                                            if (string.IsNullOrEmpty(extra.value) == false)
                                            {
                                                GUILayout.Label(Multilanguage.GetString(extra.text),
                                                    ToolStyles.XLabel);
                                            }
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void DrawPlatformSelectUI()
        {
            var selection = selector.GetSelection();

            var typeAdapter = selection.GetTypeAdapter(adapter);
            
            foreach (var platform in selection.GetActivePlatforms())
            {
                var platformInfo = typeAdapter.GetPlatformAdapterInfo(platform.name);
                if (selection.IsSupported(platformInfo))
                {
                    using (new EditorGUILayout.VerticalScope(GUILayout.Width(90)))
                    {
                        bool selected = selection.IsSelected(typeAdapter, platform.name);

                        GUIStyle styles;

                        if (adapter.IsUnity())
                        {
                            styles = ToolStyles.CheckSelectedBox;
                        }
                        else if (selector.IsNeedUpdate(typeAdapter, platform.name))
                        {
                            styles = ToolStyles.CheckUpgradeBox;
                        }
                        else
                        {
                            styles = ToolStyles.CheckBox;
                        }

                        bool used = GUILayout.Toggle(selected, platform.name, styles);
                        if (used != selected)
                        {
                            SelectAdapter(used, typeAdapter, platform.name);
                            selected = used;
                        }

                        if (selected &&
                            styles == ToolStyles.CheckUpgradeBox)
                        {
                            needUpdate = true;
                        }
                    }
                }
                else
                {
                    using (new EditorGUI.DisabledGroupScope(true))
                    {
                        GUILayout.Toggle(false, platform.name, ToolStyles.CheckBox, GUILayout.Width(90));
                    }
                }
            }
        }

        private void SelectAdapter(bool used, Adapter selectAdapter, string platformName = null)
        {
            var selection = selector.GetSelection();
            if (used == true)
            {
                if (string.IsNullOrEmpty(platformName) == false)
                {
                    if (CheckSelectableAdapter(selectAdapter, platformName))
                    {
                        selection.Select(selectAdapter, platformName);
                    }
                }
                else
                {
                    var typeAdapter = selection.GetTypeAdapter(selectAdapter);
                    foreach (var platformInfo in typeAdapter.platforms)
                    {
                        if (selection.IsActivePlatform(platformInfo.name) &&
                            CheckSelectableAdapter(typeAdapter, platformInfo.name))
                        {
                            selection.Select(typeAdapter, platformInfo.name);
                        }
                    }
                }
            }
            else
            {
                if (CheckUnSelectableAdapter(selectAdapter, platformName) == false)
                {
                    return;
                }
                selection.UnSelect(selectAdapter, platformName);
            }
        }

        private bool CheckUnSelectableAdapter(Adapter selectAdapter, string platformName)
        {
            var selection = selector.GetSelection();
            var typeAdapter = selection.GetTypeAdapter(selectAdapter);

            var dependencyAdapterList = selector.GetDependencyAdapters(typeAdapter, platformName);

            if (dependencyAdapterList.Count > 0)
            {
                string value = "";
                for (int i = 0; i < dependencyAdapterList.Count; i++)
                {
                    value += " - " + dependencyAdapterList[i].GetDisplayName();
                    if (i < dependencyAdapterList.Count - 1)
                    {
                        value += "\n";
                    }
                }

                if (control.HasAdapter(dependencyAdapterList[0]))
                {
                    if (EditorUtility.DisplayDialog(
                            Multilanguage.GetString("POPUP_SETTING_TITLE"),
                            Multilanguage.GetString("UI_NEED_ADAPTER_ALREADY", typeAdapter.GetDisplayName(), value),
                            Multilanguage.GetString("POPUP_OK")))
                    {
                    }
                }
                else
                {
                    if (EditorUtility.DisplayDialog(
                            Multilanguage.GetString("POPUP_SETTING_TITLE"),
                            Multilanguage.GetString("UI_NEED_ADAPTER_ALREADY", typeAdapter.GetDisplayName(), value),
                            Multilanguage.GetString("POPUP_FIND_DEPENDENCY"), Multilanguage.GetString("POPUP_OK")))
                    {
                        control.MoveShortCut(dependencyAdapterList[0]);
                    }
                }
                
                return false;
            }
            
            return true;
        }

        private bool CheckSelectableAdapter(Adapter selectAdapter, string platformName)
        {
            var selection = selector.GetSelection();
            var typeAdapter = selection.GetTypeAdapter(selectAdapter);
            
            if (string.IsNullOrEmpty(platformName))
            {
                foreach (var platformInfo in typeAdapter.platforms)
                {
                    if (selection.IsActivePlatform(platformInfo.name) &&
                        CheckIncludeAdapter(platformInfo) == false)
                    {
                        return false;
                    }
                }
            }
            else
            {
                PlatformInfo platformInfo = typeAdapter.GetPlatformAdapterInfo(platformName);
                if (platformInfo != null)
                {
                    return CheckIncludeAdapter(platformInfo);
                }
                return false;
            }
            return true;
        }

        private bool CheckIncludeAdapter(PlatformInfo platformInfo)
        {
            if (platformInfo != null)
            {
                if (platformInfo.include != null)
                {
                    foreach (var includeAdapterName in platformInfo.include)
                    {
                        if (CheckIncludeAdapter(includeAdapterName, platformInfo.name) == false)
                        {
                            return false;
                        }
                    }
                }
                    
                return true;
            }

            return false;
        }

        bool CheckIncludeAdapter(string includeAdapterName, string platformName)
        {
            var selection = selector.GetSelection();
            Adapter adapter = AdapterSettings.GetAdapter(includeAdapterName);
            if (adapter != null)
            {
                if (selection.IsSelected(adapter, platformName) == false)
                {
                    if (EditorUtility.DisplayDialog(
                            Multilanguage.GetString("POPUP_SETTING_TITLE"),
                            Multilanguage.GetString("UI_NEED_INCLUDE_ADAPTER", platformName, adapter.GetDisplayName()),
                            Multilanguage.GetString("POPUP_OK"),
                            Multilanguage.GetString("POPUP_CANCEL")))
                    {
                        selection.Select(adapter, platformName);

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}