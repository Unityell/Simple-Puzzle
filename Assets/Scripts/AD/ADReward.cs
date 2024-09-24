using YG;

public class ADReward : Widgets
{
    public void StartAD()
    {
        YandexGame.RewVideoShow(1);
        YandexGame.RewardVideoEvent += AD;
    }

    void AD(int Number)
    {
        if(Number == 1)
        {
            EventBus.Invoke(new CoinSignal(1, EnumCoinAction.Add));
            Enable(false);
            YandexGame.RewardVideoEvent -= AD;
        }
    }
}