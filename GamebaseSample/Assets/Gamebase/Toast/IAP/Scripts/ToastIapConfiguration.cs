namespace Toast.Iap
{
    public enum ToastServiceZone
    {
        ALPHA = 0,
        BETA = 1,
        REAL = 2
    }

    public enum StoreCode
    {
        GooglePlayStore,
        AppleAppStore,
        OneStore,
        Redbean,
        GalaxyStore,
        Unknown
    }

    public class ToastIapConfiguration
    {
        public ToastIapConfiguration()
        {
            ServiceZone = ToastServiceZone.REAL;
            StoreCode = StoreCode.Unknown;
        }

        public string AppKey { get; set; }
        public StoreCode StoreCode { get; set; }
        public ToastServiceZone ServiceZone { get; set; }
    }
}