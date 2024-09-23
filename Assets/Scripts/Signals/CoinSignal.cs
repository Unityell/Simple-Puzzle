public class CoinSignal
{
    public readonly int CoinCount;
    public readonly EnumCoinAction Action;

    public CoinSignal(int CoinCount, EnumCoinAction Action)
    {
        this.CoinCount = CoinCount;
        this.Action = Action;
    }
}