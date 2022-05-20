using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace KARC.WitchEngine
{
    public interface IObject
    {
        int ImageId { get; set; }
        Vector2 Pos { get; set; }
        void Update();
    }
}
