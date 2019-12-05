using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Toast.Gamebase.Internal
{
    public static class EnumExtention
    {
        public static string GetEnumMemberValue(this Enum value)
        {
            var enumType = value.GetType();
            var name = Enum.GetName(enumType, value);
            var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
            return enumMemberAttribute.Value;
        }
    }
}