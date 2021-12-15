using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

#if UNITY_2017_2_OR_NEWER
using UnityEngine.Networking;
#endif  // UNITY_2017_2_OR_NEWER

namespace Toast.Iap.Ongate
{
    public interface StandaloneWebGLIapService
    {
        void Reserve(IapReserveRequest reserveRequest, IAPCallbackHandler.OnResponsePurchase callback);

        void Verify(IapVerifyRequest verifyRequest, IAPCallbackHandler.OnResponsePurchase callback);

        void FindConsumableItems(IapConsumableRequest consumableRequest, IAPCallbackHandler.OnResponsePurchase callback);

        void FindItems(long appSeq, IAPCallbackHandler.OnResponsePurchase callback);
    }

    public class StandaloneWebGLServiceImpl : StandaloneWebGLIapService
    {
        private float timeout = 15;

        private IEnumerator SendPost<T>(string url, byte[] pData, IAPCallbackHandler.OnResponsePurchase callback) where T : IapApiResult
        {
            DebugUtil.Log("### SendPost URL : " + url);
            #pragma warning disable 0219
            bool isTimeout = false;
            #pragma warning restore 0219
            string errorString = "";
            string jsonString = "";

#if UNITY_2017_2_OR_NEWER
            var downloadHandler = new DownloadHandlerBuffer();
            var uploadHandler = new UploadHandlerRaw(pData);
            using (var request = new UnityWebRequest(url,
                UnityWebRequest.kHttpVerbPOST,
                downloadHandler, uploadHandler
                ))
            {
                request.timeout = System.Convert.ToInt32(timeout);
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                errorString = request.error;
#if UNITY_2020_1_OR_NEWER
                    if (request.result == UnityWebRequest.Result.ConnectionError ||
                    request.result == UnityWebRequest.Result.ProtocolError)
                    {
                        isTimeout = true;
                    }
#else
                if (request.isNetworkError || request.isHttpError)
                {
                    isTimeout = true;
                }
#endif
                else
                {
                    jsonString = request.downloadHandler.text;
                }
            }
#else
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");

            WWW www = new WWW(url, pData, headers);
            float timer = 0;

            do
            {
                if (timer > timeout)
                {
                    isTimeout = true;
                    break;
                }
                timer += Time.deltaTime;

                yield return null;
            }
            while (!www.isDone);

            if (isTimeout)
            {
                www.Dispose();
            }
            else
            {
                errorString = www.error;
                jsonString = www.text;
            }
#endif  // UNITY_2017_2_OR_NEWER

            if (isTimeout)
            {
                DebugUtil.Log("### TimeOut SendPost : " + url);
                callback(ResultFactory.CreateErrorResult(IAPErrorType.NETWORK_TIMEOUT_ERROR), null);
                yield break;
            }

            if (string.IsNullOrEmpty(errorString) == false)
            {
                callback(ResultFactory.CreateErrorResult(IAPErrorType.INAPP_SERVER_NETWORK_FAIL), null);
                yield break;
            }

            //성공
            DebugUtil.Log("### Response From Server By SendPost : " + jsonString);
            T response = JsonUtility.FromJson<T>(jsonString);
            callback(ResultFactory.CreateResultFromServerResponse(response), response);
        }

        private IEnumerator SendGet<T>(string url, IAPCallbackHandler.OnResponsePurchase callback) where T : IapApiResult
        {
            DebugUtil.Log("### SendGet URL : " + url);
            #pragma warning disable 0219
            bool isTimeout = false;
            #pragma warning restore 0219
            string errorString = "";
            string jsonString = "";

#if UNITY_2017_2_OR_NEWER            
            using (var request = UnityWebRequest.Get(url))
            {
                request.timeout = System.Convert.ToInt32(timeout);

                yield return request.SendWebRequest();

                errorString = request.error;
#if UNITY_2020_1_OR_NEWER
                    if (request.result == UnityWebRequest.Result.ConnectionError ||
                    request.result == UnityWebRequest.Result.ProtocolError)
                    {
                        isTimeout = true;
                    }
#else
                if (request.isNetworkError || request.isHttpError)
                {
                    isTimeout = true;
                }
#endif
                else
                {
                    jsonString = request.downloadHandler.text;
                }
            }
#else
            WWW www = new WWW(url);
            float timer = 0;
            
            do
            {
                if (timer > timeout)
                {
                    isTimeout = true;
                    break;
                }
                timer += Time.deltaTime;

                yield return null;
            }
            while (!www.isDone);

            if (isTimeout)
            {
                www.Dispose();
            }
            else
            {
                errorString = www.error;
                jsonString = www.text;
            }

#endif  // UNITY_2017_2_OR_NEWER

            if (isTimeout)
            {
                DebugUtil.Log("### TimeOut SendGet : " + url);
                callback(ResultFactory.CreateErrorResult(IAPErrorType.NETWORK_TIMEOUT_ERROR), null);
                yield break;
            }

            if (string.IsNullOrEmpty(errorString) == false)
            {
                callback(ResultFactory.CreateErrorResult(IAPErrorType.INAPP_SERVER_NETWORK_FAIL), null);
                yield break;
            }

            //성공
            DebugUtil.Log("### Response From Server By SendGet : " + jsonString);
            T response = JsonUtility.FromJson<T>(jsonString);
            callback(ResultFactory.CreateResultFromServerResponse(response), response);
        }

        public void Reserve(IapReserveRequest reserveRequest, IAPCallbackHandler.OnResponsePurchase callback)
        {
            string url = StandaloneWebGLResourceManager.Instance.GetUrlForIapEvent(ApiType.RESERVE);
            byte[] pData = Encoding.ASCII.GetBytes(JsonUtility.ToJson(reserveRequest).ToCharArray());

            IAPManager.Instance.StartCoroutine(SendPost<IapApiBaseResult<IapApiResultOfReserve>>(url, pData, callback));
        }

        public void Verify(IapVerifyRequest verifyRequest, IAPCallbackHandler.OnResponsePurchase callback)
        {
            string url = StandaloneWebGLResourceManager.Instance.GetUrlForIapEvent(ApiType.VERIFY);
            byte[] pData = Encoding.ASCII.GetBytes(JsonUtility.ToJson(verifyRequest).ToCharArray());

            IAPManager.Instance.StartCoroutine(SendPost<IapApiBaseResult<IapApiResultOfVerify>>(url, pData, (result, data) =>
            {
                if (result.isSuccessful)
                {
                    IapApiResultOfVerify res = ((IapApiBaseResult<IapApiResultOfVerify>)data).result;
                    callback(result, JsonUtility.ToJson(res));
                }
                else
                {
                    callback(result, data);
                }
            }));
        }

        public void FindItems(long appSeq, IAPCallbackHandler.OnResponsePurchase callback)
        {
            string url = StandaloneWebGLResourceManager.Instance.GetUrlForIapEvent(ApiType.ITEM_LIST) + "/" + appSeq;

            IAPManager.Instance.StartCoroutine(SendGet<IapApiBaseResult<IapApiResultOfItemList>>(url, (result, data) =>
            {
                if (result.isSuccessful)
                {
                    IapApiResultOfItemList res = ((IapApiBaseResult<IapApiResultOfItemList>)data).result;
                    List<IapApiResultOfItemList.Item> items = res.itemList;
                    callback(result, JsonUtilityHelper.ArrayToJson<IapApiResultOfItemList.Item>(items.ToArray()));
                }
                else
                {
                    callback(result, data);
                }

            }));
        }

        public void FindConsumableItems(IapConsumableRequest consumableRequest, IAPCallbackHandler.OnResponsePurchase callback)
        {
            string url = StandaloneWebGLResourceManager.Instance.GetUrlForIapEvent(ApiType.CONSUMABLE_LIST);
            byte[] pData = Encoding.ASCII.GetBytes(JsonUtility.ToJson(consumableRequest).ToCharArray());

            IAPManager.Instance.StartCoroutine(SendPost<IapApiResultConsumableList>(url, pData, (result, data) =>
            {
                if (result.isSuccessful)
                {
                    List<IapApiResultOfConsumable> res = ((IapApiResultConsumableList)data).result;
                    callback(result, JsonUtilityHelper.ArrayToJson<IapApiResultOfConsumable>(res.ToArray()));
                }
                else
                {
                    callback(result, data);
                }
            }));
        }

    }
}