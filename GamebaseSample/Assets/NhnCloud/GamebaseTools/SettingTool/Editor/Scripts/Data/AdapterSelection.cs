using System;
using System.Collections.Generic;
using UnityEngine;

namespace NhnCloud.GamebaseTools.SettingTool.Data
{
    public enum SelectedState
    {
        NONE,
        ANY,
        ALL,
        UPGRADEABLE,
        UPGRADE,
        DISABLE,
    }
    
    public class AdapterSelection
    {
        public interface ISelectCondition
        {
            string GetPlatformVersion(string platform);
        }
        
        public class Selection
        {
            public string platform;
            public string adapter;
        }

        private IGamebaseVersion versionCondition;
        
        public List<string> activePlatform = new List<string>();

        public List<Selection> selections = new List<Selection>();

        public AdapterSelection()
        {
        }

        public AdapterSelection(AdapterSelection copy)
        {
            if (copy != null)
            {
                activePlatform = new List<string>(copy.activePlatform.Count);
                foreach (var platform in copy.activePlatform)
                {
                    activePlatform.Add(platform);
                }
            
                selections = new List<Selection>(copy.selections.Count);
                foreach (var selection in copy.selections)
                {
                    selections.Add(new Selection { platform = selection.platform, adapter = selection.adapter });
                }
            }
        }

        public void SetCondition(IGamebaseVersion condition)
        {
            versionCondition = condition;
        }

        public void SetDefault()
        {
            activePlatform.Add(SettingToolStrings.TEXT_ANDROID);
            activePlatform.Add(SettingToolStrings.TEXT_IOS);
        }
        public void Clear()
        {
            activePlatform.Clear();
            selections.Clear();
        }

        public void Nomalize()
        {
            selections.RemoveAll(it =>
            {
                Adapter adapter = AdapterSettings.GetAdapter(it.adapter);
                if (adapter != null)
                {
                    if (it.platform.Equals(SettingToolStrings.TEXT_UNITY))
                    {
                        if (adapter.IsUnity())
                        {
                            foreach (var platform in activePlatform)
                            {
                                if (adapter.HasPlatform(platform))
                                {
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            foreach (var platform in activePlatform)
                            {
                                if (adapter.HasPlatform(platform))
                                {
                                    AddSelect(it.adapter, platform);
                                }
                            }
                        }
                        return true;
                    }
                    else if (activePlatform.Contains(it.platform) == false)
                    {
                        return true;
                    }

                    return false;
                }
                else
                {
                    return true;
                }
            });
        }
        
        public IEnumerable<PlatformData> GetActivePlatforms()
        {
            foreach (var platform in AdapterSettings.GetAllPlatforms())
            {
                if (IsActivePlatform(platform.name))
                {
                    yield return platform;
                }
            }
        }

        public bool IsActivePlatform(string platformName)
        {
            if (platformName.Equals(SettingToolStrings.TEXT_UNITY))
                return true;
            
            return activePlatform.Contains(platformName);
        }

        public void SetActivePlatform(string platformName, bool active)
        {
            if(active == true)
            {
                if (activePlatform.Contains(platformName) == false)
                {
                    activePlatform.Add(platformName);
                }                    
            }
            else
            {
                activePlatform.RemoveAll(it =>
                {
                    return string.Equals(it, platformName);
                });
                
                selections.RemoveAll(it =>
                {
                    return string.Equals(it.platform, platformName);
                });
            }
        }

        public List<Selection> GetSelections(string adapterName)
        {
            return selections.FindAll(it =>
            {
                return string.Equals(it.adapter, adapterName) == true;
            });
        }
        
        public Selection GetSelection(string adapterName, string platformName)
        {
            
            return selections.Find(it =>
            {
                return string.Equals(it.adapter, adapterName) == true && string.Equals(it.platform, platformName) == true;
            });
        }
        
        public void AddSelect(string adapterName, string platform)
        {
            Selection selection = GetSelection(adapterName, platform);

            if(selection == null)
            {
                selections.Add(new Selection() { adapter = adapterName, platform = platform });
            }
        }

        public void RemoveSelect(string adapterName, string platform)
        {
            Selection selection = GetSelection(adapterName, platform);
            if (selection != null)
            {
                selections.Remove(selection);
            }
        }

        public void RemoveSelect(string adapterName)
        {
            selections.RemoveAll(it => string.Equals(it.adapter, adapterName));
        }
        
        public bool IsSelected(string adapterName, string platform)
        {
            Selection selection = GetSelection(adapterName, platform);
            return selection != null;
        }
        public bool IsSelected(string adapterName)  
        {
            return selections.Exists(it => string.Equals(it.adapter, adapterName));
        }

        public void Select(Adapter adapter, string platformName = null)
        {
            if( adapter.HasType() == true)
            {
                UnSelect(adapter);
            }
            
            var typeAdapter = GetTypeAdapter(adapter);

            if (typeAdapter.IsUnity())
            {
                AddSelect(typeAdapter.GetName(), SettingToolStrings.TEXT_UNITY);
            }
            else
            {
                
                if (string.IsNullOrEmpty(platformName))
                {
                    foreach (var platformAdapter in typeAdapter.platforms)
                    {
                        if (IsActivePlatform(platformAdapter.name))
                        {
                            AddSelect(typeAdapter.GetName(), platformAdapter.name);
                        }
                    }
                }
                else
                {
                    AddSelect(typeAdapter.GetName(), platformName);
                }
            }
            
        }

        public void UnSelect(Adapter adapter, string platformName = null)
        {
            if(string.IsNullOrEmpty(platformName))
            {
                RemoveSelect(GetTypeAdapter(adapter).GetName());
            }
            else
            {
                if (adapter.IsUnity())
                {
                    RemoveSelect(GetTypeAdapter(adapter).GetName(), SettingToolStrings.TEXT_UNITY);
                }
                else
                {
                    RemoveSelect(GetTypeAdapter(adapter).GetName(), platformName);
                }
            }
        }

        public bool IsSelected(Adapter adapter)
        {
            adapter = GetTypeAdapter(adapter);
            if (adapter.IsUnity())
            {
                return IsSelected(adapter.GetName());
            }
            else
            {
                foreach (var platformAdapter in adapter.platforms)
                {
                    if (IsActivePlatform(platformAdapter.name) &&
                        (IsSupported(platformAdapter)))
                    {
                        if (IsSelected(adapter.GetName(), platformAdapter.name))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        public bool IsSelected(Adapter adapter, string platform)
        {
            if (adapter.IsUnity())
            {
                platform = SettingToolStrings.TEXT_UNITY;
            }

            return IsSelected(GetTypeAdapter(adapter).GetName(), platform);
        }

        public Adapter GetTypeAdapter(Adapter adapter)
        {
            if (adapter.HasType() == true)
            {
                foreach (var typeAdapter in adapter.types)
                {
                    if (IsSelected(typeAdapter.name) == true)
                    {
                        return typeAdapter;
                    }
                }
                return adapter.types[0];
            }
            else
            {
                return adapter;
            }
        }
        
        public bool CanSelectable(Adapter adapter)  
        {
            adapter = GetTypeAdapter(adapter);
            foreach (var platform in GetActivePlatforms())
            {
                var platformInfo = adapter.GetPlatformAdapterInfo(platform.name);
                if (IsSupported(platformInfo))
                {
                    return true;
                }
            }

            return false;
        }
        
        public bool IsSupported(Adapter adapter)
        {
            adapter = GetTypeAdapter(adapter);
            
            foreach (var platformAdapter in adapter.platforms)
            {
                if (IsActivePlatform(platformAdapter.name) &&
                    IsSupported(platformAdapter))
                {
                    return true;
                }
            }

            return false;
        }
        
        public string GetPlatformVersion(IGamebaseVersion version, string platform)
        {
            switch (platform)
            {
                case SettingToolStrings.TEXT_UNITY:
                    return version.GetUnityVersion();
                case SettingToolStrings.TEXT_ANDROID:
                    return version.GetAndroidVersion();
                case SettingToolStrings.TEXT_IOS:
                    return version.GetIOSVersion();
            }

            return version.GetUnityVersion();
        }
        
        public bool IsSupported(PlatformInfo platformInfo)
        {
            if (platformInfo == null)
                return false;

            if (versionCondition != null)
            {
                if (string.IsNullOrEmpty(platformInfo.maxGamebaseVersion) == false)
                {
                    return VersionUtility.CompareVersion(GetPlatformVersion(versionCondition, SettingToolStrings.TEXT_UNITY), platformInfo.maxGamebaseVersion) != -1 ||
                           VersionUtility.CompareVersion(GetPlatformVersion(versionCondition, platformInfo.name), platformInfo.maxGamebaseVersion) != -1;
                }
            }
            
            return true;
        }
        
        public bool IsUpdateRequired(Adapter adapter, string platform)
        {
            var platformInfo = adapter.GetPlatformAdapterInfo(platform);
            if (platformInfo != null)
            {
                return IsUpdateRequired(platformInfo);
            }
            
            return false;
        }
        
        public bool IsUpdateRequired(PlatformInfo platformInfo)
        {
            if (platformInfo == null)
                return false;
            
            if (platformInfo != null)
            {
                if (versionCondition != null)
                {
                    if (string.IsNullOrEmpty(platformInfo.minGamebaseVersion) == false)
                    {
                        return VersionUtility.IsUpdateRequired(GetPlatformVersion(versionCondition, SettingToolStrings.TEXT_UNITY), platformInfo.minGamebaseVersion) ||
                               VersionUtility.IsUpdateRequired(GetPlatformVersion(versionCondition, platformInfo.name), platformInfo.minGamebaseVersion);
                    }
                }
            }
            
            return false;
        }

        public string GetMinVersion(Adapter adapter, string platform)
        {
            var platformInfo = adapter.GetPlatformAdapterInfo(platform);
            if (platformInfo != null)
            {
                return GetMinVersion(platformInfo);
            }
            return String.Empty;
        }

        public string GetMinVersion(PlatformInfo platformInfo)
        {
            if (platformInfo != null)
            {
                return platformInfo.minGamebaseVersion;
            }
            
            return String.Empty;
        }
        
        
        public bool IsInstallable(Adapter adapter, string platformName)
        {
            var platformInfo = adapter.GetPlatformAdapterInfo(platformName);

            if (platformInfo?.install == null)
                return false;
            
            if (IsActivePlatform(platformName) == false)
                return false;
            
            if (IsSelected(adapter.name, platformName) == false)
                return false;

            if (IsExclude(platformName))
                return false;

            if (IsSupported(platformInfo))
            {
                return true;
            }
            
            return false;
        }
        
        public List<string> GetExcludeAdapterList()
        {
            List<string> excludeList = new List<string>();
            foreach (var adapter in AdapterSettings.GetAllAdapters())
            {
                if (adapter.exclude != null)
                {
                    if (IsSelected(adapter))
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

        public bool IsExclude(string name)
        {
            foreach (var adapter in AdapterSettings.GetAllAdapters())
            {
                if (adapter.exclude != null)
                {
                    if (IsSelected(adapter))
                    {
                        if (adapter.exclude.Contains(name))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
        
        public IEnumerable<Adapter> GetSelecteAdapters()
        {
            var excludeList = GetExcludeAdapterList();
            foreach (var adapter in AdapterSettings.GetAllAdapters())
            {
                if (excludeList.Contains(adapter.name) == false)
                {
                    if (IsSelected(adapter))
                    {
                        yield return adapter;
                    }
                }
            }
        }
        
        public IEnumerable<PlatformInfo> GetInstallPlatformInfo()
        {
            var excludeList = GetExcludeAdapterList();
            foreach (var adapter in AdapterSettings.GetAllAdapters())
            {
                if (excludeList.Contains(adapter.name) == false)
                {
                    if (adapter.IsUnity())
                    {
                        if (IsSelected(adapter.GetName()))
                        {
                            foreach (var platformAdapter in adapter.platforms)
                            {
                                if (IsActivePlatform(platformAdapter.name) &&
                                    IsSupported(platformAdapter) &&
                                    IsUpdateRequired(platformAdapter) == false)
                                {
                                    yield return platformAdapter;
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var platformAdapter in adapter.platforms)
                        {
                            if (IsSelected(adapter.GetName(), platformAdapter.name) &&
                                IsActivePlatform(platformAdapter.name) &&
                                IsSupported(platformAdapter) &&
                                IsUpdateRequired(platformAdapter) == false)
                            {
                                yield return platformAdapter;
                            }
                        }
                    }
                }
            }
        }
    }
}