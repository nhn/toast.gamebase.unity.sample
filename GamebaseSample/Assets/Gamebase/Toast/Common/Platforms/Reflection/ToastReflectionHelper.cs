using System;
using System.Reflection;

namespace Toast.Internal
{
    public static class ToastReflectionHelper
    {
        public static MethodInfo[] getMethods(string className)
        {
            var type = Type.GetType(className);

            if (type != null)
            {
                return type.GetMethods();
            }

            return null;
        }

        public static MethodInfo getMethod(string className, string methodName)
        {
            var type = Type.GetType(className);

            if (type != null)
            {
                return type.GetMethod(methodName);
            }

            return null;
        }

        public static Object invokeStatic(string className, string methodName, BindingFlags bindAttr)
        {
            Type type = Type.GetType(className);

            if (type != null)
            {
                MethodInfo method = type.GetMethod(methodName, bindAttr);

                if (method != null)
                {
                    method.Invoke(null, null);
                }
            }

            return null;
        }
    }
}
