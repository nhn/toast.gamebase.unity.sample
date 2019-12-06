namespace Toast.Gamebase.Internal
{
    public abstract class AdapterBase
    {
        public abstract string domain
        {
            get;
        }

        public abstract string version
        {
            get;
        }

        public AdapterBase()
        {
            GamebaseLog.Debug(string.Format("{0} ver.{1}", domain, version), this);
        }

        public void FireNotSupportedAPI(string domain, string methodName)
        {
            GamebaseLog.Warn(string.Format("{0} API is not supported by {1}.", methodName, domain), this);
        }
    }
}
