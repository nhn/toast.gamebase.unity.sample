using System;
using System.Collections.Generic;
using UnityEngine;

namespace Toast.Iap.Ongate
{
    public interface IapApiResult
    {
        bool IsSuccess();
        int GetCode();
        string GetMessage();
    }

    [Serializable]
    public class IapApiBaseResult<T> : IapApiResult
    {
        public int code;
        public string message;
        public T result;

        public int GetCode()
        {
            return this.code;
        }

        public string GetMessage()
        {
            return this.message;
        }

        public bool IsSuccess()
        {
            return (this.code == 0);
        }
    }

    [Serializable]
    public class IapApiStandardBaseResult<T> : IapApiResult
    {
        public Header header;
        public T result;

        public int GetCode()
        {
            return this.header.resultCode;
        }

        public string GetMessage()
        {
            return this.header.resultMessage;
        }

        public bool IsSuccess()
        {
            return this.header.isSuccessful;
        }

        [Serializable]
        public class Header
        {
            public bool isSuccessful;
            public int resultCode;
            public String resultMessage;
        }
    }

    [Serializable]
    public class IapApiResultConsumableList : IapApiResult
    {
        public Header header;
        public List<IapApiResultOfConsumable> result;

        public int GetCode()
        {
            return this.header.resultCode;
        }

        public string GetMessage()
        {
            return this.header.resultMessage;
        }

        public bool IsSuccess()
        {
            return this.header.isSuccessful;
        }

        [Serializable]
        public class Header
        {
            public bool isSuccessful;
            public int resultCode;
            public String resultMessage;
        }
    }

    [Serializable]
    public class IapApiResultOfReserve
    {
        public string marketId;
        public string paymentSeq;
        public string marketItemId;
    }

    [Serializable]
    public class IapApiResultOfVerify
    {
        public string paymentSeq;
        public long itemSeq;
        public string purchaseToken;
        public string currency;
        public float price;
    }

    [Serializable]
    public class IapApiResultOfItemList
    {
        public String appUsingStatus;
        public String marketAppId;
        public String appSeq;
        [SerializeField]
        public List<Item> itemList;

        [Serializable]
        public class Item
        {
            public int itemSeq;
            public String itemName;
            public String marketItemId;
            public String usingStatus;
            public String regYmdt;
            public String appName;
            public String marketId;
            public double price;
            public String currency;
        }
    }

    [Serializable]
    public class IapApiResultOfConsumable
    {
        public String paymentSeq;
        public int itemSeq;
        public String currency;
        public float price;
        public String purchaseToken;
    }

    public class IapReserveRequest
    {
        #region required
        public long appSeq;
        public string userChannel;
        public string userKey;
        public long itemSeq;
        public string location;
        #endregion

        #region optional
        public string currency;
        public float price;
        public string title;
        public string marketAddInfo;
        #endregion

        public IapReserveRequest(
            long appSeq,
            string userChannel,
            string userKey,
            long itemSeq,
            string location,
            string currency = "N/A",
            float price = 0.0f,
            string title = "",
            string marketAddInfo = ""
            )
        {
            this.appSeq = appSeq;
            this.userChannel = userChannel;
            this.userKey = userKey;
            this.itemSeq = itemSeq;
            this.location = location;
            this.currency = currency;
            this.price = price;
            this.title = title;
            this.marketAddInfo = marketAddInfo;
        }
    }

    public class IapVerifyRequest
    {
        #region required
        public string paymentSeq;
        public string userKey;
        public string userChannel;
        public string currency;
        public float price;
        #endregion

        public IapVerifyRequest(
            string paymentSeq,
            string userKey,
            string userChannel,
            string currency = "N/A",
            float price = 0.0f
            )
        {
            this.paymentSeq = paymentSeq;
            this.userKey = userKey;
            this.userChannel = userChannel;
            this.currency = currency;
            this.price = price;
        }
    }

    public class IapConsumableRequest
    {
        #region required
        public long appSeq;
        public string userChannel;
        public string userKey;
        #endregion

        public IapConsumableRequest(
            long appSeq,
            string userChannel,
            string userKey
            )
        {
            this.appSeq = appSeq;
            this.userChannel = userChannel;
            this.userKey = userKey;
        }
    }
}