using System.Collections.Generic;

namespace NhnCloud.GamebaseTools.SettingTool.Data
{
    public class Adapter
    {
        public string name;
        public string displayName;

        public List<ExtraInfo> extras = new List<ExtraInfo>();

        public bool unity;

        public List<Adapter> types = new List<Adapter>();

        public List<string> exclude;

        public List<PlatformInfo> platforms = new List<PlatformInfo>();

        public PlatformInfo GetPlatformAdapterInfo(string platform)
        {
            return platforms.Find(it => it.name.Equals(platform));
        }

        public bool HasDependencyAdpater(string adapterName, string platform)
        {
            var platformInfo = GetPlatformAdapterInfo(platform);
            if (platformInfo != null)
            {
                if (platformInfo.include != null)
                {
                    foreach (var includeAdapterName in platformInfo.include)
                    {
                        if (includeAdapterName.Equals(adapterName))
                        {
                            return true;
                        }
                    }
                }
            }
            
            return false;
        }
        
        public bool HasPlatform(string platform)
        {
            return GetPlatformAdapterInfo(platform) != null;
        }

        public bool IsUnity()
        {
            return unity;
        }

        public bool HasType()
        {
            return types != null && types.Count > 0;
        }

        public string GetName()
        {
            return name;
        }

        public string GetDisplayName()
        {
            if (string.IsNullOrEmpty(displayName) == false)
            {
                return Multilanguage.GetString(displayName);
            }

            return name;
        }
    }
}