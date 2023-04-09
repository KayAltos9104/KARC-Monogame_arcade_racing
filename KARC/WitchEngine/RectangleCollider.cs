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

        public static Vector2 CalculateShift (List<(Vector2 Shift, RectangleCollider Collider)> b1,
            List<(Vector2 Shift, RectangleCollider Collider)> b2, out bool isCollided)
        {
            Vector2 maxShift = Vector2.Zero;
            foreach (var r1 in b1)
                foreach (var r2 in b2)
                {
                    var rect1 = r1.Collider;
                    var rect2 = r2.Collider;
                    var maxShiftBuffer = Vector2.Zero;
                    if (IsCollided(rect1, rect2))
                    {
                        if (rect1.Boundary.Left < rect2.Boundary.Left)
                        {
                            maxShiftBuffer += new Vector2(rect2.Boundary.Left - rect1.Boundary.Right, 0);
                        }
                        else
                        {
                            maxShiftBuffer += new Vector2(rect2.Boundary.Right - rect1.Boundary.Left, 0);
                        }

                        if (rect1.Boundary.Top < rect2.Boundary.Top)
                        {
                            maxShiftBuffer += new Vector2(rect1.Boundary.Bottom - rect2.Boundary.Top, 0);
                        }
                        else
                        {
                            maxShiftBuffer += new Vector2(rect2.Boundary.Bottom - rect1.Boundary.Top, 0);
                        }
                        if (Math.Abs(maxShiftBuffer.X) > Math.Abs(maxShift.X))
                            maxShift.X = maxShiftBuffer.X;
                        if (Math.Abs(maxShiftBuffer.Y) > Math.Abs(maxShift.Y))
                            maxShift.Y = maxShiftBuffer.Y;

                    }
                }
            if (maxShift != Vector2.Zero)
                isCollided = true;
            else
                isCollided = false;
            return maxShift;
        }

        
    }
}
