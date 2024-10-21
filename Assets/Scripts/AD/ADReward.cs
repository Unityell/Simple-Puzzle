using YG;
using Zenject;
using System.Collections.Generic;

public class ADReward : Widgets
{
    [Inject] PlayFabManager PlayFabManager;

    public void StartAD()
    {
        YandexGame.RewardVideoEvent -= AD;
        UnSubscribe();
        YandexGame.RewVideoShow(1);
        YandexGame.RewardVideoEvent += AD;
        Subscribe();
    }

    protected override void SignalBox(object Obj)
    {
        switch (Obj)
        {
            case GetStatisticSignal Result:
                if (Result.Result != null && Result.Result.ContainsKey("AD"))
                {
                    int adValue = Result.Result["AD"];

                    PlayFabManager.UpdatePlayerStatistics(new Dictionary<string, int> { { "AD", adValue + 1 } });
                }
                else
                {
                    PlayFabManager.UpdatePlayerStatistics(new Dictionary<string, int> { { "AD", 1 } });
                }
                break;

            case UpdateStatisticSignal :
                UnSubscribe();
                EventBus.Invoke(new CoinSignal(1, EnumCoinAction.Add));
                break;

            default: break;
        }
    }

    void AD(int Number)
    {
        if (Number == 1)
        {
            if(YandexGame.auth)
            {
                PlayFabManager.GetPlayerStatistics(new List<string> { "AD" });
            }
            else
            {
                EventBus.Invoke(new CoinSignal(1, EnumCoinAction.Add));
            }
            YandexGame.RewardVideoEvent -= AD;
            Enable(false);
        }
    }
}