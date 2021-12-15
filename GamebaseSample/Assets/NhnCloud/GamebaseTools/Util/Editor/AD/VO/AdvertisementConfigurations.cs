namespace NhnCloud.GamebaseTools.SettingTool.Util.Ad
{
    public class AdvertisementConfigurations
    {
        public string remoteUrl;
        public string xmlFileName;
        public string imageDownloadPath;
        public string[] languages;
        public bool isActiveBG;

        public AdvertisementConfigurations(string remoteUrl, string imageDownloadPath, string xmlFileName, string[] languages, bool isActiveBG = true)
        {
            this.remoteUrl = remoteUrl;
            this.imageDownloadPath = imageDownloadPath;
            this.xmlFileName = xmlFileName;
            this.languages = languages;
            this.isActiveBG = isActiveBG;
        }
    }
}
