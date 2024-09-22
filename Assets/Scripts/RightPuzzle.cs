using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class RightPuzzle : MonoBehaviour
{
    List<Sprite> Sprites = new List<Sprite>();
    [SerializeField] Socket[] Sockets;

    public void Initialization(List<Sprite> Sprites)
    {
        this.Sprites.Clear();
        this.Sprites.AddRange(Sprites.OrderBy(a => Guid.NewGuid()).ToList());
        for (int i = 0; i < Sockets.Length; i++)
        {
            Sockets[i].Initialization(this.Sprites[i]);
        }
    }
}
