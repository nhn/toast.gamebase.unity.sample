namespace Toast.Cef.Webview
{
    public static class WebUpdateStatus
    {
        public const int INVALID = -1;
        public const int NONE = 0;
        public const int AVAILABLE = 0x100;
        public const int BROWSABLE = 0x200;
        public const int FOCUSED = 0x400;
        public const int JSDIALOG = 0x800;
        public const int ERROR = 0x1000;
        public const int TITLE = 0x2000;
        public const int POPUPBLOCK = 0x4000;
        public const int INPUTFOCUS = 0x8000;
        public const int LOAD_END = 0x100000;
        public const int PASS_POPUP_INFO = 0x200000;
        public const int CURSOR_MASK = 0xFF;
    }
}