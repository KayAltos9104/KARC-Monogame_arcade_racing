using KARC.Models;
using KARC.Objects;
using KARC.Settings;
using Microsoft.Xna.Framework;

namespace KARC.Prefabs;
public class WallGenerator : Generator
{
    public WallGenerator(int width, int height) : base(width, height)
    {
    }
    public override void CreateObject(int xTile, int yTile)
    {
        Wall w = new Wall(new Vector2(xTile, yTile), Width, Height);
        _createdObj = w;
        MultipleSprites(Sprite.wall);
       
        base.CreateObject(xTile, yTile);
    }
}
