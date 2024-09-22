using System.Collections.Generic;
using UnityEngine;

public class StartGameSignal
{
    public readonly List<Sprite> Sprites = new List<Sprite>();
    public readonly int ButtonNumber;
    public readonly Sprite FinalSprite;

    public StartGameSignal(List<Sprite> Sprites, int ButtonNumber, Sprite FinalImage)
    {
        this.Sprites.AddRange(Sprites);
        this.ButtonNumber = ButtonNumber;
        FinalSprite = FinalImage;
    }
}
