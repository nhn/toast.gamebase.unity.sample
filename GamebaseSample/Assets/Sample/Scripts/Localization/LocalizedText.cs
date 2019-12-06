using System.Text;
using UnityEngine;
using UnityEngine.UI;
namespace GamebaseSample
{
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField]
        private string[] keys;

        private void Start()
        {
            LocalizationManager.Instance.AddUpdateTextEvent(UpdateText);
            UpdateText();
        }

        private void OnDestroy()
        {
            LocalizationManager.Instance.RemoveUpdateTextEvent(UpdateText);
        }

        private void UpdateText()
        {
            if (keys == null || keys.Length == 0)
            {
                return;
            }

            Text text = GetComponent<Text>();
            StringBuilder sb = new StringBuilder();
            foreach (var key in keys)
            {
                sb.Append(LocalizationManager.Instance.GetLocalizedValue(key));
            }
            text.text = sb.ToString();

            LayoutRebuilder.ForceRebuildLayoutImmediate(text.rectTransform);
        }

        public void SetKeys(string[] keys)
        {
            this.keys = keys;
            UpdateText();
        }
    }
}