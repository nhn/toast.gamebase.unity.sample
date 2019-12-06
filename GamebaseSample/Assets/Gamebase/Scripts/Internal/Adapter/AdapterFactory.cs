using System;
using System.Collections.Generic;
using System.Linq;

namespace Toast.Gamebase.Internal
{
    public class AdapterFactory
    {
        public static T CreateAdapter<T>(string adapterName)
        {
            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies().
                SelectMany(selector => selector.GetTypes()).
                Where(predicate => typeof(T).IsAssignableFrom(predicate));
            
            foreach (Type type in types)
            {
                if (type.Name.ToLower() == adapterName.ToLower())
                {
                    return (T)Activator.CreateInstance(type);
                }
            }

            return default(T);
        }
    }
}