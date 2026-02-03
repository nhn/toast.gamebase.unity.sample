using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Toast.Gamebase.Internal.Auth.Browser
{
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
    public enum BrowserType
    {
        Safari,
        GoogleChrome,
        MicrosoftEdge,
        Firefox,
        Whale,
        //Arc,
        //Opera,
        //Brave,
        //Vivaldi
    }

    public static class Extensions
    {
        public static string GetAppName(this BrowserType type)
        {
            return type switch
            {
                BrowserType.Safari => "Safari",
                BrowserType.GoogleChrome => "Google Chrome",
                BrowserType.MicrosoftEdge => "Microsoft Edge",
                BrowserType.Firefox => "Fire fox",
                BrowserType.Whale => "Whale",
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
    }

    class MacOSBrowser : IBrowser
    {
        private static readonly Dictionary<string, BrowserType> _browsers = new Dictionary<string, BrowserType>
        {
            { "com.apple.safari", BrowserType.Safari },
            { "com.google.chrome", BrowserType.GoogleChrome },
            { "com.microsoft.edgemac", BrowserType.MicrosoftEdge },
            { "org.mozilla.firefox", BrowserType.Firefox },
            { "com.naver.whale", BrowserType.Whale },
            //{ "company.thebrowser.browser", "Arc" },
            //{ "com.operasoftware.opera", "Opera" },
            //{ "com.brave.browser", "Brave" },
            //{ "com.vivaldi.vivaldi", "Vivaldi" }
        };

        public void OpenLoginWindow(string uri)
        {
            UnityEngine.Application.OpenURL(uri);

            //var browser = GetDefaultBrowser();
            //OpenUriWithBrowser(browser, uri);
        }

        private static BrowserType GetDefaultBrowser()
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "defaults",
                Arguments = "read com.apple.LaunchServices/com.apple.launchservices.secure LSHandlers",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var proc = Process.Start(startInfo);
            var output = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit();

            if (proc.ExitCode != 0)
            {
                throw new Exception($"Failed to find default browser({proc.StandardError.ReadToEnd()}).");
            }

            foreach (var browser in _browsers)
            {
                if (output.Contains(browser.Key))
                {
                    return browser.Value;
                }
            }

            throw new Exception("Failed to find default browser.");
        }

        private static void OpenUriWithBrowser(BrowserType browser, string uri)
        {
            if (browser == BrowserType.Safari)
            {
                using var proc = Process.Start(new ProcessStartInfo
                {
                    FileName = "open",
                    Arguments = $"-a \"Safari\" \"{uri}\"",
                    UseShellExecute = false
                });

                proc.WaitForExit();
            }
            else
            {
                string args;
                if (browser == BrowserType.GoogleChrome || browser == BrowserType.Whale || browser == BrowserType.MicrosoftEdge)
                {
                    args = $"--new-window -app=\"{uri}\"";
                }
                else
                {
                    args = $"--new-window \"{uri}\"";
                }

                using var proc = Process.Start(new ProcessStartInfo
                {
                    FileName = GetBrowserExecutablePath(browser),
                    Arguments = args,
                    UseShellExecute = false
                });

                proc.WaitForExit();
            }
        }

        private static string GetBrowserExecutablePath(BrowserType browser)
        {
            var appPath = GetBrowserAppPath(browser);

            var executableName = GetExecutableNameFromInfoPlist(appPath);
            if (!string.IsNullOrEmpty(executableName))
            {
                var executablePath = Path.Combine(appPath, "Contents", "MacOS", executableName);
                if (File.Exists(executablePath))
                {
                    return executablePath;
                }
            }

            var commonExecutablePath = GetCommonExecutablePath(appPath, browser);
            if (!string.IsNullOrEmpty(commonExecutablePath) && File.Exists(commonExecutablePath))
            {
                return commonExecutablePath;
            }

            var macOSPath = Path.Combine(appPath, "Contents", "MacOS");
            if (Directory.Exists(macOSPath))
            {
                var executableFiles = Directory.GetFiles(macOSPath)
                    .Where(IsExecutableFile)
                    .ToList();

                if (executableFiles.Any())
                {
                    return executableFiles.First();
                }
            }

            throw new Exception();
        }

        private static string GetBrowserAppPath(BrowserType browser)
        {
            var searchPaths = new[]
            {
                "/Applications",
                "/System/Applications",
                $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/Applications"
            };

            foreach (var searchPath in searchPaths)
            {
                if (!Directory.Exists(searchPath)) continue;

                var appFiles = Directory.GetDirectories(searchPath, "*.app")
                    .Where(dir => Path.GetFileNameWithoutExtension(dir).Contains(browser.GetAppName()))
                    .ToList();

                if (appFiles.Any())
                {
                    return appFiles.First();
                }
            }

            throw new Exception($"Not found app({browser.GetAppName()}) path.");
        }

        private static string GetExecutableNameFromInfoPlist(string appPath)
        {
            try
            {
                var infoPlistPath = Path.Combine(appPath, "Contents", "Info.plist");
                if (!File.Exists(infoPlistPath))
                {
                    return null;
                }

                var startInfo = new ProcessStartInfo
                {
                    FileName = "plutil",
                    Arguments = $"-p \"{infoPlistPath}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = Process.Start(startInfo);
                if (process == null) return null;

                var output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                if (process.ExitCode != 0) return null;

                // CFBundleExecutable 찾기
                var lines = output.Split('\n');
                foreach (var line in lines)
                {
                    if (line.Contains("CFBundleExecutable") && line.Contains("=>"))
                    {
                        var parts = line.Split(new[] { "=>" }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length >= 2)
                        {
                            return parts[1].Trim().Trim('"');
                        }
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        private static string GetCommonExecutablePath(string appPath, BrowserType brower)
        {
            var commonPatterns = new[]
            {
                brower.GetAppName(),
                brower.GetAppName().Replace(" ", ""),  // "Google Chrome" -> "GoogleChrome"
                Path.GetFileNameWithoutExtension(appPath),
            };

            foreach (var pattern in commonPatterns)
            {
                var executablePath = Path.Combine(appPath, "Contents", "MacOS", pattern);
                if (File.Exists(executablePath))
                {
                    return executablePath;
                }
            }

            return null;
        }

        private static bool IsExecutableFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath)) return false;

                // macOS에서 실행 권한 확인
                var startInfo = new ProcessStartInfo
                {
                    FileName = "test",
                    Arguments = $"-x \"{filePath}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = Process.Start(startInfo);
                if (process == null) return false;
                process.WaitForExit();
                return process.ExitCode == 0;
            }
            catch
            {
                return false;
            }
        }

        public void CloseLoginWindow()
        {
            
        }
    }
#endif
}
