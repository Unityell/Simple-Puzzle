using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoWidget : Widgets
{
    [Header("Widget Settings")]
    [SerializeField] Button Button;
    [SerializeField] TextMeshProUGUI Header;
    [SerializeField] TextMeshProUGUI Info;
    GameButton CurrentGameButton;

    void Start() => Subscribe();

    protected override void SignalBox(object Obj)
    {
        switch (Obj)
        {
            case InfoSignal InfoSignal :

                if(InfoSignal.Button)
                {
                   CurrentGameButton = InfoSignal.Button;
                   Button.gameObject.SetActive(true); 
                }

                Button.gameObject.SetActive(InfoSignal.Button); 

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
}