using System;
using System.Security.Cryptography;
using System.Text;

namespace GamePlatform.Logger.Internal
{
    public class GpLoggerDuplicateInfo
    {
        public long CreateTime { get; private set; }
        public string Key { get; private set; }

        private string hashMd5;

        public void SetLogItem(BaseLogItem item)
        {
            Key = item.GetDuplicateJsonString();
            hashMd5 = Md5Sum(Key);
            CreateTime = item.GetCreateTime();
        }

        public int HashCode()
        {
            int prime = 31;
            int result = 1;

            result = prime * result + hashMd5.GetHashCode();
            result = prime * result + CreateTime.GetHashCode();

            return result;
        }

        public int CompareTo(GpLoggerDuplicateInfo info)
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
            var encoding = new UTF8Encoding();
            var bytes = encoding.GetBytes(strToEncrypt);

            // encrypt bytes
            var md5 = new MD5CryptoServiceProvider();
            var hashBytes = md5.ComputeHash(bytes);

            // Convert the encrypted bytes back to a string (base 16)
            var hashString = new StringBuilder();

            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashString.Append(Convert.ToString(hashBytes[i], 16).PadLeft(2, '0'));
            }

            return hashString.ToString().PadLeft(32, '0');
        }
    }
}