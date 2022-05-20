
using Microsoft.Xna.Framework;

namespace KARC.WitchEngine
{
    public interface ISolid
    {
        RectangleCollider Collider { get; set; }
        void MoveCollider(Vector2 newPos)
        {
            Collider.Boundary.Offset(newPos);
        }        
    }
}
