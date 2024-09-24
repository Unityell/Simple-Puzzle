using System.Collections.Generic;
using UnityEngine;

public class Menu : Widgets
{
    [Header("Widget Settings")]
    [SerializeField] List<GameButton> GameButtons;
    [SerializeField] GameObject Predications;

    void Start()
    {
        Predications.SetActive(PlayerPrefs.GetString("PredicateTutorial") == "Y");

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

    public void ShowPredicate()
    {
        EventBus.Invoke(new TutorialSignal("PredicateTutorial"));
        Predications.SetActive(true);
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