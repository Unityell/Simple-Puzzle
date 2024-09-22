using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Menu : MonoBehaviour
{
    [Inject] EventBus EventBus;
    [SerializeField] List<GameButton> GameButtons;
    [SerializeField] GameObject BackGround;

    void Start()
    {
        EventBus.Signal += SignalBox;
        foreach (var item in GameButtons)
        {
            item.Refresh();
        }
    }

    public void Refresh()
    {
        foreach (var item in GameButtons)
        {
            item.Refresh();
        }        
    }

    void SignalBox(object Obj)
    {
        if(Obj.GetType() == typeof(StartGameSignal))
        {
            BackGround.SetActive(false);
        }
        switch (Obj)
        {
            case "Refresh" :
                Refresh();
                break;
            case "OpenMenu" :
                BackGround.SetActive(true);
                break;
            default: break;
        }
    }
    void OnDestroy()
    {
        EventBus.Signal -= SignalBox;
    }
}
