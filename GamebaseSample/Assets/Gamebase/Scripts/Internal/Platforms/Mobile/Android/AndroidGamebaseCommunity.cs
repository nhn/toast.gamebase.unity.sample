﻿#if UNITY_EDITOR || UNITY_ANDROID
namespace Toast.Gamebase.Internal.Mobile.Android
{
    public class AndroidGamebaseCommunity : NativeGamebaseCommunity
    {
        override protected void Init()
        {
            CLASS_NAME = "com.toast.android.gamebase.plugin.GamebaseCommunityPlugin";
            messageSender = AndroidMessageSender.Instance;

            base.Init();
        }
    }
}
#endif