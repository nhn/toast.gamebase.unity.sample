using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace Toast.Cef.Webview.Internal
{
    public abstract class Webview : MonoBehaviour
    {
        private const string ERROR_SCHEME = "cef://error";

        private const string KEY_PASS_POPUP_INFO_NAME = "name";
        private const string KEY_PASS_POPUP_INFO_URL = "url";
        private const string KEY_PASS_POPUP_INFO_LEFT = "left";
        private const string KEY_PASS_POPUP_INFO_TOP = "top";
        private const string KEY_PASS_POPUP_INFO_WIDTH = "width";
        private const string KEY_PASS_POPUP_INFO_HEIGHT = "height";
        private const string KEY_PASS_POPUP_INFO_MENUBAR = "menubar";
        private const string KEY_PASS_POPUP_INFO_STATUS = "status";
        private const string KEY_PASS_POPUP_INFO_TOOLBAR = "toolbar";
        private const string KEY_PASS_POPUP_INFO_LOCATION = "location";
        private const string KEY_PASS_POPUP_INFO_SCROLLBARS = "scrollbars";
        private const string KEY_PASS_POPUP_INFO_RESIZABLE = "resizable";

        private const string SEPARATOR_COMMA = ",";
        private const string DELIMITER_VERTICAL_BAR = "|";

        protected abstract void OpenDialog(int type, string message);
        protected abstract void OpenPopupBlock();
        protected abstract void SetBlockPopupMessage(string message);
        protected abstract void OnGUIMessage();

        protected Texture2D texture;
        protected Color32[] textureBuffer;
        protected GCHandle textureHandle;

        protected Rect viewRect;
        protected bool webUpdated;

        public int index = 0;

        private CefWebviewCallback.DataDelegate<string> urlDelegate;
        private CefWebviewCallback.DataDelegate<string> titleDelegate;
        private CefWebviewCallback.DataDelegate<int> inputElementFocusDelegate;
        protected CefWebviewCallback.DataDelegate<ResponseVo.WebviewStatus> statusDelegate;

        private Texture2D cursorArrow;
        private Texture2D cursorHand;
        private Texture2D cursorIBeam;

        protected bool isShow;
        private bool isFocus;
        private bool isRendering;

        private bool mouseDown;
        private bool keyDown;

        private bool webFocus;
        private bool forceFocus;

        private string compositionString = string.Empty;
        private bool compositionInvalid;

        private Vector2 savedMousePosition = Vector2.zero;
        private Vector2 mousePosition;

        public void CreateWebview(
            int index,
            RequestVo.WebviewConfiguration configuration,
            CefWebviewCallback.CefWebviewDelegate<ResponseVo.WebviewInfo> callback)
        {
            this.index = index;
            viewRect = configuration.viewRect;

            texture = new Texture2D((int)viewRect.width, (int)viewRect.height, TextureFormat.ARGB32, false, true);
            textureBuffer = texture.GetPixels32();
            textureHandle = GCHandle.Alloc(textureBuffer, GCHandleType.Pinned);
            
            if (configuration.popupOption == null)
            {
                configuration.popupOption = new RequestVo.WebviewConfiguration.PopupOption
                {
                    blockMessage = string.Empty,
#if !UNITY_EDITOR && UNITY_STANDALONE
                    type = PopupType.POPUP
#else
                    type = PopupType.REDIRECT
#endif
                };
            }

            var popupOption = configuration.popupOption;

            if (string.IsNullOrEmpty(popupOption.blockMessage) == false)
            {
                SetBlockPopupMessage(popupOption.blockMessage);
            }

            NativeMethods.CreateWeb(
                index,
                0, 0, (int)viewRect.width, (int)viewRect.height,
                textureHandle.AddrOfPinnedObject(),
                WebOption.INVERSE_Y | WebOption.INVERSE_COLOR | popupOption.type | configuration.bgType);

            CefCleaner.Create();

            var webviewInfo = new ResponseVo.WebviewInfo
            {
                index = index,
                texture = GetTexture()
            };

            CefWebviewCallback.InvokeCefWebviewDelegate(callback, webviewInfo);
        }

        protected virtual Texture2D GetTexture()
        {
            return texture;
        }

        public void ShowWebview(
            string url,
            bool isShowScrollBar,
            CefWebviewCallback.ErrorDelegate callback,
            CefWebviewCallback.DataDelegate<string> webUrlDelegate,
            CefWebviewCallback.DataDelegate<string> webTitleDelegate,
            CefWebviewCallback.DataDelegate<int> webInputElementFocusDelegate,
            CefWebviewCallback.DataDelegate<ResponseVo.WebviewStatus> webStatusDelegate)
        {
            urlDelegate = webUrlDelegate;
            titleDelegate = webTitleDelegate;
            inputElementFocusDelegate = webInputElementFocusDelegate;
            statusDelegate = webStatusDelegate;

            mouseDown = false;
            isShow = true;
            gameObject.SetActive(isShow);

            NativeMethods.LoadWeb(index, new StringBuilder(url), isShowScrollBar);

            CefWebviewCallback.InvokeErrorDelegate(callback);
        }

        public void HideWebview(CefWebviewCallback.ErrorDelegate callback)
        {
            isRendering = false;
            isShow = false;
            gameObject.SetActive(isShow);

            DrawDefaultColor();
            SetDefaultCursor();

            CefWebviewCallback.InvokeErrorDelegate(callback);
        }

        public void ResizeWebview(int width, int height, CefWebviewCallback.ErrorDelegate callback)
        {
            var rect = new Rect(viewRect.x, viewRect.y, width, height);
            ResizeWebview(rect, callback);
        }

        public void ResizeWebview(Rect rect, CefWebviewCallback.ErrorDelegate callback)
        {
            if (texture == null)
            {
                return;
            }

            viewRect = rect;

            texture.Resize((int)viewRect.width, (int)viewRect.height);
            textureBuffer = texture.GetPixels32();

            textureHandle.Free();
            textureHandle = GCHandle.Alloc(textureBuffer, GCHandleType.Pinned);
            NativeMethods.ResizeWeb(index, textureHandle.AddrOfPinnedObject(), (int)viewRect.width, (int)viewRect.height);

            CefWebviewCallback.InvokeErrorDelegate(callback);
        }

        public void ShowScrollBar(bool isShow, CefWebviewCallback.ErrorDelegate callback)
        {
            NativeMethods.ShowScrollbar(index, isShow);

            CefWebviewCallback.InvokeErrorDelegate(callback);
        }

        public void SetFocus(bool isFocus, CefWebviewCallback.ErrorDelegate callback)
        {
            this.isFocus = isFocus;

            if (isFocus == false)
            {
                SetDefaultCursor();
            }

            CefWebviewCallback.InvokeErrorDelegate(callback);
        }

        public void SetMousePosition(Vector2 mousePosition, CefWebviewCallback.ErrorDelegate callback)
        {
            this.mousePosition = mousePosition;

            CefWebviewCallback.InvokeErrorDelegate(callback);
        }

        public void GoHome(CefWebviewCallback.ErrorDelegate callback)
        {
            if (isShow == true)
            {
                NativeMethods.GoBackForwardHome(index, WebNavigationDirection.HOME);
                CefWebviewCallback.InvokeErrorDelegate(callback);
            }
        }

        public void GoBack(CefWebviewCallback.ErrorDelegate callback)
        {
            if (isShow == true)
            {
                NativeMethods.GoBackForwardHome(index, WebNavigationDirection.BACK);
                CefWebviewCallback.InvokeErrorDelegate(callback);
            }
        }

        public void GoForward(CefWebviewCallback.ErrorDelegate callback)
        {
            if (isShow == true)
            {
                NativeMethods.GoBackForwardHome(index, WebNavigationDirection.FORWARD);
                CefWebviewCallback.InvokeErrorDelegate(callback);
            }
        }

        public void ExecuteJavaScript(string javaScript, CefWebviewCallback.ErrorDelegate callback)
        {
            if (isShow == true)
            {
                NativeMethods.ExecuteJavaScript(index, new StringBuilder(javaScript));
                CefWebviewCallback.InvokeErrorDelegate(callback);
            }
        }

        #region MonoBehaviour messages
        private void Update()
        {
            if (isShow == false)
            {
                return;
            }

            IntPtr url = IntPtr.Zero;

            int additionalInfo = 0;
            int status = NativeMethods.UpdateWeb(index, out url, out additionalInfo);

            UpdateWebviewStatus(url, status, additionalInfo);
            UpdateCursor(status);

            if (isFocus == true)
            {
                UpdateCompositionString();
            }
        }

        private void OnGUI()
        {
            if (isShow == false)
            {
                return;
            }

            if (texture != null)
            {
                if (isRendering == false)
                {
                    DrawDefaultColor();
                }
                else
                {
                    RenderTexture(textureBuffer);
                }
            }

            if (isFocus == false)
            {
                return;
            }

            Event e = Event.current;

            OnGUIMessage();

            int modifier = (e.shift ? WebInput.SHIFT : 0) | (e.alt ? WebInput.ALT : 0) | (e.control | e.command ? WebInput.CTRL : 0);
            bool controllfocused = string.IsNullOrEmpty(GUI.GetNameOfFocusedControl());
            if (controllfocused == false && webFocus == true)
            {
                if (forceFocus == true)
                {
                    GUI.FocusControl(null);
                    forceFocus = false;
                }
                else
                {
                    NativeMethods.InputWeb(index, WebInput.KEYBOARD | WebInput.LOST, 0, 0);
                }
            }

            switch (e.type)
            {
                case EventType.ScrollWheel:
                    {
                        NativeMethods.InputWeb(index, WebInput.MOUSE | WebInput.SCROLL, (int)-e.delta.x * 10, (int)-e.delta.y * 10);
                        e.Use();
                    }
                    break;
                case EventType.MouseDown:
                    {
                        mouseDown = true;
                        NativeMethods.InputWeb(index, WebInput.MOUSE | WebInput.PRESS, (int)mousePosition.x, (int)mousePosition.y);
                        if (viewRect.Contains(Input.mousePosition) == true)
                        {
                            if (controllfocused == false)
                            {
                                GUI.FocusControl(null);
                                NativeMethods.InputWeb(index, WebInput.KEYBOARD | WebInput.FOCUS, 0, 0);
                            }
                        }

                        if (string.IsNullOrEmpty(compositionString) == false && Input.inputString.Contains(compositionString) == true)
                        {
                            SetCompositionStringInvalid();
                        }

                        e.Use();
                    }
                    break;
                case EventType.MouseUp:
                    {
                        mouseDown = false;
                        NativeMethods.InputWeb(index, WebInput.MOUSE | WebInput.RELEASE, (int)mousePosition.x, (int)mousePosition.y);
                        e.Use();
                    }
                    break;
                case EventType.KeyDown:
                    {
                        if (controllfocused == true && e.character == 0)
                        {
                            bool checkCompositionStringInvalid = false;
                            switch (e.keyCode)
                            {
                                case KeyCode.Tab:
                                    {
                                        forceFocus = true;
                                        checkCompositionStringInvalid = true;
                                    }
                                    break;
                                case KeyCode.LeftArrow:
                                case KeyCode.RightArrow:
                                case KeyCode.UpArrow:
                                case KeyCode.DownArrow:
                                    {
                                        checkCompositionStringInvalid = true;
                                    }
                                    break;
                            }

                            if (checkCompositionStringInvalid == true)
                            {
                                if (string.IsNullOrEmpty(compositionString) == false && compositionString.Equals(Input.inputString) == true)
                                {
                                    SetCompositionStringInvalid();
                                }
                            }

                            if (KeyCode.None != e.keyCode)
                            {
                                NativeMethods.InputWeb(index, WebInput.KEYBOARD | WebInput.PRESS | modifier, KeyCodeHelper.NativeKeyCodes[e.keyCode], KeyCodeHelper.VirtualKeyCodes[e.keyCode]);
                                keyDown = true;
                            }

                            e.Use();
                        }
                    }
                    break;
                case EventType.KeyUp:
                    {
                        if (keyDown == true)
                        {
                            NativeMethods.InputWeb(index, WebInput.KEYBOARD | WebInput.RELEASE | modifier, KeyCodeHelper.NativeKeyCodes[e.keyCode], KeyCodeHelper.VirtualKeyCodes[e.keyCode]);
                            keyDown = false;
                            e.Use();
                        }
                    }
                    break;
                default:
                    {
                        if (savedMousePosition.x != mousePosition.x || savedMousePosition.y != mousePosition.y)
                        {
                            int flag = WebInput.MOUSE | WebInput.MOVE;
                            if (mouseDown == true)
                            {
                                flag |= WebInput.MOUSEPRESSING;
                            }
                            NativeMethods.InputWeb(index, flag, (int)mousePosition.x, (int)mousePosition.y);
                        }
                    }
                    break;
            }

            savedMousePosition.x = mousePosition.x;
            savedMousePosition.y = mousePosition.y;
        }

        private void OnDestroy()
        {
            urlDelegate = null;
            titleDelegate = null;
            inputElementFocusDelegate = null;
            statusDelegate = null;

            if (null != cursorArrow)
            {
                Destroy(cursorArrow);
            }

            if (null != cursorHand)
            {
                Destroy(cursorHand);
            }

            if (null != cursorIBeam)
            {
                Destroy(cursorIBeam);
            }

            if (null != texture)
            {
                Destroy(texture);
            }

            textureBuffer = null;
            textureHandle.Free();

            NativeMethods.RemoveWeb(index);
            WebviewIndexManager.Instance.RemoveWebview(index);

            SetDefaultCursor();
        }
        #endregion

        #region CompositionString
        private void UpdateCompositionString()
        {
            if (string.IsNullOrEmpty(Input.inputString) == false)
            {
                if (compositionInvalid == true)
                {
                    compositionInvalid = false;
                }
                else
                {
                    if (string.IsNullOrEmpty(compositionString) == false)
                    {
                        for (int i = 0; i < compositionString.Length; ++i)
                        {
                            SendBackspaceKeyEvent();
                        }
                    }

                    foreach (char c in Input.inputString)
                    {
                        NativeMethods.InputWeb(index, WebInput.KEYBOARD | WebInput.CHARACTER, c, c);
                    }

                    SetCompositionStringEmpty();
                }
            }
            else if (string.IsNullOrEmpty(Input.compositionString) == false)
            {
                if (compositionString.Equals(Input.compositionString) == false)
                {
                    if (string.IsNullOrEmpty(compositionString) == false)
                    {
                        for (int i = 0; i < compositionString.Length; ++i)
                        {
                            SendBackspaceKeyEvent();
                        }
                    }

                    foreach (char c in Input.compositionString)
                    {
                        NativeMethods.InputWeb(index, WebInput.KEYBOARD | WebInput.PRESS, c, c);
                        NativeMethods.InputWeb(index, WebInput.KEYBOARD | WebInput.CHARACTER, c, c);
                        NativeMethods.InputWeb(index, WebInput.KEYBOARD | WebInput.RELEASE, c, c);
                    }

                    compositionString = Input.compositionString;
                }
            }
            else if (string.IsNullOrEmpty(compositionString) == false && string.IsNullOrEmpty(Input.compositionString) == true)
            {
                SendBackspaceKeyEvent();
                SetCompositionStringEmpty();
            }
        }

        private void SendBackspaceKeyEvent()
        {
            NativeMethods.InputWeb(index, WebInput.KEYBOARD | WebInput.PRESS, KeyCodeHelper.NativeKeyCodes[KeyCode.Backspace], KeyCodeHelper.VirtualKeyCodes[KeyCode.Backspace]);
            NativeMethods.InputWeb(index, WebInput.KEYBOARD | WebInput.CHARACTER, KeyCodeHelper.NativeKeyCodes[KeyCode.Backspace], KeyCodeHelper.VirtualKeyCodes[KeyCode.Backspace]);
            NativeMethods.InputWeb(index, WebInput.KEYBOARD | WebInput.RELEASE, KeyCodeHelper.NativeKeyCodes[KeyCode.Backspace], KeyCodeHelper.VirtualKeyCodes[KeyCode.Backspace]);
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
        #endregion

        private void UpdateWebviewStatus(IntPtr url, int status, int additionalInfo)
        {
            if (status == WebUpdateStatus.INVALID || status == WebUpdateStatus.NONE)
            {
                return;
            }

            if ((status & WebUpdateStatus.ERROR) != 0)
            {
                string errorText = string.Format("{0}?{1}", ERROR_SCHEME, Marshal.PtrToStringAnsi(url));

                if (urlDelegate != null)
                {
                    urlDelegate(errorText);
                }

                return;
            }

            if ((status & WebUpdateStatus.BROWSABLE) != 0)
            {
                string webUrl = Marshal.PtrToStringAnsi(url);

                if (urlDelegate != null && string.IsNullOrEmpty(webUrl) == false)
                {
                    var urlArray = webUrl.Split(new[] { SEPARATOR_COMMA }, StringSplitOptions.RemoveEmptyEntries);
                    for (var i = 0; i < urlArray.Length; i++)
                    {
                        urlDelegate(urlArray[i]);
                    }
                }
            }

            if ((status & WebUpdateStatus.INPUTFOCUS) != 0)
            {
                if (inputElementFocusDelegate != null)
                {
                    inputElementFocusDelegate(additionalInfo);
                }
            }

            if ((status & WebUpdateStatus.TITLE) != 0)
            {
                string title = Marshal.PtrToStringAnsi(url);

                if (null != titleDelegate)
                {
                    titleDelegate(title);
                }
            }

            if ((status & WebUpdateStatus.JSDIALOG) != 0)
            {
                var dialogMessage = Marshal.PtrToStringAnsi(url);
                var dialogType = additionalInfo;

                OpenDialog(dialogType, dialogMessage);
                SetDefaultCursor();
            }

            if ((status & WebUpdateStatus.AVAILABLE) != 0)
            {
                webUpdated = true;
            }

            if ((status & WebUpdateStatus.FOCUSED) != 0)
            {
                webFocus = true;
            }

            if ((status & WebUpdateStatus.POPUPBLOCK) != 0)
            {
                OpenPopupBlock();
            }

            if ((status & WebUpdateStatus.LOAD_END) != 0)
            {
                if (statusDelegate == null)
                {
                    return;
                }

                isRendering = true;
                string webUrl = Marshal.PtrToStringAnsi(url);

                var vo = new ResponseVo.WebviewStatus
                {
                    status = WebUpdateStatus.LOAD_END,
                    loadEnd = new ResponseVo.WebviewStatus.LoadEnd
                    {
                        url = webUrl
                    }
                };

                statusDelegate(vo);
            }

            if ((status & WebUpdateStatus.PASS_POPUP_INFO) != 0)
            {
                if (statusDelegate == null)
                {
                    return;
                }

                var popupInfo = Marshal.PtrToStringAnsi(url);

                if (string.IsNullOrEmpty(popupInfo) == true)
                {
                    return;
                }

                var dictionary = new Dictionary<string, string>();

                try
                {
                    // Convert string to dictionary<string, string>
                    // AS-IS
                    //      key1|value1,key2|value2,key3|value3;
                    // TO-BE
                    //      dictionary[key1]=value1
                    //      dictionary[key2]=value2
                    //      dictionary[key3]=value3
                    dictionary = popupInfo.Split(new[] { SEPARATOR_COMMA }, StringSplitOptions.None)
                        .Select(part => part.Split(new[] { DELIMITER_VERTICAL_BAR }, StringSplitOptions.None))
                        .ToDictionary(split => split[0], split => split[1]);
                }
                catch (Exception e)
                {
                    //IndexOutOfRangeException

                    CefWebviewLogger.Warn(string.Format("exception message:{0}", e.Message), GetType());
                    return;
                }

                var vo = new ResponseVo.WebviewStatus
                {
                    status = WebUpdateStatus.PASS_POPUP_INFO,
                    passPopupInfo = new ResponseVo.WebviewStatus.PassPopupInfo()
                };

                if (dictionary.TryGetValue(KEY_PASS_POPUP_INFO_URL, out vo.passPopupInfo.url) == true)
                {
                    vo.passPopupInfo.name = dictionary[KEY_PASS_POPUP_INFO_NAME];
                    vo.passPopupInfo.left = int.Parse(dictionary[KEY_PASS_POPUP_INFO_LEFT]);
                    vo.passPopupInfo.top = int.Parse(dictionary[KEY_PASS_POPUP_INFO_TOP]);
                    vo.passPopupInfo.width = int.Parse(dictionary[KEY_PASS_POPUP_INFO_WIDTH]);
                    vo.passPopupInfo.height = int.Parse(dictionary[KEY_PASS_POPUP_INFO_HEIGHT]);
                    vo.passPopupInfo.menubar = (dictionary[KEY_PASS_POPUP_INFO_MENUBAR] == "0") ? false : true;
                    vo.passPopupInfo.status = (dictionary[KEY_PASS_POPUP_INFO_STATUS] == "0") ? false : true;
                    vo.passPopupInfo.toolbar = (dictionary[KEY_PASS_POPUP_INFO_TOOLBAR] == "0") ? false : true;
                    vo.passPopupInfo.location = (dictionary[KEY_PASS_POPUP_INFO_LOCATION] == "0") ? false : true;
                    vo.passPopupInfo.scrollbars = (dictionary[KEY_PASS_POPUP_INFO_SCROLLBARS] == "0") ? false : true;
                    vo.passPopupInfo.resizable = (dictionary[KEY_PASS_POPUP_INFO_RESIZABLE] == "0") ? false : true;
                }

                statusDelegate(vo);
            }
        }

        #region Cursor
        private void UpdateCursor(int status)
        {
            int cursor = (status & WebUpdateStatus.CURSOR_MASK);
            if (cursor > 0)
            {
                switch (cursor)
                {
                    case WebCursorType.HAND:
                        {
                            SetCursor(ref cursorHand, cursor);
                            break;
                        }
                    case WebCursorType.IBEAM:
                        {
                            SetCursor(ref cursorIBeam, cursor);
                            break;
                        }
                    default:
                        {
                            SetCursor(ref cursorArrow, cursor);
                            break;
                        }
                }
            }
        }

        private void SetCursor(ref Texture2D cursor, int cursorType)
        {
            if (cursor == null)
            {
                cursor = CefCursor.GetCursorTexture(cursorType);
            }

            Cursor.SetCursor(cursor, CefCursor.hotspot, CursorMode.Auto);
        }

        private void SetDefaultCursor()
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
        #endregion

        protected virtual void RenderTexture(Color32[] buffer)
        {
            if (webUpdated == true)
            {
                texture.SetPixels32(buffer);
                texture.Apply(false, false);
                webUpdated = false;
            }
        }

        private void DrawDefaultColor()
        {
            if (texture != null)
            {
                Color32 defaultColor = new Color32(255, 255, 255, 255);
                Color32[] textureColor = texture.GetPixels32();

                for (int i = 0; i < textureColor.Length; i++)
                {
                    textureColor[i] = defaultColor;
                }

                webUpdated = true;
                RenderTexture(textureColor);
            }
        }
    }
}