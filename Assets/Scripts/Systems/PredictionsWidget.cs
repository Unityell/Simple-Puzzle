using TMPro;
using UnityEngine;
using YG;
using System.Collections.Generic;

public class PredictionsWidget : Widgets
{
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
            if(PlayerPrefs.GetInt("Coins") > 0)
            {
                Enable(Switch);
                GetPridiction();  
                EventBus.Invoke(new CoinSignal(1, EnumCoinAction.Remove));                     
            }
            else
            {
                Enable(false);
                var HeaderText = YandexGame.EnvironmentData.language == "ru" ? "Нет очков!" : "No points!";
                var InfoText = YandexGame.EnvironmentData.language == "ru" ? "Чтобы получить предсказание, вам не хватает очков. Соберите пазл!" : "To receive a prediction, you need more points. Complete the puzzle!";
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
    }
}

[System.Serializable]
public class Predictions
{
    public List<string> predictions;
}