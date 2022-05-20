using KARC.WitchEngine;
using Microsoft.Xna.Framework;

namespace KARC.Objects
{
    public class Car : IObject, ISolid
    {
        public int ImageId { get; set; }
        public Vector2 Pos { get; set; }

        public Vector2 Speed { get; set; }
        public RectangleCollider Collider { get; set ; }

        public Car (Vector2 position)
        {
            Pos = position;
            Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, 77, 100);
        }

        public void Update()
        {            
            Pos += Speed;
            MoveCollider(Pos);
            Speed = new Vector2(0, Speed.Y);
        }

        public void MoveCollider(Vector2 newPos)
        {            
            Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, 77, 100);
        }
    }
}
