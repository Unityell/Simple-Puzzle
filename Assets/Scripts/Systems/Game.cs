using UnityEngine;
using Zenject;
using System.Collections;
using YG;

public class Game : Widgets
{
    [Inject] GameData GameData;
    [Inject] AudioManager AudioManager;
    [Header("Widget Settings")]
    [SerializeField] CentralPuzzle CentralPuzzle;
    [SerializeField] GameObject NextButton;
    [SerializeField] GameObject BackToMenuButton;
    [SerializeField] GameObject HintButton;
    [Header("Effects")]
    [SerializeField] ParticleSystem VFXUp;
    [SerializeField] ParticleSystem VFXDown;
    int Number;

    void Start() => Subscribe();

    protected override void SignalBox(object Obj)
    {
        switch (Obj)
        {
            case StartGameSignal StartGameSignal :
                PlayerPrefs.Save();
                CentralPuzzle.transform.localScale = Vector3.one;
                EventBus.Invoke(new TutorialSignal("GamePlayTutorial"));
                NextButton.gameObject.SetActive(false);
                HintButton.SetActive(GameData.Coins >= 1);
                Enable(true);
                Number = StartGameSignal.ButtonNumber;
                CentralPuzzle.Initialization(StartGameSignal.Sprites);
                BackToMenuButton.SetActive(true);
                break;

            case EnumSignals.Win :
                EventBus.Invoke(new CoinSignal(1, EnumCoinAction.Add));
                HintButton.SetActive(false);
                BackToMenuButton.SetActive(false);
                PlayerPrefs.SetInt("Button" + (Number + 1).ToString(), 1);
                PlayerPrefs.Save();
                EventBus.Invoke(EnumSignals.Refresh);
                AudioManager.PlaySound("Yeah", null);
                StartCoroutine(Win());
                break;
            default: break;
        }
    }

    public void Close()
    {
        YandexGame.FullscreenShow();
        AudioManager.PlaySound("SoftClick", null);
        EventBus.Invoke(EnumSignals.Clear);
        EventBus.Invoke(EnumSignals.OpenMenu);
        BackToMenuButton.SetActive(false);
        NextButton.SetActive(false);
        Enable(false);
    }

    IEnumerator Win()
    {
        VFXUp.Play();
        VFXDown.Play();

        for (int i = 0; i < 25; i++)
        {
            CentralPuzzle.transform.localScale += Vector3.one * 0.01f;
            yield return null;
        }

        yield return new WaitForSeconds(4f);
        NextButton.SetActive(true);
        VFXUp.Stop();
        VFXDown.Stop();
    }
}