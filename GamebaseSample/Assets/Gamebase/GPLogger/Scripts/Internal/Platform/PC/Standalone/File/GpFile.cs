#if UNITY_STANDALONE || UNITY_EDITOR
namespace GamePlatform.Logger.Internal
{
    using System.IO;
    using System.Text;

    public static class GpFile
    {
        public static bool Write(string path, string text)
        {
            if (string.IsNullOrEmpty(text) == true)
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
            var sb = new StringBuilder();

            try
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader streamReader = new StreamReader(fileStream))
                    {
                        while (streamReader.EndOfStream == false)
                        {
                            sb.Append(streamReader.ReadLine());
                        }
                    }
                }
            }
            catch
            {
                // ignore
            }

            return sb.ToString();
        }

        public static void Delete(string path)
        {
            if (File.Exists(path) == true)
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
            if (string.IsNullOrEmpty(path) == true)
            {
                return false;
            }

            return File.Exists(path);
        }
    }
}
#endif