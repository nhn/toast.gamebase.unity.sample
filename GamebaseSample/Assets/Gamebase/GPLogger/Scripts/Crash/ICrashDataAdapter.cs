using System.Collections.Generic;

namespace GamePlatform.Logger
{
    public interface ICrashDataAdapter
    {
        Dictionary<string, string> GetUserFields();
    }
}