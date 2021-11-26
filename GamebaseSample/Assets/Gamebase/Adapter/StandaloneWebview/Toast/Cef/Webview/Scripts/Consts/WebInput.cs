namespace Toast.Cef.Webview
{
    public static class WebInput
    {
        public const int NONE = 0;
        public const int EVENT = 0xF0;
        public const int PRESS = 0x10;
        public const int RELEASE = 0x20;
        public const int MOVE = 0x30;
        public const int SCROLL = 0x40;
        public const int CHARACTER = 0x50;
        public const int FOCUS = 0x60;
        public const int LOST = 0x70;
        public const int MODIFIER = 0xF00;
        public const int SHIFT = 0x100;
        public const int ALT = 0x200;
        public const int CTRL = 0x400;
        public const int MOUSEPRESSING = 0x100;
        public const int MASK = 0xF000;
        public const int MOUSE = 0x1000;
        public const int KEYBOARD = 0x2000;
        public const int JSDIALOG = 0x4000;
    }
}