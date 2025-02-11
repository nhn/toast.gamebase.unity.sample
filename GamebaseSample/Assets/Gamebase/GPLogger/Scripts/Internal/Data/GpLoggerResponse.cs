using System.Collections.Generic;

namespace GamePlatform.Logger.Internal
{
    public static class GpLoggerResponse
    {
        public static class Common
        {
            public class BaseResponse
            {
                public string uri;
                public Header header;

                public class Header
                {
                    public string transactionId;
                }
            }

            public class BaseBody
            {
                public bool isSuccessful;
                public int resultCode;
                public string resultMessage;
            }

            public class Log
            {
                public string type;
                public string level;
                public string message;
                public long createTime;
                public string transactionId;
                public Dictionary<string, string> userFields;
            }
        }

        /// <summary>
        /// (주의) SessionID, transactionId, CrashStyle, SymMethod 는 플러그인에서 입력
        /// SessionID: 발급된 세션 ID
        /// transactionId
        /// CrashStyle: unity-cs
        /// SymMethod: none
        /// </summary>
        public class Exception : Common.BaseResponse
        {
            public Common.BaseBody body;
        }

        public static class SetLoggerListener
        {
            public class Success : Common.BaseResponse
            {
                public Body body;

                public class Body : Common.BaseBody
                {
                    public Common.Log log;
                }
            }

            public class Filter : Common.BaseResponse
            {
                public Body body;

                public class Body : Common.BaseBody
                {
                    public Common.Log log;
                    public Filter filter;

                    public class Filter
                    {
                        /// <summary>
                        /// LogTypeFilter | DuplicateLogFilter | LogLevelFilter
                        /// </summary>
                        public string name;
                    }
                }
            }

            public class Save : Common.BaseResponse
            {
                public Body body;

                public class Body : Common.BaseBody
                {
                    public Common.Log log;
                }
            }

            public class Error : Common.BaseResponse
            {
                public Body body;

                public class Body : Common.BaseBody
                {
                    public Common.Log log;
                    public string errorMessage;
                }
            }
        }

        public class LogNCrashSettings
        {
            public int status;
            public Result result;

            public LogNCrashSettings()
            {
                result = new Result();
            }

            public class Result
            {
                public Log log;
                public Session session;
                public Crash crash;
                public Networkinsights networkinsights;
                public Filter filter;

                public Result()
                {
                    log = new Log();
                    session = new Session();
                    crash = new Crash();
                    networkinsights = new Networkinsights();
                    filter = new Filter();
                }

                public class Log
                {
                    public bool enable = true;
                }

                public class Session
                {
                    public bool enable = true;
                }

                public class Crash
                {
                    public bool enable = true;
                }

                public class Networkinsights
                {
                    public bool enable = false;
                    public List<string> urls = new List<string>();
                }

                public class Filter
                {
                    public LogLevelFilter logLevelFilter;
                    public LogTypeFilter logTypeFilter;
                    public LogDuplicateFilter logDuplicateFilter;

                    public Filter()
                    {
                        logLevelFilter = new LogLevelFilter();
                        logTypeFilter = new LogTypeFilter();
                        logDuplicateFilter = new LogDuplicateFilter();
                    }

                    public class LogLevelFilter
                    {
                        public bool enable = false;
                        public string logLevel = "WARN";
                    }

                    public class LogTypeFilter
                    {
                        public bool enable = false;
                        public List<string> logType = new List<string>();
                    }

                    public class LogDuplicateFilter
                    {
                        public bool enable = false;
                        public int expiredTime = 2;
                    }
                }
            }
        }
    }
}