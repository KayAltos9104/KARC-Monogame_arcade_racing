using KARC.Models;
using KARC.Settings;
using KARC.WitchEngine;
using Microsoft.Xna.Framework;

namespace KARC.Prefabs;

public class FinishGenerator : Generator
{
    public FinishGenerator(int width, int height) : base(width, height)
    {

    }
    public override void CreateObject(int xTile, int yTile)
    {
        Trigger2D t = new Trigger2D(new Vector2(xTile, yTile), Width, Height);
        _createdObj = t;
        MultipleSprites(Sprite.finishTape);

        base.CreateObject(xTile, yTile);
    }
}
