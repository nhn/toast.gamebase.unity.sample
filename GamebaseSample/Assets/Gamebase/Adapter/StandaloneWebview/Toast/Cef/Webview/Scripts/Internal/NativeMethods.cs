using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Toast.Cef.Webview.Internal
{
    internal static class NativeMethods
    {
        private const string DLL_NAME = "nhncef";

        [DllImport(DLL_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = false)]
        internal static extern bool Initialize(StringBuilder locale);

        [DllImport(DLL_NAME)]
        internal static extern void CreateWeb(int webviewIndex, int x, int y, int width, int height, IntPtr buffer, int option);

        [DllImport(DLL_NAME)]
        internal static extern void RemoveWeb(int webviewIndex);

        [DllImport(DLL_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = false)]
        internal static extern void LoadWeb(int webviewIndex, StringBuilder url, bool isShowScrollbar);

        [DllImport(DLL_NAME)]
        internal static extern void ResizeWeb(int webviewIndex, IntPtr buffer, int width, int height);

        [DllImport(DLL_NAME)]
        internal static extern void ShowScrollbar(int webviewIndex, bool isShow);

        [DllImport(DLL_NAME)]
        internal static extern int UpdateWeb(int webviewIndex, out IntPtr url, out int additionalInfo);

        [DllImport(DLL_NAME)]
        internal static extern void ExitCef();

        [DllImport(DLL_NAME)]
        internal static extern void InputWeb(int webviewIndex, int flags, int x, int y);

        [DllImport(DLL_NAME)]
        internal static extern void GoBackForwardHome(int webviewIndex, int direction);

        [DllImport(DLL_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = false)]
        internal static extern void ExecuteJavaScript(int webviewIndex, StringBuilder javaScript);

        [DllImport(DLL_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = false)]
        internal static extern void ReserveInvalidRedirectUrlSchemes(StringBuilder schemes);

        [DllImport(DLL_NAME)]
        internal static extern void SetDebugEnable(bool enable);
    }
}