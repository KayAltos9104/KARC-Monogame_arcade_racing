using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace KARC.WitchEngine
{
    public class RectangleCollider
    {
        public Rectangle Boundary { get; set; }
        public RectangleCollider(int x, int y, int width, int height)
        {
            Boundary = new Rectangle(x, y, width, height);             
        }

        public static bool IsCollided (RectangleCollider r1, RectangleCollider r2)
        {
            return r1.Boundary.Intersects(r2.Boundary);
        }

        public static bool IsCollided(List<(Vector2 Shift, RectangleCollider Collider)> b1, 
            RectangleCollider r2)
        {
            foreach (var r1 in b1)
            {
                if (r1.Collider.Boundary.Intersects(r2.Boundary))
                    return true;
            }
            return false;
        }

        public static bool IsCollided (List<RectangleCollider> b1, List<RectangleCollider> b2)
        {            
            foreach (var r1 in b1)
                foreach (var r2 in b2)
                {
                    if (r1.Boundary.Intersects(r2.Boundary))
                    {                       
                        return true;
                    }    
                }
            return false;
        }

        public static bool IsCollided(
            List<(Vector2 Shift, RectangleCollider Collider)> b1, 
            List<(Vector2 Shift, RectangleCollider Collider)> b2)
        {
            foreach (var r1 in b1)
                foreach (var r2 in b2)
                {
                    if (r1.Collider.Boundary.Intersects(r2.Collider.Boundary))
                    {
                        return true;
                    }
                }
            return false;
        }
    }
}
