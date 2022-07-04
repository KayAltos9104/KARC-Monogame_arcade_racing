using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace KARC.Objects
{
    public static class Factory
    {
        private static Dictionary<string, (byte type, int width, int height)> _objects = new Dictionary<string, (byte, int, int)>()
        {
            {"classicCar", ((byte)ObjectTypes.car, 77,100)},
            {"wall", ((byte)ObjectTypes.wall, 24,100)}
        };
        public static Car CreateClassicCar(float x, float y, Vector2 speed)
        {
            Car c = new Car(new Vector2(x, y));
            c.Sprites.Add((_objects["classicCar"].type, Vector2.Zero));
            c.Speed = speed;
            return c;
        }

        public static Wall CreateWall(float xInit, float yInit, float xEnd, float yEnd)
        {
            int segmentWidth = _objects["wall"].width;
            int segmentHeight = _objects["wall"].height;
            int width = Math.Abs(xEnd - xInit) == 0 ? segmentWidth : (int)Math.Abs(xEnd - xInit);
            int length = Math.Abs(yEnd - yInit) == 0 ? segmentHeight : (int)Math.Abs(yEnd - yInit);
            Wall w = new Wall(new Vector2(xInit, yInit), width, length);
            for (int i = 0; i < width/segmentWidth; i++)
                for (int j = 0; j < length/segmentHeight; j++)
                {
                    w.Sprites.Add((_objects["wall"].type, new Vector2(i*segmentWidth, j*segmentHeight)));
                }
            return w;
        }
        public enum ObjectTypes : byte
        {
            car,
            wall
        }
    }
}
