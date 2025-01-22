using System.Collections.Generic;

namespace NhnCloud.GamebaseTools.SettingTool.Data
{
    public class AdapterData
    {
        public GamebasePackageInfo packageInfo = new GamebasePackageInfo();
        public List<PlatformData> platforms = new List<PlatformData>();
        public List<AdapterCategory> categorys = new List<AdapterCategory>();

        public AdapterData()
        {
        }

        public void Add(AdapterData additional)
        {
            foreach(var addData in additional.platforms)
            {
                if (GetPlatform(addData.name) == null)
                {
                    platforms.Add(addData);
                }
            }

            foreach (var addCategory in additional.categorys)
            {
                var category = GetCategory(addCategory.name);
                if (category != null)
                {
                    foreach(var addAdapter in addCategory.adapters)
                    {
                        if(category.GetAdapter(addAdapter.name) == null)
                        {
                            category.adapters.Add(addAdapter);
                        }
                    }
                }
                else
                {
                    categorys.Add(category);
                }
            }
        }

        public PlatformData GetPlatform(string name)
        {
            return platforms.Find(it => it.name.Equals(name));
        }

        public AdapterCategory GetCategory(string name)
        {
            return categorys.Find(it => it.name.Equals(name));
        }
        
        public Adapter GetAdapter(string name)
        {
            foreach (var category in categorys)
            {
                Adapter adapter = category.adapters.Find(it => it.name.Equals(name));
                if (adapter != null)
                {
                    return adapter;
                }
            }

            return null;
        }
    }

    
}
 