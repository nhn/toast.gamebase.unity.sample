using System;

namespace Toast.Iap.Ongate
{
    internal interface IIapApiUriMapper
    {
        string GetUrlForIapEvent(ApiType apiType);
    }

    internal interface IStandaloneWebGlResourceInitializer
    {
        bool SetAppId(long appId);
    }

    public class StandaloneWebGLResourceManager : IIapApiUriMapper, IStandaloneWebGlResourceInitializer
    {
        private static readonly StandaloneWebGLResourceManager instance = new StandaloneWebGLResourceManager();
        public static StandaloneWebGLResourceManager Instance
        {
            get { return instance; }
        }

        private ServerPhase serverPhase = ServerPhase.REAL;
        public ServerPhase ServerPhase
        {
            get { return this.serverPhase; }
            set { this.serverPhase = value; }
        }

        private long appId;
        public long AppId { get { return appId; } }

        public bool SetAppId(long appId)
        {
            this.appId = appId;
            return true;
        }

        public string GetUrlForIapEvent(ApiType apiType)
        {
            ApiServer server = null;
            switch (serverPhase)
            {
                case ServerPhase.ALPHA:
                    server = new Alpha();
                    break;
                case ServerPhase.BETA:
                    server = new Beta();
                    break;
                case ServerPhase.REAL:
                    server = new Real();
                    break;
                case ServerPhase.LOCAL:
                    server = new Local();
                    break;
                default:
                    throw new NotImplementedException();
            }

            switch (apiType)
            {
                case ApiType.RESERVE:
                    return server.API_RESERVE_ONGATE;
                case ApiType.VERIFY:
                    return server.API_VERIFY_ONGATE;
                case ApiType.ITEM_LIST:
                    return server.API_GET_ITEM_LIST;
                case ApiType.CONSUMABLE_LIST:
                    return server.API_GET_CONSUMABLE_LIST;
                default:
                    throw new NotImplementedException();
            }
        }

        private abstract class ApiServer
        {
            public abstract string API_RESERVE_ONGATE { get; }
            public abstract string API_VERIFY_ONGATE { get; }
            public abstract string API_GET_ITEM_LIST { get; }
            public abstract string API_GET_CONSUMABLE_LIST { get; }
        }

        private class Alpha : ApiServer
        {
            private const string domain = "https://alpha-api-iap.cloud.toast.com";
            public override string API_RESERVE_ONGATE
            {
                get { return domain + "/reserve/ONGATE"; }
            }
            public override string API_VERIFY_ONGATE
            {
                get { return domain + "/verify/ONGATE"; }
            }
            public override string API_GET_ITEM_LIST
            {
                get { return domain + "/item/list"; }
            }
            public override string API_GET_CONSUMABLE_LIST
            {
                get { return domain + "/standard/inapp/v1/consumable/list"; }
            }
        }

        private class Beta : ApiServer
        {
            private const string domain = "https://beta-api-iap.cloud.toast.com";
            public override string API_RESERVE_ONGATE
            {
                get { return domain + "/reserve/ONGATE"; }
            }
            public override string API_VERIFY_ONGATE
            {
                get { return domain + "/verify/ONGATE"; }
            }
            public override string API_GET_ITEM_LIST
            {
                get { return domain + "/item/list"; }
            }
            public override string API_GET_CONSUMABLE_LIST
            {
                get { return domain + "/standard/inapp/v1/consumable/list"; }
            }
        }

        private class Real : ApiServer
        {
            private const string domain = "https://api-iap.cloud.toast.com";
            public override string API_RESERVE_ONGATE
            {
                get { return domain + "/reserve/ONGATE"; }
            }
            public override string API_VERIFY_ONGATE
            {
                get { return domain + "/verify/ONGATE"; }
            }
            public override string API_GET_ITEM_LIST
            {
                get { return domain + "/item/list"; }
            }
            public override string API_GET_CONSUMABLE_LIST
            {
                get { return domain + "/standard/inapp/v1/consumable/list"; }
            }
        }

        private class Local : ApiServer
        {
            private const string domain = "https://local-api-iap.cloud.toast.com";
            public override string API_RESERVE_ONGATE
            {
                get { return domain + "/reserve/ONGATE"; }
            }
            public override string API_VERIFY_ONGATE
            {
                get { return domain + "/verify/ONGATE"; }
            }
            public override string API_GET_ITEM_LIST
            {
                get { return domain + "/item/list"; }
            }
            public override string API_GET_CONSUMABLE_LIST
            {
                get { return domain + "/standard/inapp/v1/consumable/list"; }
            }
        }
    }
}