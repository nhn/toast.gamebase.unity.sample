namespace Toast.Logger
{
    public class ToastLoggerDuplicateInfo
    {
        public string HashMd5 { get; set; }
        public long CreateTime { get; set; }
        public string Key { get; set; }

        public void SetLogData(ToastLoggerLogObject log)
        {
            Key = log.GetDuplicateJSONString();
            HashMd5 = Md5Sum(Key);
            CreateTime = log.GetCreateTime();
        }

        public int HashCode()
        {
            int prime = 31;
            int result = 1;

            result = prime * result + HashMd5.GetHashCode();
            result = prime * result + CreateTime.GetHashCode();

            return result;
        }

        public int compareTo(ToastLoggerDuplicateInfo info)
        {
            if (CreateTime < info.CreateTime)
            {
                return -1;
            }
            else if (CreateTime > info.CreateTime)
            {
                return 1;
            }

            return 0;
        }

        private string Md5Sum(string strToEncrypt)
        {
            System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
            byte[] bytes = ue.GetBytes(strToEncrypt);

            // encrypt bytes
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashBytes = md5.ComputeHash(bytes);

            // Convert the encrypted bytes back to a string (base 16)
            string hashString = "";

            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
            }

            return hashString.PadLeft(32, '0');
        }
    }
}