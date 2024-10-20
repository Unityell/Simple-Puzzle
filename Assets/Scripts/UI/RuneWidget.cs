using UnityEngine;
using UnityEngine.UI;
using YG;
using TMPro;
using System.Collections.Generic;
using Zenject;

public class RuneWidget : Widgets
{
    [Inject] GameData GameData;
    [SerializeField] GameObject PredicateWidget;
    [SerializeField] Sprite[] Runes;
    [SerializeField] Button[] Images;
    [SerializeField] Color activeColor = Color.white;
    [SerializeField] Color inactiveColor = Color.black;
    [SerializeField] GameObject NextButton;

    private int selectedCount = 0;
    private bool canSelect = true;

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

    public void StartRune()
    {
        PredicateWidget.SetActive(false);
        NextButton.SetActive(false);

        if (GameData.Coins > 0)
        {
            Enable(true);
            FillImagesWithRandomRunes();
            SetButtonColors(inactiveColor);
            selectedCount = 0;
            canSelect = true;
        }
        else
        {
            Enable(false);
            var HeaderText = YandexGame.EnvironmentData.language == "ru" ? "Нет монет предсказания!" : "No fortune coins!";
            var InfoText = YandexGame.EnvironmentData.language == "ru" ? "Чтобы получить предсказание, вам не хватает монет предсказания. Соберите пазл!" : "To get a fortune, you need fortune coins. Complete the puzzle!";
            EventBus.Invoke(new InfoSignal(HeaderText, InfoText));
        }
    }

    void FillImagesWithRandomRunes()
    {
        List<int> usedIndices = new List<int>();

        for (int i = 0; i < Images.Length; i++)
        {
            if (Runes.Length > 0)
            {
                int randomIndex;
                do
                {
                    randomIndex = Random.Range(0, Runes.Length);
                } while (usedIndices.Contains(randomIndex));

                usedIndices.Add(randomIndex);
                Images[i].GetComponent<Image>().sprite = Runes[randomIndex];

                Images[i].onClick.RemoveAllListeners();

                int index = i;
                Images[i].onClick.AddListener(() => OnRuneButtonClick(index));
            }
        }
    }

    void SetButtonColors(Color color)
    {
        foreach (var button in Images)
        {
            button.GetComponent<Image>().color = color;
        }
    }

    void OnRuneButtonClick(int index)
    {
        if (!canSelect || selectedCount >= 3) return;

        Images[index].GetComponent<Image>().color = activeColor;

        selectedCount++;

        if (selectedCount >= 3)
        {
            canSelect = false;
            NextButton.SetActive(true);
        }
    }

    public void OnNextButtonClick()
    {
        ExecuteAction();
    }

    void ExecuteAction()
    {
        NextButton.SetActive(false);
        SetButtonColors(inactiveColor);
        selectedCount = 0;
        canSelect = true;
    }

    public void GetPridiction()
    {
        Widget.SetActive(false);
        PredicateWidget.SetActive(true);

        if (YandexGame.EnvironmentData.language == "ru")
        {
            Text.text = predictionsRu[Random.Range(0, predictionsRu.Count)];
        }
        else
        {
            Text.text = predictionsEn[Random.Range(0, predictionsEn.Count)];
        }   

        LayoutRebuilder.ForceRebuildLayoutImmediate(Text.transform as RectTransform);

        EventBus.Invoke(new CoinSignal(1, EnumCoinAction.Remove));
    }
}