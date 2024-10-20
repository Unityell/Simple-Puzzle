using TMPro;
using UnityEngine;
using Zenject;

public class CoinsWidget : Widgets
{
    [Inject] PlayFabManager PlayFabManager;
    [Inject] GameData GameData;
    [SerializeField] TextMeshProUGUI Text;
    [SerializeField] GameObject HintButton;

    void Awake() => Subscribe();

    protected override void SignalBox(object Obj)
    {
        switch (Obj)
        {
            case CoinSignal CoinSignal :
                switch (CoinSignal.Action)
                {
                    case EnumCoinAction.Add :
                        GameData.Coins += CoinSignal.CoinCount;
                        break;
                    case EnumCoinAction.Remove :
                        GameData.Coins -= CoinSignal.CoinCount;
                        break;
                    case EnumCoinAction.Set :
                        GameData.Coins = CoinSignal.CoinCount;
                        break;
                    default: break;
                }
                PlayerPrefs.SetInt("Coins", GameData.Coins);
                PlayerPrefs.Save();
                if(YG.YandexGame.auth) PlayFabManager.SetData(new System.Collections.Generic.Dictionary<string, string>() {{"Coins", GameData.Coins.ToString()}});
                Text.text = GameData.Coins.ToString();
                HintButton.SetActive(GameData.Coins > 0);
                break;
            default: break;
        }
    }
}