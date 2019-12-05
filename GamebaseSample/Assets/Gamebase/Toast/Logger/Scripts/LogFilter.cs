using Toast.Internal;

namespace Toast.Logger
{
    public class LogFilter
    {
        public string Name { get; private set; }

        internal static LogFilter FromName(string name)
        {
            return new LogFilter
            {
                Name = name
            };
        }

        internal static LogFilter From(JSONObject filterJson)
        {
            if (filterJson == null)
            {
                return null;
            }

            return new LogFilter
            {
                Name = filterJson.ContainsKey("name") ? filterJson["name"] : null
            };
        }

        public override string ToString()
        {
            return string.Format("Name: {0}", Name);
        }
    }
}