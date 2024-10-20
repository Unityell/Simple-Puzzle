using TMPro;
using UnityEngine;
using YG;
using UnityEngine.UI;
using System.Collections.Generic;
using Zenject;

public class PredictionsWidget : Widgets
{
    [Inject] GameData GameData;
    [SerializeField] TextAsset PredictionsRu;
    [SerializeField] TextAsset PredictionsEN;
    [SerializeField] TextMeshProUGUI Text;

    private List<string> predictionsRu;
    private List<string> predictionsEn;

    void Start() 
    {
        predictionsRu = JsonUtility.FromJson<Predictions>(PredictionsRu.text).predictions;

        predictionsEn = JsonUtility.FromJson<Predictions>(PredictionsEN.text).predictions;
    }

    public void ShowHide(bool Switch)
    {
        Text.text = "";

        if(Switch)
        {
            if(GameData.Coins > 0)
            {
                Enable(Switch);
                GetPridiction();  
                EventBus.Invoke(new CoinSignal(1, EnumCoinAction.Remove));                     
            }
            else
            {
                Enable(false);
                var HeaderText = YandexGame.EnvironmentData.language == "ru" ? "Нет монет предсказания!" : "No fortune coins!";
                var InfoText = YandexGame.EnvironmentData.language == "ru" ? "Чтобы получить предсказание, вам не хватает монет предсказания. Соберите пазл!" : "To get a fortune, you need fortune coins. Complete the puzzle!";
                EventBus.Invoke(new InfoSignal(HeaderText, InfoText));                 
            }
        }
        else
        {
            Enable(Switch);
        }
    }

    void GetPridiction()
    {
        if (YandexGame.EnvironmentData.language == "ru")
        {
            Text.text = predictionsRu[Random.Range(0, predictionsRu.Count)];
        }
        else
        {
            Text.text = predictionsEn[Random.Range(0, predictionsEn.Count)];
        }   
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(Text.transform as RectTransform);
    }
}

[System.Serializable]
public class Predictions
{
    public List<string> predictions;
}