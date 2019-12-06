using System.Collections.Generic;
using UnityEngine;

namespace GamebaseSample
{
    public class DownloadResourceView : MonoBehaviour
    {
        [SerializeField]
        private RectTransform ItemPrefab;
        [SerializeField]
        private Transform ContentRoot;

        private Dictionary<int, DownloadResourceViewItem> progressBars = new Dictionary<int, DownloadResourceViewItem>();

        private int currentaIndex = 0;

        public int ThreadCount { get { return progressBars.Count; } set { Invalidate(value); } }

        public DownloadResourceViewItem this[int index] { get { return progressBars[index]; } }

        protected void OnAwake()
        {
            progressBars.Add(currentaIndex++, ItemPrefab.GetComponentInChildren<DownloadResourceViewItem>());
        }

        private void Invalidate(int newCount)
        {
            if (newCount == progressBars.Count || newCount <= 0)
            {
                return;
            }

            var count = progressBars.Count;
            if (newCount > count)
            {
                var createCount = newCount - count;
                for (int i = 0; i < createCount; i++)
                {
                    AddThreadProgress();
                }
            }
            else if (newCount < count)
            {
                var deleteCount = count - newCount;
                for (int i = 0; i < deleteCount; i++)
                {
                    RemoveThreadProgress();
                }
            }
        }

        private void AddThreadProgress()
        {
            var newObject = Instantiate(ItemPrefab, ContentRoot, true);
            newObject.transform.localPosition = Vector3.zero;
            newObject.transform.localScale = Vector3.one;

            var progressBar = newObject.GetComponentInChildren<DownloadResourceViewItem>();
            if (progressBar == null)
            {
                Debug.LogError("Failed to found the ProgressBar");
                return;
            }

            progressBars.Add(currentaIndex++, progressBar);
        }

        private void RemoveThreadProgress()
        {
            var index = --currentaIndex;
            Destroy(progressBars[index].gameObject);
            progressBars.Remove(index);
        }
    }
}