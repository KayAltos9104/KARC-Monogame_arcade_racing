using KARC.WitchEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace KARC.Objects
{
    public static class Factory
    {
        private static Dictionary<string, (int type, int width, int height)> _objects = new Dictionary<string, (int, int, int)>()
        {
            {"classicCar", ((int)ObjectTypes.car, 77,100)},
            {"wall", ((int)ObjectTypes.wall, 24,24)},
            {"trigger", (-1, 20, 20)},           
        };
        public static Car CreateClassicCar(float x, float y, Vector2 speed)
        {
            Car c = new Car(new Vector2(x, y),  _objects["classicCar"].height, _objects["classicCar"].width);
            c.Sprites.Add((_objects["classicCar"].type, Vector2.Zero));
            c.Speed = speed;
            return c;
        }

        public static Wall CreateWall(float xInit, float yInit, float xEnd, float yEnd, int tileSize)
        {
            int segmentWidth = _objects["wall"].width;
            int segmentHeight = _objects["wall"].height;
            int width = Math.Abs(xEnd - xInit) == 0 ? tileSize : (int)Math.Abs(xEnd - xInit);
            int length = Math.Abs(yEnd - yInit) == 0 ? tileSize : (int)Math.Abs(yEnd - yInit);
            Wall w = new Wall(new Vector2(xInit, yInit), width, length);
            for (int i = 0; i < width/segmentWidth; i++)
                for (int j = 0; j < length/segmentHeight; j++)
                {
                    w.Sprites.Add((_objects["wall"].type, new Vector2(i*segmentWidth, j*segmentHeight)));
                }
            return w;
        }

        public static Trigger2D CreateTrigger(float xInit, float yInit, float xEnd, float yEnd, int spriteId, int tileSize)
        {
            int segmentWidth = _objects["trigger"].width;
            int segmentHeight = _objects["trigger"].height;           
            int width = Math.Abs(xEnd - xInit) == 0 ? tileSize : (int)Math.Abs(xEnd - xInit);
            int length = Math.Abs(yEnd - yInit) == 0 ? tileSize : (int)Math.Abs(yEnd - yInit);
            Trigger2D t = new Trigger2D(new Vector2(xInit, yInit), width, length);
            for (int i = 0; i < width / segmentWidth; i++)
                for (int j = 0; j < length / segmentHeight; j++)
                {
                    t.Sprites.Add((spriteId, new Vector2(i * segmentWidth, j * segmentHeight)));
                }
            return t;
        }

        public enum ObjectTypes : byte
        {
            car,
            wall,
            window,
            finish,
        }
    }
}
