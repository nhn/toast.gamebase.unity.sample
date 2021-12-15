using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Toast.Internal
{
    public static class ToastAES
    {
        public static String AESEncrypt256(String Input, String key)
        {
            RijndaelManaged aes = new RijndaelManaged();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] cipherKeyBytes = new byte[32];
            int len = keyBytes.Length;
            if (len > cipherKeyBytes.Length)
            {
                len = cipherKeyBytes.Length;
            }
            Array.Copy(keyBytes, cipherKeyBytes, len);
            aes.Key = cipherKeyBytes;
            aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] xBuff = null;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                {
                    byte[] xXml = Encoding.UTF8.GetBytes(Input);
                    cs.Write(xXml, 0, xXml.Length);
                }
                xBuff = ms.ToArray();
            }

            String Output = Convert.ToBase64String(xBuff);
            return Output;
        }

        public static String AESDecrypt256(String Input, String key)
        {
            RijndaelManaged aes = new RijndaelManaged();

            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] cipherKeyBytes = new byte[32];
            int len = keyBytes.Length;
            if (len > cipherKeyBytes.Length)
            {
                len = cipherKeyBytes.Length;
            }
            Array.Copy(keyBytes, cipherKeyBytes, len);
            aes.Key = cipherKeyBytes;
            aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            var decrypt = aes.CreateDecryptor();
            byte[] xBuff = null;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
                {
                    byte[] xXml = Convert.FromBase64String(Input);
                    cs.Write(xXml, 0, xXml.Length);
                }

                xBuff = ms.ToArray();
            }

            String Output = Encoding.UTF8.GetString(xBuff);
            return Output;
        }
    }
}

