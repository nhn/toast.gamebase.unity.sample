using System;

namespace NhnCloud.GamebaseTools.SettingTool.Ui
{
    public interface ISettingToolUI : IDisposable
    {
        void Initialize();

        string GetToolName();
        string GetName();
        
        void Draw();
    }
}