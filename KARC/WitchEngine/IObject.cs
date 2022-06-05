using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace KARC.WitchEngine
{
    public interface IObject
    {       

        public List<(int ImageId, Vector2 ImagePos)> Sprites { get; set; }

        Vector2 Pos { get;}

        Vector2 Speed { get; set; }
        void Update();

        void Move (Vector2 pos);
    }
}
