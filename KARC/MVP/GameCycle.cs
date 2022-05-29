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

        private char[,] _map = new char[11, 100];
        private int _tileSize = 100;
        public int PlayerId { get; set; }
        public Dictionary<int, IObject> Objects { get; set; }
        public void Initialize()
        {
            Objects = new Dictionary<int, IObject>();            
            _map[5, 98] = 'P';
            _map[4, 4] = 'C';
            _map[6, 2] = 'C';
            _map[5, 2] = 'W';
            for (int y = 0; y < _map.GetLength(1); y++)
            {
                _map[0, y] = 'W';
                _map[_map.GetLength(0) - 1, y] = 'W';
            }

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
            Wall w = new Wall(new Vector2(x, y));
            w.ImageId = (byte)spriteId;
            //w.ImageId = (byte)ObjectTypes.car;
            return w;
        }

        public void Update()
        {
            Vector2 playerInitPos = Objects[PlayerId].Pos;
            Dictionary<int, Vector2> collisionObjects = new Dictionary<int, Vector2>();
            foreach (var i in Objects.Keys)
            {
                Vector2 initPos = Objects[i].Pos;
                Objects[i].Update();
                collisionObjects.Add(i, initPos);
            }
            //List<int> collisions = new List<int>();
            foreach (var i in collisionObjects.Keys)
            {
                foreach (var j in collisionObjects.Keys)
                {
                    if (i==j) continue;
                    CalculateObstacleCollision((collisionObjects[i], i), (collisionObjects[j], j));
                    //collisions.Add(i);
                    //collisions.Add(j);
                }
            }           

            Vector2 playerShift = Objects[PlayerId].Pos - playerInitPos;
            Updated.Invoke(this, new GameplayEventArgs { Objects = Objects, POVShift = playerShift });
        }

        private void CalculateObstacleCollision((Vector2 initPos, int Id) obj1, (Vector2 initPos, int Id) obj2)
        {
            bool isCollided = false;
            if (Objects[obj1.Id] is ISolid p1 && Objects[obj2.Id] is ISolid p2)
            {
                Vector2 oppositeDirection = new Vector2(0, 0);
                while (RectangleCollider.IsCollided(p1.Collider, p2.Collider))
                {
                    if (obj1.initPos != Objects[obj1.Id].Pos)
                    {
                        oppositeDirection = Objects[obj1.Id].Pos - obj1.initPos;
                        oppositeDirection.Normalize();
                        Objects[obj1.Id].Move(Objects[obj1.Id].Pos - oppositeDirection);
                    }
                    if (obj2.initPos != Objects[obj2.Id].Pos)
                    {
                        oppositeDirection = Objects[obj2.Id].Pos - obj2.initPos;
                        oppositeDirection.Normalize();
                        Objects[obj2.Id].Move(Objects[obj2.Id].Pos - oppositeDirection);
                    }
                    if (isCollided)
                    {
                        Objects[obj1.Id].Speed = new Vector2(0, 0);
                        Objects[obj2.Id].Speed = new Vector2(0, 0);
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
