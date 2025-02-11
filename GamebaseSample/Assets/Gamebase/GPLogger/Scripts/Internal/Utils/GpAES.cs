using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GamePlatform.Logger.Internal.Utils
{
    public static class GpAES
    {
        private const int KEY_SIZE = 256;
        private const int BLOCK_SIZE = 128;
        private const int CIPHER_KEY_BYTES_SIZE = 32;

        public static string AESEncrypt256(string input, string key)
        {
            RijndaelManaged aes = new RijndaelManaged
            {
                KeySize = KEY_SIZE,
                BlockSize = BLOCK_SIZE,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };

            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] cipherKeyBytes = new byte[CIPHER_KEY_BYTES_SIZE];
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
                    byte[] xXml = Encoding.UTF8.GetBytes(input);
                    cs.Write(xXml, 0, xXml.Length);
                }

                xBuff = ms.ToArray();
            }

            string Output = Convert.ToBase64String(xBuff);

            return Output;
        }

        public static string AESDecrypt256(string input, string key)
        {
            RijndaelManaged aes = new RijndaelManaged
            {
                KeySize = KEY_SIZE,
                BlockSize = BLOCK_SIZE,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };

            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] cipherKeyBytes = new byte[CIPHER_KEY_BYTES_SIZE];
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
                    byte[] xXml = Convert.FromBase64String(input);
                    cs.Write(xXml, 0, xXml.Length);
                }

                xBuff = ms.ToArray();
            }

            string Output = Encoding.UTF8.GetString(xBuff);

            return Output;
        }
    }
}

