using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GamebaseSample
{
    public class Ingame : MonoBehaviour
    {
        public static IngameAgent Agent;

        public GameObject StartPointObject;
        public GameObject SpawnAreaObject;
        public GameObject PlayingAreaObject;

        public PlayerRocket Player;

        public Text uiScore;
        public Text uiTimer;

        public GameObject uiResult;
        public Text uiResultScore;

        private void Awake()
        {
            Agent = new IngameAgent(this);
            Agent.Start();
        }

        private void OnDestroy()
        {
            Time.timeScale = 1f;
            Agent = null;
        }

        private void Update()
        {
            Agent.Update();

            uiScore.text = Agent.GameScore.ToString();
            uiTimer.text = Agent.PlayingTimerString;
        }

        public void Pause()
        {
            Agent.Pause();

            PopupManager.ShowCommonPopup(
                gameObject,
                PopupManager.CommonPopupType.SMALL_SIZE,
                LocalizationManager.Instance.GetLocalizedValue(GameStrings.GAME_EXIT_TITLE),
                LocalizationManager.Instance.GetLocalizedValue(GameStrings.GAME_EXIT_MESSAGE),
                LocalizationManager.Instance.GetLocalizedValue(GameStrings.GAME_EXIT),
                () => { Agent.Finish(); },
                LocalizationManager.Instance.GetLocalizedValue(GameStrings.GAME_CONTINUE),
                () => { Agent.Continue(); });
        }

        public void PrintResult()
        {
            uiResultScore.text = Agent.GameScore.ToString();
            uiResult.transform.SetAsLastSibling();
            uiResult.SetActive(true);
        }

        public void Exit()
        {
            SceneManager.LoadSceneAsync("main");
        }
    }
}