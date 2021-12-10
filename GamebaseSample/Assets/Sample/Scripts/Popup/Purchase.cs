using Toast.Gamebase;
using UnityEngine;
using UnityEngine.UI;

namespace GamebaseSample
{
    public class Purchase : MonoBehaviour
    {
        [SerializeField]
        private Text nameText;
        [SerializeField]
        private Text descText;
        [SerializeField]
        private Text priceText;

        private GamebaseResponse.Purchase.PurchasableItem purchasableItem;

        public void SetProduct(GamebaseResponse.Purchase.PurchasableItem purchasableItem)
        {
            this.purchasableItem = purchasableItem;
            nameText.text = purchasableItem.itemName;
            descText.text = "Desc";
            priceText.text = string.Format("{0} {1:#,###.##}", purchasableItem.currency, purchasableItem.price);

            gameObject.SetActive(true);

            Logger.Debug(string.Format("itemName:{0}", purchasableItem.itemName), this);
        }

        #region UIButton.onClick	
        public void ClickBuyButton()
        {
            PopupManager.ShowCommonPopup(
                transform.parent.parent.gameObject,
                PopupManager.CommonPopupType.SMALL_SIZE,
                LocalizationManager.Instance.GetLocalizedValue(GameStrings.PURCHASE_TITLE),
                LocalizationManager.Instance.GetLocalizedValue(GameStrings.PURCHASE_BUY_CONTEXT),
                LocalizationManager.Instance.GetLocalizedValue(GameStrings.PURCHASE_TITLE),
                () => { RequestPurchase(purchasableItem.gamebaseProductId); },
                LocalizationManager.Instance.GetLocalizedValue(GameStrings.COMMON_CANCEL_BUTTON),
                null);
        }
        #endregion

        private void RequestPurchase(string gamebaseProductId)
        {
            Loading.GetInstance().ShowLoading(gameObject);

            Gamebase.Purchase.RequestPurchase(gamebaseProductId, (purchasableReceipt, error) =>
            {
                Loading.GetInstance().HideLoading();

                if (Gamebase.IsSuccess(error) == true)
                {
                    PopupManager.ShowCommonPopup(
                        transform.parent.parent.gameObject,
                        PopupManager.CommonPopupType.SMALL_SIZE,
                        LocalizationManager.Instance.GetLocalizedValue(GameStrings.PURCHASE_TITLE),
                        string.Format("{0}\n{1}", LocalizationManager.Instance.GetLocalizedValue(GameStrings.PURCHASE_SUCCESS_CONTEXT), purchasableItem.itemName),
                        LocalizationManager.Instance.GetLocalizedValue(GameStrings.COMMON_OK_BUTTON),
                        null);
                }
                else
                {
                    PopupManager.ShowErrorPopup(
                        transform.parent.parent.gameObject,
                        GameStrings.REQUEST_PURCHASE_ERROR,
                        error.ToString(),
                        GameStrings.COMMON_OK_BUTTON,
                        null);
                }
            });
        }
    }
}