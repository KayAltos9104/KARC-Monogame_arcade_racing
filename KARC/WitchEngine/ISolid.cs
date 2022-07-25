
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace KARC.WitchEngine
{
    public interface ISolid
    {
        //RectangleCollider Collider { get; set; }
        List <(Vector2 shift, RectangleCollider collider)> Colliders { get; set; }
        void MoveCollider(Vector2 newPos);

               
    }
}
