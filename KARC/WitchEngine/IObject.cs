using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace KARC.WitchEngine
{
    public interface IObject
    {
        int ImageId { get; set; }
        Vector2 Pos { get;}

        Vector2 Speed { get; set; }
        void Update();

        void Move (Vector2 pos);
    }
}
