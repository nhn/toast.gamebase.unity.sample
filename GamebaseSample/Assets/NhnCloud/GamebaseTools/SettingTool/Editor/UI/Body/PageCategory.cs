using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace NhnCloud.GamebaseTools.SettingTool.Ui
{
    using Data;

    public class PageCategory : IPage, CategoryAdapterUI.IControl
    {
        private CategoryAdapterUI.IControl controler;
        private SettingOption settingData;

        private Vector2 scrollPos;

        private AdapterCategory category;

        public PageCategory(CategoryAdapterUI.IControl controler, AdapterCategory category)
        {
            this.controler = controler;
            this.category = category;
        }

        public void Initialize()
        {
        }
        
        public void SetSettingData(SettingOption settingData)
        {
            this.settingData = settingData;
        }
        
        public SettingOption GetSettingData()
        {
            return settingData;
        }
        
        public PageType GetPageType()
        {
            return PageType.Category;
        }
        
        public string GetPageName()
        {
            return category.GetDisplayName();
        }

        public void Draw()
        {
            using (new EditorGUILayout.VerticalScope(ToolStyles.padding_top_left_10, GUILayout.ExpandHeight(true)))
            {
                GUILayout.Label(category.GetDisplayName(), ToolStyles.TitleLabel);
                if (string.IsNullOrEmpty(category.description) == false)
                {
                    GUILayout.Label(Multilanguage.GetString(category.description), ToolStyles.DefaultLabel);
                }

                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUI.skin.box, GUILayout.ExpandWidth(true));
                {
                    if (category != null)
                    {
                        DrawCategoryAdapterList(category);

                        EditorGUILayout.EndScrollView();
                    }
                }
            }
        }

        public void DrawControlUI()
        {
        }

        private void DrawCategoryAdapterList(AdapterCategory catogoryData)
        {
            var selection = settingData.GetSelection();

            EditorGUILayout.BeginVertical();
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    var hasAdapters = from adapter in catogoryData.adapters
                                        where selection.CanSelectable(adapter)
                                        select new CategoryAdapterUI(this, adapter);

                    using (new EditorGUILayout.VerticalScope())
                    {
                        if (catogoryData.adapters == null)
                        {
                            DrawBlankAdapter(catogoryData.name);
                        }
                        else
                        {
                            foreach (var adapter in hasAdapters)
                            {
                                adapter.Draw();
                            }

                            DrawDependencies(catogoryData);
                        }
                    }
                }
                
                EditorGUILayout.EndVertical();
            }
        }

        private void DrawDependencies(AdapterCategory catogoryData)
        {
            var selection = settingData.GetSelection();

            var dependencies = new List<string>();
            foreach (var adapter in catogoryData.adapters)
            {
                foreach (var platformInfo in adapter.platforms)
                {
                    if (platformInfo.include != null &&
                        platformInfo.include.Count > 0 &&
                        selection.IsSelected(adapter, platformInfo.name))
                    {
                        foreach (var include in platformInfo.include)
                        {
                            if (dependencies.Contains(include) == false)
                            {
                                dependencies.Add(include);
                            }
                        }
                    }
                }
            }

            if (dependencies.Count > 0)
            {
                EditorGUILayout.BeginHorizontal(ToolStyles.MiniBox);
                {
                    GUILayout.Label("Dependencies", ToolStyles.BoldLabel);
                    
                    EditorGUILayout.EndHorizontal();
                }
                
                foreach (var dependency in dependencies)
                {
                    var category = new CategoryAdapterUI(this, AdapterSettings.GetAdapter(dependency));
                    category.Draw();
                }
            }
        }

        private void DrawBlankAdapter(string title)
        {
            EditorGUILayout.BeginVertical(ToolStyles.MiniBox);
            {
                GUILayout.Label(title, ToolStyles.AdapterCategory);
                GUILayout.Label("No adapter exists.", ToolStyles.AdapterCategory);

                EditorGUILayout.EndVertical();
            }
        }
        
        public bool HasAdapter(Adapter adapter)
        {
            return category.adapters.Contains(adapter);
        }
        
        public void MoveShortCut(Adapter adapter)
        {
            controler.MoveShortCut(adapter);
        }
    }
}