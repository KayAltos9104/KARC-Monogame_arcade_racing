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

        bool _isPaused;

        bool _isGameOver;

        private int _currentId;

        private char[,] _map = new char[11, 2000];
        private int _tileSize = 100;
        private Vector2 _playerShift;

        public (int width, int height) Resolution;
        public int PlayerId { get; set; }
        public Dictionary<int, IObject> Objects { get; set; }
        public Dictionary<int, ISolid> SolidObjects { get; set; }
        public void Initialize((int width, int height) resolution)
        {
            Resolution = resolution;
            Objects = new Dictionary<int, IObject>();
            SolidObjects = new Dictionary<int, ISolid>();
            _playerShift = Vector2.Zero;

            _isPaused = false;
            _isGameOver = false;

            _map[5, 7] = 'P';
            _map[4, 2] = 'C';
            _map[6, 2] = 'C';
            _map[0, 0] = '1';
            _map[0, 1999] = '1';
            _map[_map.GetLength(0) - 1, 0] = '2';
            _map[_map.GetLength(0) - 1, 1999] = '2';
            
            _currentId = 1;
            bool isPlacedPlayer = false;
            for (int y = 0; y < _map.GetLength(1); y++)
                for (int x = 0; x < _map.GetLength(0); x++)
                {
                    if (_map[x, y] != '\0')
                    {
                        IObject generatedObject = null;
                        if (int.TryParse(_map[x, y].ToString(), out int corner1))
                        {
                            for (int yCorner = 0; yCorner < _map.GetLength(1); yCorner++)
                                for (int xCorner = 0; xCorner < _map.GetLength(0); xCorner++)
                                {
                                    if (int.TryParse(_map[xCorner, yCorner].ToString(), out int corner2)&&(x!=xCorner||y!=yCorner))
                                    {
                                        if (corner1==corner2)
                                        {
                                            generatedObject = GenerateObject('W', x, y, xCorner, yCorner);
                                            _map[x, y] = '\0';
                                            _map[xCorner, yCorner] = '\0';
                                        }
                                    }
                                }
                        }
                        else
                        {
                            generatedObject = GenerateObject(_map[x, y], x, y);
                        }                       

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
                        if (generatedObject is ISolid s)
                            SolidObjects.Add(_currentId, s);
                        _currentId++;
                    }
                }
            _playerShift = new Vector2(
                    -Resolution.width / 2 + Objects[PlayerId].Pos.X,
                    -Resolution.height * 0.8f + Objects[PlayerId].Pos.Y
                );
            Updated.Invoke(this, new GameplayEventArgs()
            {
                Objects = Objects,
                POVShift = _playerShift
            }) ;
        }

        private IObject GenerateObject(char sign, int xTile, int yTile)
        {
            float x = xTile * _tileSize;
            float y = yTile * _tileSize;
            IObject generatedObject = null;
            if (sign == 'P' || sign == 'C')
            {
                generatedObject = Factory.CreateClassicCar(x + _tileSize / 2, y + _tileSize / 2, speed: new Vector2(0, 0));
            }            
            return generatedObject;
        }

        private IObject GenerateObject(char sign, int xInitTile, int yInitTile, int xEndTile, int yEndTile)
        {
            float xInit = xInitTile * _tileSize;
            float yInit = yInitTile * _tileSize;
            float xEnd = xEndTile * _tileSize;
            float yEnd = yEndTile * _tileSize;
            IObject generatedObject = null;
            if (sign == 'W')
            {
                generatedObject = Factory.CreateWall(xInit + _tileSize / 2, yInit + _tileSize / 2, 
                    xEnd + _tileSize / 2, yEnd + _tileSize / 2);
            }

            return generatedObject;
        }       

        public void Update()
        {
            if (_isPaused)
            {

            }
            else
            {
                Vector2 playerInitPos = Objects[PlayerId].Pos;
                Dictionary<int, Vector2> collisionObjects = new Dictionary<int, Vector2>();
                foreach (var i in Objects.Keys)
                {
                    Vector2 initPos = Objects[i].Pos;
                    Objects[i].Update();
                    if (SolidObjects.ContainsKey(i))
                        collisionObjects.Add(i, initPos);
                }
                List<(int, int)> processedObjects = new List<(int, int)>();

                foreach (var i in collisionObjects.Keys)
                {
                    foreach (var j in collisionObjects.Keys)
                    {
                        if (i == j || processedObjects.Contains((j, i))) continue;
                        CalculateObstacleCollision((collisionObjects[i], i), (collisionObjects[j], j));
                        processedObjects.Add((i, j));
                    }
                }
                Car player = (Car)Objects[PlayerId];
                if (!player.IsLive)
                    _isGameOver = true;

                if (_isGameOver)
                {
                    _isPaused = true;
                    MessageBox gameOverMessage = new MessageBox(new Vector2(
                        Resolution.width/2-100, Resolution.height/2), 
                        "Игра окончена!\nВы проиграли!\nНажмите R для перезагрузки"
                        );
                    Objects.Add(_currentId, gameOverMessage);
                    _currentId++;
                }
                _playerShift += Objects[PlayerId].Pos - playerInitPos;
                Updated.Invoke(this, new GameplayEventArgs { Objects = Objects, POVShift = _playerShift });
            }            
        }

        private void CalculateObstacleCollision((Vector2 initPos, int Id) obj1, (Vector2 initPos, int Id) obj2)
        {
            bool isCollided = false;

            Vector2 oppositeDirection = new Vector2(0, 0);
            while (RectangleCollider.IsCollided(SolidObjects[obj1.Id].Collider, SolidObjects[obj2.Id].Collider))
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
                isCollided = true;
            }
            if (isCollided)
            {
                Objects[obj1.Id].Speed = new Vector2(0, 0);
                Objects[obj2.Id].Speed = new Vector2(0, 0);
                if (Objects[obj1.Id] is Car)
                {
                    Car c = (Car)Objects[obj1.Id];
                    c.IsLive = false;
                }
                if (Objects[obj2.Id] is Car)
                {
                    Car c = (Car)Objects[obj2.Id];
                    c.IsLive = false;
                }                
            }
        }

        public void ChangePlayerSpeed(IGameplayModel.Direction dir)
        {
            if (_isPaused)
            {

            }
            else
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
        }
        
        public void SwitchPause()
        {
            if(_isPaused)
                _isPaused = false;
            else
                _isPaused = true;
        }

    }
}
