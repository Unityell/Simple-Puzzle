using UnityEngine;
using UnityEngine.UI;
using YG;
using System;

public class DailyBonusWidget : Widgets
{
    [SerializeField] Button[] Buttons;
    [SerializeField] GameObject NextButton;

    private const int MaxBonusDays = 7;
    private string LastLoginDateKey = "GameData";
    private string BonusProgressKey = "GameDataValue";

    private void OnEnable() => YandexGame.GetDataEvent += StartWidget;
    private void OnDisable() => YandexGame.GetDataEvent -= StartWidget;

    void StartWidget()
    {
        string todayDate = DateTime.Today.ToString("yyyy-MM-dd");

        string savedDate = PlayerPrefs.GetString(LastLoginDateKey, "");

        if (savedDate != todayDate)
        {
            PlayerPrefs.SetString(LastLoginDateKey, todayDate);

            int bonusProgress = PlayerPrefs.GetInt(BonusProgressKey, 0);

            if (bonusProgress >= MaxBonusDays)
            {
                Widget.SetActive(false);
                return;
            }

            UpdateBonusButtons();
        }
    }

    void UpdateBonusButtons()
    {
        int bonusProgress = PlayerPrefs.GetInt(BonusProgressKey, 0);

        for (int i = 0; i < Buttons.Length; i++)
        {
            if (i < bonusProgress)
            {
                Buttons[i].interactable = false;
            }
            else if (i == bonusProgress)
            {
                Buttons[i].interactable = true;
                Buttons[i].onClick.RemoveAllListeners();
                Buttons[i].onClick.AddListener(() => OnBonusButtonClick());
            }
            else
            {
                Buttons[i].interactable = false;
            }
        }

        if (bonusProgress >= MaxBonusDays)
        {
            Widget.SetActive(false);
        }
        else
        {
            Widget.SetActive(true);
        }
    }

    void OnBonusButtonClick()
    {
        foreach (var button in Buttons)
        {
            button.interactable = false;
        }

        NextButton.SetActive(true);

        int bonusProgress = PlayerPrefs.GetInt(BonusProgressKey, 0);
        bonusProgress++;
        PlayerPrefs.SetInt(BonusProgressKey, bonusProgress);

        EventBus.Invoke(new CoinSignal(bonusProgress, EnumCoinAction.Add));
    }
}