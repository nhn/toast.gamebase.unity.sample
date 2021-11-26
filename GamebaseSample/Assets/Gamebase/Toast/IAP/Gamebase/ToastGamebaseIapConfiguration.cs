using System.Collections.Generic;

namespace Toast.Iap
{
    public class ToastGamebaseIapConfiguration
    {
        public const string STORE_ONGATE = "ONGATE";

        public Dictionary<string, Dictionary<string, string>> ExtraData = new Dictionary<string, Dictionary<string, string>>();
        
        public string StoreCode { get; set; }
        public string AppKey { get; set; }
        public ToastServiceZone ServiceZone { get; set; }

        public ToastGamebaseIapConfiguration()
        {
            ServiceZone = ToastServiceZone.REAL;
        }

        public bool SetExtraData(string storeCode, Dictionary<string, string> extraData)
        {
            if (extraData == null)
            {
                return false;
            }

            ExtraData[storeCode] = extraData;

            return true;
        }
    }
}