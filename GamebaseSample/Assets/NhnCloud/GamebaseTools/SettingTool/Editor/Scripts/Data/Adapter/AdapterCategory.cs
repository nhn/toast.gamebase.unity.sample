using System.Collections.Generic;

namespace NhnCloud.GamebaseTools.SettingTool.Data
{
    public class AdapterCategory
    {
        public string name;
        public string displayName;
        public string description;

        public AdapterCategory()
        {

        }

        public AdapterCategory(string name)
        {
            this.name = name;
        }

        public List<Adapter> adapters = new List<Adapter>();

        public Adapter GetAdapter(string name)
        {
            return adapters.Find(adapter =>
            {
                return adapter.name.Equals(name) == true;
            });
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