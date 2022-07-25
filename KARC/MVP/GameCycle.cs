using KARC.Objects;
using KARC.WitchEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KARC.MVP
{
    public class GameCycle : IGameplayModel
    {
        public event EventHandler<GameplayEventArgs> Updated = delegate { };
        public event EventHandler<GameOverEventArgs> GameFinished = delegate { };

        bool _isPaused;

        bool _isGameOver;

        private int _currentId;

        private char[,] _map;
        private int _tileSize = 100;
        private Vector2 _playerShift;
        private int _score;        

        private Random _random = new Random();

        private int _finishPos;
        private float _distance;

        public (int width, int height) Resolution;
        public int PlayerId { get; set; }
        public Dictionary<int, IObject> Objects { get; set; }
        public Dictionary<int, ISolid> SolidObjects { get; set; }
        public Dictionary<int, ITrigger> Triggers { get; set; }

        private void GenerateMap(int width, int height)
        {
            _map = new char[width, height];
            _map[width / 2, height - 1] = 'P';
            _map[1, 0] = '1';
            _map[1, _map.GetLength(1) - 1] = '1';
            _map[_map.GetLength(0) - 2, 0] = '2';
            _map[_map.GetLength(0) - 2, _map.GetLength(1) - 1] = '2';

            _map[2, 0] = 'F';
            _map[_map.GetLength(0) - 3, 1] = 'F';
        }
        private void GenerateEnemies(float enemiesFraction)
        {
            for (int y = 2; y < _map.GetLength(1) - 10; y++)
                for (int x = 2; x < _map.GetLength(0) - 3; x++)
                {
                    bool isClearBorders = true;
                    for (int yN = -1; yN <= 1; yN++)
                        for (int xN = -1; xN <= 1; xN++)
                        {
                            if (_map[x+xN, y+yN] != '\0')
                                isClearBorders = false;
                        }
                    if ((_map[x, y] == '\0' && isClearBorders) && _random.NextDouble() <= enemiesFraction)
                    {
                        _map[x, y] = 'C';
                    }
                }
        }
        public void Initialize((int width, int height) resolution)
        {
            Resolution = resolution;
            Objects = new Dictionary<int, IObject>();
            SolidObjects = new Dictionary<int, ISolid>();
            Triggers = new Dictionary<int, ITrigger>();

            _playerShift = Vector2.Zero;
            _score = 0;

            _isPaused = false;
            _isGameOver = false;
            _currentId = 1;
            bool isPlacedPlayer = false;
            GenerateMap(11, 2000);
            GenerateEnemies(0.020f);

            for (int y = 0; y < _map.GetLength(1); y++)
                for (int x = 0; x < _map.GetLength(0); x++)
                {
                    if (_map[x, y] != '\0')
                    {
                        IObject generatedObject = null;
                        if (int.TryParse(_map[x, y].ToString(), out int corner1))
                        {
                            _map[x, y] = '\0';
                            for (int yCorner = 0; yCorner < _map.GetLength(1); yCorner++)
                                for (int xCorner = 0; xCorner < _map.GetLength(0); xCorner++)
                                {
                                    if (int.TryParse(_map[xCorner, yCorner].ToString(), out int corner2))
                                    {
                                        if (corner1==corner2)
                                        {
                                            generatedObject = GenerateObject('W', x, y, xCorner, yCorner);                                           
                                            _map[xCorner, yCorner] = '\0';
                                        }
                                    }
                                }
                        }
                        else if (_map[x,y] == 'F')
                        {
                            _map[x, y] = '\0';
                            for (int yCorner = 0; yCorner < _map.GetLength(1); yCorner++)
                                for (int xCorner = 0; xCorner < _map.GetLength(0); xCorner++)
                                {
                                    if (_map[xCorner, yCorner] == 'F')
                                    {
                                        generatedObject = GenerateObject('F', x, y, xCorner, yCorner);
                                        var trigger = (ITrigger)generatedObject;
                                        trigger.Triggered += CalculateWin;
                                        _map[xCorner, yCorner] = '\0';                                        
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

                        if (generatedObject is ITrigger t)
                            Triggers.Add(_currentId, t);
                        _currentId++;
                    }
                }

            _distance = Math.Abs(_finishPos - Objects[PlayerId].Pos.Y);

            _playerShift = new Vector2(
                    -Resolution.width / 2 + Objects[PlayerId].Pos.X,
                    -Resolution.height * 0.8f + Objects[PlayerId].Pos.Y
                );

            Updated.Invoke(this, new GameplayEventArgs()
            {
                Objects = Objects,
                POVShift = _playerShift,
                Score = _score,
                Speed = (int)Objects[PlayerId].Speed.Y,
                DistanceToFinish = 1
            });
        }
        private IObject GenerateObject(char sign, int xTile, int yTile)
        {
            float x = xTile * _tileSize;
            float y = yTile * _tileSize;
            IObject generatedObject = null;
            if (sign == 'P' || sign == 'C')
            {
                //generatedObject = Factory.CreateClassicCar(x + _tileSize / 2, y + _tileSize / 2, speed: new Vector2(0, 0));
                generatedObject = Factory.CreateComplexCar(x + _tileSize / 2, y + _tileSize / 2, speed: new Vector2(0, 0));
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
                generatedObject = Factory.CreateWall(xInit, yInit, 
                    xEnd+_tileSize, yEnd + _tileSize, tileSize: _tileSize);
            }
            if (sign == 'F')
            {
                generatedObject = Factory.CreateTrigger(xInit, yInit,
                    xEnd + _tileSize, yEnd + _tileSize, spriteId: (byte)Factory.ObjectTypes.finish, tileSize:_tileSize);
                _finishPos = (int)yInit;
            }
            return generatedObject;            
        }       

        public void Update()
        {
            if (!_isPaused)            
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
                        if (i == j || processedObjects.Contains((j, i)) || Objects[i].Speed == Vector2.Zero) continue;
                        CalculateObstacleCollision((collisionObjects[i], i), (collisionObjects[j], j));
                        processedObjects.Add((i, j));
                    }
                    foreach (var t in Triggers)
                    {
                        CalculateTrigger(i, t.Value);
                    }
                }
                Car player = (Car)Objects[PlayerId];
                if (!player.IsLive)                   
                    ProcessGameOver(isWin : false);
                
               // _playerShift += Objects[PlayerId].Pos - playerInitPos;
               _playerShift.Y += Objects[PlayerId].Pos.Y - playerInitPos.Y;

                var s = Objects.OrderBy(pair => pair.Value.Layer);
                Dictionary<int, IObject> sortedObjects = new Dictionary<int, IObject>(s);

                if((int)Objects[PlayerId].Speed.Y<0) _score++;
                Updated.Invoke(this, new GameplayEventArgs
                {
                    Objects = sortedObjects,
                    POVShift = _playerShift,
                    Score = _score,
                    Speed = (int)Objects[PlayerId].Speed.Y,
                    DistanceToFinish = Math.Abs(_finishPos - Objects[PlayerId].Pos.Y) / _distance
                });
            }            
        }

        private void CalculateObstacleCollision((Vector2 initPos, int Id) obj1, (Vector2 initPos, int Id) obj2)
        {
            bool isCollided = false;

            Vector2 oppositeDirection;
            
            //while (RectangleCollider.IsCollided(SolidObjects[obj1.Id].Collider, SolidObjects[obj2.Id].Collider))
            while (RectangleCollider.IsCollided(SolidObjects[obj1.Id].Colliders, SolidObjects[obj2.Id].Colliders))
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

        private void CalculateTrigger (int i, ITrigger t)
        {
            if (RectangleCollider.IsCollided(SolidObjects[i].Colliders, t.Collider))
            {
                t.OnTrigger(Objects[i], i);
            }
        }       
        private void CalculateWin(object sender, TriggerEventArgs e)
        {
            if (e.ActivatorId == PlayerId)                
                ProcessGameOver(isWin : true);
        }

        private void ProcessGameOver (bool isWin)
        {
            if (!_isGameOver)
            {
                _isGameOver = true;
                GameFinished.Invoke(this, new GameOverEventArgs { IsWin = isWin });
            }            
        }

        public void ChangePlayerSpeed(IGameplayModel.Direction dir)
        {           
            if (!_isPaused&&!_isGameOver)
            {
                Car p = (Car)Objects[PlayerId];
                switch (dir)
                {
                    case IGameplayModel.Direction.forward:
                        {
                            p.Speed += new Vector2(0, -20);
                            break;
                        }
                    case IGameplayModel.Direction.backward:
                        {
                            p.Speed += new Vector2(0, 0);
                            break;
                        }
                    case IGameplayModel.Direction.right:
                        {
                            p.Speed += new Vector2(8, 0);
                            break;
                        }
                    case IGameplayModel.Direction.left:
                        {
                            p.Speed += new Vector2(-8, 0);
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

