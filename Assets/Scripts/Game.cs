using UnityEngine;
using Zenject;
using System.Collections;
using UnityEngine.UI;
using YG;

public class Game : MonoBehaviour
{
    [Inject] EventBus EventBus;
    [Inject] AudioManager AudioManager;

    [SerializeField] RightPuzzle Right;
    [SerializeField] LeftPuzzle Left;
    [SerializeField] GameObject Switch;
    [SerializeField] ParticleSystem VFX;
    [SerializeField] Image FinalImage;
    [SerializeField] GameObject BlackScreen;
    [SerializeField] GameObject UI;
    [SerializeField] GameObject NextButton;
    [SerializeField] Animator Anim;
    int Number;

    void Start()
    {
        EventBus.Signal += SignalBox;
    }

    public void BlackScreenOff()
    {
        BlackScreen.SetActive(false);
        PlayerPrefs.SetInt("BlackScreen", 1);
        PlayerPrefs.Save();
    }

    void SignalBox(object Obj)
    {
        if(Obj.GetType() == typeof(StartGameSignal))
        {
            BlackScreen.SetActive(PlayerPrefs.GetInt("BlackScreen") == 0);
            NextButton.gameObject.SetActive(false);
            UI.SetActive(true);
            StartGameSignal Signal = Obj as StartGameSignal;
            Switch.SetActive(true);
            FinalImage.gameObject.SetActive(false);
            Number = Signal.ButtonNumber;
            Right.gameObject.SetActive(true);
            Right.Initialization(Signal.Sprites);
            Left.Initialization(Signal.Sprites);
            FinalImage.sprite = Signal.FinalSprite;
            return;
        }
        switch (Obj)
        {
            case "Win" :
                PlayerPrefs.SetInt("Button" + (Number + 1).ToString(), 1);
                PlayerPrefs.Save();
                EventBus.Invoke("Refresh");
                FinalImage.gameObject.SetActive(true);
                AudioManager.PlaySound("UwU", null);
                StartCoroutine(Win());
                break;
            default: break;
        }
    }

    public void Close()
    {
        YandexGame.FullscreenShow();
        AudioManager.PlaySound("SoftClick", null);
        EventBus.Invoke("Clear");
        EventBus.Invoke("OpenMenu");
        Switch.SetActive(false);
    }

    IEnumerator Win()
    {
        VFX.Play();
        UI.SetActive(false);
        Anim.Play("StartCameraAnim", 0, 0);
        Right.gameObject.SetActive(false);
        yield return new WaitForSeconds(4f);
        NextButton.SetActive(true);
        VFX.Stop();
    }
    
    void OnDestroy()
    {
        EventBus.Signal -= SignalBox;
    }
}
