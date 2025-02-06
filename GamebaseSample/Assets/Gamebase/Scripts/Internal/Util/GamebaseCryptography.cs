﻿using System.IO;
using System.Security.Cryptography;

namespace Toast.Gamebase.Internal
{
    public class GamebaseCryptography
    {
        private ICryptoTransform encryptTransform = null;
        private ICryptoTransform decryptTransform = null;
        private readonly byte[] key = { 113,  50,  47,  78,  83, 56,   88,  38,  61,  69,  13,  62,  60, 207,  45, 113,
                                        240, 236, 230, 236,  14, 169,  10,  19,  49, 238, 127, 213,  69, 142,  95,  16 };
        private readonly byte[] iv = {  125, 154,  69,  31, 127,   8, 112, 250, 188,  39, 230,  10,  31, 139,  39, 139 };

        private static GamebaseCryptography instance = null;

        public static GamebaseCryptography Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GamebaseCryptography();
                    instance.Init();
                }

                return instance;
            }
        }
        
        private void Init()
        {
            using var rm = new RijndaelManaged();
            encryptTransform = rm.CreateEncryptor(key, iv);
            decryptTransform = rm.CreateDecryptor(key, iv);
        }

        public string EncryptStringToString(string original)
        {
            byte[] encrypted = EncryptStringToBytes(original);
            return ConvertByteArrToString(encrypted);
        }
        public byte[] EncryptStringToBytes(string original)
        {
            byte[] encrypted = null;

            using (var ms = new MemoryStream())
            {
                using var cs = new CryptoStream(ms, encryptTransform, CryptoStreamMode.Write);
                using (var sw = new StreamWriter(cs))
                {
                    sw.Write(original);
                }

                encrypted = ms.ToArray();
            }

            return encrypted;
        }
        
        public string DecryptStringFromString(string encrypted)
        {
            return DecryptStringFormBytes(ConvertStringToByteArray(encrypted));
        }

        public string DecryptStringFormBytes(byte[] encrypted)
        {
            string original = null;

            using (var ms = new MemoryStream(encrypted))
            {
                using var cs = new CryptoStream(ms, decryptTransform, CryptoStreamMode.Read);
                using var sr = new StreamReader(cs);
                original = sr.ReadToEnd();
            }

            return original;
        }


        public byte[] ConvertStringToByteArray(string original)
        {
            byte[] convert = null;

            if (original.Length > 0)
            {
                convert = new byte[original.Length / 3];
                int originalIndex = 0;
                int convertIndex = 0;
                byte value;

                do
                {
                    value = byte.Parse(original.Substring(originalIndex, 3));
                    convert[convertIndex++] = value;
                    originalIndex += 3;
                }
                while (originalIndex < original.Length);
            }

            return convert;
        }


        public string ConvertByteArrToString(byte[] original)
        {
            string convert = string.Empty;
            byte val;

            for (int i = 0; i <= original.GetUpperBound(0); ++i)
            {
                val = original[i];

                if (val < (byte)10)
                {
                    convert += "00" + val.ToString();
                }
                else if (val < (byte)100)
                {
                    convert += "0" + val.ToString();
                }
                else
                {
                    convert += val.ToString();
                }
            }

            return convert;
        }
    }
}