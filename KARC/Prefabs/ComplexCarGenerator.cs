using KARC.Models;
using KARC.Objects;
using KARC.Settings;
using KARC.WitchEngine;
using Microsoft.Xna.Framework;

namespace KARC.Prefabs;

public class ComplexCarGenerator : Generator
{
    public ComplexCarGenerator() : base(1, 1)
    {

    }
    public override void CreateObject(int xTile, int yTile)
    {
        int cabinWidth = SpriteParameters.Sprites[Sprite.wall].width;
        int cabinHeight = SpriteParameters.Sprites[Sprite.wall].height;
        int carWidth = SpriteParameters.Sprites[Sprite.car].width;
        int carHeight = SpriteParameters.Sprites[Sprite.car].height;
        Car c = new Car(new Vector2(xTile, yTile));
        var cabin = new RectangleCollider(
            xTile, 
            yTile, 
            cabinWidth,
            cabinHeight);

        var hull = new RectangleCollider(
            xTile, 
            yTile,
            carWidth,
            carHeight - cabinHeight);
        c.Colliders.Add((new Vector2(carWidth / 2 - cabinWidth / 2, 0), cabin));
        c.Colliders.Add((new Vector2(0, cabinHeight), hull));
        c.Sprites.Add(((byte)Sprite.car, Vector2.Zero));  
        _createdObj = c;
        base.CreateObject(xTile, yTile);
    }
}
