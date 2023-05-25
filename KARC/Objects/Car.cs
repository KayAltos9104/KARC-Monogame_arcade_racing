using KARC.Animations;
using KARC.Models;
using KARC.Settings;
using KARC.WitchEngine;
using KARC.WitchEngine.Animations;
using Microsoft.Xna.Framework;
using System.Collections.Generic;


namespace KARC.Objects;

public class Car : IObject, ISolid, IAnimated
{
    private Vector2 _speed;

    private Vector2 _center;
    public AnimationController Animation { get; set; }
    public Vector2 Pos { get; private set; }
    public bool IsLive { get; private set; }       
    public bool IsImmortal { get; set; }

    public Vector2 Speed 
    {
        get
        {
            return _speed;
        }
        set
        {
            _speed = value;
            if (_speed.Y > 1000)
                _speed.Y = 1000;
            else if (_speed.Y < -1000)
                _speed.Y = -1000;

        }
    }        
    public List<(int ImageId, Vector2 ImagePos)> Sprites { get; set; }        
    public float Layer { get; set; }
    public List<(Vector2 Shift, RectangleCollider Collider)> Colliders { get; set; }
    public bool isMoved { get; set; }

    public Car(Vector2 position)
    {
        Pos = position;
        IsLive = true;
        Sprites = new List<(int ImageId, Vector2 ImagePos)>();
        Colliders = new List<(Vector2, RectangleCollider)>();            
        Layer = 0.5f;
        Animation = new AnimationController(Pos);
        Animation.AddAnimation("explosion", ExplosionAnimationBuilder.CreateExplosionAnimation());
        _center = new Vector2(SpriteParameters.Sprites[Sprite.car].width / 2,
            SpriteParameters.Sprites[Sprite.car].height / 2);
    }
    public Car (Vector2 position, int height, int width):this(position)
    {            
        Colliders.Add((Vector2.Zero, new RectangleCollider((int)Pos.X, (int)Pos.Y, width, height)));
    }
    public void Update(GameTime gameTime)
    {        
        if (IsLive)
        {
            Move(Pos + Speed*(gameTime.ElapsedGameTime.Milliseconds/1000.0f));
            Speed = new Vector2(0, Speed.Y);                
        }
        else
        {            
            Speed = new Vector2(0, 0);
        }

        if (Animation.ActiveAnimation != null)
        {       
            Animation.UpdateAnimation(gameTime);
            Animation.Move(Pos + _center);
            
        }
    }
    public void Move (Vector2 newPos)
    {
        Pos = newPos;
        MoveCollider(Pos);
    }
    public void MoveCollider(Vector2 newPos)
    {            
        foreach(var c in Colliders)
        {
            c.Collider.Boundary = new Rectangle(
                (int)(Pos.X + c.Shift.X), (int)(Pos.Y + c.Shift.Y), 
                c.Collider.Boundary.Width, c.Collider.Boundary.Height);
        }
    }

    public void Die ()
    {
        if (!IsImmortal)
        {
            Animation.PlayAnimation("explosion");
            IsLive = false;
        }      
    }
}




