using KARC.WitchEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;


namespace KARC.Objects
{
    public static class Factory
    {
        private static Dictionary<string, (int type, int width, int height)> _objects = new Dictionary<string, (int, int, int)>()
        {
            {"classicCar", ((int)ObjectTypes.car, 77, 100)},
            {"wall", ((int)ObjectTypes.wall, 24, 24)},
            {"trigger", (-1, 20, 20)},
            {"finishCounterField",((int)ObjectTypes.finishCounterField, 1000, 28) },
            {"finishTape",((int)ObjectTypes.finish, 20, 20) },
            {"shield",((int)ObjectTypes.shield, 48, 48) },
        };
        public static Car CreateClassicCar(float x, float y, Vector2 speed)
        {
            Car c = new Car(new Vector2(x, y),  _objects["classicCar"].height, _objects["classicCar"].width);
            c.Sprites.Add((_objects["classicCar"].type, Vector2.Zero));            
            c.Speed = speed;
            return c;
        }

        public static Car CreateComplexCar(float x, float y, Vector2 speed)
        {
            Car c = new Car(new Vector2(x, y));
            var cabin = new RectangleCollider((int)x,(int)y, _objects["wall"].width, _objects["wall"].height);
            var hull = new RectangleCollider((int)x, (int)y, 
                _objects["classicCar"].width, _objects["classicCar"].height - _objects["wall"].height);
            c.Colliders.Add((new Vector2(_objects["classicCar"].width/2- _objects["wall"].width/2, 0), cabin));
            c.Colliders.Add((new Vector2(0, _objects["wall"].height), hull));
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

        public static Wall CreateWall(float x, float y, int size)
        {
            int segmentWidth = _objects["wall"].width;
            int segmentHeight = _objects["wall"].height;
            Wall w = new Wall(new Vector2(x, y), size, size);
            for (int i = 0; i < size / segmentWidth; i++)
                for (int j = 0; j < size / segmentHeight; j++)
                {
                    w.Sprites.Add((_objects["wall"].type, new Vector2(i * segmentWidth, j * segmentHeight)));
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
        public static Trigger2D CreateShield(float x, float y)
        {           
            Trigger2D t = new Trigger2D(new Vector2(x, y), _objects["shield"].width, _objects["shield"].height);
            t.Sprites.Add((_objects["shield"].type, new Vector2(0, 0)));
            return t;
        }
        public static Trigger2D CreateTrigger(float x, float y, int size, int spriteId)
        {
            int segmentWidth = _objects["trigger"].width;
            int segmentHeight = _objects["trigger"].height;            
            Trigger2D t = new Trigger2D(new Vector2(x, y), size, size);
            for (int i = 0; i < size / segmentWidth; i++)
                for (int j = 0; j < size / segmentHeight; j++)
                {
                    t.Sprites.Add((spriteId, new Vector2(i * segmentWidth, j * segmentHeight)));
                }
            return t;
        }

        public static FinishCounter CreateFinishCounter (float x, float y)
        {
            var f = new FinishCounter( pos: new Vector2(x, y), text: String.Empty, 
                carSign: (_objects["wall"].type, 
                new Vector2(0, (1+_objects["finishCounterField"].height - _objects["wall"].height) / 2.0f)),
                finishSign: ((int)ObjectTypes.finish, 
                new Vector2(0, (1+_objects["finishCounterField"].height - _objects["finishTape"].height) / 2.0f)),
                _objects["finishCounterField"].width);

            f.CarSignShift = _objects["wall"].width * 1.3f;
            f.FinishSignShift = -_objects["finishTape"].width * 1.3f;
            f.Sprites.Insert(0,(_objects["finishCounterField"].type, Vector2.Zero));

            return f;
        }

        public enum ObjectTypes : byte
        {
            car,
            wall,
            window,
            finish,
            finishCounterField,
            shield,
            explosion
        }
    }
}
