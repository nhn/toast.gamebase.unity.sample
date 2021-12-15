using Toast.Iap.Ongate;
using UnityEngine;

public class ToastIAPConsoleView : MonoBehaviour
{
    private static readonly int BUTTON_HEIGHT = Screen.height / 10;
    private static readonly int MENU_COUNT = 3;
    private static readonly int BUTTON_FONT_SIZE = 30;
    private static readonly int LABEL_FONT_SIZE = 30;
    private Vector2 scrollPosition = Vector2.zero;

    private static string sdkLog = "";
    private static int sdkLogCount = 1;

    // Use this for initialization
    void Start()
    {
        // TODO : IAP Unity 플러그인을 초기화 한다.
        ToastIapOngate.Initialize();

        // TODO : IAP 로그 정보의 노출 여부를 설정 한다. 릴리즈 시에는 false 로 설정하는 것을 권장합니다.
        ToastIapOngate.SetDebugMode(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

#if UNITY_EDITOR
        if (Debug.isDebugBuild)
        {
            MonoBehaviour[] monoBehaviours = this.GetComponentsInChildren<MonoBehaviour>();
            int count = monoBehaviours.Length;

            for (int i = 0; i < count; ++i)
            {
                if (null == monoBehaviours[i])
                {
                    Debug.LogError("Have a missing script in gameobject. gameobject name is '" + this.name + "'");
                    return;
                }
            }
        }
#endif
    }

    void OnGUI()
    {
        int y = 0;
        int scrollContentsWidth = Screen.width;
        int labalHeight = (LABEL_FONT_SIZE * sdkLogCount * 3);
        int scrollContentsHeight = BUTTON_HEIGHT * MENU_COUNT + labalHeight;
        scrollPosition = GUI.BeginScrollView(new Rect(0, 0, Screen.width, Screen.height), scrollPosition, new Rect(0, 0, scrollContentsWidth, scrollContentsHeight));

        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = BUTTON_FONT_SIZE;

        GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.fontSize = LABEL_FONT_SIZE;

        if (GUI.Button(new Rect(0, y, scrollContentsWidth, BUTTON_HEIGHT), "SetUserId", buttonStyle))
        {
            // TODO : 애플리케이션 실행시마다 유저ID를 등록 한다.
            Result result = ToastIapOngate.SetOngateUserId("unity_user001");
            PrintLog("InAppPurchase.RegisterUserId() -> " + result);
        }

        if (GUI.Button(new Rect(0, y += BUTTON_HEIGHT, scrollContentsWidth, BUTTON_HEIGHT), "Purchase", buttonStyle))
        {
            PrintLog("ToastIapOngate.Purchase()");
            Debug.Log("###");
            // TODO : 입앱 결제 요청
            ToastIapOngate.Purchase(ToastIAPConfig.ITEM_ID, (Result result, object data) =>
            {
                if (!result.IsSuccessful)
                {
                    // TODO : 에러 처리
                    PrintLog("RequestPurchaseCallback.OnCallback() -> Failed! -> " + result.ResultCode + ":" + result.ResultString);
                    return;
                }

                ///  Examples)
                ///  {
                ///	"paymentSeq": "2014082210002092",
                ///	"purchaseToken": "5PYSHgisiCU8BditHnDbPhmlS/0DSt4JDs2UMyg1/EY8oC6Q8qkuw5VBo7GNrBYLNUy656GCAh7h9e1BtXeoBA",
                ///	"itemSeq": 1000001,
                /// "currency": "KRW",
                /// "price": 1000.0
                ///	}

                string json = JsonUtility.ToJson(data);
                PrintLog("RequestPurchaseCallback.OnCallback():" + json);
                Debug.Log("### :" + json);
                // TODO : 결제 결과를 애플리케이션 서버에 전달하여 Consume API 를 통해 검증 이후 아이템을 지급 한다.
            });
        }

        if (GUI.Button(new Rect(0, y += BUTTON_HEIGHT, scrollContentsWidth, BUTTON_HEIGHT), "RequestConsumablePurchases", buttonStyle))
        {
            PrintLog("ToastIapOngate.RequestConsumablePurchases()");

            // TODO : 애플리케이션 실행시 마다 결제내역을 조회 한다.
            ToastIapOngate.RequestConsumablePurchases((Result result, object data) =>
            {
                if (!result.IsSuccessful)
                {
                    PrintLog("QueryPurchasesCallback.OnCallback() -> Failed! -> " + result.ResultCode + ":" + result.ResultString);
                    return;
                }

                /// Examples)				 * 
                /// [{
                ///	"paymentSeq": "2014082510002163",
                ///	"itemSeq": 1000208,
                ///	"purchaseToken": "8nkx3SzHKlI74vmgQLzHExmlS/0DSt4JDs2UMyg1/EY8oC6Q8qkuw5VBo7GNrBYLNUy656GCAh7h9e1BtXeoBA",
                /// "currency": "KRW",
                /// "price": 1000.0
                /// },
                /// {
                ///	"paymentSeq": "2014082510002164",
                ///	"itemSeq": 1000209,
                ///	"purchaseToken": "8nkx3SzATKlI74vmgQLzHExmlS/0DSt4JDs2UMyg1/EY8oC6Q8qkuw5VBo7GNrBYLNUy656GCAh7h9e1BtXeoBA",
                /// "currency": "KRW",
                /// "price": 1000.0
                ///	}]

                string json = System.Convert.ToString(data);
                PrintLog("QueryPurchasesCallback.OnCallback():" + json);

                // TODO : 결제 내역이 존재 하면, 애플리케이션 서버에 전달하여 Consume API를 통해 검증 후 아이템을 지급 한다.

            });
        }

        if (GUI.Button(new Rect(0, y += BUTTON_HEIGHT, scrollContentsWidth, BUTTON_HEIGHT), "RequestProductDetails", buttonStyle))
        {
            PrintLog("ToastIapOngate.RequestProductDetails()");

            //InAppPurchase.SetAppId
            // TODO : 구매 가능한 상품 내역 조회
            ToastIapOngate.RequestProductDetails((Result result, object data) =>
            {
                if (!result.IsSuccessful)
                {
                    PrintLog("QueryItemsCallback.OnCallback() -> Failed! -> " + result.ResultCode + ":" + result.ResultString);
                    return;
                }

                /// Examples)				 *
                /// [{
                ///	"itemSeq": 1000208,
                ///	"itemName": "Test item 01",
                ///	"marketItemId": "item01",
                /// "price": 1000,
                /// "currency": "KRW",
                /// },
                /// {
                ///	"itemSeq": 1000209,
                ///	"itemName": "Test item 02",
                ///	"marketItemId": "item02",
                /// "price": 7.99,
                /// "currency": "USD"
                ///	}]

                string json = System.Convert.ToString(data);
                PrintLog("QueryItemsCallback.OnCallback():" + json);

                // TODO : 상품내역 조회 결과로 필요한 처리를 한다.

            });
        }

        GUI.Label(new Rect(0, y += BUTTON_HEIGHT, scrollContentsWidth, labalHeight), sdkLog, labelStyle);
        GUI.EndScrollView();
    }

    static void PrintLog(string text)
    {
        sdkLog = "IAP] " + text + "\n" + sdkLog;
        sdkLogCount++;
    }
}
