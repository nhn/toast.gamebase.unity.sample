using System.Collections.Generic;
using System;
using UnityEditor;

namespace NhnCloud.GamebaseTools.SettingTool.Ui
{
    using Data;

    public enum PageType
    {
        State = 0,
        Install,
        Update,
        Category,
        Edit,
    }

    public interface IPage
    {
        void Initialize();

        void SetSettingData(SettingOption selector);
        
        SettingOption GetSettingData();

        string GetPageName();

        void Draw();

        void DrawControlUI();
    }
}