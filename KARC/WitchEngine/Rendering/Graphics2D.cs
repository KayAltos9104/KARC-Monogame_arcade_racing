using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using KARC.Settings;
using KARC.WitchEngine.Animations;

namespace KARC.WitchEngine.Primitives;
public static class Graphics2D
{
    public static SpriteBatch SpriteBatch;
    public static GraphicsDeviceManager Graphics;
    public static Vector2 VisualShift = new Vector2(0, 0);
    private static Rectangle VisionArea;

    public static void UpdateVisionArea()
    {
        VisionArea = new Rectangle(-100, -100, Graphics.PreferredBackBufferWidth+100, Graphics.PreferredBackBufferHeight+100);
    }
    public static void UpdateVisionArea (int x, int y, int width, int height)
    {
        VisionArea = new Rectangle(x, y, width, height);
    }
    public static bool IsInVisionArea(Vector2 pos)
    {
        if (VisionArea.Contains(pos)) return true;
        else return false;
    }
    public static void RenderObject(IObject obj)
    {
        foreach (var sprite in obj.Sprites)
        {
            if (sprite.ImageId == -1)
                continue;

            Vector2 v = obj.Pos + sprite.ImagePos - VisualShift;
            if (IsInVisionArea(v))
            {
                SpriteBatch.Draw(
                texture: LoadableObjects.Textures[sprite.ImageId],
                position: obj.Pos - VisualShift + sprite.ImagePos,
                sourceRectangle: null,
                Color.White,
                rotation: 0,
                origin: Vector2.Zero,
                scale: 1,
                SpriteEffects.None,
                layerDepth: obj.Layer);
            }            
        }
    }
    
    public static void RenderAnimation (IAnimated a)
    {
        if (a.Animation.ActiveAnimation != null &&
            a.Animation.ActiveAnimation.IsActive)
        {            
            var centerShift = a.Animation.ActiveAnimation.IsCentered ?
                new Vector2(a.Animation.ActiveAnimation.CurrentFrame.Width / 2,
                a.Animation.ActiveAnimation.CurrentFrame.Height / 2) :
                Vector2.Zero;
            Vector2 v = a.Animation.Pos - VisualShift;
            if (IsInVisionArea(v))
            {
                SpriteBatch.Draw(
               LoadableObjects.Textures[a.Animation.ActiveAnimation.GetPictureId()],
                v,
                new Rectangle(
                    a.Animation.ActiveAnimation.CurrentFrame.Point,
                    new Point(
                    a.Animation.ActiveAnimation.CurrentFrame.Width,
                    a.Animation.ActiveAnimation.CurrentFrame.Height)),
                Color.White,
                0,
                centerShift,
                1,
                SpriteEffects.None, a.Animation.Layer);
            }
        }
    }
   
    public static void DrawLine(Vector2 point1, Vector2 point2, Color color)
    {
        DrawLine(point1, point2, color, 1);
    }

    public static void DrawLine(Vector2 point1, Vector2 point2, Color color, int width)
    {
        Texture2D pixel = new Texture2D(SpriteBatch.GraphicsDevice, 1, width);
        var colorArray = new Color[width];
        for (int i = 0; i < width; i++)
        {
            colorArray[i] = Color.White;
        }
        pixel.SetData(colorArray);
        float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
        float length = Vector2.Distance(point1, point2);

        SpriteBatch.Draw(pixel, point1, null, color, angle, Vector2.Zero, new Vector2(length, 1), SpriteEffects.None, 0);
    }

    public static void DrawRectangle (int x, int y, int width, int height, Color color)
    {
        DrawRectangle(x, y,width, height, color, 1);
    }

    public static void DrawRectangle(int x, int y, int width, int height, Color color, int contourWidth)
    {
        Vector2 leftTop = new Vector2(x, y);
        Vector2 rightTop = new Vector2(x + width, y);
        Vector2 leftBottom = new Vector2(x, y + height);
        Vector2 rightBottom = new Vector2(x + width, y + height);
        DrawLine(leftTop, rightTop, color, contourWidth);
        DrawLine(rightTop, rightBottom, color, contourWidth);
        DrawLine(rightBottom, leftBottom, color, contourWidth);
        DrawLine(leftBottom, leftTop, color, contourWidth);
    }

    public static void FillRectangle (int x, int y, int width, int height, Color color)
    {
        Texture2D pixel = new Texture2D(SpriteBatch.GraphicsDevice, 1, 1);
        SpriteBatch.Draw(pixel, new Rectangle(x, y, width, height), color);
    }

    public static void DrawCircle(Vector2 center, float radius, Color color)
    {
        float angle = 0f;
        int segments = 180;
        float angleIncrement = MathHelper.TwoPi / segments;

        Vector2 p1 = new Vector2(radius, 0) + center;
        Vector2 p2;

        for (int i = 0; i < segments; i++)
        {
            angle += angleIncrement;
            p2 = new Vector2(radius * (float)Math.Cos(angle), radius * (float)Math.Sin(angle)) + center;
            DrawLine(p1, p2, color);
            p1 = p2;
        }
    }

    public static void FillCircle (int radius)
    {
        Texture2D circleTexture = new Texture2D(SpriteBatch.GraphicsDevice, radius * 2, radius * 2);

        Color[] data = new Color[circleTexture.Width * circleTexture.Height];

        float diameter = radius * 2f;
        float radiusSquared = radius * radius;

        for (int x = 0; x < circleTexture.Width; x++)
        {
            for (int y = 0; y < circleTexture.Height; y++)
            {
                float distanceSquared = (x - radius) * (x - radius) + (y - radius) * (y - radius);
                if (distanceSquared <= radiusSquared)
                {
                    data[x + y * circleTexture.Width] = Color.White;
                }
                else
                {
                    data[x + y * circleTexture.Width] = Color.Transparent;
                }
            }
        }

        circleTexture.SetData(data);
        SpriteBatch.Draw(circleTexture, new Vector2(300, 300), Color.White);
    }
}
