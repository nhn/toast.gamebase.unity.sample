using System.Collections.Generic;
using System.Linq;

namespace NhnCloud.GamebaseTools.SettingTool.Data
{    public class LegacyAdapterSettings
    {
        public bool useAndroid;
        public bool useIOS;

        public Platform unity;
        public Platform android;
        public Platform ios;

        public class Platform
        {
            public Category authentication;
            public Category purchase;
            public Category push;
            public Category etc;

            public class Category
            {
                public string name;
                public List<Adapter> adapters;

                public class Adapter
                {
                    public string name;
                    public bool used;
                }

                public List<string> GetSelectionList()
                {
                    List<string> selectionList = new List<string>();
                    if(adapters != null)
                    {
                        foreach (var adapter in adapters)
                        {
                            if (adapter.used)
                            {
                                selectionList.Add(adapter.name);
                            }
                        }
                    }
                    return selectionList;
                }
            }

            public List<string> GetSelectionList()
            {
                List<string> selectionList = new List<string>();
                selectionList = selectionList.Union(authentication.GetSelectionList()).ToList();
                selectionList = selectionList.Union(purchase.GetSelectionList()).ToList();
                selectionList = selectionList.Union(push.GetSelectionList()).ToList();
                selectionList = selectionList.Union(etc.GetSelectionList()).ToList();
                return selectionList;
            }
        }

        public AdapterSelection Convert()
        {
            AdapterSelection selection = new AdapterSelection();

            if (useAndroid == true)
            {
                selection.activePlatform.Add(SettingToolStrings.TEXT_ANDROID);
                foreach(var name in android.GetSelectionList())
                {
                    selection.AddSelect(name, SettingToolStrings.TEXT_ANDROID);
                }
            }
            if (useIOS == true)
            {
                selection.activePlatform.Add(SettingToolStrings.TEXT_IOS);
                foreach (var name in ios.GetSelectionList())
                {
                    selection.AddSelect(name, SettingToolStrings.TEXT_IOS);
                }
            }

            foreach (var name in unity.GetSelectionList())
            {
                if(name.Equals("Facebook"))
                {
                    selection.AddSelect(name, SettingToolStrings.TEXT_ANDROID);
                    selection.AddSelect(name, SettingToolStrings.TEXT_IOS);
                }
                else
                {
                    selection.AddSelect(name, SettingToolStrings.TEXT_UNITY);
                }                
            }

            return selection;
        }
        
    }
}
 