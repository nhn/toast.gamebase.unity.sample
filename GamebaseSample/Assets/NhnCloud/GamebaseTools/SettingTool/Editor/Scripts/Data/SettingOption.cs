using System.Collections.Generic;
using System.Linq;

namespace NhnCloud.GamebaseTools.SettingTool
{
    using Data;

    public class SettingOption
    {
        private IGamebaseVersion gamebaseVersion;

        public AdapterSelection selection;

        public SettingOption(IGamebaseVersion gamebaseVersion, AdapterSelection selection)
        {
            this.gamebaseVersion = gamebaseVersion;
            this.selection = selection;
            
            selection.SetCondition(gamebaseVersion);
        }

        public void Nomalize()
        {
            selection.Nomalize();
        }
        
        public IGamebaseVersion GetGamebaseVersion()
        {
            return gamebaseVersion;
        }

        public AdapterSelection GetSelection()
        {
            return selection;
        }

        public string GetUnityVersion()
        {
            return gamebaseVersion.GetUnityVersion();
        }

        public string GetAndroidVersion()
        {
            return gamebaseVersion.GetAndroidVersion();
        }

        public string GetIOSVersion()
        {
            return gamebaseVersion.GetIOSVersion();
        }

        public PlatformData GetPlatform(string name)
        {
            return AdapterSettings.GetPlatform(name);
        }
            
        public bool IsActivePlatform(string platformName)
        {
            return selection.IsActivePlatform(platformName);
        }
        
        public List<string> GetExcludeAdapterList()
        {
            List<string> excludeList = new List<string>();
            foreach (var adapter in AdapterSettings.GetAllAdapters())
            {
                if (adapter.exclude != null)
                {
                    if (selection.IsSelected(adapter))
                    {
                        foreach (var exclude in adapter.exclude)
                        {
                            if (excludeList.Contains(exclude) == false)
                            {
                                excludeList.Add(exclude);
                            }    
                        }
                    }
                }
            }

            return excludeList;
        }

        public List<Adapter> GetDependencyAdapters(Adapter selectAdapter, string platformName)
        {
            var excludeList = GetExcludeAdapterList();

            List<Adapter> dependencyAdapterList = new List<Adapter>();
            foreach (var adapter in AdapterSettings.GetAllAdapters())
            {
                if (excludeList.Contains(adapter.name) == false)
                {
                    if (string.IsNullOrEmpty(platformName) ||
                        selectAdapter.IsUnity())
                    {
                        foreach (var platform in selectAdapter.platforms)
                        {
                            if (selection.IsSelected(adapter, platform.name) &&
                                adapter.HasDependencyAdpater(selectAdapter.name, platform.name) == true)
                            {
                                dependencyAdapterList.Add(adapter);
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (selection.IsSelected(adapter, platformName) &&
                            adapter.HasDependencyAdpater(selectAdapter.name, platformName) == true)
                        {
                            dependencyAdapterList.Add(adapter);
                        }
                    }
                }
            }

            return dependencyAdapterList;
        }

        public IEnumerable<InstallInfo> GetInstallInfos(string platformName)
        {
            foreach (var adapter in AdapterSettings.GetAllAdapters())
            {
                if (selection.IsInstallable(adapter, platformName))
                {
                    var platformInfo = adapter.GetPlatformAdapterInfo(platformName);
                    yield return platformInfo.install;
                }
            }
        }
        
        public bool IsUse(Adapter adapter, string platformName = null)
        {
            adapter = selection.GetTypeAdapter(adapter);
            if (adapter.IsUnity())
            {
                return selection.IsSelected(adapter.GetName());
            }
            else
            {
                foreach (var platformAdapter in adapter.platforms)
                {
                    if (string.IsNullOrEmpty(platformName) == false &&
                        platformName.Equals(platformAdapter.name) == false)
                    {
                        continue;
                    }
                    
                    if (IsActivePlatform(platformAdapter.name) &&
                        selection.IsSupported(platformAdapter))
                    {
                        if (selection.IsSelected(adapter.GetName(), platformAdapter.name))
                        {
                            return true;
                        }
                    } 
                }

                return false;
            }
        }

        public IEnumerable<PlatformData> GetActivePlatforms()
        {
            foreach (var platform in selection.GetActivePlatforms())
            {
                if (IsActivePlatform(platform.name))
                {
                    yield return platform;
                }
            }
        }
        
        public bool IsActiveSelected(Adapter adapter, string platformName)
        {
            if (selection.IsActivePlatform(platformName) == false)
            {
                return false;
            }

            return selection.IsSelected(adapter, platformName);
        }
        
        public void UpdatePlatformSetting()
        {
            foreach (var platform in selection.GetActivePlatforms())
            {
                try
                {
                    if (platform.CheckCanRun())
                    {
                        platform.Run();    
                    }
                }
                catch (System.Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                }
            }
        }
        
        public bool CheckPlatform()
        {
            foreach (var activePlatform in selection.GetActivePlatforms())
            {
                if (activePlatform.Check() == false)
                {
                    return false; 
                }
            }

            return true;
        }
        
        public string GetNeedUpdateNameList()
        {
            string names = "";
            var needUpdateAdapters = NeedUpdateAdapters();
            foreach (Adapter adapter in needUpdateAdapters)
            {
                names += " - " + adapter.GetDisplayName(); 
                if (adapter != needUpdateAdapters.Last())
                {
                    names += "\n"; 
                }
            }

            return names;
        }
        
        public IEnumerable<Adapter> NeedUpdateAdapters()
        {
            foreach (Adapter adapter in AdapterSettings.GetAllAdapters())
            {
                if(selection.IsExclude(adapter.name) == false)
                {
                    if (GetSelectedState(adapter) == SelectedState.UPGRADE)
                    {
                        yield return adapter;
                    }
                }
            }
        }

        public bool IsNeedUpdate()
        {
            foreach (Adapter adapter in AdapterSettings.GetAllAdapters())
            {
                if(selection.IsExclude(adapter.name) == false)
                {
                    if (GetSelectedState(adapter) == SelectedState.UPGRADE)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        
        public bool IsNeedUpdate(Adapter adapter, string platformName)
        {
            if (selection.IsExclude(adapter.name) == true)
            {
                return false;
            }

            if (selection.IsUpdateRequired(adapter, platformName))
            {
                return true;
            }

            return false;
        }
        
        public SelectedState GetSelectedState(Adapter adapter)
        {
            adapter = selection.GetTypeAdapter(adapter);
            if (selection.CanSelectable(adapter) == false)
            {
                return SelectedState.DISABLE;
            }
            
            if (adapter.IsUnity())
            {
                if (IsNeedUpdate(adapter, SettingToolStrings.TEXT_UNITY))
                {
                    return selection.IsSelected(adapter.GetName()) ? SelectedState.UPGRADE : SelectedState.UPGRADEABLE;
                }
                else
                {
                    return selection.IsSelected(adapter.GetName()) ? SelectedState.ALL : SelectedState.NONE;    
                }
            }
            else
            {
                int supportedCount = 0;
                int needUpdateCount = 0;
                int selectedCount = 0;
                foreach (var platformAdapter in adapter.platforms)
                {
                    if (IsActivePlatform(platformAdapter.name) == false)
                    {
                        continue;
                    }

                    if (selection.IsSupported(platformAdapter) == true)
                    {
                        supportedCount++;

                        bool needUpdate = false;
                        if (IsNeedUpdate(adapter, platformAdapter.name))
                        {
                            needUpdateCount++;
                            needUpdate = true;
                        }
                        
                        if (IsActiveSelected(adapter, platformAdapter.name))
                        {
                            selectedCount++;
                            if (needUpdate)
                            {
                                return SelectedState.UPGRADE;
                            }
                        }
                    }
                }

                if (supportedCount == 0)
                {
                    return SelectedState.DISABLE;
                }
                else if (selectedCount == 0)
                {
                    return supportedCount == needUpdateCount ? SelectedState.UPGRADEABLE : SelectedState.NONE;
                }
                else if(selectedCount == supportedCount)
                {
                    return SelectedState.ALL;
                }
                else
                {
                    return SelectedState.ANY;
                }
            }
        }

        public SettingOption GetSettingOption()
        {
            return this;
        }
    }
}