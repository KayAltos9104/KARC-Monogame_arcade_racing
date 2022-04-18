using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace KARC
{
    public interface IObject
    {
        int ImageId { get; set; }
        Vector2 Pos { get; set; }
        void Update();       
    }
}
