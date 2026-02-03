using AOT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Toast.Gamebase.Internal.Auth.Browser
{
# if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
    class WindowsBrowser : IBrowser
    {
        [DllImport("shell32.dll")]
        private static extern IntPtr ShellExecute(IntPtr hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, int nShowCmd);

        [DllImport("kernel32.dll")]
        private static extern uint GetLastError();

        private const int SW_SHOWNORMAL = 1;

        private HashSet<IntPtr> _new = null;

        public void OpenLoginWindow(string url)
        {
            _new = new HashSet<IntPtr>();
            HashSet<IntPtr> before = GetBrowserWindowHandles();
            HashSet<IntPtr> after = null;

            try
            {
                string browserPath = GetBrowserExecutablePath();
                string arguments = $"--new-window --app=\"{url}\"";

                IntPtr result = ShellExecute(IntPtr.Zero, "open", browserPath, arguments, null, SW_SHOWNORMAL);

                if ((int)result <= 32)
                {
                    uint error = GetLastError();
                    UnityEngine.Debug.LogError($"ShellExecute 실패: {result}, LastError: {error}");

                    // 대안 방법 시도
                    TrySimpleUrlOpen(url);
                }
                else
                {
                    UnityEngine.Debug.Log("브라우저 실행 성공");
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"브라우저 실행 오류: {ex.Message}");
                TrySimpleUrlOpen(url);
            }

            _ = Task.Run(async () =>
            {
                // 윈도우 핸들 발견할 때까지 약간의 시간이 필요할 수 있으므로, 재시도 로직을 추가한다.
                var retryCount = 5;
                while (retryCount-- > 0)
                {
                    after = GetBrowserWindowHandles();
                    var newWindows = after.Except(before).ToList();
                    if (newWindows.Count > 0)
                    {
                        foreach (var newWindow in newWindows)
                        {
                            _new.Add(newWindow);
                        }

                        break;
                    }

                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
            });
        }

        private void TrySimpleUrlOpen(string url)
        {
            try
            {
                IntPtr result = ShellExecute(IntPtr.Zero, "open", url, null, null, SW_SHOWNORMAL);

                if ((int)result <= 32)
                {
                    UnityEngine.Debug.LogError($"URL 열기 실패: {result}");
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"URL 열기 최종 실패: {ex.Message}");
            }
        }

        private HashSet<IntPtr> GetBrowserWindowHandles()
        {
            var handles = new HashSet<IntPtr>();

            EnumWindows(EnumWindowsCallback, GCHandle.ToIntPtr(GCHandle.Alloc(handles)));

            return handles;
        }

        [MonoPInvokeCallback(typeof(EnumWindowsProc))]
        private static bool EnumWindowsCallback(IntPtr hWnd, IntPtr lParam)
        {
            if (!IsWindowVisible(hWnd)) return true;

            var className = new StringBuilder(256);
            GetClassName(hWnd, className, className.Capacity);

            if (className.ToString().Contains("Chrome_WidgetWin"))
            {
                var handle = GCHandle.FromIntPtr(lParam);
                var target = (HashSet<IntPtr>)handle.Target;
                target.Add(hWnd);
            }

            return true;
        }

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        private static string GetBrowserExecutablePath()
        {
            try
            {
                if (TryGetExecutablePath(GetDefaultBrowser(), out var path) && File.Exists(path))
                {
                    return path;
                }

                throw new NotFoundBrowserException();
            }
            catch
            {
                throw new NotFoundBrowserException();
            }
        }

        private static string GetDefaultBrowser()
        {
            string subKey = @"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice";
            string progId = GetRegistryValue(HKEY_CURRENT_USER, subKey, "ProgId");

            if (string.IsNullOrEmpty(progId))
                throw new InvalidOperationException("ProgId not found in registry.");

            return progId;
        }

        private static bool TryGetExecutablePath(string browser, out string path)
        {
            string subKey = $@"{browser}\shell\open\command";
            string command = GetRegistryValue(HKEY_CLASSES_ROOT, subKey, null);

            if (string.IsNullOrEmpty(command))
            {
                path = string.Empty;
                return false;
            }

            try
            {
                path = ExtractExecutablePath(command);
                return true;
            }
            catch
            {
                path = string.Empty;
                return false;
            }
        }

        private static string GetRegistryValue(UIntPtr hKey, string subKey, string valueName)
        {
            if (RegOpenKeyEx(hKey, subKey, 0, KEY_READ, out UIntPtr hkResult) != 0)
                return string.Empty;

            try
            {
                uint dataSize = 1024;
                var data = new StringBuilder((int)dataSize);

                if (RegQueryValueEx(hkResult, valueName, 0, out uint type, data, ref dataSize) == 0)
                {
                    return data.ToString();
                }
            }
            finally
            {
                RegCloseKey(hkResult);
            }

            return string.Empty;
        }

        private static string ExtractExecutablePath(string input)
        {
            var match = Regex.Match(input, "\"([^\"]+\\.exe)\"");
            return match.Success ? match.Groups[1].Value
                : throw new InvalidOperationException("Executable path could not be extracted.");
        }

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
        private static extern int RegOpenKeyEx(UIntPtr hKey, string subKey, int ulOptions, int samDesired, out UIntPtr hkResult);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
        private static extern int RegQueryValueEx(UIntPtr hKey, string lpValueName, int lpReserved, out uint lpType, StringBuilder lpData, ref uint lpcbData);

        [DllImport("advapi32.dll")]
        private static extern int RegCloseKey(UIntPtr hKey);

        private static readonly UIntPtr HKEY_CURRENT_USER = new UIntPtr(0x80000001u);
        private static readonly UIntPtr HKEY_CLASSES_ROOT = new UIntPtr(0x80000000u);
        private const int KEY_READ = 0x20019;

        public void CloseLoginWindow()
        {
            const uint WM_CLOSE = 0x0010;

            _ = Task.Run(async () =>
            {
                // 새로운 로그인 창을 확인할 때까지 약간의 시간이 필요하기 때문에 재시도 로직을 추가한다.
                var retryCount = 5;
                while (retryCount-- > 0)
                {
                    if (_new.Count > 0)
                    {
                        foreach (var hWnd in _new)
                        {
                            PostMessage(hWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                        }
                        break;
                    }

                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
            });
        }

        [DllImport("user32.dll")]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
    }
#endif
}