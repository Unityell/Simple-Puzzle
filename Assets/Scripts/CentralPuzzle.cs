using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class CentralPuzzle : Widgets
{
    [SerializeField] Socket[] Sockets;
    List<Sprite> ResultGroup = new List<Sprite>();

    void Start()
    {
        Subscribe();
    }

    public void Initialization(List<Sprite> ResultGroup)
    {
        var ShuffledSprites = ResultGroup.OrderBy(a => Guid.NewGuid()).ToList();

        for (int i = 0; i < Sockets.Length; i++)
        {
            Sockets[i].Initialization(ShuffledSprites[i]);
        }

        this.ResultGroup.AddRange(ResultGroup);
    }

    protected override void SignalBox(object Obj)
    {
        switch (Obj)
        {
            case EnumSignals.Check:
                if (CheckSprites() && ResultGroup.Count > 0) 
                {
                    EventBus.Invoke(EnumSignals.Win);
                    ResultGroup.Clear();
                    for (int i = 0; i < Sockets.Length; i++)
                    {
                        Sockets[i].Enable(false);
                    }
                }
                break;
            case EnumSignals.Clear:
                for (int i = 0; i < Sockets.Length; i++)
                {
                    Sockets[i].Clear();
                }
                ResultGroup.Clear();
                break;
            default: break;
        }
    }

    public void SwapIncorrectPiece(int piecesToSwap)
    {
        if (Sockets.Length != ResultGroup.Count) return;

        int incorrectCount = 0;
        List<int> incorrectIndices = new List<int>();

        for (int i = 0; i < Sockets.Length; i++)
        {
            if (Sockets[i].GetSprite() && Sockets[i].GetSprite().name != ResultGroup[i].name)
            {
                incorrectCount++;
                incorrectIndices.Add(i);
            }
        }

        int swapCount = Math.Min(piecesToSwap, incorrectCount);
        int swappedCount = 0;

        if (swapCount <= 0) return;

        for (int i = 0; i < incorrectIndices.Count; i++)
        {
            int incorrectIndex = incorrectIndices[i];

            for (int j = 0; j < Sockets.Length; j++)
            {
                if (j < ResultGroup.Count && ResultGroup[j].name == Sockets[incorrectIndex].GetSprite().name)
                {
                    if (Sockets[j].GetSprite() && Sockets[j].GetSprite().name != ResultGroup[j].name)
                    {
                        var tempSprite = Sockets[j].GetSprite();
                        Sockets[j].SetItemToSocket(Sockets[incorrectIndex].GetSprite());
                        Sockets[incorrectIndex].SetItemToSocket(tempSprite);

                        swappedCount++;

                        if (swappedCount >= swapCount)
                        {
                            EventBus.Invoke(new CoinSignal(1, EnumCoinAction.Remove));
                            return;
                        }
                    }
                }
            }
        }
    }


    bool CheckSprites()
    {
        for (int i = 0; i < ResultGroup.Count; i++)
        {
            if (Sockets[i].GetSprite() && ResultGroup[i].name != Sockets[i].GetSprite().name)
            {
                return false;
            }
        }
        return true;
    }
}