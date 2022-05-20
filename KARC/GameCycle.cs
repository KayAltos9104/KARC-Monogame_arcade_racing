using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace KARC
{
    public class GameCycle : IGameplayModel
    {
        public event EventHandler<GameplayEventArgs> Updated = delegate { };
        

        private int _currentId;

        private char[,] _map = new char [10, 500];
        private int _tileSize = 100;       
        public int PlayerId { get; set;}
        public Dictionary<int, IObject> Objects { get; set; }
        public void Initialize()
        {
            Objects = new Dictionary<int, IObject>();
            _map[5, 498] = 'P';
            _map[4, 4] = 'C';
            _map[6, 4] = 'C';
            for (int y = 0; y < _map.GetLength(1); y++)
            {
                _map[0, y] = 'W';
                _map[_map.GetLength(0)-1, y] = 'W';
            }

            _currentId = 1;
            bool isPlacedPlayer = false;
            for (int y = 0; y < _map.GetLength(1); y++)
                for (int x = 0; x < _map.GetLength(0); x++)
                {                  
                    if (_map[x,y]!='\0')
                    {
                        IObject generatedObject = GenerateObject(_map[x, y], x, y);                        
                        if (_map[x, y] == 'P'&&!isPlacedPlayer)
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
                Objects = this.Objects,
                POVShift = new Vector2(this.Objects[PlayerId].Pos.X,
                this.Objects[PlayerId].Pos.Y)
            });
        }
        
        private IObject GenerateObject (char sign, int xTile, int yTile)
        {
            float x = xTile * _tileSize;
            float y = yTile * _tileSize;
            IObject generatedObject = null;
            if (sign == 'P'|| sign == 'C')
            {
                generatedObject = CreateCar(x+_tileSize / 2, y + _tileSize / 2, spriteId: ObjectTypes.car, speed: new Vector2(0, 0));
            }            
            else if (sign == 'W')
            {
                generatedObject = CreateWall(x + _tileSize / 2, y + _tileSize / 2, spriteId: ObjectTypes.wall);
            } 
            return generatedObject;
        }

        private Car CreateCar(float x, float y, ObjectTypes spriteId, Vector2 speed)
        {
            Car c = new Car();
            c.ImageId = (byte)spriteId;
            c.Pos = new Vector2(x, y);
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
            foreach (var o in Objects.Values)
            {                           
                o.Update();
            }
            Vector2 playerShift = Objects[PlayerId].Pos - playerInitPos;
            Updated.Invoke(this, new GameplayEventArgs { Objects = this.Objects, POVShift = playerShift });
        }

        public void ChangePlayerSpeed(IGameplayModel.Direction dir)
        {
            Car p = (Car)Objects[PlayerId];
            switch (dir)
            {
                case IGameplayModel.Direction.forward:
                    {
                        p.Speed += new Vector2(0, -1);                        
                        break;
                    }
                case IGameplayModel.Direction.backward:
                    {
                        p.Speed += new Vector2(0, 1);                      
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
        public enum ObjectTypes: byte
        {
            car,
            wall
        }
        
    }
}
