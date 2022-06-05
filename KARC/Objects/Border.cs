using KARC.WitchEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace KARC.Objects
{
    public class Border : IObject, ISolid
    {
        public int ImageId { get; set; }

        public Vector2 Pos { get; set; }

        public Vector2 Speed { get; set; }
        public RectangleCollider Collider { get; set; }

        public Border(Vector2 position)
        {
            Pos = position;
            Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, 24, 200000);
        }

        public void Move(Vector2 pos)
        {
            
        }

        public void MoveCollider(Vector2 newPos)
        {
            
        }

        public void Update()
        {
            
        }
    }
}
