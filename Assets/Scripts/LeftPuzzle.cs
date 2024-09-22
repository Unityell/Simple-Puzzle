using UnityEngine;
using Zenject;
using System.Collections.Generic;

public class LeftPuzzle : MonoBehaviour
{
    [Inject] EventBus EventBus;
    [SerializeField] Socket[] Sockets;
    List <Sprite> ResultGroup = new List<Sprite>();

    void Start()
    {
        EventBus.Signal += SignalBox;
    }

    public void Initialization(List<Sprite> ResultGroup)
    {
        this.ResultGroup.AddRange(ResultGroup);
    }

    void SignalBox(object Obj)
    {
        switch (Obj)
        {
            case "Check" :
                if(CheckSprites())
                {
                    EventBus.Invoke("Win");
                }
                break;
            case "Clear" :
                for (int i = 0; i < Sockets.Length; i++)
                {
                    Sockets[i].Clear();
                }
                this.ResultGroup.Clear();
                break;
            default: break;
        }
    }

    bool CheckSprites()
    {
        for (int i = 0; i < ResultGroup.Count; i++)
        {   
            if(ResultGroup [i] != Sockets[i].GetSprite())
            {
                return false;
            }
        }
        return true;
    }

    void OnDestroy()
    {
        EventBus.Signal -= SignalBox;
    }
}
