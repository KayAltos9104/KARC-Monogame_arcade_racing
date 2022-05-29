using KARC.WitchEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace KARC.Objects
{
    class Wall : IObject, ISolid
    {
        public int ImageId { get; set; }
        public Vector2 Pos { get; set; }
        public Vector2 Speed { get; set; }
        public RectangleCollider Collider { get; set; }

        public Wall(Vector2 position)
        {
            Pos = position;
            Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, 24, 100);
        }

        public void Move(Vector2 newPos)
        {
            Pos = newPos;
            MoveCollider(Pos);
        }

        public void Update()
        {
            
        }

        public void MoveCollider(Vector2 newPos)
        {
            Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, 24, 100);
        }
    }
}
