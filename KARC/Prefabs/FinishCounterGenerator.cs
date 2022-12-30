using KARC.Models;
using KARC.Objects;
using KARC.Settings;
using Microsoft.Xna.Framework;
using System;

namespace KARC.Prefabs;

public class FinishCounterGenerator : Generator
{
    public FinishCounterGenerator(int width, int height) : base(width, height)
    {

    }
    public override void CreateObject(int xTile, int yTile)
    {
        var f = new FinishCounter(pos: new Vector2(xTile, yTile), text: String.Empty,
               carSign: ((int)Sprite.wall,
               new Vector2(
                   0, 
                   (1 + SpriteParameters.Sprites[Sprite.finishCounterWindow].height -
                   SpriteParameters.Sprites[Sprite.wall].height) / 2.0f)),
               finishSign: ((int)Sprite.finishTape,
               new Vector2(
                   0, 
                   (1 + SpriteParameters.Sprites[Sprite.finishCounterWindow].height -
               SpriteParameters.Sprites[Sprite.finishTape].height) / 2.0f)),
               SpriteParameters.Sprites[Sprite.finishCounterWindow].width);

        f.CarSignShift = SpriteParameters.Sprites[Sprite.wall].width * 1.3f;
        f.FinishSignShift = -SpriteParameters.Sprites[Sprite.finishTape].width * 1.3f;
        f.Sprites.Insert(0, ((int)Sprite.finishTape, Vector2.Zero));
        _createdObj = f;
        base.CreateObject(xTile, yTile);
    }
}
