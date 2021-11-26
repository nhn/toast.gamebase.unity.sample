using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Toast.Cef.Webview.Internal
{
    public class CefWebviewImplementation
    {
        private static readonly CefWebviewImplementation instance = new CefWebviewImplementation();

        public static CefWebviewImplementation Instance
        {
            get { return instance; }
        }

        public bool IsInitialized()
        {
            return CefUnitySdk.IsInitialized;
        }

        public void Initialize(string locale, CefWebviewCallback.ErrorDelegate callback)
        {
            if (CefUnitySdk.IsInitialized == true)
            {
                CefWebviewCallback.InvokeErrorDelegate(callback);
                return;
            }

            CefUnitySdk.IsInitialized = NativeMethods.Initialize(new StringBuilder(locale));

            if (CefUnitySdk.IsInitialized == true)
            {
                CefWebviewCallback.InvokeErrorDelegate(callback);
            }
            else
            {
                CefWebviewCallback.InvokeErrorDelegate(callback, new CefWebviewError(CefWebviewErrorCode.INTERNAL_ERROR));
            }
        }

        public void SetDebugEnable(bool enable)
        {
            CefUnitySdk.DebugLogEnabled = enable;
            NativeMethods.SetDebugEnable(enable);
        }

        public void CreateWebview(
            RequestVo.WebviewConfiguration configuration,
            CefWebviewCallback.CefWebviewDelegate<ResponseVo.WebviewInfo> callback)
        {
            if (CefUnitySdk.IsInitialized == false)
            {
                CefWebviewCallback.InvokeCefWebviewDelegate(callback, null, new CefWebviewError(CefWebviewErrorCode.NOT_INITIALIZED));
                return;
            }

            Webview webview;

            if (configuration.useTexture == true)
            {
                webview = new GameObject().AddComponent<ProvideTextureWebview>();
            }
            else
            {
                webview = new GameObject().AddComponent<NotProvideTextureWebview>();
                Object.DontDestroyOnLoad(webview.gameObject);
            }

            var webviewIndex = WebviewIndexManager.Instance.AddWebview(webview);
            webview.name = string.Format("CefWebview{0}", webviewIndex);
            webview.CreateWebview(webviewIndex, configuration, callback);
        }

        public void RemoveWebview(int webviewIndex, CefWebviewCallback.ErrorDelegate callback)
        {
            if (CanInvoke(callback, webviewIndex) == false)
            {
                return;
            }

            var webview = WebviewIndexManager.Instance.GetWebview(webviewIndex);
            Object.Destroy(webview.gameObject);

            CefWebviewCallback.InvokeErrorDelegate(callback);
        }

        public void ShowWebview(
            int webviewIndex,
            string url,
            bool isShowScrollBar,
            CefWebviewCallback.ErrorDelegate callback,
            CefWebviewCallback.DataDelegate<string> webUrlDelegate,
            CefWebviewCallback.DataDelegate<string> webTitleDelegate,
            CefWebviewCallback.DataDelegate<int> webInputElementFocusDelegate,
            CefWebviewCallback.DataDelegate<ResponseVo.WebviewStatus> webStatusDelegate)
        {
            if (CanInvoke(callback, webviewIndex) == false)
            {
                return;
            }

            var webview = WebviewIndexManager.Instance.GetWebview(webviewIndex);
            webview.ShowWebview(
                url,
                isShowScrollBar,
                callback,
                webUrlDelegate,
                webTitleDelegate,
                webInputElementFocusDelegate,
                webStatusDelegate);
        }

        public void HideWebview(int webviewIndex, CefWebviewCallback.ErrorDelegate callback)
        {
            if (CanInvoke(callback, webviewIndex) == false)
            {
                return;
            }

            var webview = WebviewIndexManager.Instance.GetWebview(webviewIndex);
            webview.HideWebview(callback);
        }

        public void ResizeWebview(int webviewIndex, int width, int height, CefWebviewCallback.ErrorDelegate callback)
        {
            if (CanInvoke(callback, webviewIndex) == false)
            {
                return;
            }

            var webview = WebviewIndexManager.Instance.GetWebview(webviewIndex);
            webview.ResizeWebview(width, height, callback);
        }

        public void ResizeWebview(int webviewIndex, Rect rect, CefWebviewCallback.ErrorDelegate callback)
        {
            if (CanInvoke(callback, webviewIndex) == false)
            {
                return;
            }

            var webview = WebviewIndexManager.Instance.GetWebview(webviewIndex);
            webview.ResizeWebview(rect, callback);
        }

        public void ShowScrollBar(int webviewIndex, bool isShow, CefWebviewCallback.ErrorDelegate callback)
        {
            if (CanInvoke(callback, webviewIndex) == false)
            {
                return;
            }

            var webview = WebviewIndexManager.Instance.GetWebview(webviewIndex);
            webview.ShowScrollBar(isShow, callback);
        }

        public void SetFocus(int webviewIndex, bool isFocus, CefWebviewCallback.ErrorDelegate callback)
        {
            if (CanInvoke(callback, webviewIndex) == false)
            {
                return;
            }

            var webview = WebviewIndexManager.Instance.GetWebview(webviewIndex);
            webview.SetFocus(isFocus, callback);
        }

        public void SetMousePosition(int webviewIndex, Vector2 mousePosition, CefWebviewCallback.ErrorDelegate callback)
        {
            if (CanInvoke(callback, webviewIndex) == false)
            {
                return;
            }

            var webview = WebviewIndexManager.Instance.GetWebview(webviewIndex);
            webview.SetMousePosition(mousePosition, callback);
        }

        public void GoHome(int webviewIndex, CefWebviewCallback.ErrorDelegate callback)
        {
            if (CanInvoke(callback, webviewIndex) == false)
            {
                return;
            }

            var webview = WebviewIndexManager.Instance.GetWebview(webviewIndex);
            webview.GoHome(callback);
        }

        public void GoBack(int webviewIndex, CefWebviewCallback.ErrorDelegate callback)
        {
            if (CanInvoke(callback, webviewIndex) == false)
            {
                return;
            }

            var webview = WebviewIndexManager.Instance.GetWebview(webviewIndex);
            webview.GoBack(callback);
        }

        public void GoForward(int webviewIndex, CefWebviewCallback.ErrorDelegate callback)
        {
            if (CanInvoke(callback, webviewIndex) == false)
            {
                return;
            }

            var webview = WebviewIndexManager.Instance.GetWebview(webviewIndex);
            webview.GoForward(callback);
        }

        public void SetInvalidRedirectUrlScheme(List<string> schemeList, CefWebviewCallback.ErrorDelegate callback)
        {
            if (IsInitialize(callback) == false)
            {
                return;
            }

            NativeMethods.ReserveInvalidRedirectUrlSchemes(new StringBuilder(string.Join(";", schemeList.ToArray())));
            CefWebviewCallback.InvokeErrorDelegate(callback);
        }

        public void ExecuteJavaScript(int webviewIndex, string javaScript, CefWebviewCallback.ErrorDelegate callback)
        {
            if (CanInvoke(callback, webviewIndex) == false)
            {
                return;
            }

            var webview = WebviewIndexManager.Instance.GetWebview(webviewIndex);
            webview.ExecuteJavaScript(javaScript, callback);
        }

        private bool CanInvoke(CefWebviewCallback.ErrorDelegate callback, int webviewIndex)
        {
            if (IsInitialize(callback) == false)
            {
                return false;
            }

            if (HasWebview(webviewIndex, callback) == false)
            {
                return false;
            }

            return true;
        }

        private bool IsInitialize(CefWebviewCallback.ErrorDelegate callback)
        {
            if (CefUnitySdk.IsInitialized == false)
            {
                CefWebviewCallback.InvokeErrorDelegate(callback, new CefWebviewError(CefWebviewErrorCode.NOT_INITIALIZED));
            }

            return CefUnitySdk.IsInitialized;
        }

        private bool HasWebview(int webviewIndex, CefWebviewCallback.ErrorDelegate callback)
        {
            if (WebviewIndexManager.Instance.GetWebview(webviewIndex) == null)
            {
                CefWebviewCallback.InvokeErrorDelegate(callback, new CefWebviewError(CefWebviewErrorCode.WEBVIEW_NOT_FOUND));
                return false;
            }

            return true;
        }
    }
}