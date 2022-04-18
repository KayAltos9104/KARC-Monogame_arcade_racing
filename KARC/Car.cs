using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace KARC
{
    public class Car : IObject
    {
        public int ImageId {get; set;}
        public Vector2 Pos { get; set; }

        public Vector2 Speed { get; set; }

        public void Update()
        {
            Pos += Speed;
        }
    }
}
