using System;
using System.Collections.Generic;
using Toast.Internal;
using UnityEngine;

namespace Toast.Iap.Ongate
{
    public class StandaloneWebGLPlugin : ScriptableObject, IAPService
    {
        private string userId;
        private string userChannel = "GF";
        private string location;
        private bool _isCachedItemInfo = false;
        private Dictionary<string, long> _itemInfo = new Dictionary<string, long>(); // key : itemId, value : itemseq

        private StandaloneWebGLIapService iapService;

        public StandaloneWebGLPlugin()
        {
            iapService = new StandaloneWebGLServiceImpl();
        }

        public void AsyncProcessesIncompletePurchases(IAPCallbackHandler.OnResponsePurchase callback)
        {
            DebugUtil.Log("Unsupported platform");
            //throw new NotImplementedException();
        }

        public void RequestProductDetails(IAPCallbackHandler.OnResponsePurchase callback)
        {
            long appSeq = StandaloneWebGLResourceManager.Instance.AppId;
            DebugUtil.Log(String.Format("### QUERY_ITEMLIST - AppID:{0} ###", appSeq));
            iapService.FindItems(appSeq, (result, data) =>
            {
                if (result.isSuccessful)
                {
                    _itemInfo.Clear();

                    string jsonString = System.Convert.ToString(data);
                    JSONArray jsonArray = JSONNode.Parse(jsonString).AsArray;
                    if (jsonArray.Count > 0)
                    {
                        foreach (JSONNode item in jsonArray)
                        {
                            _itemInfo.Add(item["marketItemId"], item["itemSeq"]);
                        }
                    }

                    _isCachedItemInfo = true;
                }

                if (callback != null)
                {
                    callback(result, data);
                }
            });
        }

        public void RequestConsumablePurchases(IAPCallbackHandler.OnResponsePurchase callback)
        {
            DebugUtil.Log("### QUERY_PURCHASES ###");
            long appSeq = StandaloneWebGLResourceManager.Instance.AppId;

            IapConsumableRequest consumableRequest = new IapConsumableRequest(appSeq, userChannel, userId);
            DebugUtil.Log(" ### Consumable Request : " + JsonUtility.ToJson(consumableRequest));
            iapService.FindConsumableItems(consumableRequest, callback);
        }

        public void Purchase(string itemId, IAPCallbackHandler.OnResponsePurchase callback)
        {
            if (_isCachedItemInfo == false)
            {
                RequestProductDetails((Result res, object data) =>
                {
                    if (_isCachedItemInfo == false)
                    {
                        Result result = new Result(false, res.resultCode, res.resultString);
                        callback(result, null);
                        return;
                    }
                    else
                    {
                        PurchaseItemId(itemId, callback);
                    }
                });

                return;
            }

            PurchaseItemId(itemId, callback);
        }

        private void PurchaseItemId(string itemId, IAPCallbackHandler.OnResponsePurchase callback)
        {
            if (string.IsNullOrEmpty(itemId))
            {
                Result result = new Result(false, 998, "Product ID MUST NOT be null or empty!!");
                callback(result, null);
                return;
            }

            if (_itemInfo.ContainsKey(itemId) == false)
            {
                Result result = new Result(false, 999, "Purchase request is invliad. Please confirm your itemId.");
                callback(result, null);
                return;
            }

            long itemSeq = _itemInfo[itemId];
            DebugUtil.Log(String.Format("### REQUEST_WEBGL_PURCHASE - ITEM ID:{0} ###", itemId));

            long appSeq = StandaloneWebGLResourceManager.Instance.AppId;
            IapReserveRequest reserveRequest = new IapReserveRequest(appSeq, userChannel, userId, itemSeq, LocationUtil.Location);
            DebugUtil.Log(" ### Reserve Request : " + JsonUtility.ToJson(reserveRequest));

            iapService.Reserve(reserveRequest, (result, data) =>
            {
                if (!result.isSuccessful)
                {
                    callback(result, data);
                }
                else
                {
                    DebugUtil.Log("### Reserve Reponse :" + JsonUtility.ToJson(data));
                    IapApiResultOfReserve response = ((IapApiBaseResult<IapApiResultOfReserve>)data).result;

                    IapVerifyRequest verifyRequest = new IapVerifyRequest(response.paymentSeq, userId, userChannel);
                    DebugUtil.Log("### Verify Request :" + JsonUtility.ToJson(verifyRequest));
                    iapService.Verify(verifyRequest, callback);
                }
            });
        }

        public bool SetOngateUserId(string userId)
        {
            this.userId = userId;
            this.location = LocationUtil.Location;
            DebugUtil.Log(String.Format("### userId : {0}, location : {1}", userId, location));

            return true;
        }

        public void SetDebugMode(bool isDebuggable)
        {
            DebugUtil.EnableDebug = isDebuggable;
        }
    }
}