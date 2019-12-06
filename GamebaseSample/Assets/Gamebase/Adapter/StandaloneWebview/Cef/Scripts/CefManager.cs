using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using IntPtr = System.IntPtr;

namespace Toast.Cef.Webview
{
    public delegate void WebUrlDelegate(string url);
    public delegate void WebTitleDelegate(string title);
    public delegate void WebInputElementFocusDelegate(int type);

    public class CefManager : MonoBehaviour
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        private const string                    ERROR_SCHEME                = "cef://error";
        private const string                    POPUP_BLOCK_DEFAULT_MESSAGE = "게임내에서 사용할 수 없는 링크입니다.";

        private bool                            initialize                  = false;
        private bool                            isShow                      = false;
        private bool                            webFocus                    = false;
        private bool                            forceFocus                  = false;
        private Texture2D                       texture                     = null;
        private Color32[]                       textureBuffer               = null;
        private GCHandle                        textureHandle               = new GCHandle();
        private Texture2D                       cursorArrow                 = null;
        private Texture2D                       cursorHand                  = null;
        private Texture2D                       cursorIBeam                 = null;
        private Vector2                         cursorHopspot               = new Vector2(5, 0);
        private bool                            webUpdated                  = false;
        private Rect                            webRect                     = Rect.zero;
        private Vector2                         mouse                       = Vector2.zero;
        private bool                            keyDown                     = false;
        private string                          compositionString           = string.Empty;
        private bool                            compositionInvalid          = false;
        private int                             fontSize                    = 0;
        private bool                            mouseDown                   = false;

        #region js dialog
        private string                          jsDialogMessage             = string.Empty;
        private Rect                            jsDialogRect                = new Rect(0, 0, 0, 0);
        private Rect                            jsDialogLeftButtonRect      = new Rect(0, 0, 0, 0);
        private Rect                            jsDialogRightButtonRect     = new Rect(0, 0, 0, 0);
        private Texture2D                       jsDialogBg                  = null;
        private GUIStyle                        jsDialogStyle               = null;
        private GUIStyle                        jsDialogBtnStyle            = null;
        private Texture2D                       jsDialogBtnBg               = null;
        private int                             jsDialogMinHeight           = 60;
        private JS_DIALOG_TYPE                  jsDialogType                = JS_DIALOG_TYPE.ALERT;
        #endregion

        #region popup block
        private bool                            popupBlock                  = false;
        private string                          popupBlockMessage           = string.Empty;
        private float                           popupBlockNotiTime          = 1.0f;
        private float                           popupBlockNotiStartTime     = 0.0f;
        private GUIStyle                        popupBlockDialogStyle       = null;
        private Rect                            popupBlockDialogRect        = new Rect(0, 0, 0, 0);
        private int                             popupBlockDialogMinHeight   = 60;
        private Texture2D                       popupBlockDialogBg          = null;
        #endregion

        #region delegate
        private WebUrlDelegate                  urlDelegate                 = null;
        private WebTitleDelegate                titleDelegate               = null;
        private WebInputElementFocusDelegate    inputElementFocusDelegate   = null;
        #endregion
        
        #region web update state
        private const int                       WEB_UPDATE_INVALID          = -1;
        private const int                       WEB_UPDATE_NONE             = 0;
        private const int                       WEB_UPDATE_AVAILABLE        = 0x100;
        private const int                       WEB_UPDATE_BROWSABLE        = 0x200;
        private const int                       WEB_UPDATE_FOCUSED          = 0x400;
        private const int                       WEB_UPDATE_JSDIALOG         = 0x800;
        private const int                       WEB_UPDATE_ERROR            = 0x1000;
        private const int                       WEB_UPDATE_TITLE            = 0x2000;
        private const int                       WEB_UPDATE_POPUPBLOCK       = 0x4000;
        private const int                       WEB_UPDATE_INPUTFOCUS       = 0x8000;
        private const int                       WEB_UPDATE_CURSOR_MASK      = 0xFF;
        #endregion

        #region web option
        private const int                       WEB_OPTION_INVERSE_COLOR    = 0x1;
        private const int                       WEB_OPTION_INVERSE_Y        = 0x2;
        private const int                       WEB_OPTION_CLEAR_COOKIES    = 0x8;
        #endregion

        #region web input
        private const int                       WEB_INPUT_NONE              = 0;
        private const int                       WEB_INPUT_EVENT             = 0xF0;
        private const int                       WEB_INPUT_PRESS             = 0x10;
        private const int                       WEB_INPUT_RELEASE           = 0x20;
        private const int                       WEB_INPUT_MOVE              = 0x30;
        private const int                       WEB_INPUT_SCROLL            = 0x40;
        private const int                       WEB_INPUT_CHARACTER         = 0x50;
        private const int                       WEB_INPUT_FOCUS             = 0x60;
        private const int                       WEB_INPUT_LOST              = 0x70;
        private const int                       WEB_INPUT_MODIFIER          = 0xF00;
        private const int                       WEB_INPUT_SHIFT             = 0x100;
        private const int                       WEB_INPUT_ALT               = 0x200;
        private const int                       WEB_INPUT_CTRL              = 0x400;
        private const int                       WEB_INPUT_MOUSEPRESSING     = 0x100;
        private const int                       WEB_INPUT_MASK              = 0xF000;
        private const int                       WEB_INPUT_MOUSE             = 0x1000;
        private const int                       WEB_INPUT_KEYBOARD          = 0x2000;
        private const int                       WEB_INPUT_JSDIALOG          = 0x4000;
        #endregion

        private static CefManager m_instance = null;
        public static CefManager GetInstance()
        {
            if (m_instance == null)
            {
                m_instance = new GameObject("CefManager").AddComponent<CefManager>();
                m_instance.Init();

                DontDestroyOnLoad(m_instance.gameObject);
            }

            return m_instance;
        }

        public static void ExecuteJavaScript(StringBuilder javaScript)
        {
            NativeMethods.ExecuteJavaScript(javaScript);
        }

        public static void SetDebugEnable(bool enable)
        {
            NativeMethods.SetDebugEnable(enable);
        }

        public static void UnloadWeb(bool shutdown)
        {
            NativeMethods.UnloadWeb(shutdown);
        }
        
        public void SetPopupBlockNotiTime(float time)
        {
            popupBlockNotiTime = time;
        }

        public void SetPopupBlockMessage(string message)
        {
            popupBlockMessage = message;
            SettingPopupBlockDialog();
        }

        public void GoBack()
        {
            if (isShow == false)
            {
                return;
            }

            NativeMethods.GoBackForwardHome((int)WEB_NAVIGATION_DIRECTION.BACK);
        }

        public void GoForward()
        {
            if (isShow == false)
            {
                return;
            }

            NativeMethods.GoBackForwardHome((int)WEB_NAVIGATION_DIRECTION.FORWARD);
        }

        public void GoHome()
        {
            if (isShow == false)
            {
                return;
            }

            NativeMethods.GoBackForwardHome((int)WEB_NAVIGATION_DIRECTION.HOME);
        }

        public void SetInvalidRedirectUrlScheme(StringBuilder schemes)
        {
            NativeMethods.ReserveInvalidRedirectUrlSchemes(schemes);
        }

        public void ShowWeb(Rect viewRect, 
                           StringBuilder url, 
                           WebUrlDelegate webUrlDelegate, 
                           StringBuilder locale, 
                           WebTitleDelegate webTitleDelegate,
                           WebInputElementFocusDelegate webInputElementFocusDelegate, 
                           string popupBlockMessage)
        {
            //Debug.Log("[CefManager.ShowWeb] viewRect : " + viewRect + " url : " + url + " locale : " + locale);

            urlDelegate                 += webUrlDelegate;
            titleDelegate               += webTitleDelegate;
            inputElementFocusDelegate   += webInputElementFocusDelegate;
            isShow                      = true;
            mouseDown                   = false;

            if (string.IsNullOrEmpty(popupBlockMessage) == false)
            {
                this.popupBlockMessage = popupBlockMessage;
            }

            if (webRect.width != viewRect.width || webRect.height != viewRect.height)
            {
                //Debug.Log("=> rect not same");

                if (null != texture)
                {
                    Destroy(texture);
                }

                webRect = viewRect;
                texture = new Texture2D((int)webRect.width, (int)webRect.height, TextureFormat.ARGB32, false, true);
                textureBuffer = texture.GetPixels32();
                textureHandle = GCHandle.Alloc(textureBuffer, GCHandleType.Pinned);
            }
            else
            {
                //Debug.Log("=> rect same");

                if (null != texture)
                {
                    for (int i = 0; i < textureBuffer.Length; ++i)
                    {
                        textureBuffer[i] = Color.white;
                    }

                    webUpdated = true;
                }
            }

            NativeMethods.LoadWeb(0, 0, (int)webRect.width, (int)webRect.height,
                                  textureHandle.AddrOfPinnedObject(),
                                  url,
                                  WEB_OPTION_INVERSE_Y | WEB_OPTION_INVERSE_COLOR | WEB_OPTION_CLEAR_COOKIES,
                                  locale);

            if (initialize == false)
            {
                initialize = true;

#if UNITY_EDITOR
                CefEditorCleaner.Create();
#endif
            }
        }

        public void ShowWeb(int x, int y, int width, int height, 
                            StringBuilder url, 
                            WebUrlDelegate webUrlDelegate, 
                            StringBuilder locale, 
                            WebTitleDelegate webTitleDelegate,
                            WebInputElementFocusDelegate webInputElementFocusDelegate, 
                            string popupBlockMessage)
        {
            ShowWeb(new Rect(x, y, width, height), url, webUrlDelegate, locale, webTitleDelegate, webInputElementFocusDelegate, popupBlockMessage);
        }

        public void RemoveUrlDelegate(WebUrlDelegate webUrlDelegate)
        {
            urlDelegate -= webUrlDelegate;
        }

        public void RemoveTitleDelegate(WebTitleDelegate webTitleDelegate)
        {
            titleDelegate -= webTitleDelegate;
        }

        public void RemoveInputFocusDelegate(WebInputElementFocusDelegate webInputElementFocusDelegate)
        {
            inputElementFocusDelegate -= webInputElementFocusDelegate;
        }

        public void HideWeb()
        {
            //Debug.Log("[CefManager.HideWeb]");

            urlDelegate = null;
            titleDelegate = null;
            inputElementFocusDelegate = null;
            isShow = false;
            jsDialogMessage = string.Empty;

            UnloadWeb(false);
            SetDefaultCursor();
        }

        private void Init()
        {
            Color32 dialogCornerColor       = new Color32(0xff, 0xff, 0xff, 0x00);
            Color32 dialogBorderColor       = new Color32(0xc5, 0xc5, 0xc5, 0xff);
            Color32 dialobBgColor           = new Color32(0xfb, 0xfb, 0xfb, 0xff);
            jsDialogBg                      = new Texture2D(4, 4, TextureFormat.ARGB32, false, false);

            jsDialogBg.SetPixels32(new Color32[] { dialogCornerColor, dialogBorderColor, dialogBorderColor, dialogCornerColor,
                                                   dialogBorderColor, dialobBgColor,     dialobBgColor,     dialogBorderColor,
                                                   dialogBorderColor, dialobBgColor,     dialobBgColor,     dialogBorderColor,
                                                   dialogCornerColor, dialogBorderColor, dialogBorderColor, dialogCornerColor });
            jsDialogBg.Apply(false, false);

            Color32 btnCornerColor          = new Color32(0xc5, 0xd7, 0xfa, 0x00);
            Color32 btnBorderColor          = new Color32(0x3e, 0x7e, 0xf8, 0xff);
            Color32 btnBgColor              = new Color32(0xf9, 0xf9, 0xf9, 0xff);
            jsDialogBtnBg                   = new Texture2D(4, 4, TextureFormat.ARGB32, false, false);

            jsDialogBtnBg.SetPixels32(new Color32[] { btnCornerColor, btnBorderColor, btnBorderColor, btnCornerColor,
                                                      btnBorderColor, btnBgColor,     btnBgColor,     btnBorderColor,
                                                      btnBorderColor, btnBgColor,     btnBgColor,     btnBorderColor,
                                                      btnCornerColor, btnBorderColor, btnBorderColor, btnCornerColor });

            jsDialogBtnBg.Apply(false, false);


            Color32 blockDialogCornerColor  = new Color32(0xff, 0xff, 0xff, 0x00);
            Color32 blockDialogBorderColor  = new Color32(0xb7, 0xa2, 0x73, 0xff);
            Color32 blockDialobBgColor      = new Color32(0xf9, 0xf8, 0xc3, 0xff);
            popupBlockDialogBg              = new Texture2D(4, 4, TextureFormat.ARGB32, false, false);

            popupBlockDialogBg.SetPixels32(new Color32[] { blockDialogCornerColor, blockDialogBorderColor, blockDialogBorderColor, blockDialogCornerColor,
                                                           blockDialogBorderColor, blockDialobBgColor,     blockDialobBgColor,     blockDialogBorderColor,
                                                           blockDialogBorderColor, blockDialobBgColor,     blockDialobBgColor,     blockDialogBorderColor,
                                                           blockDialogCornerColor, blockDialogBorderColor, blockDialogBorderColor, blockDialogCornerColor });
            popupBlockDialogBg.Apply(false, false);


            popupBlockMessage = POPUP_BLOCK_DEFAULT_MESSAGE;
        }

        private void InitJsDialogBtnSize()
        {
            jsDialogLeftButtonRect.width    = fontSize * 4;
            jsDialogLeftButtonRect.height   = fontSize * 3;

            jsDialogRightButtonRect.width   = jsDialogLeftButtonRect.width;
            jsDialogRightButtonRect.height  = jsDialogLeftButtonRect.height;
        }

        private void SendBackspaceKeyEvent()
        {
            NativeMethods.InputWeb(WEB_INPUT_KEYBOARD | WEB_INPUT_PRESS, NativeKeyCodes[KeyCode.Backspace], VirtualKeyCodes[KeyCode.Backspace]);
            NativeMethods.InputWeb(WEB_INPUT_KEYBOARD | WEB_INPUT_CHARACTER, NativeKeyCodes[KeyCode.Backspace], VirtualKeyCodes[KeyCode.Backspace]);
            NativeMethods.InputWeb(WEB_INPUT_KEYBOARD | WEB_INPUT_RELEASE, NativeKeyCodes[KeyCode.Backspace], VirtualKeyCodes[KeyCode.Backspace]);
        }

        private void SetCompositionStringInvalid()
        {
            compositionInvalid = true;
            SetCompositionStringEmpty();
        }

        private void SetCompositionStringEmpty()
        {
            compositionString = string.Empty;
        }

        private void SettingJsDialog()
        {
            if (jsDialogStyle == null)
            {
                jsDialogStyle                       = new GUIStyle();
                jsDialogStyle.alignment             = TextAnchor.MiddleCenter;
                jsDialogStyle.border                = new RectOffset(2, 2, 2, 2);
                //jsDialogStyle.padding               = new RectOffset(2, 2, 2, 2);
                jsDialogStyle.normal.background     = jsDialogBg;
                jsDialogStyle.normal.textColor      = Color.black;
            }

            if (jsDialogBtnStyle == null)
            {
                jsDialogBtnStyle                    = new GUIStyle();
                jsDialogBtnStyle.alignment          = TextAnchor.MiddleCenter;
                jsDialogBtnStyle.border             = new RectOffset(2, 2, 2, 2);
                jsDialogBtnStyle.normal.background  = jsDialogBtnBg;
                jsDialogBtnStyle.normal.textColor   = Color.black;
            }

            if (string.IsNullOrEmpty(jsDialogMessage) == false)
            {
                // dialog rect 
                jsDialogRect = webRect;

                int width                   = 0;
                int height                  = 0;
                int buttonHeightWeight      = (int)((float)jsDialogRightButtonRect.height * 1.5f);
                int topPadding              = 5;

                CalculateWidthHeight(out width, out height, jsDialogMessage, jsDialogMinHeight, 1);


                jsDialogRect.width          = width;
                jsDialogRect.height         = height + buttonHeightWeight + topPadding;
                jsDialogRect.x              = (webRect.width - jsDialogRect.width) * 0.5f;
                jsDialogRect.y              = webRect.y;
                jsDialogStyle.padding       = new RectOffset(0, 0, topPadding, buttonHeightWeight);

                // button rect
                jsDialogRightButtonRect.x   = (jsDialogRect.x + jsDialogRect.width) - (int)((float)jsDialogRightButtonRect.width * 1.5f);
                jsDialogRightButtonRect.y   = jsDialogRect.y + jsDialogRect.height - buttonHeightWeight;

                jsDialogLeftButtonRect.x    = jsDialogRightButtonRect.x - jsDialogRightButtonRect.width - topPadding;
                jsDialogLeftButtonRect.y    = jsDialogRightButtonRect.y;
            }
        }

        private void SettingPopupBlockDialog()
        {
            if (popupBlockDialogStyle == null)
            {
                popupBlockDialogStyle                   = new GUIStyle();
                popupBlockDialogStyle.alignment         = TextAnchor.MiddleCenter;
                popupBlockDialogStyle.border            = new RectOffset(2, 2, 2, 2);
                popupBlockDialogStyle.normal.background = popupBlockDialogBg;
                popupBlockDialogStyle.normal.textColor  = new Color(0.4705f, 0.3451f, 0.1921f);
            }

            if (string.IsNullOrEmpty(popupBlockMessage) == false)
            {
                // dialog rect 
                popupBlockDialogRect = webRect;

                int width   = 0;
                int height  = 0;

                CalculateWidthHeight(out width, out height, popupBlockMessage, popupBlockDialogMinHeight);

                popupBlockDialogRect.width  = width;
                popupBlockDialogRect.height = height;
                popupBlockDialogRect.x      = (webRect.width - popupBlockDialogRect.width) * 0.5f;
                popupBlockDialogRect.y      = (webRect.height - popupBlockDialogRect.height) * 0.5f;
            }
        }

        private void CalculateWidthHeight(out int width, out int height, string message, int minHeight, int linePlugCount = 0)
        {
            string[] split = message.Split('\n');
            int lineCount = split.Length;

            height = (lineCount + linePlugCount) * fontSize;
            width = 0;

            for (int i = 0; i < split.Length; ++i)
            {
                int stringCount = split[i].Length;
                if (width < stringCount)
                {
                    width = stringCount;
                }
            }

            width *= (int)((float)fontSize * 1.5f);

            if (height > (int)webRect.height)
            {
                height = (int)webRect.height;
            }
            else if (height < minHeight)
            {
                height = minHeight;
            }

            if (width > (int)webRect.width)
            {
                width = (int)webRect.width;
            }
            else if (width < (int)(webRect.width * 0.3f))
            {
                width = (int)(webRect.width * 0.3f);
            }
        }

        // Update is called once per frame
        private void Update()
        {
            if (isShow == false)
            {
                return;
            }

            if (initialize == true)
            {
                if (webFocus == true)
                {
                    if (string.IsNullOrEmpty(Input.inputString) == false)
                    {
                        //Debug.Log("[CefManater.Update] Input.inputstring : " + Input.inputString);

                        if (compositionInvalid == true)
                        {
                            //Debug.Log("=> composition invalid");
                            compositionInvalid = false;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(compositionString) == false)
                            {
                                //Debug.Log("=> compositionString not empty : " + compositionString);

                                for(int i = 0; i <  compositionString.Length; ++i)
                                {
                                    //Debug.Log("==> delete : " + c);
                                    SendBackspaceKeyEvent();
                                }
                            }

                            foreach (char c in Input.inputString)
                            {
                                //Debug.Log("==> input : " + c);
                                NativeMethods.InputWeb(WEB_INPUT_KEYBOARD | WEB_INPUT_CHARACTER, c, c);
                            }

                            SetCompositionStringEmpty();
                        }
                    }
                    else if (string.IsNullOrEmpty(Input.compositionString) == false)
                    {
                        //Debug.Log("[CefManater.Update] Input.composition string : " + Input.compositionString);
                        if (compositionString.Equals(Input.compositionString) == false)
                        {
                            //Debug.Log("=> compositionString not equal Input.compositionString [ " + compositionString + " ] : [ " + Input.compositionString + "]");
                            if (string.IsNullOrEmpty(compositionString) == false)
                            {
                                //Debug.Log("==> backspace");

                                for (int i = 0; i < compositionString.Length; ++i)
                                {
                                    SendBackspaceKeyEvent();
                                }
                            }

                            foreach (char c in Input.compositionString)
                            {
                                //Debug.Log("===> input : " + c);
                                NativeMethods.InputWeb(WEB_INPUT_KEYBOARD | WEB_INPUT_PRESS, c, c);
                                NativeMethods.InputWeb(WEB_INPUT_KEYBOARD | WEB_INPUT_CHARACTER, c, c);
                                NativeMethods.InputWeb(WEB_INPUT_KEYBOARD | WEB_INPUT_RELEASE, c, c);
                            }

                            compositionString = Input.compositionString;
                        }
                    }
                    else if (string.IsNullOrEmpty(compositionString) == false && string.IsNullOrEmpty(Input.compositionString) == true)
                    {
                        //Debug.Log("[CefManager.Update] Input.compositionString empty, compositionString not empty : " + compositionString);

                        SendBackspaceKeyEvent();
                        SetCompositionStringEmpty();
                    }
                }

                IntPtr url = IntPtr.Zero;

                int additionalInfo = 0;
                int state = NativeMethods.UpdateWeb(out url, out additionalInfo);

                //string debug = string.Format("[CefManager] state {0:X}", state);
                //Debug.Log(debug);

                if (WEB_UPDATE_INVALID != state && WEB_UPDATE_NONE != state)
                {
                    if ((state & WEB_UPDATE_ERROR) != 0)
                    {
                        string errorText = string.Format("{0}?{1}", ERROR_SCHEME, Marshal.PtrToStringAnsi(url));
                        //Debug.Log("=> Load Error [" + errorCode + "] [" + errorText);

                        if (null != urlDelegate)
                        {
                            urlDelegate(errorText);
                        }

                        //HideWeb();
                    }
                    else
                    {
                        if ((state & WEB_UPDATE_BROWSABLE) != 0)
                        {
                            string webUrl = Marshal.PtrToStringAnsi(url);
                            //Debug.Log("=> browser url " + webUrl);

                            if (null != urlDelegate)
                            {
                                urlDelegate(webUrl);
                            }
                        }

                        if ((state & WEB_UPDATE_INPUTFOCUS) != 0)
                        {
                            if (null != inputElementFocusDelegate)
                            {
                                inputElementFocusDelegate(additionalInfo);
                            }
                        }

                        if ((state & WEB_UPDATE_TITLE) != 0)
                        {
                            string title = Marshal.PtrToStringAnsi(url);
                            //Debug.Log("=> title : " + title);

                            if (null != titleDelegate)
                            {
                                titleDelegate(title);
                            }
                        }

                        if ((state & WEB_UPDATE_JSDIALOG) != 0)
                        {
                            jsDialogMessage = Marshal.PtrToStringAnsi(url);
                            jsDialogType    = (JS_DIALOG_TYPE)Enum.ToObject(typeof(JS_DIALOG_TYPE), additionalInfo);

                            Debug.Log("=> jsdialog message : " + jsDialogMessage + " type : " + jsDialogType.ToString());

                            SettingJsDialog();
                            SetDefaultCursor();
                        }

                        if ((state & WEB_UPDATE_AVAILABLE) != 0)
                        {
                            //Debug.Log("=> update available");
                            webUpdated = true;
                        }

                        if ((state & WEB_UPDATE_FOCUSED) != 0)
                        {
                            //Debug.Log("=> update focused");
                            webFocus = true;
                        }

                        if ((state & WEB_UPDATE_POPUPBLOCK) != 0)
                        {
                            popupBlock = true;
                            popupBlockNotiStartTime = Time.realtimeSinceStartup;
                        }

                        int cursor = (state & WEB_UPDATE_CURSOR_MASK);
                        if (0 < cursor)
                        {
                            //Debug.Log("=> update cursor change : " + cursor);

                            WEB_CURSOR_TYPE cursorType = (WEB_CURSOR_TYPE)Enum.ToObject(typeof(WEB_CURSOR_TYPE), cursor);
                            switch (cursorType)
                            {
                                case WEB_CURSOR_TYPE.CT_HAND:
                                    {
                                        SetCursor(ref cursorHand, cursorHopspot, cursorType);
                                        break;
                                    }
                                case WEB_CURSOR_TYPE.CT_IBEAM:
                                    {
                                        SetCursor(ref cursorIBeam, cursorHopspot, cursorType);
                                        break;
                                    }
                                default:
                                    {
                                        SetCursor(ref cursorArrow, cursorHopspot, cursorType);
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
        }

        private void OnGUI()
        {
            if (0 == fontSize)
            {
                fontSize = GUI.skin.font.fontSize;

                InitJsDialogBtnSize();
                SettingPopupBlockDialog();
            }

            if (isShow == false)
            {
                return;
            }

            // rendering
            if (texture != null)
            {
                if (webUpdated == true)
                {
                    texture.SetPixels32(textureBuffer);
                    texture.Apply(false, false);
                    webUpdated = false;
                }

                GUI.DrawTexture(webRect, texture, ScaleMode.ScaleAndCrop, false, 0);
            }

            // js dialog message
            if (string.IsNullOrEmpty(jsDialogMessage) == false)
            {
                GUI.Box(jsDialogRect, jsDialogMessage, jsDialogStyle);

                if (JS_DIALOG_TYPE.ALERT == jsDialogType)
                {
                    if (GUI.Button(jsDialogRightButtonRect, "OK", jsDialogBtnStyle) == true)
                    {
                        jsDialogMessage = string.Empty;

                        NativeMethods.InputWeb(WEB_INPUT_JSDIALOG, 1, 0);
                    }
                }
                else
                {
                    if (GUI.Button(jsDialogLeftButtonRect, "OK", jsDialogBtnStyle) == true)
                    {
                        jsDialogMessage = string.Empty;

                        NativeMethods.InputWeb(WEB_INPUT_JSDIALOG, 1, 0);
                    }
                    else if (GUI.Button(jsDialogRightButtonRect, "Cancel", jsDialogBtnStyle) == true)
                    {
                        jsDialogMessage = string.Empty;
                        Debug.Log("js cancel btn clicked");
                        NativeMethods.InputWeb(WEB_INPUT_JSDIALOG, 0, 0);
                    }
                }
                
                return;
            }

            // popup block notify
            if (popupBlock == true)
            {
                GUI.Box(popupBlockDialogRect, popupBlockMessage, popupBlockDialogStyle);

                if (Time.realtimeSinceStartup - popupBlockNotiStartTime >= popupBlockNotiTime)
                {
                    popupBlock = false;
                }
            }

            // event
            Event e = Event.current;

            float   mouseX          = Input.mousePosition.x - webRect.x;
            float   mouseY          = Screen.height - Input.mousePosition.y - webRect.y;
            int     modifier        = (e.shift ? WEB_INPUT_SHIFT : 0) | (e.alt ? WEB_INPUT_ALT : 0) | (e.control | e.command ? WEB_INPUT_CTRL : 0);
            bool    controllfocused = string.IsNullOrEmpty(GUI.GetNameOfFocusedControl());

            //Debug.Log("GUI.GetNameOfFocusedControl() " + GUI.GetNameOfFocusedControl());
            //Debug.Log("controllfocused : " + controllfocused);

            if (controllfocused == false && webFocus == true)
            {
                if (forceFocus == true)
                {
                    GUI.FocusControl(null);
                    forceFocus = false;
                }
                else
                {
                    NativeMethods.InputWeb(WEB_INPUT_KEYBOARD | WEB_INPUT_LOST, 0, 0);
                }
            }
            switch (e.type)
            {
                case EventType.ScrollWheel:
                    {
                        //Debug.Log("[CefManager] scroll");
                        NativeMethods.InputWeb(WEB_INPUT_MOUSE | WEB_INPUT_SCROLL, (int)-e.delta.x * 10, (int)-e.delta.y * 10);
                        e.Use();
                    }
                    break;
                case EventType.MouseDown:
                    {
                        //Debug.Log("[CefManager] mouse down compositionString [ " + compositionString + " ] Input.inputString [ " + Input.inputString + " ]");
                        mouseDown = true;
                        NativeMethods.InputWeb(WEB_INPUT_MOUSE | WEB_INPUT_PRESS, (int)mouseX, (int)mouseY);
                        if (webRect.Contains(Input.mousePosition) == true) 
                        {
                            if (controllfocused == false)
                            {
                                //Debug.Log("=> web rect & not focused");
                                GUI.FocusControl(null);
                                NativeMethods.InputWeb(WEB_INPUT_KEYBOARD | WEB_INPUT_FOCUS, 0, 0);
                            }
                        }

                        if (string.IsNullOrEmpty(compositionString) == false  && Input.inputString.Contains(compositionString) == true)
                        {
                           // Debug.Log("=> set compositionString invalid");
                            SetCompositionStringInvalid();
                        }

                        e.Use();
                    }
                    break;
                case EventType.MouseUp:
                    {
                        //Debug.Log("[CefManager] mouse up");
                        mouseDown = false;
                        NativeMethods.InputWeb(WEB_INPUT_MOUSE | WEB_INPUT_RELEASE, (int)mouseX, (int)mouseY);
                        e.Use();
                    }
                    break;
                case EventType.KeyDown:
                    {
                        //Debug.Log("[CefManager] key down e.keycode[" + e.keyCode + "] e.character[" + e.character + "]");
                        //Debug.Log("=> Input.string : " + Input.inputString + " Input.compositionString : " + Input.compositionString + " cursor : " + Input.compositionCursorPos);
                        if (controllfocused == true && e.character == 0)
                        {
                            bool checkCompositionStringInvalid = false;
                            switch (e.keyCode)
                            {
                                case KeyCode.Tab:
                                    {
                                        //Debug.Log("==> tab");

                                        forceFocus = true;
                                        checkCompositionStringInvalid = true;
                                    }
                                    break;
                                case KeyCode.LeftArrow:
                                case KeyCode.RightArrow:
                                case KeyCode.UpArrow:
                                case KeyCode.DownArrow:
                                    {
                                        //Debug.Log("==> arrow");

                                        checkCompositionStringInvalid = true;
                                    }
                                    break;
                            }

                            if (checkCompositionStringInvalid == true)
                            {
                                if (string.IsNullOrEmpty(compositionString) == false && compositionString.Equals(Input.inputString) == true)
                                {
                                    //Debug.Log("===> compositionString == Input.inputString");
                                    SetCompositionStringInvalid();
                                }
                            }

                            if (KeyCode.None != e.keyCode)
                            {
                                NativeMethods.InputWeb(WEB_INPUT_KEYBOARD | WEB_INPUT_PRESS | modifier, NativeKeyCodes[e.keyCode], VirtualKeyCodes[e.keyCode]);
                                keyDown = true;
                            }

                            e.Use();
                        }
                    }
                    break;
                case EventType.KeyUp:
                    {
                        //Debug.Log("[CefManager] key up e.keycode[" + e.keyCode + "] e.character[" + e.character + "]");
                        //Debug.Log("=> Input.string : " + Input.inputString + " Input.compositionString : " + Input.compositionString);
                        if (keyDown == true)
                        {
                            NativeMethods.InputWeb(WEB_INPUT_KEYBOARD | WEB_INPUT_RELEASE | modifier, NativeKeyCodes[e.keyCode], VirtualKeyCodes[e.keyCode]);
                            keyDown = false;
                            e.Use();
                        }
                    }
                    break;
                default:
                    {
                        if (mouse.x != mouseX || mouse.y != mouseY)
                        {
                            //Debug.Log("[CefManager] mouse move");
                            int flag = WEB_INPUT_MOUSE | WEB_INPUT_MOVE;
                            if (mouseDown == true)
                            {
                                flag |= WEB_INPUT_MOUSEPRESSING;
                            }
                            NativeMethods.InputWeb(flag, (int)mouseX, (int)mouseY);
                        }
                    }
                    break;
            }

            mouse.x = mouseX;
            mouse.y = mouseY;
        }


        private void OnApplicationQuit()
        {
            //Debug.Log("CefManager:OnApplicationQuit");
        }

        private void OnDisable()
        {
            //Debug.Log("CefManager:OnDisable");

            UnloadWeb(false);
        }

        private void OnDestroy()
        {
            //Debug.Log("CefManager:OnDestroy");

            if (null != cursorArrow)
            {
                Destroy(cursorArrow);
            }

            if (null != cursorHand)
            {
                Destroy(cursorHand);
            }

            if (null != texture)
            {
                Destroy(texture);
            }

            SetDefaultCursor();

#if UNITY_STANDALONE && !UNITY_EDITOR
        //Debug.Log("=> unload all");
        UnloadWeb(true);
#endif
        }

        private void SetDefaultCursor()
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        internal static Dictionary<KeyCode, int> VirtualKeyCodes = new Dictionary<KeyCode, int>()
    {
        { KeyCode.Backspace, 0x08 },
        { KeyCode.Delete, 0x2E },
        { KeyCode.Tab, 0x09 },
        { KeyCode.Clear, 0x0C },
        { KeyCode.Return, 0x0D },
        { KeyCode.Pause, 0x13 },
        { KeyCode.Escape, 0x1B },
        { KeyCode.Space, 0x20 },
        { KeyCode.Keypad0, 0x60 },
        { KeyCode.Keypad1, 0x61 },
        { KeyCode.Keypad2, 0x62 },
        { KeyCode.Keypad3, 0x63 },
        { KeyCode.Keypad4, 0x64 },
        { KeyCode.Keypad5, 0x65 },
        { KeyCode.Keypad6, 0x66 },
        { KeyCode.Keypad7, 0x67 },
        { KeyCode.Keypad8, 0x68 },
        { KeyCode.Keypad9, 0x69 },
        { KeyCode.KeypadPeriod, 0x6E },
        { KeyCode.KeypadDivide, 0x6F },
        { KeyCode.KeypadMultiply, 0x6A },
        { KeyCode.KeypadMinus, 0x6D },
        { KeyCode.KeypadPlus, 0x6B },
        { KeyCode.KeypadEnter, 0x10F },
        { KeyCode.KeypadEquals, 0x110 },
        { KeyCode.UpArrow, 0x26 },
        { KeyCode.DownArrow, 0x28 },
        { KeyCode.RightArrow, 0x27 },
        { KeyCode.LeftArrow, 0x25 },
        { KeyCode.Insert, 0x2D },
        { KeyCode.Home, 0x24 },
        { KeyCode.End, 0x23 },
        { KeyCode.PageUp, 0x21 },
        { KeyCode.PageDown, 0x22 },
        { KeyCode.F1, 0x70 },
        { KeyCode.F2, 0x71 },
        { KeyCode.F3, 0x72 },
        { KeyCode.F4, 0x73 },
        { KeyCode.F5, 0x74 },
        { KeyCode.F6, 0x75 },
        { KeyCode.F7, 0x76 },
        { KeyCode.F8, 0x77 },
        { KeyCode.F9, 0x78 },
        { KeyCode.F10, 0x79 },
        { KeyCode.F11, 0x7A },
        { KeyCode.F12, 0x7B },
        { KeyCode.F13, 0x7C },
        { KeyCode.F14, 0x7D },
        { KeyCode.F15, 0x7E },
        { KeyCode.Alpha0, 0x30 },
        { KeyCode.Alpha1, 0x31 },
        { KeyCode.Alpha2, 0x32 },
        { KeyCode.Alpha3, 0x33 },
        { KeyCode.Alpha4, 0x34 },
        { KeyCode.Alpha5, 0x35 },
        { KeyCode.Alpha6, 0x36 },
        { KeyCode.Alpha7, 0x37 },
        { KeyCode.Alpha8, 0x38 },
        { KeyCode.Alpha9, 0x39 },
        { KeyCode.Exclaim, 0x21 },
        { KeyCode.DoubleQuote, 0x22 },
        { KeyCode.Hash, 0x23 },
        { KeyCode.Dollar, 0x24 },
        { KeyCode.Ampersand, 0x26 },
        { KeyCode.Quote, 0xDE },
        { KeyCode.LeftParen, 0x28 },
        { KeyCode.RightParen, 0x29 },
        { KeyCode.Asterisk, 0x2A },
        { KeyCode.Plus, 0x2B },
        { KeyCode.Comma, 0xBC },
        { KeyCode.Minus, 0xBD },
        { KeyCode.Period, 0xBE },
        { KeyCode.Slash, 0xBF },
        { KeyCode.Colon, 0x3A },
        { KeyCode.Semicolon, 0xBA },
        { KeyCode.Less, 0x3C },
        { KeyCode.Equals, 0x3D },
        { KeyCode.Greater, 0x3E },
        { KeyCode.Question, 0x3F },
        { KeyCode.At, 0x40 },
        { KeyCode.LeftBracket, 0x5B },
        { KeyCode.Backslash, 0xDC },
        { KeyCode.RightBracket, 0xDD },
        { KeyCode.Caret, 0x5E },
        { KeyCode.Underscore, 0x5F },
        { KeyCode.BackQuote, 0xC0 },
        { KeyCode.A, 0x41 },
        { KeyCode.B, 0x42 },
        { KeyCode.C, 0x43 },
        { KeyCode.D, 0x44 },
        { KeyCode.E, 0x45 },
        { KeyCode.F, 0x46 },
        { KeyCode.G, 0x47 },
        { KeyCode.H, 0x48 },
        { KeyCode.I, 0x49 },
        { KeyCode.J, 0x4A },
        { KeyCode.K, 0x4B },
        { KeyCode.L, 0x4C },
        { KeyCode.M, 0x4D },
        { KeyCode.N, 0x4E },
        { KeyCode.O, 0x4F },
        { KeyCode.P, 0x50 },
        { KeyCode.Q, 0x51 },
        { KeyCode.R, 0x52 },
        { KeyCode.S, 0x53 },
        { KeyCode.T, 0x54 },
        { KeyCode.U, 0x55 },
        { KeyCode.V, 0x56 },
        { KeyCode.W, 0x57 },
        { KeyCode.X, 0x58 },
        { KeyCode.Y, 0x59 },
        { KeyCode.Z, 0x5A },
        { KeyCode.Numlock, 0x90 },
        { KeyCode.CapsLock, 0x14 },
        { KeyCode.ScrollLock, 0x91 },
        { KeyCode.RightShift, 0xA1 },
        { KeyCode.LeftShift, 0xA0 },
        { KeyCode.RightControl, 0xA3 },
        { KeyCode.LeftControl, 0xA2 },
        { KeyCode.RightAlt, 0xA5 },
        { KeyCode.LeftAlt, 0xA4 },
        { KeyCode.LeftCommand, 0x5B },
        { KeyCode.LeftWindows, 0x5B },
        { KeyCode.RightCommand, 0x5C },
        { KeyCode.RightWindows, 0x5C },
        { KeyCode.AltGr, 0x139 },
        { KeyCode.Help, 0x2F },
        { KeyCode.Print, 0x2A },
        { KeyCode.SysReq, 0x13D },
        { KeyCode.Break, 0x03 },
        { KeyCode.Menu, 0x12 },
        { KeyCode.None, 0 }
    };
#if !UNITY_EDITOR_OSX && !UNITY_STANDALONE_OSX
        private static Dictionary<KeyCode, int> NativeKeyCodes = VirtualKeyCodes;
#else
	static Dictionary<KeyCode, int> NativeKeyCodes = new Dictionary<KeyCode, int>()
	{
		{ KeyCode.A, 0x00 },
		{ KeyCode.S, 0x01 },
		{ KeyCode.D, 0x02 },
		{ KeyCode.F, 0x03 },
		{ KeyCode.H, 0x04 },
		{ KeyCode.G, 0x05 },
		{ KeyCode.Z, 0x06 },
		{ KeyCode.X, 0x07 },
		{ KeyCode.C, 0x08 },
		{ KeyCode.V, 0x09 },
		{ KeyCode.B, 0x0B },
		{ KeyCode.Q, 0x0C },
		{ KeyCode.W, 0x0D },
		{ KeyCode.E, 0x0E },
		{ KeyCode.R, 0x0F },
		{ KeyCode.Y, 0x10 },
		{ KeyCode.T, 0x11 },
		{ KeyCode.Alpha1, 0x12 },
		{ KeyCode.Alpha2, 0x13 },
		{ KeyCode.Alpha3, 0x14 },
		{ KeyCode.Alpha4, 0x15 },
		{ KeyCode.Alpha6, 0x16 },
		{ KeyCode.Alpha5, 0x17 },
		{ KeyCode.Equals, 0x18 },
		{ KeyCode.Alpha9, 0x19 },
		{ KeyCode.Alpha7, 0x1A },
		{ KeyCode.Minus, 0x1B },
		{ KeyCode.Alpha8, 0x1C },
		{ KeyCode.Alpha0, 0x1D },
		{ KeyCode.RightBracket, 0x1E },
		{ KeyCode.O, 0x1F },
		{ KeyCode.U, 0x20 },
		{ KeyCode.LeftBracket, 0x21 },
		{ KeyCode.I, 0x22 },
		{ KeyCode.P, 0x23 },
		{ KeyCode.L, 0x25 },
		{ KeyCode.J, 0x26 },
		{ KeyCode.Quote, 0x27 },
		{ KeyCode.K, 0x28 },
		{ KeyCode.Semicolon, 0x29 },
		{ KeyCode.Backslash, 0x2A },
		{ KeyCode.Comma, 0x2B },
		{ KeyCode.Slash, 0x2C },
		{ KeyCode.N, 0x2D },
		{ KeyCode.M, 0x2E },
		{ KeyCode.Period, 0x2F },
		{ KeyCode.BackQuote, 0x32 },
		{ KeyCode.KeypadPeriod, 0x41 },
		{ KeyCode.KeypadMultiply, 0x43 },
		{ KeyCode.KeypadPlus, 0x45 },
		{ KeyCode.KeypadDivide, 0x4B },
		{ KeyCode.KeypadEnter, 0x4C },
		{ KeyCode.KeypadMinus, 0x4E },
		{ KeyCode.KeypadEquals, 0x51 },
		{ KeyCode.Keypad0, 0x52 },
		{ KeyCode.Keypad1, 0x53 },
		{ KeyCode.Keypad2, 0x54 },
		{ KeyCode.Keypad3, 0x55 },
		{ KeyCode.Keypad4, 0x56 },
		{ KeyCode.Keypad5, 0x57 },
		{ KeyCode.Keypad6, 0x58 },
		{ KeyCode.Keypad7, 0x59 },
		{ KeyCode.Keypad8, 0x5B },
		{ KeyCode.Keypad9, 0x5C },
		{ KeyCode.Return, 0x24 },
		{ KeyCode.Tab, 0x30 },
		{ KeyCode.Space, 0x31 },
		{ KeyCode.Backspace, 0x33 },
		{ KeyCode.Escape, 0x35 },
		{ KeyCode.LeftCommand, 0x37 },
		{ KeyCode.LeftShift, 0x38 },
		{ KeyCode.CapsLock, 0x39 },
		{ KeyCode.LeftAlt, 0x3A },
		{ KeyCode.LeftControl, 0x3B },
		{ KeyCode.RightCommand, 0x36 },
		{ KeyCode.RightShift, 0x3C },
		{ KeyCode.RightAlt, 0x3D },
		{ KeyCode.RightControl, 0x3E },
		{ KeyCode.F5, 0x60 },
		{ KeyCode.F6, 0x61 },
		{ KeyCode.F7, 0x62 },
		{ KeyCode.F3, 0x63 },
		{ KeyCode.F8, 0x64 },
		{ KeyCode.F9, 0x65 },
		{ KeyCode.F11, 0x67 },
		{ KeyCode.F13, 0x69 },
		{ KeyCode.F14, 0x6B },
		{ KeyCode.F10, 0x6D },
		{ KeyCode.F12, 0x6F },
		{ KeyCode.F15, 0x71 },
		{ KeyCode.Home, 0x73 },
		{ KeyCode.PageUp, 0x74 },
		{ KeyCode.Delete, 0x75 },
		{ KeyCode.F4, 0x76 },
		{ KeyCode.End, 0x77 },
		{ KeyCode.F2, 0x78 },
		{ KeyCode.PageDown, 0x79 },
		{ KeyCode.F1, 0x7A },
		{ KeyCode.LeftArrow, 0x7B },
		{ KeyCode.RightArrow, 0x7C },
		{ KeyCode.DownArrow, 0x7D },
		{ KeyCode.UpArrow, 0x7E },
		// not used in MacOS
		{ KeyCode.Clear, 0x0C },
		{ KeyCode.Pause, 0x13 },
		{ KeyCode.Insert, 0x2D },
		{ KeyCode.Exclaim, 0x21 },
		{ KeyCode.DoubleQuote, 0x22 },
		{ KeyCode.Hash, 0x23 },
		{ KeyCode.Dollar, 0x24 },
		{ KeyCode.Ampersand, 0x26 },
		{ KeyCode.LeftParen, 0x28 },
		{ KeyCode.RightParen, 0x29 },
		{ KeyCode.Asterisk, 0x2A },
		{ KeyCode.Plus, 0x2B },
		{ KeyCode.Colon, 0x3A },
		{ KeyCode.Less, 0x3C },
		{ KeyCode.Greater, 0x3E },
		{ KeyCode.Question, 0x3F },
		{ KeyCode.At, 0x40 },
		{ KeyCode.Caret, 0x5E },
		{ KeyCode.Underscore, 0x5F },
		{ KeyCode.Numlock, 0x90 },
		{ KeyCode.ScrollLock, 0x91 },
		{ KeyCode.LeftWindows, 0x5B },
		{ KeyCode.RightWindows, 0x5C },
		{ KeyCode.AltGr, 0x139 },
		{ KeyCode.Help, 0x2F },
		{ KeyCode.Print, 0x2A },
		{ KeyCode.SysReq, 0x13D },
		{ KeyCode.Break, 0x03 },
		{ KeyCode.Menu, 0x12 },
		{ KeyCode.None, 0 }
	};
#endif

        private void SetCursor(ref Texture2D cursor, Vector2 hotspot, WEB_CURSOR_TYPE cursorType)
        {
            if (cursor == null)
            {
                cursor = CreateCursor(cursorType);
            }
            Cursor.SetCursor(cursor, hotspot, CursorMode.Auto);
        }

        private Texture2D CreateCursor(WEB_CURSOR_TYPE cursorType)
        {
            int size = 32;
            Texture2D result = new Texture2D(size, size, TextureFormat.ARGB32, false, false);

            Color32[] pixels = null;

            if (WEB_CURSOR_TYPE.CT_HAND == cursorType)
            {
                pixels = HandCursor();
            }
            else if (WEB_CURSOR_TYPE.CT_IBEAM == cursorType)
            {
                pixels = IBeamCursor();
            }
            else
            {
                pixels = ArrowCursor();
            }

            int pixelIndex1 = 0;
            int pixelIndex2 = 0;
            Color32 pixel = new Color32();

            for (int y = 0; y < size / 2; ++y)
            {
                for (int x = 0; x < size; ++x)
                {
                    pixelIndex1 = y * size + x;
                    pixelIndex2 = (size - y - 1) * size + x;

                    pixel = pixels[pixelIndex1];
                    pixels[pixelIndex1] = pixels[pixelIndex2];
                    pixels[pixelIndex2] = pixel;

                    //Debug.Log("[CefManager] CreateCursor i : " + i + " j : " + j);
                }
            }
            result.SetPixels32(pixels);
            result.Apply(false, false);
#if UNITY_EDITOR
            result.alphaIsTransparency = true;
#endif
            return result;
        }

        private static Color32[] ArrowCursor()
        {
            Color32 _ = new Color32(0x00, 0x00, 0x00, 0x00), f = new Color32(0xFF, 0xFF, 0xFF, 0xFF),
                    s = new Color32(0x00, 0x00, 0x00, 0xFF);
            return new Color32[]
                    { _, _, _, _, _, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, f, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, f, f, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, f, f, f, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, f, f, f, f, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, f, f, f, f, f, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, f, f, f, f, f, f, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, f, f, f, f, f, f, f, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, f, f, f, f, f, f, f, f, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, f, f, f, f, f, f, f, f, f, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, f, f, f, f, f, f, f, f, f, f, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, f, f, f, f, f, f, f, f, f, f, f, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, f, f, f, f, f, f, f, s, s, s, s, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, f, f, f, f, f, f, f, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, f, f, f, s, f, f, f, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, f, f, s, _, s, f, f, f, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, f, s, _, _, s, f, f, f, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, s, _, _, _, _, s, f, f, f, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, _, _, _, _, _, _, s, f, f, f, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, _, _, _, _, _, _, _, s, s, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _ };
        }

        private static Color32[] HandCursor()
        {
            Color32 _ = new Color32(0x00, 0x00, 0x00, 0x00), f = new Color32(0xFF, 0xFF, 0xFF, 0xFF),
                    s = new Color32(0x00, 0x00, 0x00, 0xFF);
            return new Color32[]
                    { _, _, _, _, _, s, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, s, f, f, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, s, f, f, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, s, f, f, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, s, f, f, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, s, f, f, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, s, f, f, s, s, s, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, s, f, f, s, f, f, s, s, s, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, s, f, f, s, f, f, s, f, f, s, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      s, s, s, _, s, f, f, s, f, f, s, f, f, s, f, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      s, f, f, s, s, f, f, s, f, f, s, f, f, s, f, f, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      s, f, f, f, s, f, f, f, f, f, f, f, f, s, f, f, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, s, f, f, f, f, f, f, f, f, f, f, f, f, f, f, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, s, f, f, f, f, f, f, f, f, f, f, f, f, f, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, s, f, f, f, f, f, f, f, f, f, f, f, f, f, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, s, f, f, f, f, f, f, f, f, f, f, f, f, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, s, f, f, f, f, f, f, f, f, f, f, f, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, s, f, f, f, f, f, f, f, f, f, f, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, s, f, f, f, f, f, f, f, f, f, f, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, f, f, f, f, f, f, f, f, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, f, f, f, f, f, f, f, f, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, s, s, s, s, s, s, s, s, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _ };
        }

        private static Color32[] IBeamCursor()
        {
            Color32 _ = new Color32(0x00, 0x00, 0x00, 0x00),
                    s = new Color32(0x00, 0x00, 0x00, 0xFF);
            return new Color32[]
                    { _, _, _, s, s, s, s, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, s, s, s, s, s, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _,
                      _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _ };
        }
#endif
    }


    internal static class NativeMethods
    {
        private const string DLL_NAME = "nhncef";

        [DllImport(DLL_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = false)]
        internal static extern int LoadWeb(int x, int y, int width, int height, IntPtr buffer, StringBuilder url, int option, StringBuilder locale);

        [DllImport(DLL_NAME)]
        internal static extern int UpdateWeb(out IntPtr url, out int additionalInfo);

        [DllImport(DLL_NAME)]
        internal static extern void UnloadWeb(bool shutdown);

        [DllImport(DLL_NAME)]
        internal static extern void InputWeb(int flags, int x, int y);

        [DllImport(DLL_NAME)]
        internal static extern void GoBackForwardHome(int direction);

        [DllImport(DLL_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = false)]
        internal static extern void ExecuteJavaScript(StringBuilder javaScript);

        [DllImport(DLL_NAME, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = false)]
        internal static extern void ReserveInvalidRedirectUrlSchemes(StringBuilder schemes);

        [DllImport(DLL_NAME)]
        internal static extern void SetDebugEnable(bool enable);
    }

}