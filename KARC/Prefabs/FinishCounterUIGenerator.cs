using KARC.Models;
using KARC.Objects;
using Microsoft.Xna.Framework;
using System;

namespace KARC.Prefabs;

public class FinishCounterUIGenerator : Generator
{
    public FinishCounterUIGenerator() : base(1, 1)
    {
    }
    public override void CreateObject(int xTile, int yTile)
    {
        var f = new FinishCounter(pos: new Vector2(xTile, yTile));

        //f.CarSignShift = f.width * 1.3f;
        //f.FinishSignShift = -_objects["finishTape"].width * 1.3f;
        

        //_createdObj = f;


        base.CreateObject(xTile, yTile);
    }
}
