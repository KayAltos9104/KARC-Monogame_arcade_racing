using KARC.Objects;
using KARC.WitchEngine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.ComponentModel;

namespace KARC.Objects
{
    public class Car : IObject, ISolid
    {
        private Vector2 _speed;        
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
                if (_speed.Y > 10)
                    _speed.Y = 10;
                else if (_speed.Y < -10)
                    _speed.Y = -10;

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
        }
        public Car (Vector2 position, int height, int width):this(position)
        {            
            Colliders.Add((Vector2.Zero, new RectangleCollider((int)Pos.X, (int)Pos.Y, width, height)));
        }
        public void Update(GameTime gameTime)
        {
            if (IsLive)
            {
                Move(Pos + Speed);
                Speed = new Vector2(0, Speed.Y);                
            }
            //else
            //{
            //    Speed = new Vector2(0, 0);
            //}
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
                IsLive = false;
        }
    }
}




