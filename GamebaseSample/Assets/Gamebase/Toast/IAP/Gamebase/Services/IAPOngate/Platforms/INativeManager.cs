namespace Toast.Iap.Ongate
{
    public interface INativeManager
    {
        void Init();
        IAPService NativePlugin { get; }
        void OnNativeMessage(string jsonResult);
    }
}