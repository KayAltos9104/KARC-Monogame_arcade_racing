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

        private char[,] _map = new char [10,8];
        private int _tileSize = 100;       
        public int PlayerId { get; set;}
        public Dictionary<int, IObject> Objects { get; set; }
        public void Initialize()
        {
            Objects = new Dictionary<int, IObject>();
            _map[5, 6] = 'P';
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
                    if (_map[x, y]=='P' && !isPlacedPlayer)
                    {
                        Car player = new Car();
                        player.ImageId = 1;
                        player.Pos = new Vector2(x*_tileSize - 33 +_tileSize/2, y*_tileSize-50 + _tileSize / 2);
                        player.Speed = new Vector2(0, 0);
                        PlayerId = _currentId;                        
                        Objects.Add(_currentId, player);
                        isPlacedPlayer = true;
                        _currentId++;
                    }
                    else if (_map[x, y] == 'C')
                    {
                        Car anotherCar = new Car();
                        anotherCar.Pos = new Vector2(x * _tileSize - 33 + _tileSize / 2, y * _tileSize - 50 + _tileSize / 2);
                        anotherCar.Speed = new Vector2(0, 0);
                        anotherCar.ImageId = 1;
                        Objects.Add(_currentId, anotherCar);
                        _currentId++;
                    }
                    else if (_map[x,y] == 'W')
                    {
                        Wall w = new Wall();
                        w.Pos = new Vector2(x * _tileSize - 12 + _tileSize / 2, y * _tileSize - 50 + _tileSize / 2);                        
                        w.ImageId = 2;
                        Objects.Add(_currentId, w);
                        _currentId++;
                    }
                }
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
                        p.Speed += new Vector2(1, 0);                       
                        break;
                    }
                case IGameplayModel.Direction.left:
                    {
                        p.Speed += new Vector2(-1, 0);                        
                        break;
                    }
            }
        }

        
    }
}
