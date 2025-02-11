namespace GamePlatform.Logger
{
    public class LogFilter
    {
        public string Name { get; private set; }

        public static LogFilter FromName(string name)
        {
            return new LogFilter
            {
                Name = name
            };
        }

        public override string ToString()
        {
            return string.Format("Name: {0}", Name);
        }
    }
}