using System.Collections.Generic;
using UnityEngine;

public class Tutorial : Widgets
{
    [SerializeField] List<GameObject> Tutorials;

    void Start() => Subscribe();

    public void SaveTutorialState(string Name)
    {
        PlayerPrefs.SetString(Name, "Y");
        PlayerPrefs.Save();
    }

    protected override void SignalBox(object Obj)
    {
        switch (Obj)
        {
            case TutorialSignal TutorialSignal :
                if(PlayerPrefs.GetString(TutorialSignal.Name) != "Y")
                {
                    Tutorials.Find(x => x.name == TutorialSignal.Name).SetActive(true);
                }
                break;
            default: break;
        }
    }
}
