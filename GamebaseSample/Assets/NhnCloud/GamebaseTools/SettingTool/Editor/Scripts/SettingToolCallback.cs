namespace NhnCloud.GamebaseTools.SettingTool
{
    public static class SettingToolCallback
    {
        public delegate void VoidDelegate();
        public delegate void ErrorDelegate(SettingToolError error);
        public delegate void DataDelegate<T>(T data);
        public delegate void SettingToolDelegate<T>(T data, SettingToolError error);
    }
}