namespace Toast.Gamebase
{
    public static class GamebaseEventCategory
    {
        public const string LOGGED_OUT = "loggedOut";
        public const string IDP_REVOKED = "idPRevoked";
        public const string SERVER_PUSH_APP_KICKOUT = "serverPushAppKickout";
        public const string SERVER_PUSH_APP_KICKOUT_MESSAGE_RECEIVED = "serverPushAppKickoutMessageReceived";
        public const string SERVER_PUSH_TRANSFER_KICKOUT = "serverPushTransferKickout";
        public const string OBSERVER_LAUNCHING = "observerLaunching";
        public const string OBSERVER_NETWORK = "observerNetwork";
        public const string OBSERVER_HEARTBEAT = "observerHeartbeat";
        public const string PURCHASE_UPDATED = "purchaseUpdated";
        public const string PUSH_RECEIVED_MESSAGE = "pushReceivedMessage";
        public const string PUSH_CLICK_MESSAGE = "pushClickMessage";
        public const string PUSH_CLICK_ACTION = "pushClickAction";

        //------------------------------
        //  Unity standalone only
        //------------------------------
        public const string OBSERVER_WEBVIEW = "observerWebview";
        public const string OBSERVER_INTROSPECT = "observerIntrospect";
    }
}
