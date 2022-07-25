
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace KARC.WitchEngine
{
    public interface ISolid
    {        
        List <(Vector2 Shift, RectangleCollider Collider)> Colliders { get; set; }
        void MoveCollider(Vector2 newPos);               
    }
}
