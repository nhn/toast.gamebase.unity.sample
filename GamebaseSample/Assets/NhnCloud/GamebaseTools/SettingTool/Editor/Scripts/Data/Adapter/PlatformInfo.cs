using System.Collections.Generic;

namespace NhnCloud.GamebaseTools.SettingTool.Data
{
    public class PlatformInfo
    {
        public string name;

        public string minGamebaseVersion;
        public string maxGamebaseVersion;

        public List<string> include;
        
        public InstallInfo install;
    }
}