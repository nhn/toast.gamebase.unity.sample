using System.Collections.Generic;

namespace Toast.Gamebase.Internal
{
    public class GameNoticeResponse : BaseVO
    {
        public class GameNoticeInfo
        {
            public string url;
            public long latestNoticeTimeMillis = -1L;
        }
        
        public CommonResponse.Header header;
        public GameNoticeInfo gameNotice;
    }
}
