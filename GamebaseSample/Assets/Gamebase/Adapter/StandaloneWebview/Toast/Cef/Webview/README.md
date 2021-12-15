# CEF Webview for Unity

* [CEF(Chromuim embedded Framework)](https://bitbucket.org/chromiumembedded/cef)
* CEF Webview for Unity는 CEF를 사용한 Webview입니다.

# Environments

* Unity v5.6.6 이상
* nhncef.dll v2.0.3.0

## Supported Platforms

* Unity Standalone
    * Windows 7 이상
    * MAC OS는 지원하지 않습니다.

## Specification

CefWebview는 텍스처 제공 여부에 따라 두 가지 Webview로 구분합니다.

| | 텍스처를 제공하지 않는 Webview | 텍스처를 제공하는 Webview |
| -- | -- | -- |
| 텍스처 렌더링 | CefWebview 내부 OnGUI | 사용자 GUI 툴 (NGUI, UGUI) |
| UI Drawing | CefWebview | CefWebview는 데이터만 전달하므로 사용자가 관리 |
| 마우스 좌표 계산 | CefWebview | 사용자가 SetMousePosition API를 사용하여 CefWebview로 전달 |

### 텍스처를 제공하지 않는 Webview

MonoBehaviour의 OnGUI message에서 텍스처를 렌더링 하므로 `GUI 계층의 최상단`에 Webview를 노출합니다.

**Example**

* CreateWebview API 호출 시, `useTexture 값을 false로 입력`하여 인스턴스를 생성합니다.
* 텍스처는 항상 `null`로 반환합니다.

```cs
public void CreateNotProvideTextureWebview()
{
    var rect = new Rect(0, 0, Screen.width, Screen.height);
    var configuration = new RequestVo.WebviewConfiguration
    {
        useTexture = false,
        viewRect = rect,
        popupOption = new RequestVo.WebviewConfiguration.PopupOption
        {
            blockMessage = TXT_POPUP_BLOCK_MESSAGE,
#if !UNITY_EDITOR && UNITY_STANDALONE
            type = PopupType.POPUP
#else
            type = PopupType.REDIRECT
#endif
	    },
	    bgType = BgType.OPAQUE
    };

    CefWebview.CreateWebview(
        useTexture,
        viewRect,
        popupOption,
        (webviewInfo, error) =>
        {
            if (CefWebview.IsSuccess(error) == true)
            {
                Debug.Log(string.Format("CreateWebview succeeded. index:{0}", webviewInfo.index));
            }
            else
            {
                Debug.Log(string.Format("CreateWebview failed. error:{0}", error));
            }
        });
}
```

### 텍스처를 제공하는 Webview

NGUI, UGUI와 같은 GUI 툴의 제약 없이 Webview를 제공하기 위해 CEF Webview의 버퍼가 설정된 텍스처를 사용자에게 제공합니다.

**Example**

* CreateWebview API 호출 시, `useTexture 값을 true로 입력`하여 인스턴스를 생성합니다.
* 텍스처는 `ResponseVo.WebviewInfo`의 인스턴스에 포함하여 반환합니다.

```cs
public class UguiSample
{
    public RawImage rawImage;

    public void CreateProvideTextureWebview()
    {
        var rect = new Rect(0, 0, Screen.width, Screen.height);
        var configuration = new RequestVo.WebviewConfiguration
        {
            useTexture = true,
            viewRect = rect,
            popupOption = new RequestVo.WebviewConfiguration.PopupOption
            {
                blockMessage = TXT_POPUP_BLOCK_MESSAGE,
#if !UNITY_EDITOR && UNITY_STANDALONE
                type = PopupType.POPUP
#else
                type = PopupType.REDIRECT
#endif
            },
            bgType = BgType.OPAQUE
        };

        CefWebview.CreateWebview(
            useTexture,
            viewRect,
            popupOption,
            (webviewInfo, error) =>
            {
                if (CefWebview.IsSuccess(error) == true)
                {
                    Debug.Log(string.Format("CreateWebview succeeded. index:{0}, texture:{1}", webviewInfo.index, webviewInfo.texture));

                    rawImage.texture = webviewInfo.texture;
                }
                else
                {
                    Debug.Log(string.Format("CreateWebview failed. error:{0}", error));
                }
            });
    }
}
```

#### 텍스처 렌더링

> !참고
> 텍스처를 제공하는 Webview만 해당하는 내용입니다.

GUI 툴에서 제공하는 방식으로 사용자가 텍스처를 렌더링 해야 합니다.

#### UI Drawing

> !참고
> 텍스처를 제공하는 Webview만 해당하는 내용입니다.

ShowWebview API 호출 시, 입력한 webStatusDelegate를 통해 ResponseVo.WebviewStatus 인스턴스가 전달됩니다. (GUI 처리가 필요한 경우에만 호출됩니다.) 

WebviewStatus 인스턴스에는 Webview의 현재 상태 값(WebUpdateStatus)이 포함되어 있으며, WebUpdateStatus.JSDIALOG와 WebUpdateStatus.POPUPBLOCK과 같이 GUI 처리가 필요한 경우에는 사용자 처리가 필요합니다.

**Example**

```cs
public void ShowWebview()
{
    var webviewUrl = "http://naver.com";

    CefWebview.ShowWebview(
        // webviewIndex,
        // url,
        // isShowScrollBar,
        // callback,
        // webUrlDelegate,
        // webTitleDelegate,
        webInputElementFocusDelegate,
        (webviewStatus) =>
        {
            switch (webviewStatus.status)
            {
                case WebUpdateStatus.JSDIALOG:
                {
                    // Dialog GUI
                    Debug.Log(string.Format(
                        "message:{0}, jsDialogType:{1}",
                        webviewStatus.jsDialog.message,
                        // JsDialogType.ALERT, JsDialogType.CONFIRM, JsDialogType.PROMPT
                        webviewStatus.jsDialog.type));

                    // Dialog의 버튼 클릭 시, CEF Webview에서 이벤트를 처리하기 위해 아래 delegate를 호출해야 합니다.
                    // OK 버튼 클릭 : webviewStatus.jsDialog.clickButtonDelegate(true);
                    // Cancel 버튼 클릭 : webviewStatus.jsDialog.clickButtonDelegate(false);
                    break;
                }
                case WebUpdateStatus.POPUPBLOCK:
                {
                    // PopupBlock GUI
                    Debug.Log(string.Format("message:{0}", webviewStatus.popupBlock.message));
                    break;
                }
                case WebUpdateStatus.LOAD_END:
                {
                    // 페이지 로딩 완료
                    Debug.Log(string.Format("url:{0}", webviewStatus.loadEnd.url));
                    break;
                }
                case WebUpdateStatus.PASS_POPUP_INFO:
                {
                    // 팝업이 노출되지 않고, 데이터가 전달됩니다.
                    Debug.Log(
                        string.Format(
                            "pass popup info.\n" +
                            "name:{0}\n" +
                            "url:{1}\n" +
                            "left:{2}\n" +
                            "top:{3}\n" +
                            "width:{4}\n" +
                            "height:{5}\n" +
                            "menubar:{6}\n" +
                            "status:{7}\n" +
                            "toolbar:{8}\n" +
                            "location:{9}\n" +
                            "scrollbars{10}\n" +
                            "resizable:{11}",
                            vo.passPopupInfo.name,
                            vo.passPopupInfo.url,
                            vo.passPopupInfo.left,
                            vo.passPopupInfo.top,
                            vo.passPopupInfo.width,
                            vo.passPopupInfo.height,
                            vo.passPopupInfo.menubar,
                            vo.passPopupInfo.status,
                            vo.passPopupInfo.toolbar,
                            vo.passPopupInfo.location,
                            vo.passPopupInfo.scrollbars,
                            vo.passPopupInfo.resizable));
                    break;
                }
            }
        });
}
```

#### 마우스 좌표 계산

> !참고
> 텍스처를 제공하는 Webview만 해당하는 내용입니다.

NGUI, UGUI와 같은 GUI 툴마다 사용하는 카메라 및 좌표 처리 방식이 다르므로 Webview 기준의 마우스 좌표를 SetMousePosition API를 사용하여 CefWebview로 전달해야 합니다.

**Example**

```cs
private void Upate()
{
    // Webview 좌, 상단이 0, 0으로 계산된 mousePosition을 전달해야 한다.
    CefWebview.SetMousePosition(webviewIndex, mousePosition, callback);
}
```

#### 포커스 처리

멀티 Webview 사용 시 키보드 입력과 같은 Input 이벤트를 처리하기 위한 포커스 처리가 필요합니다.

> !주의
> Webview 텍스처 위에 Dialog UI와 같은 다른 GUI가 노출될 경우에도 포커스 처리가 필요합니다.

# API

CefWebview의 API를 사용하기 위해서는 아래 namespace를 using 해야 합니다.

```cs
using Toast.Cef.Webview;
```

## IsSuccess

CefWebviewError를 사용하여 성공 여부를 판단합니다.

### Interface

```cs
static bool IsSuccess(CefWebviewError error)
```

### Parameters

* error
    * CefWebviewError 인스턴스입니다.

### Returns

* bool
    * 성공 여부입니다.

### Example

```cs
public void SampleIsSuccess(CefWebviewError error)
{
    CefWebview.IsSuccess(error);
}
```

## IsInitialized

CefWebview의 초기화 여부를 확인합니다.

### Interface

```cs
static bool IsInitialized()
```

### Parameters

* 없음

### Returns

* bool
    * 초기화 여부입니다.

### Example

```cs
public void SampleIsInitialized()
{
    CefWebview.IsInitialized();
}
```

## Initialize

CefWebview를 초기화합니다.
CefWebview의 모든 API 호출 전, 반드시 호출되어야 합니다.

### Interface

```cs
static void Initialize(string locale, CefWebviewCallback.ErrorDelegate callback)
```

### Parameters

* locale
    * 입력된 locale 정보를 CEF로 전달합니다.
* callback
    * CefWebviewError 인스턴스를 콜백으로 전달합니다.

### Returns

* 없음

### Example

```cs
CefWebview.Initialize("ko", (error) =>
{
    if (CefWebview.IsSuccess(error) == true)
    {
        Debug.Log("Initialize succeeded.");
    }
    else
    {
        Debug.Log(string.Format("Initialize failed. error:{0}", error));
    }
});
```

## SetDebugEnable

디버그 모드를 활성화합니다.
애플리케이션 배포 시에는 false로 설정하십시오.

### Interface

```cs
static void SetDebugEnable(bool enable)
```

### Parameters

* enable
    * 디버그 활성화 여부입니다.

### Returns

* 없음

### Example

```cs
public void SampleSetDebugEnable()
{
    CefWebview.SetDebugEnable(true);
}
```

## CreateWebview

Webview의 인스턴스를 생성합니다.

### Interface

```cs
static void CreateWebview(
    RequestVo.WebviewConfiguration configuration,
    CefWebviewCallback.CefWebviewDelegate<ResponseVo.WebviewInfo> callback)
    {
        CefWebviewImplementation.Instance.CreateWebview(configuration, callback);
    }
```

### Parameters

* configuration
    * useTexture
        * True : 텍스처를 제공하는 Webview를 사용합니다.
        * False : 텍스처를 제공하지 않는 Webview를 사용합니다.
    * viewRect : Webview의 좌표 및 크기를 지정합니다.
    * bgType : CefWebview 배경을 투명, 불투명으로 설정합니다.
    * popupOption
        * type : 팝업 타입을 지정합니다. (PopupType.POPUP, PopupType.REDIRECT, PopupType.BLOCK, PopupType.PASS_INFO)
        * blockMessage : 팝업 타입을 PopupType.BLOCK으로 설정하여, 팝업이 차단되면 노출되는 메시지를 설정합니다.
* callback
    * data 
        * ResponseVo.WebviewInfo 인스턴스를 콜백으로 전달합니다.
            * index
                * 생성된 Webview의 인덱스입니다.
            * texture
                * CEF Webview의 버퍼가 설정된 텍스처입니다.
                * useTexture를 false로 설정할 경우에는 항상 null을 반환합니다.
    * error
        * CefWebviewError 인스턴스가 콜백으로 전달됩니다.

### Returns

* 없음

### Example

```cs
public class UguiSample
{
    public RawImage rawImage;

    public void SampleCreateWebview()
    {
        var rect = new Rect(0, 0, Screen.width, Screen.height);
        var configuration = new RequestVo.WebviewConfiguration
        {
            useTexture = true,
            viewRect = rect,
            popupOption = new RequestVo.WebviewConfiguration.PopupOption
            {
                blockMessage = TXT_POPUP_BLOCK_MESSAGE,
#if !UNITY_EDITOR && UNITY_STANDALONE
                type = PopupType.POPUP
#else
                type = PopupType.REDIRECT
#endif
            },
            bgType = BgType.OPAQUE
        };

        CefWebview.CreateWebview(
        useTexture,
        viewRect,
        popupOption,
        (data, error) =>
        {
            if (CefWebview.IsSuccess(error) == true)
            {
                Debug.Log(string.Format("CreateWebview succeeded. index:{0}, texture:{1}", data.index, data.texture));
            }
            else
            {
                Debug.Log(string.Format("CreateWebview failed. error:{0}", error));
            }
        });
    }
}
```

## RemoveWebview

생성된 Webview의 인스턴스를 제거합니다.

### Interface

```cs
static void RemoveWebview(int webviewIndex, CefWebviewCallback.ErrorDelegate callback)
```

### Parameters

* webviewIndex
    * Webview의 인덱스입니다.
* callback
    * CefWebviewError 인스턴스를 콜백으로 전달합니다.

### Returns

* 없음

### Example

```cs
public void SampleRemoveWebview(int webviewIndex)
{
    CefWebview.RemoveWebview(webviewIndex, (error) =>
    {
        if (CefWebview.IsSuccess(error) == true)
        {
            Debug.Log("RemoveWebview succeeded.");
        }
        else
        {
            Debug.Log(string.Format("RemoveWebview failed. error:{0}", error));
        }
    });
}
```

## ShowWebview

Webview를 오픈합니다.

### Interface

```cs
static void ShowWebview(
       int webviewIndex,
       string url,
       bool isShowScrollBar,
       CefWebviewCallback.ErrorDelegate callback,
       CefWebviewCallback.DataDelegate<string> webUrlDelegate = null,
       CefWebviewCallback.DataDelegate<string> webTitleDelegate = null,
       CefWebviewCallback.DataDelegate<int> webInputElementFocusDelegate = null,
       CefWebviewCallback.DataDelegate<ResponseVo.WebviewStatus> webStatusDelegate = null)
```

### Parameters

* webviewIndex
    * Webview의 인덱스입니다.
* url
    * 로드할 페이지의 URL입니다.
* isShowScrollBar
    * ScrollBar 표시 여부를 설정합니다.
* callback
    * CefWebviewError 인스턴스를 콜백으로 전달합니다.
* webUrlDelegate
    * 로드 및 Redirection된 페이지의 URL 정보를 반환합니다.
* webTitleDelegate
    * 로드 및 Redirection된 페이지의 Title 정보를 반환합니다
* webInputElementFocusDelegate
    * 포커싱 된 Element의 타입을 패스워드 타입과 나머지 타입으로 구분한 값이 반환됩니다.
        * Password:1, ELSE:100
    * 조합 문자(한글, 일본어 등) 사용 중 패스워드 타입의 Element로 포커싱이 변경될 경우, 조합 문자가 그대로 입력되는 이슈를 처리할 수 있습니다. (Example 참조)
* webStatusDelegate
    * [UI Drawing 참고](#UI-Drawing)

### Returns

* 없음

**Example**

```cs
public void SampleShowWebview(int webviewIndex)
{
    var webviewUrl = "http://naver.com";
    
    CefWebview.ShowWebview(
        webviewIndex,
        webviewUrl,
        true,
        (error) =>
        {
            if (CefWebview.IsSuccess(error) == true)
            {
                Debug.Log("ShowWebview succeeded.");
            }
            else
            {
                Debug.Log(string.Format("ShowWebview failed. error:{0}", error));
            }
        },
        (url) =>
        {
            if (url.Contains("cef://error") == true)
            {
                Debug.Log("Webview 오픈이 실패하였습니다.");
            }
        },
        (title) =>
        {
            Debug.Log(string.Format("title:{0}", title));
        },
        (type) =>
        {
            if (type == InputElementType.PASSWORD)
            {
				// Unity v5.6.4f1 미만의 버전에서는 IMECompositionMode가 정상 동작하지 않는 이슈가 있으므로 CefWebview 지원 버전인 Unity v5.6.6 이상을 사용하시길 권장합니다.
                Input.imeCompositionMode = IMECompositionMode.Off;
            }
            else
            {
                Input.imeCompositionMode = IMECompositionMode.On;
            }
        },
        (webviewStatus) =>
        {
            switch (webviewStatus.status)
            {
                case WebUpdateStatus.JSDIALOG:
                {
                    // Dialog GUI
                    Debug.Log(string.Format(
                        "message:{0}, jsDialogType:{1}",
                        webviewStatus.jsDialog.message,
                        // JsDialogType.ALERT, JsDialogType.CONFIRM, JsDialogType.PROMPT
                        webviewStatus.jsDialog.type));
                        
                    // Dialog의 버튼 클릭 시, CEF Webview에서 이벤트를 처리하기 위해 아래 delegate를 호출해야 합니다.
                    // OK 버튼 클릭 : webviewStatus.jsDialog.clickButtonDelegate(true);
                    // Cancel 버튼 클릭 : webviewStatus.jsDialog.clickButtonDelegate(false);
                    break;
                }
                case WebUpdateStatus.POPUPBLOCK:
                {
                    // PopupBlock GUI
                    Debug.Log(string.Format("message:{0}", webviewStatus.popupBlock.message));
                    break;
                }
                case WebUpdateStatus.LOAD_END:
                {
                    // 페이지 로딩 완료
                    Debug.Log(string.Format("url:{0}", webviewStatus.loadEnd.url));
                    break;
                }
                case WebUpdateStatus.PASS_POPUP_INFO:
                {
                    // 팝업이 노출되지 않고, 데이터가 전달됩니다.
                    Debug.Log(
                        string.Format(
                            "pass popup info.\n" +
                            "name:{0}\n" +
                            "url:{1}\n" +
                            "left:{2}\n" +
                            "top:{3}\n" +
                            "width:{4}\n" +
                            "height:{5}\n" +
                            "menubar:{6}\n" +
                            "status:{7}\n" +
                            "toolbar:{8}\n" +
                            "location:{9}\n" +
                            "scrollbars{10}\n" +
                            "resizable:{11}",
                            vo.passPopupInfo.name,
                            vo.passPopupInfo.url,
                            vo.passPopupInfo.left,
                            vo.passPopupInfo.top,
                            vo.passPopupInfo.width,
                            vo.passPopupInfo.height,
                            vo.passPopupInfo.menubar,
                            vo.passPopupInfo.status,
                            vo.passPopupInfo.toolbar,
                            vo.passPopupInfo.location,
                            vo.passPopupInfo.scrollbars,
                            vo.passPopupInfo.resizable));
                    break;
                }
            }
        });
}
```

## HideWebview

Webview를 숨깁니다.

### Interface

```cs
static void HideWebview(int webviewIndex, CefWebviewCallback.ErrorDelegate callback)
```

### Parameters

* webviewIndex
    * Webview의 인덱스입니다.
* callback
    * CefWebviewError 인스턴스를 콜백으로 전달합니다.

### Returns

* 없음

### Example

```cs
public void SampleHideWebview(int webviewIndex)
{
    CefWebview.HideWebview(webviewIndex, (error) =>
    {
        if (CefWebview.IsSuccess(error) == true)
        {
            Debug.Log("HideWebview succeeded.");
        }
        else
        {
            Debug.Log(string.Format("HideWebview failed. error:{0}", error));
        }
    });
}
```

## ResizeWebview

생성된 Webview의 사이즈를 변경합니다.

### Interface

```cs
static void ResizeWebview(int webviewIndex, int width, int height, CefWebviewCallback.ErrorDelegate callback)
```

### Parameters

* webviewIndex
    * Webview의 인덱스입니다.
* width
    * 변경할 Webview의 너비입니다.
* height
    * 변경할 Webview의 높이입니다.
* callback
    * CefWebviewError 인스턴스를 콜백으로 전달합니다.

### Returns

* 없음

### Example

```cs
public void SampleResizeWebview(int webviewIndex)
{
    CefWebview.ResizeWebview(webviewIndex, 800, 600, (error) =>
    {
        if (CefWebview.IsSuccess(error) == true)
        {
            Debug.Log("ResizeWebview succeeded.");
        }
        else
        {
            Debug.Log(string.Format("ResizeWebview failed. error:{0}", error));
        }
    });
}
```

## ResizeWebview

생성된 Webview의 위치와 사이즈를 변경합니다.

### Interface

```cs
static void ResizeWebview(int webviewIndex, Rect rect, CefWebviewCallback.ErrorDelegate callback)
```

### Parameters

* webviewIndex
    * Webview의 인덱스입니다.
* rect
    * 변경할 Webview의 위치와 사이즈입니다.
* callback
    * CefWebviewError 인스턴스를 콜백으로 전달합니다.

### Returns

* 없음

### Example

```cs
public void SampleResizeWebview(int webviewIndex)
{
    var rect = new Rect(0, 0, 300, 300);

    CefWebview.ResizeWebview(webviewIndex, rect, (error) =>
    {
        if (CefWebview.IsSuccess(error) == true)
        {
            Debug.Log("ResizeWebview succeeded.");
        }
        else
        {
            Debug.Log(string.Format("ResizeWebview failed. error:{0}", error));
        }
    });
}
```

## ShowScrollBar

Webview의 ScrollBar 표시 여부를 설정합니다.

### Interface

```cs
static void ShowScrollBar(int webviewIndex, bool isShow, CefWebviewCallback.ErrorDelegate callback)
```

### Parameters

* webviewIndex
    * Webview의 인덱스입니다.
* isShow
    * ScrollBar 표시 여부입니다.
* callback
    * CefWebviewError 인스턴스를 콜백으로 전달합니다.

### Returns

* 없음

### Example

```cs
public void SampleShowScrollBar(int webviewIndex, bool isShow)
{
    CefWebview.ShowScrollBar(webviewIndex, isShow, (error) =>
    {
        if (CefWebview.IsSuccess(error) == true)
        {
            Debug.Log("ShowScrollBar succeeded.");
        }
        else
        {
            Debug.Log(string.Format("ShowScrollBar failed. error:{0}", error));
        }
    });
}
```

## SetFocus

Webview의 포커스를 설정합니다.

### Interface

```cs
static void SetFocus(int webviewIndex, bool isFocus, CefWebviewCallback.ErrorDelegate callback)
```

### Parameters

* webviewIndex
    * Webview의 인덱스입니다.
* isFocus
    * 포커스 여부입니다.
* callback
    * CefWebviewError 인스턴스를 콜백으로 전달합니다.

### Returns

* 없음

### Example

```cs
public void SampleSetFocus(int webviewIndex)
{
    CefWebview.SetFocus(webviewIndex, true, (error) =>
    {
        if (CefWebview.IsSuccess(error) == true)
        {
            Debug.Log("SetFocus succeeded.");
        }
        else
        {
            Debug.Log(string.Format("SetFocus failed. error:{0}", error));
        }
    });
}
```

## SetMousePosition

> !참고
> 텍스처를 제공하는 Webview만 해당하는 내용입니다.

Webview가 기준이 되는 마우스 상대 좌표를 설정합니다.
Webview 좌측 상단이 (0, 0)입니다.

### Interface

```cs
static void SetMousePosition(int webviewIndex, Vector2 mousePosition, CefWebviewCallback.ErrorDelegate callback)
```

### Parameters

* webviewIndex
    * Webview의 인덱스입니다.
* mousePosition
    * Webview가 기준이 되는 마우스 상대 좌표입니다.
* callback
    * CefWebviewError 인스턴스를 콜백으로 전달합니다.

### Returns

* 없음

### Example

```cs
public void SampleSetMousePosition(int webviewIndex, Vector2 mousePosition)
{
    CefWebview.SetMousePosition(webviewIndex, mousePosition, (error) =>
    {
        if (CefWebview.IsSuccess(error) == true)
        {
            Debug.Log("SetMousePosition succeeded.");
        }
        else
        {
            Debug.Log(string.Format("SetMousePosition failed. error:{0}", error));
        }
    });
}
```

## GoHome

홈으로 이동합니다.

### Interface

```cs
static void GoHome(int webviewIndex, CefWebviewCallback.ErrorDelegate callback)
```

### Parameters

* webviewIndex
    * Webview의 인덱스입니다.
* callback
    * CefWebviewError 인스턴스를 콜백으로 전달합니다.

### Returns

* 없음

### Example

```cs
public void SampleGoHome(int webviewIndex)
{
    CefWebview.GoHome(webviewIndex, (error) =>
    {
        if (CefWebview.IsSuccess(error) == true)
        {
            Debug.Log("GoHome succeeded.");
        }
        else
        {
            Debug.Log(string.Format("GoHome failed. error:{0}", error));
        }
    });
}
```

## GoBack

뒤로 이동합니다.

### Interface

```cs
static void GoBack(int webviewIndex, CefWebviewCallback.ErrorDelegate callback)
```

### Parameters

* webviewIndex
    * Webview의 인덱스입니다.
* callback
    * CefWebviewError 인스턴스를 콜백으로 전달합니다.

### Returns

* 없음

### Example

```cs
public void SampleGoBack(int webviewIndex)
{
    CefWebview.GoBack(webviewIndex, (error) =>
    {
        if (CefWebview.IsSuccess(error) == true)
        {
            Debug.Log("GoBack succeeded.");
        }
        else
        {
            Debug.Log(string.Format("GoBack failed. error:{0}", error));
        }
    });
}
```

## GoForward

앞으로 이동합니다.

### Interface

```cs
static void GoForward(int webviewIndex, CefWebviewCallback.ErrorDelegate callback)
```

### Parameters

* webviewIndex
    * Webview의 인덱스입니다.
* callback
    * CefWebviewError 인스턴스를 콜백으로 전달합니다.

### Returns

* 없음

### Example

```cs
public void SampleGoForward(int webviewIndex)
{
    CefWebview.GoForward(webviewIndex, (error) =>
    {
        if (CefWebview.IsSuccess(error) == true)
        {
            Debug.Log("GoForward succeeded.");
        }
        else
        {
            Debug.Log(string.Format("GoForward failed. error:{0}", error));
        }
    });
}
```

## SetInvalidRedirectUrlScheme

유효하지 않은 scheme이 포함된 url의 경우, 오류로 처리되지 않기 위해 custom scheme을 등록해야 합니다.

### Interface

```cs
static void SetInvalidRedirectUrlScheme(List<string> schemeList, CefWebviewCallback.ErrorDelegate callback)
```

### Parameters

* webviewIndex
    * Webview의 인덱스입니다.
* schemes
    * scheme list입니다.
* callback
    * CefWebviewError 인스턴스를 콜백으로 전달합니다.

### Returns

* 없음

### Example

```cs
public void SampleSetInvalidRedirectUrlScheme(int webviewIndex)
{
    var schemeList = new List<string>
    {
        "scheme1:",
        "scheme2:"
    };
    CefWebview.SetInvalidRedirectUrlScheme(schemeList, (error) =>
    {
        if (CefWebview.IsSuccess(error) == true)
        {
            Debug.Log("SetInvalidRedirectUrlScheme succeeded.");
        }
        else
        {
            Debug.Log(string.Format("SetInvalidRedirectUrlScheme failed. error:{0}", error));
        }
    });
}
```

## ExecuteJavaScript

자바스크립트를 실행합니다.

### Interface

```cs
static void ExecuteJavaScript(int webviewIndex, string javaScript, CefWebviewCallback.ErrorDelegate callback)
```

### Parameters

* webviewIndex
    * Webview의 인덱스입니다.
* javaScript
    * CEF에서 실행할 자바스크립트입니다. 
* callback
    * CefWebviewError 인스턴스를 콜백으로 전달합니다.

### Returns

* 없음

### Example

```cs
public void SampleExecuteJavaScript(int webviewIndex)
{
    CefWebview.ExecuteJavaScript(webviewIndex, "alert('ExecuteJavaScript');", (error) =>
    {
        if (CefWebview.IsSuccess(error) == true)
        {
            Debug.Log("ExecuteJavaScript succeeded.");
        }
        else
        {
            Debug.Log(string.Format("ExecuteJavaScript failed. error:{0}", error));
        }
    });
}
```