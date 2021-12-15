using System;
using System.Reflection;
using System.Text;

namespace Toast.Gamebase.Internal
{
    public class BaseVO
    {
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            Type type = GetType();
            MemberInfo[] members = type.GetMembers();

            foreach (var member in members)
            {
                try
                {
                    sb.AppendLine(string.Format("{0}:{1}", member.Name, GetMemberValue(member)));
                }
                catch { }
            }

            return sb.ToString();
        }

        private string GetMemberValue(MemberInfo member)
        {
            return Convert.ToString(((FieldInfo)member).GetValue(this));
        }
    }
}