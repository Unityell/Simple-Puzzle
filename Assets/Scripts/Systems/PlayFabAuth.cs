using UnityEngine;
using Zenject;
using YG;

public class PlayFabAuth : MonoBehaviour
{
    [Inject] EventBus EventBus;
    [Inject] PlayFabManager PlayFabManager;
    [Inject] GameData GameData;

    void OnEnable() => YandexGame.GetDataEvent += Setup;

    void Setup()
    {   
        if(!YandexGame.auth)
        {
            EventBus.Invoke(EnumSignals.Start);
            YandexGame.GameReadyAPI();
            EventBus.Invoke(new CoinSignal(PlayerPrefs.GetInt("Coins", default), EnumCoinAction.Set));
            return;
        }

        EventBus.Subscribe(SignalBox);
        PlayFabManager.Login(YandexGame.playerId);
        GameData.Lang = YandexGame.EnvironmentData.language == "en" ? EnumLanguage.EN : EnumLanguage.RU;
    }

    void SignalBox(object Obj)
    {
        switch (Obj)
        {
            case EnumSignals.LoginSucces :
                PlayFabManager.GetData(new System.Collections.Generic.List<string>() {"Coins", "LastData"});
                break;
            case PlayFab.ClientModels.GetUserDataResult Result :
                if(Result.Data.Count == 0)
                {
                    PlayFabManager.SetData(new System.Collections.Generic.Dictionary<string, string>() {{"Coins", PlayerPrefs.GetInt("Coins", default).ToString()}, {"LastData", "0"}});
                    EventBus.Invoke(new CoinSignal(GameData.Coins, EnumCoinAction.Set));
                }
                else
                {
                    string coinsString = Result.Data["Coins"].Value;
                    GameData.Coins = int.Parse(coinsString);
                    EventBus.Invoke(new CoinSignal(GameData.Coins, EnumCoinAction.Set));      
                } 
                GameData.Auth = true;
                EventBus.Invoke(EnumSignals.Start); 
                YandexGame.GameReadyAPI();
                YandexGame.ConsumePurchases();
                EventBus.Unsubscribe(SignalBox);
                break;
            case EnumSignals.LoginError :
                GameData.Auth = false;
                EventBus.Unsubscribe(SignalBox);
                EventBus.Invoke(new CoinSignal(PlayerPrefs.GetInt("Coins", default), EnumCoinAction.Set));
                EventBus.Invoke(EnumSignals.Start);
                YandexGame.GameReadyAPI();
                break;
            default: break;
        }
    }

    void OnDestroy()
    {
        YandexGame.GetDataEvent -= Setup;
        if(!YandexGame.auth) return;
        EventBus.Unsubscribe(SignalBox);
    }
}