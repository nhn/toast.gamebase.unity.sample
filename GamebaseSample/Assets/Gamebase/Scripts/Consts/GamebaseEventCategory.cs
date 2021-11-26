namespace Toast.Gamebase
{
    public static class GamebaseEventCategory
    {
        public const string SERVER_PUSH_APP_KICKOUT = "serverPushAppKickout";
        public const string SERVER_PUSH_TRANSFER_KICKOUT = "serverPushTransferKickout";
        public const string OBSERVER_LAUNCHING = "observerLaunching";
        public const string OBSERVER_NETWORK = "observerNetwork";
        public const string OBSERVER_HEARTBEAT = "observerHeartbeat";
        public const string OBSERVER_WEBVIEW = "observerWebview";// Unity standalone only
        public const string OBSERVER_INTROSPECT = "observerIntrospect";// Unity standalone only
        public const string PURCHASE_UPDATED = "purchaseUpdated";
        public const string PUSH_RECEIVED_MESSAGE = "pushReceivedMessage";
        public const string PUSH_CLICK_MESSAGE = "pushClickMessage";
        public const string PUSH_CLICK_ACTION = "pushClickAction";
    }
}
