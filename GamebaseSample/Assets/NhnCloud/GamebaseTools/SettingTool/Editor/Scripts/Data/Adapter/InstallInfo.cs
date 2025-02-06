using System.Collections.Generic;

namespace NhnCloud.GamebaseTools.SettingTool.Data
{
    public class InstallInfo
    {
        public string name;
        public string version;
        
        public List<string> repositories;
     
        public List<string> dirs;
        public List<string> packages;
    }
}