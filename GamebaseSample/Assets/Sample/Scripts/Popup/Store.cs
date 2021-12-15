using Toast.Gamebase;
using UnityEngine;

namespace GamebaseSample
{
    public class Store : MonoBehaviour
    {
        [SerializeField]
        private Purchase[] productList = null;

        private void Start()
        {
            for (int i = 0; i < productList.Length; i++)
            {
                productList[i].gameObject.SetActive(false);
            }

            RequestItemListPurchasable();
        }

        #region UIButton.onClick
        public void ClickCloseButton()
        {
            Destroy(gameObject);
        }
        #endregion

        private void RequestItemListPurchasable()
        {
            Loading.GetInstance().ShowLoading(gameObject);

            Logger.Debug(string.Format("{0}: Gamebase.Purchase.RequestItemListPurchasable", "Begin"), this, "RequestItemListPurchasable");
            Gamebase.Purchase.RequestItemListPurchasable((purchasableItemList, error) =>
            {
                Logger.Debug(string.Format("{0}: Gamebase.Purchase.RequestItemListPurchasable", Gamebase.IsSuccess(error)), this, "RequestItemListPurchasable");
                if (Gamebase.IsSuccess(error) == true)
                {
                    if (purchasableItemList == null)
                    {
                        Logger.Debug("There are no items available for purchase. Register your product in the TOAST Console ", this, "RequestItemListPurchasable");
                        return;
                    }

                    for (int i = 0; i < purchasableItemList.Count; i++)
                    {
                        if (productList.Length > i)
                        {
                            productList[i].SetProduct(purchasableItemList[i]);
                        }
                    }

                    Loading.GetInstance().HideLoading();
                }
                else
                {
                    PopupManager.ShowErrorPopup(
                        gameObject,
                        GameStrings.REQUEST_ITEM_LIST_PURCHASABLE_ERROR,
                        error.ToString(),
                        GameStrings.COMMON_OK_BUTTON,
                        () =>
                        {
                            Destroy(gameObject);
                        });

                    Loading.GetInstance().HideLoading();
                }
            });
        }
    }
}