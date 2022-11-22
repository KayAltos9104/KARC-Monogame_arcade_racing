
using KARC.WitchEngine;
using KARC.WitchEngine.Animations;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace KARC.Objects;

public class Explosion : IObject, IAnimated
{
    public float Layer { get; set; }
    public List<(int ImageId, Vector2 ImagePos)> Sprites { get; set ; }
    public Vector2 Pos { get; private set; }
    public Vector2 Speed { get; set; }
    public Dictionary<string, Animator> Animations { get; private set; }
    public Animator ActiveAnimation { get; set; }

    public Explosion(Vector2 position)
    {
        Layer = 0.4f;
        Sprites = new List<(int ImageId, Vector2 ImagePos)>();
        Pos = position;
        Speed = Vector2.Zero;
        Animations = new Dictionary<string, Animator>();
        ActiveAnimation = null;
    }

    public void Move(Vector2 pos)
    {
        Pos = pos;
    }
    public void Update()
    {
        
    }
}
