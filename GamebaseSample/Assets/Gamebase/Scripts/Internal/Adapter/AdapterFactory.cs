using System;
using System.Linq;

namespace Toast.Gamebase.Internal
{
    public static class AdapterFactory
    {
        public static T CreateAdapter<T>(string adapterName)
        {
            var assembly = AppDomain.CurrentDomain.Load("Assembly-CSharp");
            var type = assembly.GetTypes().SingleOrDefault((t) =>
            {
                return t.Name.Equals(adapterName, StringComparison.OrdinalIgnoreCase);
            });

            if (type == null)
            {
                return default(T);
            }

            return (T)Activator.CreateInstance(type);
        }
    }
}