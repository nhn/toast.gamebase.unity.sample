using System.Collections.Generic;

namespace Toast.Gamebase.Internal
{
    public class ImageNoticeResponse
    {
        public class ImageNotices : BaseVO
        {
            public class ImageNoticeWeb
            {
                public class ImageNoticeInfo
                {
                    public class ImageInfo
                    {
                        public int width;
                        public int height;
                    }

                    public int imageNoticeId;
                    public string path;
                    public string clickScheme;
                    public string clickType;
                    public string theme;
                    public ImageInfo imageInfo;
                }

                public string domain;
                public int footerHeight;
                public List<ImageNoticeInfo> pageList;
            }

            public CommonResponse.Header header;
            public ImageNoticeWeb imageNoticeWeb;
        }
    }
}
