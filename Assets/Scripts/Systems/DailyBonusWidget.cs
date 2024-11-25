using UnityEngine;
using UnityEngine.UI;
using YG;
using Zenject;

public class DailyBonusWidget : Widgets
{
    [Inject] PlayFabManager PlayFabManager;
    [SerializeField] Button[] Buttons;
    [SerializeField] GameObject NextButton;
    [SerializeField] RectTransform Point;

    private const int MaxBonusDays = 7;
    private const int SecondsInDay = 86400000 / 2; 
    private string BonusProgressKey = "GameDataValue";

    void Awake()
    {
        EventBus.Subscribe(SignalBox);
    }

    protected override void SignalBox(object Obj)
    {
        switch (Obj)
        {
            case PlayFab.ClientModels.GetUserDataResult Result:
                if (Result.Data.ContainsKey("LastData"))
                {
                    long lastDataTime = long.Parse(Result.Data["LastData"].Value);
                    StartWidget(lastDataTime);
                }
                else
                {
                    StartWidget(0);
                }
                break;
            default: break;
        }
    }

    void StartWidget(long lastData)
    {
        long todayDate = YandexGame.ServerTime();

        if (lastData == 0 || (todayDate - lastData) >= SecondsInDay)
        {
            int bonusProgress = PlayerPrefs.GetInt(BonusProgressKey, 0);

            if (bonusProgress < MaxBonusDays)
            {
                UpdateBonusButtons(bonusProgress);
            }
        }
        else
        {
            Enable(false);
        }
    }

    void UpdateBonusButtons(int bonusProgress)
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            if (i < bonusProgress)
            {
                Buttons[i].interactable = false;
            }
            else if (i == bonusProgress)
            {
                Buttons[i].interactable = true;
                Point.transform.position = Buttons[i].transform.position;
                Buttons[i].onClick.RemoveAllListeners();
                Buttons[i].onClick.AddListener(() => OnBonusButtonClick(bonusProgress));
            }
            else
            {
                Buttons[i].interactable = false;
            }
        }

        if (bonusProgress >= MaxBonusDays)
        {
            Enable(false);
        }
        else
        {
            Enable(true);
        }
    }

    void OnBonusButtonClick(int bonusProgress)
    {
        foreach (var button in Buttons)
        {
            button.interactable = false;
        }

        NextButton.SetActive(true);
        bonusProgress += 5;
        PlayerPrefs.SetInt(BonusProgressKey, bonusProgress);
        PlayerPrefs.Save();

        PlayFabManager.SetData(new System.Collections.Generic.Dictionary<string, string>() {{"LastData", YandexGame.ServerTime().ToString()}});

        EventBus.Invoke(new CoinSignal(bonusProgress, EnumCoinAction.Add));
    }
}