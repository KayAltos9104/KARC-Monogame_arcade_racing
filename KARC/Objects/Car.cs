using KARC.WitchEngine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace KARC.Objects
{
    public class Car : IObject, ISolid
    {
        private Vector2 _speed;
       
        public Vector2 Pos { get; private set; }

        public bool IsLive { get; set; }

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
        //public RectangleCollider Collider { get; set ; }
        public List<(int ImageId, Vector2 ImagePos)> Sprites { get; set; }
        public float Layer { get; set; }
        public List<(Vector2 shift, RectangleCollider collider)> Colliders { get; set; }

        public Car (Vector2 position, int height, int width)
        {
            Pos = position;
            IsLive = true;
            Sprites = new List<(int ImageId, Vector2 ImagePos)>();
            Colliders = new List<(Vector2 shift, RectangleCollider collider)>();
            Colliders.Add((Vector2.Zero, new RectangleCollider((int)Pos.X, (int)Pos.Y, width, height)));
            Layer = 0.5f;
        }

        public void Update()
        {
            if (IsLive)
            {
                Move(Pos + Speed);
                Speed = new Vector2(0, Speed.Y);
                //Speed = new Vector2(0, 0);
            }
        }

        public void Move (Vector2 newPos)
        {
            Pos = newPos;
            MoveCollider(Pos);
        }

        public void MoveCollider(Vector2 newPos)
        {

            //Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, Collider.Boundary.Width, Collider.Boundary.Height);
            foreach(var c in Colliders)
            {
                c.collider.Boundary = new Rectangle((int)(Pos.X + c.shift.X), (int)(Pos.Y + c.shift.Y), c.collider.Boundary.Width, c.collider.Boundary.Height);
            }
        }
    }
}
