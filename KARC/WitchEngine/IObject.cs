using Microsoft.Xna.Framework;

using System.Collections.Generic;

namespace KARC.WitchEngine
{
    public interface IObject
    {       
        float Layer { get;set; }
        List<(int ImageId, Vector2 ImagePos)> Sprites { get; set; }
        Vector2 Pos { get;}
        Vector2 Speed { get; set; }       
        void Update();
        void Move (Vector2 pos);
    }
}


