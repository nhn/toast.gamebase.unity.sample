namespace Toast.Iap.Ongate
{
    /// <summary>
    /// Enum Of Server Phase.
    /// </summary>
    public enum ServerPhase
    {
        LOCAL,
        ALPHA,
        BETA,
        REAL
    }

    /// <summary>
    /// Iap Server Api Type
    /// </summary>
    public enum ApiType
    {
        RESERVE,
        VERIFY,
        ITEM_LIST,
        CONSUMABLE_LIST
    }
}
