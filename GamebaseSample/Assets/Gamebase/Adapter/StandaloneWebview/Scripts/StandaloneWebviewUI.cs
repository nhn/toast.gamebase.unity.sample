#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
using UnityEngine;

public class StandaloneWebviewUI : MonoBehaviour
{
    private enum BUTTON_TEXTURE_TYPE
    {
        BUTTON_CLOSE,
        BUTTON_BACK
    }

    private const int       PADDING                     = 9;
    private const int       TITLE_BAR_TEXTURE_WIDTH     = 2;
    private const int       TITLE_BAR_TEXTURE_HEIGHT    = 41;
    private const string    BUTTON_NAME_CLOSE           = "gamebase-close-white";
    private const string    BUTTON_NAME_BACK            = "gamebase-back-white";

    private Texture titleBarTexture;
    private Texture2D[] backTexture;
    private Texture2D[] closeTexture;

    private bool isActiveWebview = false;
    private bool isBackButtonVisible = false;
    private int titleBarHeight = 0;

    private string title;
    private System.Action backCallback;
    private System.Action closeCallback;    
    private string cloasButtonName;
    private string backButtonName;

    

    public int GetTitleBarHeight()
    {
        return titleBarHeight;
    }

    public void HideWebviewUI()
    {
        SetActiveWebview(false);
    }

    public void SetDefault()
    {
        SetTitleBar(0, new Color(75 / 255f, 150 / 255f, 230 / 255f, 1));
        SetBackButtonVisible(true);
        SetButtonTexture(BUTTON_NAME_CLOSE, BUTTON_NAME_BACK);
        SetTitle("");
    }

    public void SetActiveWebview(bool isActiveWebview)
    {
        this.isActiveWebview = isActiveWebview;
    }

    public void SetCallback(System.Action backCallback, System.Action closeCallback)
    {
        this.backCallback = backCallback;
        this.closeCallback = closeCallback;
    }

    public void SetTitle(string title)
    {
        this.title = title;
    }

    public void SetTitleBar(int titleBarHeight, Color color)
    {
        this.titleBarHeight = titleBarHeight;

        Texture2D texture = new Texture2D(TITLE_BAR_TEXTURE_WIDTH, TITLE_BAR_TEXTURE_HEIGHT);

        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
        titleBarTexture = texture;

        if (titleBarTexture.height > titleBarHeight)
        {
            this.titleBarHeight = titleBarTexture.height;
        }
    }

    public void SetButtonTexture(string cloasButtonName, string backButtonName)
    {               
        if (false == string.IsNullOrEmpty(cloasButtonName))
        {
            closeTexture = LoadButtonTexture(BUTTON_TEXTURE_TYPE.BUTTON_CLOSE, cloasButtonName, this.cloasButtonName);
        }
        else
        {
            closeTexture = LoadButtonTexture(BUTTON_TEXTURE_TYPE.BUTTON_CLOSE, BUTTON_NAME_CLOSE, this.cloasButtonName);
        }
        
        if (false == string.IsNullOrEmpty(backButtonName))
        {
            backTexture = LoadButtonTexture(BUTTON_TEXTURE_TYPE.BUTTON_CLOSE, backButtonName, this.backButtonName);
        }
        else
        {
            backTexture = LoadButtonTexture(BUTTON_TEXTURE_TYPE.BUTTON_CLOSE, BUTTON_NAME_BACK, this.backButtonName);
        }
    }

    private Texture2D[] LoadButtonTexture(BUTTON_TEXTURE_TYPE buttonType, string textureName, string cachingTextureName = null)
    {
        Texture2D[] buttonTexture = new Texture2D[3];
        Texture2D buttonSource;

        switch(buttonType)
        {
            case BUTTON_TEXTURE_TYPE.BUTTON_BACK:
                {
                    if(true == textureName.Equals(cloasButtonName))
                    {
                        return closeTexture;
                    }
                    cloasButtonName = textureName;
                    break;
                }
            case BUTTON_TEXTURE_TYPE.BUTTON_CLOSE:
                {
                    if (true == textureName.Equals(backButtonName))
                    {
                        return backTexture;
                    }
                    backButtonName = textureName;
                    break;
                }                
        }
 
        buttonSource = Resources.Load(textureName) as Texture2D;        

        buttonTexture[0] = buttonSource;
        buttonTexture[1] = AddTextureColor(buttonSource, true);
        buttonTexture[2] = AddTextureColor(buttonSource, false);

        return buttonTexture;
    }

    private Texture2D AddTextureColor(Texture2D targetTexture, bool isAdd)
    {
        Texture2D source = new Texture2D(targetTexture.width, targetTexture.height);
        Color targetColor = new Color(0.2f, 0.2f, 0.2f, 0f);

        for (int y = 0; y < source.height; y++)
        {
            for (int x = 0; x < source.width; x++)
            {
                Color color = targetTexture.GetPixel(x, y);

                if (isAdd)
                {
                    color += targetColor;
                }
                else
                {
                    color -= targetColor;
                }

                source.SetPixel(x, y, color);
            }
        }
        source.Apply();
        return source;
    }

    public void SetBackButtonVisible(bool isBackButtonVisible)
    {
        this.isBackButtonVisible = isBackButtonVisible;
    }

    void OnGUI()
    {
        if (false == isActiveWebview)
        {
            return;
        }

        GUI.DrawTexture(new Rect(0, 0, Screen.width, titleBarHeight), titleBarTexture);

        ShowTitle();
        ShowCloseButton();
        ShowBackButton();
    }

    private void ShowTitle()
    {
        if (true == string.IsNullOrEmpty(title))
        {
            return;
        }

        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.white;
        style.fontSize = 20;
        style.alignment = TextAnchor.MiddleCenter;

        int textWidth = Screen.width - (PADDING * 2);
        Vector2 textVector = style.CalcSize(new GUIContent(title));

        int textTop = (titleBarHeight - style.fontSize) / 2;

        string titleText = "";

        if (textVector.x > textWidth)
        {
            for (int i = 0; i < title.Length; i++)
            {
                Vector2 size = style.CalcSize(new GUIContent(title.Substring(0, i) + "..."));
                if (size.x > textWidth)
                {
                    break;
                }
                titleText = title.Substring(0, i) + "...";
            }
        }
        else
        {
            titleText = title;
        }

        GUI.Label(
            new Rect(PADDING, textTop, textWidth, 20),
            titleText,
            style
            );
    }

    private void ShowCloseButton()
    {
        GUIStyle guiStyle = new GUIStyle();
        guiStyle.normal.background = closeTexture[0];
        guiStyle.hover.background = closeTexture[1];
        guiStyle.active.background = closeTexture[2];

        int buttonTop = (titleBarHeight - closeTexture[0].height) / 2;

        if (true == GUI.Button(new Rect(Screen.width - closeTexture[0].width - PADDING, buttonTop, closeTexture[0].width, closeTexture[0].height), "", guiStyle))
        {
            if (null != closeCallback)
            {
                closeCallback();
            }
        }
    }

    public void ShowBackButton()
    {
        if (true != isBackButtonVisible || null == backCallback)
        {
            return;
        }

        GUIStyle guiStyle = new GUIStyle();
        guiStyle.normal.background = backTexture[0];
        guiStyle.hover.background = backTexture[1];
        guiStyle.active.background = backTexture[2];

        int buttonTop = (titleBarHeight - backTexture[0].height) / 2;

        if (true == GUI.Button(new Rect(PADDING, buttonTop, backTexture[0].width, backTexture[0].height), "", guiStyle))
        {
            if (null != backCallback)
            {
                backCallback();
            }
        }
    }
}
#endif