using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class InfoWidget : MonoBehaviour
{
    [Inject] EventBus EventBus;
    [SerializeField] Button Button;
    [SerializeField] TextMeshProUGUI Header;
    [SerializeField] TextMeshProUGUI Info;
    GameButton CurrentGameButton;

    void Start()
    {
        EventBus.Signal += SignalBox;
    }

    void SignalBox(object Obj)
    {
        switch (Obj)
        {
            case InfoSignal InfoSignal :

                if(InfoSignal.Button)
                {
                   CurrentGameButton = InfoSignal.Button;
                   Button.gameObject.SetActive(true); 
                }

                Header.text = InfoSignal.Header;
                Info.text = InfoSignal.Info;
                break;
            default: break;
        }
    }

    public void OnClick()
    {
        CurrentGameButton.Cast();
        Button.gameObject.SetActive(false);
        Header.text = "";
        Info.text = "";
    }

    void OnDestroy()
    {
        EventBus.Signal -= SignalBox;
    }
}