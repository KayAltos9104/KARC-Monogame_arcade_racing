using KARC.WitchEngine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace KARC.Objects
{
    public class Car : IObject, ISolid
    {
        private Vector2 _speed;
       
        public Vector2 Pos { get; private set; }

        public Vector2 Speed 
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = value;
                if (_speed.Y > 20)
                    _speed.Y = 20;
                else if (_speed.Y < -20)
                    _speed.Y = -20;

            }
        }
        public RectangleCollider Collider { get; set ; }
        public List<(int ImageId, Vector2 ImagePos)> Sprites { get; set; }

        public Car (Vector2 position)
        {
            Pos = position;
            Sprites = new List<(int ImageId, Vector2 ImagePos)>();
            Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, 77, 100);
        }

        public void Update()
        {            
            Move(Pos + Speed);
            //Speed = new Vector2(0, Speed.Y);
            Speed = new Vector2(0, 0);
            
        }

        public void Move (Vector2 newPos)
        {
            Pos = newPos;
            MoveCollider(Pos);
        }

        public void MoveCollider(Vector2 newPos)
        {
            Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, Collider.Boundary.Width, Collider.Boundary.Height);
        }
    }
}
