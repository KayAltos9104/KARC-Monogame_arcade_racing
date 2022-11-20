using Microsoft.Xna.Framework;

namespace KARC.WitchEngine.Animations
{
    public class AnimationFrame
    {
        public Point Point { get; set; }
        public int Width { get; set; }
        public int Height { get; set; } 

        public AnimationFrame (int x, int y, int width, int height)
        {
            Point = new Point(x, y);
            Width = width;
            Height = height;
        }
    }
}
