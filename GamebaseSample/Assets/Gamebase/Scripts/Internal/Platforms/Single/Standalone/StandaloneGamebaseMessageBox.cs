#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Toast.Gamebase.Internal.Single.Standalone
{
    public class StandaloneGamebaseMessageBox
    {
        // https://msdn.microsoft.com/ko-kr/library/windows/desktop/ms645505(v=vs.85).aspx

        #region windows message

        private const int WH_CALLWNDPROCRET     = 12;
        private const int WM_DESTROY            = 0x0002;
        private const int WM_INITDIALOG         = 0x0110;
        private const int WM_TIMER              = 0x0113;
        private const int WM_USER               = 0x400;
        private const int DM_GETDEFID           = WM_USER + 0;

        #endregion

        #region message box

        public const uint MB_OK                 = 0x00000000;
        public const uint MB_OKCANCEL           = 0x00000001;

        public const uint MB_ICONEXCLAMATION    = 0x00000030;
        public const uint MB_ICONWARNING        = 0x00000030;
        public const uint MB_ICONINFORMATION    = 0x00000040;
        public const uint MB_ICONASTERISK       = 0x00000040;
        public const uint MB_ICONQUESTION       = 0x00000020;
        public const uint MB_ICONSTOP           = 0x00000010;
        public const uint MB_ICONERROR          = 0x00000010;
        public const uint MB_ICONHAND           = 0x00000010;

        public const uint MB_DEFBUTTON1         = 0x00000000;
        public const uint MB_DEFBUTTON2         = 0x00000100;

        public const int IDOK                   = 1;
        public const int IDCANCEL               = 2;
        
        public static string OK                 = "&OK";
        public static string Cancel             = "&Cancel";

        #endregion
        
        #region windows api

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport("user32.dll")]
        private static extern int UnhookWindowsHookEx(IntPtr idHook);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool EnumChildWindows(IntPtr hWndParent, EnumChildProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "GetClassNameW", CharSet = CharSet.Unicode)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern int GetDlgCtrlID(IntPtr hwndCtl);

        [DllImport("user32.dll")]
        private static extern IntPtr GetDlgItem(IntPtr hDlg, int nIDDlgItem);

        [DllImport("user32.dll", EntryPoint = "SetWindowTextW", CharSet = CharSet.Unicode)]
        private static extern bool SetWindowText(IntPtr hWnd, string lpString);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int MessageBox(IntPtr hWnd, String text, String caption, uint type);

        [DllImport("kernel32.dll")]
        static extern uint GetCurrentThreadId();

        [DllImport("user32.dll")]
        private static extern IntPtr GetActiveWindow();

        #endregion
        
        [StructLayout(LayoutKind.Sequential)]
        public struct CWPRETSTRUCT
        {
            public IntPtr   lResult;
            public IntPtr   lParam;
            public IntPtr   wParam;
            public uint     message;
            public IntPtr   hwnd;
        };

        private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);
        private delegate bool EnumChildProc(IntPtr hWnd, IntPtr lParam);


        private static HookProc         hookProc    = null;
        private static EnumChildProc    enumProc    = null;

        [ThreadStatic]
        private static IntPtr           hHook       = IntPtr.Zero;

        [ThreadStatic]
        private static int              buttonCount = 0;
        
        static StandaloneGamebaseMessageBox()
        {
            hookProc    = new HookProc(MessageBoxHookProc);
            enumProc    = new EnumChildProc(MessageBoxEnumProc);
            hHook       = IntPtr.Zero;

            ResetButtonText();
        }

        public static void Register()
        {
            if (IntPtr.Zero != hHook)
            {
                throw new NotSupportedException("One hook per thread allowed.");
            }

            hHook = SetWindowsHookEx(WH_CALLWNDPROCRET, hookProc, IntPtr.Zero, (int)GetCurrentThreadId());
        }

        public static void Unregister()
        {
            if (hHook != IntPtr.Zero)
            {
                UnhookWindowsHookEx(hHook);
                hHook = IntPtr.Zero;
            }
        }

        private static IntPtr MessageBoxHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
            {
                return CallNextHookEx(hHook, nCode, wParam, lParam);
            }

            CWPRETSTRUCT msg = (CWPRETSTRUCT)Marshal.PtrToStructure(lParam, typeof(CWPRETSTRUCT));
            IntPtr hook = hHook;

            if (WM_INITDIALOG == msg.message)
            {
                StringBuilder className = new StringBuilder(10);
                GetClassName(msg.hwnd, className, className.Capacity);

                if (className.ToString() == "#32770")
                {
                    buttonCount = 0;
                    EnumChildWindows(msg.hwnd, enumProc, IntPtr.Zero);

                    if (1 == buttonCount)
                    {
                        IntPtr hButton = GetDlgItem(msg.hwnd, IDCANCEL);
                        if (IntPtr.Zero != hButton)
                        {
                            SetWindowText(hButton, OK);
                        }
                    }
                }
            }

            return CallNextHookEx(hook, nCode, wParam, lParam);
        }

        private static bool MessageBoxEnumProc(IntPtr hWnd, IntPtr lParam)
        {
            StringBuilder className = new StringBuilder(10);
            GetClassName(hWnd, className, className.Capacity);
            if (className.ToString() == "Button")
            {
                int ctlId = GetDlgCtrlID(hWnd);
                switch (ctlId)
                {
                    case IDOK:
                        {
                            SetWindowText(hWnd, OK);
                        } break;
                    case IDCANCEL:
                        {
                            SetWindowText(hWnd, Cancel);
                        } break;
                }

                ++buttonCount;
            }

            return true;
        }

        public static int ShowMessageBox(string title, string message, uint type)
        {
            Register();

            int result = MessageBox(GetActiveWindow(), message, title, type);

            Unregister();

            ResetButtonText();

            return result;
        }

        private static void ResetButtonText()
        {
            OK      = DisplayLanguage.Instance.GetString("common_ok_button");
            Cancel  = DisplayLanguage.Instance.GetString("common_cancel_button");
        }
    }
}

#endif