using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace KARC.WitchEngine.Primitives;
public static class Graphics2D
{
    public static SpriteBatch SpriteBatch;
    public static void DrawLine(Vector2 point1, Vector2 point2, Color color)
    {
        Texture2D pixel = new Texture2D(SpriteBatch.GraphicsDevice, 1, 1);
        pixel.SetData(new[] { Color.White });

        float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
        float length = Vector2.Distance(point1, point2);

        SpriteBatch.Draw(pixel, point1, null, color, angle, Vector2.Zero, new Vector2(length, 1), SpriteEffects.None, 0);
    }

    public static void DrawRectangle (int x, int y, int width, int height, Color color)
    {
        Vector2 leftTop = new Vector2 (x, y);
        Vector2 rightTop = new Vector2(x + width, y);
        Vector2 leftBottom = new Vector2(x, y + height);
        Vector2 rightBottom = new Vector2(x + width, y + height);
        DrawLine(leftTop, rightTop, color);
        DrawLine(leftTop, leftBottom, color);
        DrawLine(rightTop, rightBottom, color);
        DrawLine(leftBottom, rightBottom, color);
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
