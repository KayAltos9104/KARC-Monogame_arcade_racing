using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace KARC.WitchEngine
{
    public class Trigger2D : IObject, ITrigger
    {
        public event EventHandler<TriggerEventArgs> Triggered = delegate { };
        public List<(int ImageId, Vector2 ImagePos)> Sprites { get; set; }

        public Vector2 Pos { get; private set; }

        public Vector2 Speed { get; set; }
        public RectangleCollider Collider { get; set; }
        public float Layer { get; set; }

        public Trigger2D(Vector2 position, int width, int length)
        {
            Pos = position;
            Sprites = new List<(int ImageId, Vector2 ImagePos)>();
            Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, width, length);
            Layer = 0.4f;
        }
        public void Move(Vector2 pos)
        {
            Pos = pos;
            Collider = new RectangleCollider((int)Pos.X, (int)Pos.Y, Collider.Boundary.Width, Collider.Boundary.Height);           
        }

        public void OnTrigger(IObject activator, int id)
        {
            Triggered.Invoke(this, new TriggerEventArgs() { Activator = activator, ActivatorId = id });
        }

        public void Update() { }        
    }
}
