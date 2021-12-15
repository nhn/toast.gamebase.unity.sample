#if UNITY_STANDALONE || UNITY_EDITOR
namespace Toast.Internal
{
    using System.IO;

    public static class ToastFile
    {
        public static bool Write(string path, string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            try
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter(fileStream))
                    {
                        streamWriter.WriteLine(text);
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static string Read(string path)
        {
            string text = "";

            try
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader streamReader = new StreamReader(fileStream))
                    {
                        while (!streamReader.EndOfStream)
                        {
                            text += streamReader.ReadLine();
                        }
                    }
                }
            }
            catch
            {
                // ignore
            }

            return text;
        }

        public static void Delete(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                }
                catch
                {
                    // ignore
                }
            }
        }

        public static bool Exists(string path)
        {
            if(string.IsNullOrEmpty(path))
            {
                return false;
            }

            return File.Exists(path);
        }
    }
}
#endif