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

                    public long imageNoticeId;
                    public string path;
                    public string clickScheme;
                    public string clickType;
                    public ImageInfo imageInfo;
                }

                public string type;
                public string theme;
                public bool hasImageNotice;
                public long rollingImageNoticeId = -1;
                public long nextPopupTimeMillis = -1;
                public string address;
                public int footerHeight;
                public List<ImageNoticeInfo> pageList;
            }

            public CommonResponse.Header header;
            public ImageNoticeWeb imageNoticeWeb;
        }
    }
}
