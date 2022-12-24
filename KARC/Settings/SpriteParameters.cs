using System.Collections.Generic;

namespace KARC.Settings;

public static class SpriteParameters
{
    public static IReadOnlyDictionary <Sprite, (int width, int height)> Sprites = new Dictionary<Sprite, (int, int)>()
    {
        {Sprite.car, (77, 100) },
        {Sprite.wall, (24, 24) },
        {Sprite.finishCounterWindow, (1000, 28)},
        {Sprite.finishTape, (20, 20) },
        {Sprite.shield, (48, 48) },           
    };    
}
public enum Sprite : byte
{
    car,
    wall,
    window,
    finishTape,
    finishCounterWindow,
    shield, 
    explosion
}
