using UnityEngine;
using UnityEngine.UI;

namespace GamebaseSample
{
    public class DownloadResourceViewItem : MonoBehaviour
    {
        [SerializeField]
        private Text filenameText;
        [SerializeField]
        private Text downloadStatusText;
        [SerializeField]
        private Progressbar downloadProgress;

        public string FileName { get { return filenameText.text; } set { filenameText.text = value; } }

        public void ProgressStatus(float percent)
        {
            downloadStatusText.text = string.Format("{0}%", Mathf.FloorToInt(percent));
            downloadProgress.Percentage = percent;
        }
    }
}