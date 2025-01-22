using System.Collections.Generic;

namespace NhnCloud.GamebaseTools.SettingTool.Data
{
    public static class AdapterSettings
    {
        private static AdapterData adapterData = new AdapterData();
        
        public static AdapterSelection savedSelection = null;
        public static AdapterSelection updatedSelection = new AdapterSelection();

        public static void Initialize(AdapterData data, AdapterSelection selection)
        {
            adapterData = data;
            savedSelection = selection;
            if (savedSelection != null)
            {
                savedSelection.Nomalize();
                updatedSelection = new AdapterSelection(savedSelection);
            }
            else
            {
                updatedSelection.SetDefault();
            }
        }

        public static void Clear()
        {
            savedSelection = null;
            updatedSelection.Clear();
        }

        public static GamebasePackageInfo GetPackageInfo()
        {
            return adapterData.packageInfo;
        }
        
        public static IEnumerable<PackagePath> GetAllPackages()
        {
            foreach (var package in adapterData.packageInfo.packages)
            {
                yield return package;
            }
        }
        
        public static IEnumerable<PlatformData> GetAllPlatforms()
        {
            foreach (var platform in adapterData.platforms)
            {
                yield return platform;
            }
        }
        
        public static PlatformData GetPlatform(string name)
        {
            foreach (var platform in GetAllPlatforms())
            {
                if (name.Equals(platform.name))
                {
                    return platform;
                }
            }

            return null;
        }
        
        public static IEnumerable<Adapter> GetAllAdapters()
        {
            foreach (var category in adapterData.categorys)
            {
                foreach (var adapter in category.adapters)
                {
                    if (adapter.HasType())
                    {
                        foreach (var typeAdapter in adapter.types)
                        {
                            yield return typeAdapter;
                        }
                    }
                    else
                    {
                        yield return adapter;    
                    }
                }
            }
        }

        public static Adapter GetAdapter(string name)
        {
            foreach (var adapter in GetAllAdapters())
            {
                if (name.Equals(adapter.name))
                {
                    return adapter;
                }
            }

            return null;
        }
        
        public static IEnumerable<AdapterCategory> GetAllCategorys()
        {
            foreach (var category in adapterData.categorys)
            {
                yield return category;
            }
        }
    }
}
 