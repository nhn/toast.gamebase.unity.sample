using NhnCloud.GamebaseTools.SettingTool.Data;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NhnCloud.GamebaseTools.SettingTool.Ui
{
    public class SdkSetting
    {
        private const string TEXT_UNITY = "Unity";
        private const string TEXT_ANDROID = "Android";
        private const string TEXT_IOS = "iOS";

        private const int ADAPTER_LIST_WIDTH = 984;
        private const int BUTTON_WIDTH = 80;
        private const int BUTTON_HEIGHT = 30;

        private Rect sdkSettingArea;

        private List<string> platforms;
        private bool _useAndroid;
        private bool _useiOS;
        private int _selectedPlatformIndex;
        private SettingToolResponse.AdapterSettings vo;
        private SettingToolResponse.AdapterSettings.Platform platformData;
        private SettingToolCallback.VoidDelegate onClickSetting;
        private SettingToolCallback.VoidDelegate onClickRemove;


        private Vector2 scrollPos;

        private bool UseAndroid
        {
            get { return _useAndroid; }
            set
            {
                if (_useAndroid != value)
                {
                    vo.useAndroid = value;
                    if (value == false)
                    {
                        DeselectPlatformData(TEXT_ANDROID);
                    }
                }

                _useAndroid = value;
            }
        }

        private bool UseiOS
        {
            get { return _useiOS; }
            set
            {
                if (_useiOS != value)
                {
                    vo.useIOS = value;
                    if (value == false)
                    {
                        DeselectPlatformData(TEXT_IOS);
                    }
                }

                _useiOS = value;
            }
        }

        private int SelectedPlatformIndex
        {
            get { return _selectedPlatformIndex; }
            set
            {
                if (_selectedPlatformIndex != value)
                {
                    ChangeAdapterListData(platforms[value]);
                }

                _selectedPlatformIndex = value;
            }
        }

        public SdkSetting(Rect sdkSettingArea)
        {
            this.sdkSettingArea = sdkSettingArea;
        }

        public void Initialize(SettingToolCallback.VoidDelegate onClickSetting, SettingToolCallback.VoidDelegate onClickRemove)
        {
            this.onClickSetting = onClickSetting;
            this.onClickRemove = onClickRemove;

            platforms = new List<string>
            {
                TEXT_UNITY
            };

            vo = DataManager.GetData<SettingToolResponse.AdapterSettings>(DataKey.ADAPTER_SETTINGS);

            UseAndroid = vo.useAndroid;
            UseiOS = vo.useIOS;

            ModifyPlatforms(UseAndroid, TEXT_ANDROID);
            ModifyPlatforms(UseiOS, TEXT_IOS);

            platformData = vo.unity;
        }

        public void Draw()
        {
            GUILayout.BeginArea(sdkSettingArea, ToolStyles.padding_top_left_right_20);
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.BeginVertical();
                    {
                        DrawSdkSetting();

                        EditorGUILayout.EndVertical();
                    }

                    EditorGUILayout.EndHorizontal();
                }

                GUILayout.EndArea();
            }
        }

        private void DrawSdkSetting()
        {
            EditorGUILayout.BeginVertical();
            {
                GUILayout.Label(Multilanguage.GetString("UI_TEXT_SDK_SETTING"), ToolStyles.TitleLabel);

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.BeginVertical();
                    {
                        GUILayout.Label(Multilanguage.GetString("UI_TEXT_SELECT_ADDITIONAL_PLATFORMS"), ToolStyles.DefaultLabel);

                        GUILayout.Space(10);
                        GUILayout.Space(10);

                        EditorGUILayout.BeginHorizontal();
                        {
                            DrawCheckBox();

                            EditorGUILayout.EndHorizontal();
                        }

                        GUILayout.Space(10);

                        EditorGUILayout.EndVertical();
                    }

                    DrawSettingAndRemoveButton();

                    EditorGUILayout.EndHorizontal();
                }

                DrawTap();
                DrawAdapterContainer();

                EditorGUILayout.EndVertical();
            }
        }

        private void DrawCheckBox()
        {
            if (GUILayout.Toggle(UseAndroid, Multilanguage.GetString("UI_TEXT_USE_ANDROID_PLATFORM"), ToolStyles.CheckBox, GUILayout.Width(180)) != UseAndroid)
            {
                UseAndroid = !UseAndroid;
                ModifyPlatforms(UseAndroid, TEXT_ANDROID);
            }

            if (GUILayout.Toggle(UseiOS, Multilanguage.GetString("UI_TEXT_USE_IOS_PLATFORM"), ToolStyles.CheckBox, GUILayout.Width(180)) != UseiOS)
            {
                UseiOS = !UseiOS;
                ModifyPlatforms(UseiOS, TEXT_IOS);
            }
        }

        private void DrawSettingAndRemoveButton()
        { 
            if (GUILayout.Button(Multilanguage.GetString("UI_BUTTON_SETTINGS"), GUILayout.Width(BUTTON_WIDTH), GUILayout.Height(BUTTON_HEIGHT)) == true)
            {
                if (onClickSetting != null)
                {
                    if (EditorUtility.DisplayDialog(
                        Multilanguage.GetString("POPUP_SETTING_TITLE"),
                        Multilanguage.GetString("POPUP_009_MESSAGE"),
                        Multilanguage.GetString("POPUP_OK"),
                        Multilanguage.GetString("POPUP_CANCEL")) == true)
                    {
                        onClickSetting();
                    }
                }
            }

            if (Directory.Exists(Path.Combine(Application.dataPath, "Gamebase")) == false)
            {
                GUI.enabled = false;
            }
            
            if (GUILayout.Button(Multilanguage.GetString("UI_BUTTON_REMOVE"), GUILayout.Width(BUTTON_WIDTH), GUILayout.Height(BUTTON_HEIGHT)) == true)
            {
                if (EditorUtility.DisplayDialog(
                    Multilanguage.GetString("POPUP_REMOVE_TITLE"),
                    Multilanguage.GetString("POPUP_010_MESSAGE"),
                    Multilanguage.GetString("POPUP_OK"),
                    Multilanguage.GetString("POPUP_CANCEL")) == true)
                {
                    if (onClickRemove != null)
                    {
                        onClickRemove();
                    }
                }   
            }
            GUI.enabled = true;
        }

        public void RemoveSettings()
        {
            DeselectPlatformData(TEXT_UNITY);
            UseAndroid = false;
            UseiOS = false;
            ModifyPlatforms(false, TEXT_ANDROID);
            ModifyPlatforms(false, TEXT_IOS);
        }

        private void DrawTap()
        {
            SelectedPlatformIndex = GUILayout.Toolbar(SelectedPlatformIndex, platforms.ToArray());
        }

        private void DrawAdapterContainer()
        {
            if (platformData == null)
            {
                return;
            }

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUI.skin.box);
            {
                EditorGUILayout.BeginHorizontal(GUILayout.Width(ADAPTER_LIST_WIDTH / 4));
                {
                    DrawAdapterList(platformData.authentication);
                    DrawAdapterList(platformData.purchase);
                    DrawAdapterList(platformData.push);
                    DrawAdapterList(platformData.etc);

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndScrollView();
            }
        }

        private void DrawAdapterList(SettingToolResponse.AdapterSettings.Platform.Category catogoryData)
        {
            EditorGUILayout.BeginVertical();
            {
                if (catogoryData.adapters == null)
                {
                    DrawBlankAdapter(catogoryData.name);
                }
                else
                {
                    foreach (var adapter in catogoryData.adapters)
                    {
                        // Draw Adapter
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        {
                            GUILayout.Label(catogoryData.name, ToolStyles.AdapterCategory);

                            if (GUILayout.Toggle(adapter.used, adapter.name, ToolStyles.CheckBox) != adapter.used)
                            {
                                // Multiple selection in categories is not possible.
                                if (catogoryData.onlyOneCanBeSelectedFromCategory == true)
                                {
                                    if (GetAdapter(catogoryData.name, adapter.name, platformData).used == false)
                                    {
                                        ResetCafegoryData(catogoryData.adapters);
                                    }
                                }

                                // You can only choose either Unity or Native.
                                if (adapter.canOnlyChooseEitherUnityOrNative == true)
                                {
                                    if (platformData == vo.unity)
                                    {
                                        if (GetAdapter(catogoryData.name, adapter.name, vo.android).used == true ||
                                            GetAdapter(catogoryData.name, adapter.name, vo.ios).used == true)
                                        {
                                            if (EditorUtility.DisplayDialog(
                                                Multilanguage.GetString("POPUP_DEPENDENCIES_TITLE"),
                                                Multilanguage.GetString("POPUP_001_MESSAGE"),
                                                Multilanguage.GetString("POPUP_OK"),
                                                Multilanguage.GetString("POPUP_CANCEL")) == true)
                                            {
                                                DeselecteAllAdaptersLikeAdapterName(catogoryData.name, adapter.name);
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (GetAdapter(catogoryData.name, adapter.name, vo.unity).used == true)
                                        {
                                            if (EditorUtility.DisplayDialog(
                                                Multilanguage.GetString("POPUP_DEPENDENCIES_TITLE"),
                                                (platformData == vo.android)?Multilanguage.GetString("POPUP_002_MESSAGE"): Multilanguage.GetString("POPUP_003_MESSAGE"),
                                                Multilanguage.GetString("POPUP_OK"),
                                                Multilanguage.GetString("POPUP_CANCEL")) == true)
                                            {
                                                DeselecteAllAdaptersLikeAdapterName(catogoryData.name, adapter.name);
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }

                                adapter.used = !adapter.used;
                            }


                            EditorGUILayout.EndVertical();
                        }
                    }
                }

                EditorGUILayout.EndVertical();
            }
        }

        private void DrawBlankAdapter(string title)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                GUILayout.Label(title, ToolStyles.AdapterCategory);
                GUILayout.Label("No adapter exists.", ToolStyles.AdapterCategory);

                EditorGUILayout.EndVertical();
            }
        }

        private void ChangeAdapterListData(string platform)
        {
            platformData = FindInstanceValueFromObject<SettingToolResponse.AdapterSettings.Platform>(platform, vo);
        }

        private void ModifyPlatforms(bool isAdd, string element)
        {
            var selectedPlatformName = platforms[SelectedPlatformIndex];

            if (isAdd == true)
            {
                if (platforms.Contains(element) == false)
                {
                    if (element.Equals(TEXT_IOS) == true)
                    {
                        platforms.Insert(platforms.Count, element);
                    }
                    else
                    {
                        platforms.Insert(1, element);
                    }
                }

                SelectedPlatformIndex = platforms.IndexOf(selectedPlatformName);
            }
            else
            {
                if (platforms.Contains(element) == true)
                {
                    platforms.Remove(element);
                }

                if (selectedPlatformName.Equals(element) == true)
                {
                    SelectedPlatformIndex = 0;
                }
                else
                {
                    SelectedPlatformIndex = platforms.IndexOf(selectedPlatformName);
                }
            }
        }

        private void DeselectPlatformData(string platform)
        {
            ResetPlatformData(FindInstanceValueFromObject<SettingToolResponse.AdapterSettings.Platform>(platform.ToLower(), vo));
        }

        private void ResetPlatformData(SettingToolResponse.AdapterSettings.Platform platform)
        {
            ResetCafegoryData(platform.authentication.adapters);
            ResetCafegoryData(platform.purchase.adapters);
            ResetCafegoryData(platform.push.adapters);
            ResetCafegoryData(platform.etc.adapters);
        }

        private void ResetCafegoryData(List<SettingToolResponse.AdapterSettings.Platform.Category.Adapter> adapters)
        {
            if (adapters == null)
            {
                return;
            }

            foreach (var adapter in adapters)
            {
                adapter.used = false;
            }
        }

        private void DeselecteAllAdaptersLikeAdapterName(string category, string adapterName)
        {
            if (platformData == vo.unity)
            {
                GetAdapter(category, adapterName, vo.android).used = false;
                GetAdapter(category, adapterName, vo.ios).used = false;
            }
            else
            {
                GetAdapter(category, adapterName, vo.unity).used = false;
            }
        }

        private SettingToolResponse.AdapterSettings.Platform.Category.Adapter GetAdapter(
            string category,
            string adapterName,
            SettingToolResponse.AdapterSettings.Platform obj)
        {
            var adapters = FindInstanceValueFromObject<SettingToolResponse.AdapterSettings.Platform.Category>(category, obj).adapters;
            foreach (var adapter in adapters)
            {
                if (adapter.name.Equals(adapterName) == true)
                {
                    return adapter;
                }
            }

            return null;
        }

        private T FindInstanceValueFromObject<T>(string name, object obj)
        {
            var memberInfo = obj.GetType().GetMember(name.ToLower(), BindingFlags.Instance | BindingFlags.Public)[0];
            var fieldInfo = (FieldInfo)memberInfo;
            return (T)fieldInfo.GetValue(obj);
        }
    }
}