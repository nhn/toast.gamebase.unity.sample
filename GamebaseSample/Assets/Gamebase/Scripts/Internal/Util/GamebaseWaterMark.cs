using UnityEngine;

namespace Toast.Gamebase.Internal
{
    public class GamebaseWaterMark : MonoBehaviour
    {
        private const int WATER_MARK_WIDTH = 300;
        private const int WATER_MARK_HEIGHT = 40;
        private const int OFFSET = 20;
        private const string WATER_MARK_MESSAGE = "Sandbox Environment";
        private GUIStyle guiStyle = null;

        private void Start()
        {
            guiStyle = new GUIStyle();
            guiStyle.normal.textColor = new Color32(128, 128, 128, 128);
            guiStyle.fontSize = 30;
            guiStyle.alignment = TextAnchor.MiddleCenter;
        }

        private void OnGUI()
        {
            if (guiStyle != null)
            {
                GUI.Label(new Rect((Screen.width - WATER_MARK_WIDTH) / 2, Screen.height - WATER_MARK_HEIGHT - OFFSET, WATER_MARK_WIDTH, WATER_MARK_HEIGHT), WATER_MARK_MESSAGE, guiStyle);
            }
        }

        public static void ShowWaterMark()
        {
#if HIDE_WATERMARK
            return;
#endif
            if (Gamebase.IsSandbox() == true)
            {
                GamebaseComponentManager.AddComponent<GamebaseWaterMark>(GamebaseGameObjectManager.GameObjectType.WATER_MARK_TYPE);             
            }
        }
    }
}