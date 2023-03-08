using KARC.Models;
using KARC.Settings;
using KARC.WitchEngine;
using Microsoft.Xna.Framework;

namespace KARC.Prefabs;

public class ShieldGenerator : Generator
{
    public ShieldGenerator() : base(1, 1)
    {

    }
    public override void CreateObject(int xTile, int yTile)
    {
        Trigger2D t = new Trigger2D(new Vector2(xTile, yTile), Width, Height);
        _createdObj = t;
        t.Sprites.Add(((byte)Sprite.shield, Vector2.Zero));
        //MultipleSprites(Sprite.shield);       
        base.CreateObject(xTile, yTile);
    }
}
