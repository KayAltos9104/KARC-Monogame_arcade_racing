using KARC.Objects;
using KARC.WitchEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace KARC.MVP
{
    public class GameCycle : IGameplayModel
    {
        public event EventHandler<GameplayEventArgs> Updated = delegate { };


        private int _currentId;

        private char[,] _map = new char[11, 500];
        private int _tileSize = 100;
        public int PlayerId { get; set; }
        public Dictionary<int, IObject> Objects { get; set; }
        public void Initialize()
        {
            Objects = new Dictionary<int, IObject>();
            //_map[5, 498] = 'P';
            _map[5, 7] = 'P';
            //_map[4, 4] = 'C';
            _map[6, 2] = 'C';
            //for (int y = 0; y < _map.GetLength(1); y++)
            //{
            //    _map[0, y] = 'W';
            //    _map[_map.GetLength(0) - 1, y] = 'W';
            //}

            _currentId = 1;
            bool isPlacedPlayer = false;
            for (int y = 0; y < _map.GetLength(1); y++)
                for (int x = 0; x < _map.GetLength(0); x++)
                {
                    if (_map[x, y] != '\0')
                    {
                        IObject generatedObject = GenerateObject(_map[x, y], x, y);
                        if (_map[x, y] == 'P' && !isPlacedPlayer)
                        {
                            PlayerId = _currentId;
                            isPlacedPlayer = true;
                            Objects.Add(_currentId, generatedObject);
                        }
                        else if (_map[x, y] != 'P')
                        {
                            Objects.Add(_currentId, generatedObject);
                        }
                        _currentId++;
                    }
                }

            Updated.Invoke(this, new GameplayEventArgs()
            {
                Objects = Objects,
                POVShift = new Vector2(Objects[PlayerId].Pos.X,
                Objects[PlayerId].Pos.Y)
            });
        }

        private IObject GenerateObject(char sign, int xTile, int yTile)
        {
            float x = xTile * _tileSize;
            float y = yTile * _tileSize;
            IObject generatedObject = null;
            if (sign == 'P' || sign == 'C')
            {
                generatedObject = CreateCar(x + _tileSize / 2, y + _tileSize / 2, spriteId: ObjectTypes.car, speed: new Vector2(0, 0));
            }
            else if (sign == 'W')
            {
                generatedObject = CreateWall(x + _tileSize / 2, y + _tileSize / 2, spriteId: ObjectTypes.wall);
            }
            return generatedObject;
        }

        private Car CreateCar(float x, float y, ObjectTypes spriteId, Vector2 speed)
        {
            Car c = new Car(new Vector2(x, y));               
            c.ImageId = (byte)spriteId;            
            c.Speed = speed;            
            return c;
        }

        private Wall CreateWall(float x, float y, ObjectTypes spriteId)
        {
            Wall w = new Wall();
            w.Pos = new Vector2(x, y);
            w.ImageId = (byte)spriteId;
            return w;
        }

        public void Update()
        {
            Vector2 playerInitPos = Objects[PlayerId].Pos;
            for (int i = 1; i <= Objects.Keys.Count; i++)
            {
                Vector2 objInitPos = Objects[i].Pos;
                Objects[i].Update();
                if (Objects[i] is ISolid p1 && objInitPos != Objects[i].Pos)
                {
                    for (int j = 1; j <= Objects.Keys.Count; j++)
                    {
                        if (i == j)
                            continue;
                        if (Objects[j] is ISolid p2)
                        {
                            bool isCollided = false;
                            while (RectangleCollider.IsCollided(p1.Collider, p2.Collider))
                            {                                
                                Vector2 oppositeDirection = Objects[i].Pos - objInitPos;
                                oppositeDirection.Normalize();                               
                                Objects[i].Move(Objects[i].Pos - oppositeDirection);
                                isCollided = true;
                            }
                            if (isCollided)
                                Objects[i].Speed = new Vector2(0, 0);
                        }
                    }
                }
            }            
            Vector2 playerShift = Objects[PlayerId].Pos - playerInitPos;
            Updated.Invoke(this, new GameplayEventArgs { Objects = Objects, POVShift = playerShift });
        }

        private void CalculateObstacleCollision ((Vector2 initPos, IObject form) obj1, (Vector2 initPos, IObject form) obj2)
        {
            bool isCollided = false;
            if (obj1.form is ISolid p1 && obj2.form is ISolid p2)
            {
                Vector2 oppositeDirection = new Vector2(0, 0);
                while (RectangleCollider.IsCollided(p1.Collider, p2.Collider))
                {
                    if (obj1.initPos != obj1.form.Pos)
                    {
                        oppositeDirection = obj1.form.Pos - obj1.initPos;
                        oppositeDirection.Normalize();
                        obj1.form.Move(obj1.form.Pos - oppositeDirection);
                    }
                    if (obj2.initPos != obj2.form.Pos)
                    {
                        oppositeDirection = obj2.form.Pos - obj2.initPos;
                        oppositeDirection.Normalize();
                        obj2.form.Move(obj2.form.Pos - oppositeDirection);
                    }
                    if (isCollided)
                    {
                        obj1.form.Speed = new Vector2(0, 0);
                        obj2.form.Speed = new Vector2(0, 0);
                    }                        
                }
            }
        }

        public void ChangePlayerSpeed(IGameplayModel.Direction dir)
        {
            Car p = (Car)Objects[PlayerId];
            switch (dir)
            {
                case IGameplayModel.Direction.forward:
                    {
                        p.Speed += new Vector2(0, -5);
                        break;
                    }
                case IGameplayModel.Direction.backward:
                    {
                        p.Speed += new Vector2(0, 5);
                        break;
                    }
                case IGameplayModel.Direction.right:
                    {
                        p.Speed += new Vector2(5, 0);
                        break;
                    }
                case IGameplayModel.Direction.left:
                    {
                        p.Speed += new Vector2(-5, 0);
                        break;
                    }
            }
        }
        public enum ObjectTypes : byte
        {
            car,
            wall
        }

    }
}
