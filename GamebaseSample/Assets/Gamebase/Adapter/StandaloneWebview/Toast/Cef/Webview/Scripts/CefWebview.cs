using System.Collections.Generic;
using Toast.Cef.Webview.Internal;
using UnityEngine;

namespace Toast.Cef.Webview
{
    public static class CefWebview
    {
        /// <summary>
        /// CefWebviewError를 사용하여 성공 여부를 판단합니다.
        /// </summary>
        /// <param name="error">CefWebviewError 인스턴스입니다.</param>
        /// <returns>성공 여부입니다.</returns>
        /// <example>
        /// Example Usage : 
        /// <code>
        /// public void SampleIsSuccess(CefWebviewError error)
        /// {
        ///     Debug.Log(string.Format("isSuccess:{0}", CefWebview.IsSuccess(error)));
        /// }
        /// </code>
        /// </example>
        public static bool IsSuccess(CefWebviewError error)
        {
            if (error == null || error.code == CefWebviewErrorCode.SUCCESS)
            {
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// CefWebview의 초기화 여부를 확인합니다.
        /// </summary>
        /// <returns>초기화 여부입니다.</returns>
        /// <example>
        /// Example Usage : 
        /// <code>
        /// public void SampleIsInitialized()
        /// {
        ///     Debug.Log(string.Format("isInitialized:{0}", CefWebview.IsInitialized()));
        /// }
        /// </code>
        /// </example>
        public static bool IsInitialized()
        {
            return CefWebviewImplementation.Instance.IsInitialized();
        }

        /// <summary>
        /// CefWebview를 초기화합니다.
        /// CefWebview API 호출 전, 반드시 호출되어야 합니다.
        /// </summary>
        /// <param name="locale">입력된 locale 정보를 CEF로 전달합니다.</param>
        /// <param name="callback">CefWebviewError 인스턴스를 콜백으로 전달합니다.</param>
        /// <example>
        /// Example Usage : 
        /// <code>
        /// public void SampleInitialize()
        /// {
        ///     CefWebview.Initialize("ko", (error) =>
        ///     {
        ///         if (CefWebview.IsSuccess(error) == true)
        ///         {
        ///             Debug.Log("Initialize succeeded.");
        ///         }
        ///         else
        ///         {
        ///             Debug.Log(string.Format("Initialize failed. error:{0}", error));
        ///         }
        ///     });
        /// }
        /// </code>
        /// </example>
        public static void Initialize(string locale, CefWebviewCallback.ErrorDelegate callback)
        {
            CefWebviewImplementation.Instance.Initialize(locale, callback);
        }

        /// <summary>
        /// 디버그 모드를 활성화합니다.
        /// </summary>
        /// <param name="enable">디버그 활성화 여부입니다.</param>
        /// <example>
        /// Example Usage : 
        /// <code>
        /// public void SampleSetDebugEnable()
        /// {
        ///     CefWebview.SetDebugEnable(true);
        /// }
        /// </code>
        /// </example>
        public static void SetDebugEnable(bool enable)
        {
            CefWebviewImplementation.Instance.SetDebugEnable(enable);
        }

        /// <summary>
        /// Webview의 인스턴스를 생성합니다.
        /// </summary>
        /// <param name="configuration">
        ///     useTexture
        ///         True : 텍스처를 제공하는 Webview를 사용합니다.
        ///         False : 텍스처를 제공하지 않는 Webview를 사용합니다.
        ///     viewRect : Webview의 좌표 및 크기를 지정합니다.
        ///     bgType : CefWebview 배경을 투명, 불투명으로 설정합니다.
        ///     popupOption
        ///         type : 팝업 타입을 지정합니다. (PopupType.POPUP, PopupType.REDIRECT, PopupType.BLOCK, PopupType.PASS_INFO)
        ///         blockMessage : 팝업 타입을 PopupType.BLOCK으로 설정하여, 팝업이 차단되면 노출되는 메시지를 설정합니다.
        /// </param>
        /// <param name="callback">
        ///     data : ResponseVo.WebviewInfo 인스턴스를 콜백으로 전달합니다.
        ///         index : 생성된 Webview의 인덱스입니다.
        ///         texture : CEF Webview의 버퍼가 설정된 텍스처입니다. useTexture를 false로 설정할 경우에는 항상 null을 전달합니다.
        ///     error : CefWebviewError 인스턴스가 콜백으로 전달됩니다.
        /// </param>
        /// <example>
        /// Example Usage : 
        /// <code>
        /// public void SampleCreateWebview()
        /// {
        /// 	var rect = new Rect(0, 0, Screen.width, Screen.height);
        /// 	var configuration = new RequestVo.WebviewConfiguration
        /// 	{
        /// 	    useTexture = true,
        /// 	    viewRect = rect,
        /// 	    popupOption = new RequestVo.WebviewConfiguration.PopupOption
        /// 	    {
        /// 	        blockMessage = TXT_POPUP_BLOCK_MESSAGE,
        /// #if !UNITY_EDITOR && UNITY_STANDALONE
        /// 	        type = PopupType.POPUP
        /// #else
        /// 	        type = PopupType.REDIRECT
        /// #endif
        ///         },
        ///         bgType = BgType.OPAQUE
        ///     };
        /// 
        /// 	CefWebview.CreateWebview(
        /// 	configuration,
        /// 	(data, error) =>
        /// 	{
        /// 		if (CefWebview.IsSuccess(error) == true)
        /// 		{
        /// 			Debug.Log(string.Format("CreateWebview succeeded. index:{0}, texture:{1}", data.index, data.texture));
        /// 		}
        /// 		else
        /// 		{
        /// 			Debug.Log(string.Format("CreateWebview failed. error:{0}", error));
        /// 		}
        /// 	});
        /// }
        /// </code>
        /// </example>
        public static void CreateWebview(
            RequestVo.WebviewConfiguration configuration,
            CefWebviewCallback.CefWebviewDelegate<ResponseVo.WebviewInfo> callback)
        {
            CefWebviewImplementation.Instance.CreateWebview(configuration, callback);
        }

        /// <summary>
        /// 생성된 Webview의 인스턴스를 제거합니다.
        /// </summary>
        /// <param name="webviewIndex">Webview의 인덱스입니다.</param>
        /// <param name="callback">CefWebviewError 인스턴스를 콜백으로 전달합니다.</param>
        /// <example>
        /// Example Usage : 
        /// <code>
        /// public void SampleRemoveWebview(int webviewIndex)
        /// {
        ///     CefWebview.RemoveWebview(webviewIndex, (error) =>
        ///     {
        ///         if (CefWebview.IsSuccess(error) == true)
        ///         {
        ///             Debug.Log("RemoveWebview succeeded.");
        ///         }
        ///         else
        ///         {
        ///             Debug.Log(string.Format("RemoveWebview failed. error:{0}", error));
        ///         }
        ///     });
        /// }
        /// </code>
        /// </example>
        public static void RemoveWebview(int webviewIndex, CefWebviewCallback.ErrorDelegate callback)
        {
            CefWebviewImplementation.Instance.RemoveWebview(webviewIndex, callback);
        }

        /// <summary>
        /// CefWebview를 오픈합니다.
        /// </summary>
        /// <param name="webviewIndex">Webview의 인덱스입니다.</param>
        /// <param name="url">로드할 페이지의 URL입니다.</param>
        /// <param name="isShowScrollBar">ScrollBar 표시 여부를 설정합니다.</param>
        /// <param name="callback">CefWebviewError 인스턴스를 콜백으로 전달합니다.</param>
        /// <param name="webUrlDelegate">로드 및 Redirection된 페이지의 URL 정보를 반환합니다.</param>
        /// <param name="webTitleDelegate">로드 및 Redirection된 페이지의 Title 정보를 반환합니다</param>
        /// <param name="webInputElementFocusDelegate">
        /// 포커싱 된 Element의 타입을 패스워드 타입과 나머지 타입으로 구분한 값이 반환됩니다. Password:1, ELSE:100
        /// 조합 문자(한글, 일본어 등) 사용 중 패스워드 타입의 Element로 포커싱이 변경될 경우, 조합 문자가 그대로 입력되는 이슈를 처리할 수 있습니다. (Example 참조)
        /// </param>
        /// <param name="webStatusDelegate">ResponseVo.WebviewStatus의 인스턴스를 콜백으로 전달합니다.</param>
        /// <example>
        /// Example Usage : 
        /// <code>
        /// public void SampleShowWebview(int webviewIndex)
        /// {
        ///     var webviewUrl = "http://naver.com";
        ///     
        ///     CefWebview.ShowWebview(
        ///         webviewIndex,
        ///         webviewUrl,
        ///         true,
        ///         (error) =>
        ///         {
        ///             if (CefWebview.IsSuccess(error) == true)
        ///             {
        ///                 Debug.Log("ShowWebview succeeded.");
        ///             }
        ///             else
        ///             {
        ///                 Debug.Log(string.Format("ShowWebview failed. error:{0}", error));
        ///             }
        ///         },
        ///         (url) =>
        ///         {
        ///             if (url.Contains("cef://error") == true)
        ///             {
        ///                 Debug.Log("Webview를 오픈하지 못했습니다.");
        ///             }
        ///         },
        ///         (title) =>
        ///         {
        ///             Debug.Log(string.Format("title:{0}", title));
        ///         },
        ///         (type) =>
        ///         {
        ///             if (type == InputElementType.PASSWORD)
        ///             {
        ///                 Input.imeCompositionMode = IMECompositionMode.Off;
        ///             }
        ///             else
        ///             {
        ///                 Input.imeCompositionMode = IMECompositionMode.On;
        ///             }
        ///         },
        ///         (webviewStatus) =>
        ///         {
        ///             switch (webviewStatus.status)
        ///             {
        ///                 case WebUpdateStatus.JSDIALOG:
        ///                 {
        ///                     // Dialog GUI
        ///                     Debug.Log(string.Format(
        ///                         "message:{0}, jsDialogType:{1}",
        ///                         webviewStatus.jsDialog.message,
        ///                         // JsDialogType.ALERT, JsDialogType.CONFIRM, JsDialogType.PROMPT
        ///                         webviewStatus.jsDialog.type));
        ///                         
        ///                     // Dialog의 버튼 클릭 시, CEF Webview에서 이벤트를 처리하기 위해 아래 delegate를 호출해야 합니다.
        ///                     // OK 버튼 클릭 : webviewStatus.jsDialog.clickButtonDelegate(true);
        ///                     // Cancel 버튼 클릭 : webviewStatus.jsDialog.clickButtonDelegate(false);
        ///                     break;
        ///                 }
        ///                 case WebUpdateStatus.POPUPBLOCK:
        ///                 {
        ///                     // PopupBlock GUI
        ///                     Debug.Log(string.Format("message:{0}", webviewStatus.popupBlock.message));
        ///                     break;
        ///                 }
        ///                 case WebUpdateStatus.LOAD_END:
        ///                 {
        ///                     // 페이지 로딩 완료
        ///                     Debug.Log(string.Format("url:{0}", webviewStatus.loadEnd.url));
        ///                     break;
        ///                 }
        ///                 case WebUpdateStatus.PASS_POPUP_INFO:
        ///                 {
        ///                     // 팝업이 노출되지 않고, 데이터가 전달됩니다.
        ///                     Debug.Log(
        ///                         string.Format(
        ///                             "pass popup info.\n" +
        ///                             "name:{0}\n" +
        ///                             "url:{1}\n" +
        ///                             "left:{2}\n" +
        ///                             "top:{3}\n" +
        ///                             "width:{4}\n" +
        ///                             "height:{5}\n" +
        ///                             "menubar:{6}\n" +
        ///                             "status:{7}\n" +
        ///                             "toolbar:{8}\n" +
        ///                             "location:{9}\n" +
        ///                             "scrollbars{10}\n" +
        ///                             "resizable:{11}",
        ///                             vo.passPopupInfo.name,
        ///                             vo.passPopupInfo.url,
        ///                             vo.passPopupInfo.left,
        ///                             vo.passPopupInfo.top,
        ///                             vo.passPopupInfo.width,
        ///                             vo.passPopupInfo.height,
        ///                             vo.passPopupInfo.menubar,
        ///                             vo.passPopupInfo.status,
        ///                             vo.passPopupInfo.toolbar,
        ///                             vo.passPopupInfo.location,
        ///                             vo.passPopupInfo.scrollbars,
        ///                             vo.passPopupInfo.resizable));
        ///                     break;
        ///                 }
        ///             }
        ///         });
        /// }
        /// </code>
        /// </example>
        public static void ShowWebview(
            int webviewIndex,
            string url,
            bool isShowScrollBar,
            CefWebviewCallback.ErrorDelegate callback,
            CefWebviewCallback.DataDelegate<string> webUrlDelegate = null,
            CefWebviewCallback.DataDelegate<string> webTitleDelegate = null,
            CefWebviewCallback.DataDelegate<int> webInputElementFocusDelegate = null,
            CefWebviewCallback.DataDelegate<ResponseVo.WebviewStatus> webStatusDelegate = null)
        {
            CefWebviewImplementation.Instance.ShowWebview(
                webviewIndex,
                url,
                isShowScrollBar,
                callback,
                webUrlDelegate,
                webTitleDelegate,
                webInputElementFocusDelegate,
                webStatusDelegate);
        }

        /// <summary>
        /// Webview를 숨깁니다.
        /// </summary>
        /// <param name="webviewIndex">Webview의 인덱스입니다.</param>
        /// <param name="callback">CefWebviewError 인스턴스를 콜백으로 전달합니다.</param>
        /// <example>
        /// Example Usage : 
        /// <code>
        /// public void SampleHideWebview(int webviewIndex)
        /// {
        ///     CefWebview.HideWebview(webviewIndex, (error) =>
        ///     {
        ///         if (CefWebview.IsSuccess(error) == true)
        ///         {
        ///             Debug.Log("HideWebview succeeded.");
        ///         }
        ///         else
        ///         {
        ///             Debug.Log(string.Format("HideWebview failed. error:{0}", error));
        ///         }
        ///     });
        /// }
        /// </code>
        /// </example>
        public static void HideWebview(int webviewIndex, CefWebviewCallback.ErrorDelegate callback)
        {
            CefWebviewImplementation.Instance.HideWebview(webviewIndex, callback);
        }

        /// <summary>
        /// 생성된 Webview의 사이즈를 변경합니다.
        /// </summary>
        /// <param name="webviewIndex">Webview의 인덱스입니다.</param>
        /// <param name="width">변경할 Webview의 너비입니다.</param>
        /// <param name="height">변경할 Webview의 높이입니다.</param>
        /// <param name="callback">CefWebviewError 인스턴스를 콜백으로 전달합니다.</param>
        /// <example>
        /// Example Usage : 
        /// <code>
        /// public void SampleResizeWebview(int webviewIndex)
        /// {
        ///     CefWebview.ResizeWebview(webviewIndex, 800, 600, (error) =>
        ///     {
        ///         if (CefWebview.IsSuccess(error) == true)
        ///         {
        ///             Debug.Log("ResizeWebview succeeded.");
        ///         }
        ///         else
        ///         {
        ///             Debug.Log(string.Format("ResizeWebview failed. error:{0}", error));
        ///         }
        ///     });
        /// }
        /// </code>
        /// </example>
        public static void ResizeWebview(int webviewIndex, int width, int height, CefWebviewCallback.ErrorDelegate callback)
        {
            CefWebviewImplementation.Instance.ResizeWebview(webviewIndex, width, height, callback);
        }

        /// <summary>
        /// 생성된 Webview의 위치와 사이즈를 변경합니다.
        /// </summary>
        /// <param name="webviewIndex">Webview의 인덱스입니다.</param>
        /// <param name="rect">변경할 Webview의 위치와 사이즈입니다.</param>
        /// <param name="callback">CefWebviewError 인스턴스를 콜백으로 전달합니다.</param>
        /// <example>
        /// Example Usage : 
        /// <code>
        /// public void SampleResizeWebview(int webviewIndex)
        /// {
        ///     var rect = new Rect(0, 0, 300, 300);
        /// 
        ///     CefWebview.ResizeWebview(webviewIndex, rect, (error) =>
        ///     {
        ///         if (CefWebview.IsSuccess(error) == true)
        ///         {
        ///             Debug.Log("ResizeWebview succeeded.");
        ///         }
        ///         else
        ///         {
        ///             Debug.Log(string.Format("ResizeWebview failed. error:{0}", error));
        ///         }
        ///     });
        /// }
        /// </code>
        /// </example>
        public static void ResizeWebview(int webviewIndex, Rect rect, CefWebviewCallback.ErrorDelegate callback)
        {
            CefWebviewImplementation.Instance.ResizeWebview(webviewIndex, rect, callback);
        }

        /// <summary>
        /// Webview의 ScrollBar 표시 여부를 설정합니다.
        /// </summary>
        /// <param name="webviewIndex">Webview의 인덱스입니다.</param>
        /// <param name="isShow">ScrollBar 표시 여부입니다.</param>
        /// <param name="callback">CefWebviewError 인스턴스를 콜백으로 전달합니다.</param>
        /// <example>
        /// Example Usage : 
        /// <code>
        /// public void SampleShowScrollBar(int webviewIndex, bool isShow)
        /// {
        ///     CefWebview.ShowScrollBar(webviewIndex, isShow, (error) =>
        ///     {
        ///         if (CefWebview.IsSuccess(error) == true)
        ///         {
        ///             Debug.Log("ShowScrollBar succeeded.");
        ///         }
        ///         else
        ///         {
        ///             Debug.Log(string.Format("ShowScrollBar failed. error:{0}", error));
        ///         }
        ///     });
        /// }
        /// </code>
        /// </example>
        public static void ShowScrollBar(int webviewIndex, bool isShow, CefWebviewCallback.ErrorDelegate callback)
        {
            CefWebviewImplementation.Instance.ShowScrollBar(webviewIndex, isShow, callback);
        }

        /// <summary>
        /// Webview의 포커스를 설정합니다.
        /// </summary>
        /// <param name="webviewIndex">Webview의 인덱스입니다.</param>
        /// <param name="isFocus">포커스 여부입니다.</param>
        /// <param name="callback">CefWebviewError 인스턴스를 콜백으로 전달합니다.</param>
        /// <example>
        /// Example Usage : 
        /// <code>
        /// public void SampleSetFocus(int webviewIndex)
        /// {
        ///     CefWebview.SetFocus(webviewIndex, true, (error) =>
        ///     {
        ///         if (CefWebview.IsSuccess(error) == true)
        ///         {
        ///             Debug.Log("SetFocus succeeded.");
        ///         }
        ///         else
        ///         {
        ///             Debug.Log(string.Format("SetFocus failed. error:{0}", error));
        ///         }
        ///     });
        /// }
        /// </code>
        /// </example>
        public static void SetFocus(int webviewIndex, bool isFocus, CefWebviewCallback.ErrorDelegate callback)
        {
            CefWebviewImplementation.Instance.SetFocus(webviewIndex, isFocus, callback);
        }

        /// <summary>
        /// CefWebview가 기준이 되는 마우스 상대 좌표를 설정합니다.
        /// CefWebview 좌측 상단이 (0, 0)입니다.
        /// </summary>
        /// <param name="webviewIndex">Webview의 인덱스입니다.</param>
        /// <param name="mousePosition">마우스 상대 좌표입니다.</param>
        /// <param name="callback">CefWebviewError 인스턴스를 콜백으로 전달합니다.</param>
        /// <example>
        /// Example Usage : 
        /// <code>
        /// public void SampleSetMousePosition(int webviewIndex, Vector2 mousePosition)
        /// {
        ///     CefWebview.SetMousePosition(webviewIndex, mousePosition, (error) =>
        ///     {
        ///         if (CefWebview.IsSuccess(error) == true)
        ///         {
        ///             Debug.Log("SetMousePosition succeeded.");
        ///         }
        ///         else
        ///         {
        ///             Debug.Log(string.Format("SetMousePosition failed. error:{0}", error));
        ///         }
        ///     });
        /// }
        /// </code>
        /// </example>
        public static void SetMousePosition(int webviewIndex, Vector2 mousePosition, CefWebviewCallback.ErrorDelegate callback)
        {
            CefWebviewImplementation.Instance.SetMousePosition(webviewIndex, mousePosition, callback);
        }

        /// <summary>
        /// 홈으로 이동합니다.
        /// </summary>
        /// <param name="webviewIndex">Webview의 인덱스입니다.</param>
        /// <param name="callback">CefWebviewError 인스턴스를 콜백으로 전달합니다.</param>
        /// <example>
        /// Example Usage : 
        /// <code>
        /// public void SampleGoHome(int webviewIndex)
        /// {
        ///     CefWebview.GoHome(webviewIndex, (error) =>
        ///     {
        ///         if (CefWebview.IsSuccess(error) == true)
        ///         {
        ///             Debug.Log("GoHome succeeded.");
        ///         }
        ///         else
        ///         {
        ///             Debug.Log(string.Format("GoHome failed. error:{0}", error));
        ///         }
        ///     });
        /// }
        /// </code>
        /// </example>
        public static void GoHome(int webviewIndex, CefWebviewCallback.ErrorDelegate callback)
        {
            CefWebviewImplementation.Instance.GoHome(webviewIndex, callback);
        }

        /// <summary>
        /// 뒤로 이동합니다.
        /// </summary>
        /// <param name="webviewIndex">Webview의 인덱스입니다.</param>
        /// <param name="callback">CefWebviewError 인스턴스를 콜백으로 전달합니다.</param>
        /// <example>
        /// Example Usage : 
        /// <code>
        /// public void SampleGoBack(int webviewIndex)
        /// {
        ///     CefWebview.GoBack(webviewIndex, (error) =>
        ///     {
        ///         if (CefWebview.IsSuccess(error) == true)
        ///         {
        ///             Debug.Log("GoBack succeeded.");
        ///         }
        ///         else
        ///         {
        ///             Debug.Log(string.Format("GoBack failed. error:{0}", error));
        ///         }
        ///     });
        /// }
        /// </code>
        /// </example>
        public static void GoBack(int webviewIndex, CefWebviewCallback.ErrorDelegate callback)
        {
            CefWebviewImplementation.Instance.GoBack(webviewIndex, callback);
        }

        /// <summary>
        /// 앞으로 이동합니다.
        /// </summary>
        /// <param name="webviewIndex">Webview의 인덱스입니다.</param>
        /// <param name="callback">CefWebviewError 인스턴스를 콜백으로 전달합니다.</param>
        /// <example>
        /// Example Usage : 
        /// <code>
        /// public void SampleGoForward(int webviewIndex)
        /// {
        ///     CefWebview.GoForward(webviewIndex, (error) =>
        ///     {
        ///         if (CefWebview.IsSuccess(error) == true)
        ///         {
        ///             Debug.Log("GoForward succeeded.");
        ///         }
        ///         else
        ///         {
        ///             Debug.Log(string.Format("GoForward failed. error:{0}", error));
        ///         }
        ///     });
        /// }
        /// </code>
        /// </example>
        public static void GoForward(int webviewIndex, CefWebviewCallback.ErrorDelegate callback)
        {
            CefWebviewImplementation.Instance.GoForward(webviewIndex, callback);
        }

        /// <summary>
        /// 유효하지 않은 scheme이 포함된 url의 경우, 오류로 처리되지 않기 위해 custom scheme을 등록해야 합니다.
        /// </summary>
        /// <param name="schemeList">scheme list입니다.</param>
        /// <param name="callback">CefWebviewError 인스턴스를 콜백으로 전달합니다.</param>
        /// <example>
        /// Example Usage : 
        /// <code>
        /// public void SampleSetInvalidRedirectUrlScheme(int webviewIndex)
        /// {
        ///     var schemeList = new List&lt;string>
        ///     {
        ///         "scheme1:",
        ///         "scheme2:"
        ///     };
        ///     CefWebview.SetInvalidRedirectUrlScheme(schemeList, (error) =>
        ///     {
        ///         if (CefWebview.IsSuccess(error) == true)
        ///         {
        ///             Debug.Log("SetInvalidRedirectUrlScheme succeeded.");
        ///         }
        ///         else
        ///         {
        ///             Debug.Log(string.Format("SetInvalidRedirectUrlScheme failed. error:{0}", error));
        ///         }
        ///     });
        /// }
        /// </code>
        /// </example>
        public static void SetInvalidRedirectUrlScheme(List<string> schemeList, CefWebviewCallback.ErrorDelegate callback)
        {
            CefWebviewImplementation.Instance.SetInvalidRedirectUrlScheme(schemeList, callback);
        }

        /// <summary>
        /// 자바스크립트를 실행합니다.
        /// </summary>
        /// <param name="webviewIndex">Webview의 인덱스입니다.</param>
        /// <param name="javaScript">CefWebview에서 실행할 자바스크립트입니다.</param>
        /// <param name="callback">CefWebviewError 인스턴스를 콜백으로 전달합니다.</param>
        /// <example>
        /// Example Usage : 
        /// <code>
        /// public void SampleExecuteJavaScript(int webviewIndex)
        /// {
        ///     CefWebview.ExecuteJavaScript(webviewIndex, "alert('ExecuteJavaScript');", (error) =>
        ///     {
        ///         if (CefWebview.IsSuccess(error) == true)
        ///         {
        ///             Debug.Log("ExecuteJavaScript succeeded.");
        ///         }
        ///         else
        ///         {
        ///             Debug.Log(string.Format("ExecuteJavaScript failed. error:{0}", error));
        ///         }
        ///     });
        /// }
        /// </code>
        /// </example>
        public static void ExecuteJavaScript(int webviewIndex, string javaScript, CefWebviewCallback.ErrorDelegate callback)
        {
            CefWebviewImplementation.Instance.ExecuteJavaScript(webviewIndex, javaScript, callback);
        }
    }
}