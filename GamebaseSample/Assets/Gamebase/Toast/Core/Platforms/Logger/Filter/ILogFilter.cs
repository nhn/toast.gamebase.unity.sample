namespace Toast.Core
{

    public interface ILogFilter
    {
        bool Filter(LogObject logData);
    }

}