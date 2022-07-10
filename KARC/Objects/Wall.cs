using KARC.WitchEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace KARC.Objects
{
    public class Wall : IObject, ISolid
    {       
        public Vector2 Pos { get; set; }
        public Vector2 Speed { get; set; }
        public RectangleCollider Collider { get; set; }
        public List<(int ImageId, Vector2 ImagePos)> Sprites { get; set; }
        public float Layer { get; set; }

        public Wall(Vector2 position, int width, int length)
        {
            Pos = position;
            Sprites = new List<(int ImageId, Vector2 ImagePos)>();
            Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, width, length);
            Layer = 0.5f;
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
            Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, Collider.Boundary.Width, Collider.Boundary.Height);
        }
    }
}
