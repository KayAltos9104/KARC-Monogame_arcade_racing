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
