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
            EventBus.Invoke(new CoinSignal(PlayerPrefs.GetInt("Coins"), EnumCoinAction.Set));
            return;
        }

        EventBus.Subscribe(SignalBox);
        PlayFabManager.Login();
        GameData.Lang = YandexGame.EnvironmentData.language == "en" ? EnumLanguage.EN : EnumLanguage.RU;
    }

    void SignalBox(object Obj)
    {
        switch (Obj)
        {
            case EnumSignals.LoginSucces :
                PlayFabManager.GetData(new System.Collections.Generic.List<string>() {"Coins"});
                break;
            case PlayFab.ClientModels.GetUserDataResult Result :
                if(Result.Data.Count == 0)
                {
                    PlayFabManager.SetData(new System.Collections.Generic.Dictionary<string, string>() {{"Coins", "0"}});
                    EventBus.Invoke(new CoinSignal(GameData.Coins, EnumCoinAction.Set));
                }
                else
                {
                    string coinsString = Result.Data["Coins"].Value;
                    GameData.Coins = int.Parse(coinsString);
                    EventBus.Invoke(new CoinSignal(GameData.Coins, EnumCoinAction.Set));      
                } 
                EventBus.Invoke(EnumSignals.Start); 
                EventBus.Unsubscribe(SignalBox);
                break;
            case EnumSignals.LoginError :
                EventBus.Invoke(new CoinSignal(PlayerPrefs.GetInt("Coins"), EnumCoinAction.Set));
                EventBus.Invoke(EnumSignals.Start);
                EventBus.Unsubscribe(SignalBox);
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