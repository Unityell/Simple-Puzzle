using TMPro;
using UnityEngine;

public class CoinsWidget : Widgets
{
    [SerializeField] TextMeshProUGUI Text;
    [SerializeField] GameObject HintButton;

    void Start() => Subscribe();

    void OnEnable()
    {
        Text.text = PlayerPrefs.GetInt("Coins").ToString();
    }

    protected override void SignalBox(object Obj)
    {
        switch (Obj)
        {
            case CoinSignal CoinSignal :
                var CurrentCoins = PlayerPrefs.GetInt("Coins");
                PlayerPrefs.SetInt("Coins", CoinSignal.Action == EnumCoinAction.Add ? CurrentCoins + CoinSignal.CoinCount : CurrentCoins - CoinSignal.CoinCount);
                PlayerPrefs.Save();
                Text.text = PlayerPrefs.GetInt("Coins").ToString();
                if(PlayerPrefs.GetInt("Coins") == 0)
                {
                    HintButton.SetActive(false);
                }
                break;
            default: break;
        }
    }
}