using System.Collections.Generic;
using UnityEngine;

public class Menu : Widgets
{
    [Header("Widget Settings")]
    [SerializeField] List<GameButton> GameButtons;

    void Start()
    {
        Subscribe();

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


    protected override void SignalBox(object Obj)
    {
        switch (Obj)
        {
            case StartGameSignal :
                Enable(false);
                break;
            case EnumSignals.Refresh :
                Refresh();
                break;
            case EnumSignals.OpenMenu :
                Enable(true);
                break;
            default: break;
        }
    }
}