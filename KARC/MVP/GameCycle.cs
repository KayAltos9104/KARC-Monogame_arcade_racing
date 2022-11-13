using KARC.Objects;
using KARC.WitchEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static System.Formats.Asn1.AsnWriter;
using System.Xml.Linq;
using System.Security.AccessControl;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework.Graphics;

namespace KARC.MVP
{
    public class GameCycle : IGameplayModel
    {
        public event EventHandler<GameplayEventArgs> Updated = delegate { };
        public event EventHandler<GameOverEventArgs> GameFinished = delegate { };

        public GameTime GameTime { get; set; }

        private bool _isPaused;

        private bool _isGameOver;

        private int _currentId;

        private char[,] _map;        

        private int _screenWidth = 1000;
        private int _screenHeight = 1000;
        private int _tileSize = 100;


        private Vector2 _playerShift;
        private int _score;        

        private Random _random = new Random();

        private int _finishPos;
        private float _distance;
        private int _framesPerCollisionUpdate = 1;
        private int _framesPassed;

        public (int width, int height) Resolution;
        public int PlayerId { get; set; }
        public Dictionary<int, IObject> Objects { get; set; }
        public Dictionary<int, ISolid> SolidObjects { get; set; }
        public Dictionary<int, ITrigger> Triggers { get; set; }        
        public Dictionary<string, Timer> Timers { get; set; }
        public Dictionary<string, Factory.ObjectTypes> Effects { get; set; }
        private void GenerateMap(int width, int height)
        {
            _map = new char[width, height];
            _map[width / 2, height - 1] = 'P';
            //_map[width / 2, height - 3] = 'W';
            _map[1, 0] = '1';
            _map[1, _map.GetLength(1) - 1] = '1';
            _map[_map.GetLength(0) - 2, 0] = '2';
            _map[_map.GetLength(0) - 2, _map.GetLength(1) - 1] = '2';

            _map[2, 0] = 'F';
            _map[_map.GetLength(0) - 3, 1] = 'F';
            _map[5, 4995] = 'S';
            for (int y = 2; y < _map.GetLength(1) - 20; y++)
                for (int x = 2; x < _map.GetLength(0) - 3; x++)
                {
                    bool isClearBorders = true;
                    for (int yN = -1; yN <= 1; yN++)
                        for (int xN = -1; xN <= 1; xN++)
                        {
                            if (_map[x + xN, y + yN] != '\0')
                                isClearBorders = false;
                        }
                    if (_map[x, y] == '\0' && isClearBorders && _random.NextDouble() <= 0.001f)
                    {
                        _map[x, y] = 'W';
                    }
                }

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
                    if (_map[x, y] == '\0' && isClearBorders && _random.NextDouble() <= enemiesFraction)
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
            Timers = new Dictionary<string, Timer>();
            Effects = new Dictionary<string, Factory.ObjectTypes>();
            _playerShift = Vector2.Zero;
            _score = 0;

            _isPaused = false;
            _isGameOver = false;
            _currentId = 1;
            bool isPlacedPlayer = false;
            GenerateMap(11, 5000);
            GenerateEnemies(0.010f);          

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
                        }
                      
                        Objects.Add(_currentId, generatedObject);
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
                Effects = new List<(byte, int timeLeft)>(),
                DistanceToFinish = 1
            });
        }
        private IObject GenerateObject(char sign, int xTile, int yTile)
        {
            float x = xTile * _tileSize;
            float y = yTile * _tileSize;
            IObject generatedObject = null;
            if (sign == 'C')
            {                
                generatedObject = Factory.CreateComplexCar(
                    x + _tileSize / 2, y + _tileSize / 2, speed: new Vector2(0, _random.Next(-15, -5)));
            }
            else if (sign == 'P')
            {
                generatedObject = Factory.CreateComplexCar(
                    x + _tileSize / 2, y + _tileSize / 2, speed: new Vector2(0, _random.Next(0, 0)));
            }
            else if (sign == 'W')
            {
                generatedObject = Factory.CreateWall(x + _tileSize / 2, y + _tileSize / 2, _tileSize / 2);
            }
            else if (sign == 'S')
            {
                generatedObject = Factory.CreateShield(x + _tileSize / 2, y + _tileSize / 2);
                var trigger = (ITrigger)generatedObject;
                trigger.Triggered += GiveShield;
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
                (int screenX, int screenY) playerScreen = GetScreenNumber(Objects[PlayerId].Pos);               
                Vector2 playerInitPos = Objects[PlayerId].Pos;

                Dictionary<int, Vector2> collisionObjects = new Dictionary<int, Vector2>();
                //Обновление состояния объектов
                foreach (var i in Objects.Keys)
                {
                    Vector2 initPos = Objects[i].Pos;
                    var objectScreen = GetScreenNumber (initPos);
                    Objects[i].Update();                    
                //Запись тех объектов, для которых нужно обсчитывать столкновение
                    if (SolidObjects.ContainsKey(i))
                    {                        
                        if (IsOnNeighborScreen(playerScreen, objectScreen) || IsLongSolid(SolidObjects[i]))
                        {
                            collisionObjects.Add(i, initPos);
                            Objects[i].Update();
                        }
                        else if (_framesPassed >= _framesPerCollisionUpdate)
                        {
                            _framesPassed = 0;
                            collisionObjects.Add(i, initPos);
                            Objects[i].Update();
                        }
                    }  
                }

                //Обработка столкновений
                List<(int, int)> processedObjects = new List<(int, int)>();
                foreach (var i in collisionObjects.Keys)
                {
                    foreach (var j in collisionObjects.Keys)
                    {
                        if (i == j || processedObjects.Contains((j, i)) || collisionObjects[i] == Objects[i].Pos)
                            continue;
                        if (CalculateObstacleCollision((collisionObjects[i], i), (collisionObjects[j], j)))
                        {
                            CalculateCrushing(i, j);
                        }
                        processedObjects.Add((i, j));
                    }
                    foreach (var t in Triggers)
                    {
                        CalculateTrigger(i, t.Value);
                        if (!t.Value.IsActive)
                        {
                            Objects.Remove(t.Key);
                            Triggers.Remove(t.Key);
                        }
                    }
                }

                foreach (var timer in Timers.Values)
                {
                    timer.Update();
                }
                    

                Car player = (Car)Objects[PlayerId];
                if (!player.IsLive)                   
                    ProcessGameOver(isWin : false);    
                
               //Сдвиг игрока для смещения камеры
               _playerShift.Y += Objects[PlayerId].Pos.Y - playerInitPos.Y;

                //Сортировка объектов по слоям для отрисовки
                var s = Objects.OrderBy(pair => pair.Value.Layer);
                Dictionary<int, IObject> sortedObjects = new Dictionary<int, IObject>(s);

                if((int)Objects[PlayerId].Speed.Y<0) _score++;

                _framesPassed++;

                var effectsOut = new List<(byte, int timeLeft)>();
                foreach (var e in Effects)
                {
                    effectsOut.Add(((byte)e.Value, Timers[e.Key].Time));
                }

                Updated.Invoke(this, new GameplayEventArgs
                {
                    Objects = sortedObjects,
                    POVShift = _playerShift,
                    Score = _score,                    
                    Speed = (int)Objects[PlayerId].Speed.Y,
                    Effects = effectsOut,
                    DistanceToFinish = Math.Abs(_finishPos - Objects[PlayerId].Pos.Y) / _distance
                });                
            }            
        }

        private (int X, int Y) GetScreenNumber (Vector2 pos)
        {
            return ((int)pos.X / _screenWidth, (int)pos.Y / _screenHeight);
        }
        

        private bool IsOnNeighborScreen((int X, int Y) screen1, (int X, int Y) screen2)
        {
            for (int y = screen1.Y - 1; y <= screen1.Y + 1; y++)
                for (int x = screen1.X - 1; x <= screen1.X + 1; x++)
                {
                    if (screen2 == (x, y))
                        return true;
                }
            return false;
        }
        private bool IsOnNeighborScreen(Vector2 pos1, Vector2 pos2)
        {
            var screen1 = GetScreenNumber(pos1);
            var screen2 = GetScreenNumber(pos2);
            return IsOnNeighborScreen(screen1, screen2);
        }
        private bool IsLongSolid(ISolid o)
        {
            return o.Colliders[0].Collider.Boundary.Width > _screenWidth ||
                           o.Colliders[0].Collider.Boundary.Height > _screenHeight;
        }
        private bool CalculateObstacleCollision((Vector2 initPos, int Id) obj1, (Vector2 initPos, int Id) obj2)
        {
            Vector2 oppositeDirection;
            bool isCollided = false;
            byte tries = 100;
            while (RectangleCollider.IsCollided(SolidObjects[obj1.Id].Colliders, SolidObjects[obj2.Id].Colliders)&&tries>0)
            {
                if(tries > 1)
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
                    tries--;
                }
                else
                {
                    var oppositeDirection1 = Objects[obj1.Id].Pos - obj1.initPos;
                    var oppositeDirection2 = Objects[obj2.Id].Pos - obj2.initPos;
                    Objects[obj1.Id].Move(Objects[obj1.Id].Pos - 10 * oppositeDirection1);
                    Objects[obj2.Id].Move(Objects[obj2.Id].Pos - 10 * oppositeDirection2);
                    tries--;
                }                
            }
            return isCollided;
        }
        private void CalculateCrushing (int Id1, int Id2)
        {
            Objects[Id1].Speed = new Vector2(0, 0);
            Objects[Id2].Speed = new Vector2(0, 0);
            if (Objects[Id1] is Car)
            {
                Car c = (Car)Objects[Id1];
                c.Die();
            }
            if (Objects[Id2] is Car)
            {
                Car c = (Car)Objects[Id2];
                c.Die();
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

        private void GiveShield(object sender, TriggerEventArgs e)
        {            
            if (e.ActivatorId == PlayerId)
            {
                var playerCar = Objects[PlayerId] as Car;
                playerCar.IsImmortal = true;
                Timer immortalTimer = new Timer(600); //Пока в качестве времени выступают тики, что неправильно. Тут примерно 10 секунд
                var timerId = Guid.NewGuid().ToString();
                Effects.Add(timerId, Factory.ObjectTypes.shield);
                immortalTimer.TimeIsOver +=
                (s, a) =>
                {                    
                    playerCar.IsImmortal = false;
                    Effects.Remove(timerId);
                };
               
                Timers.Add(timerId, immortalTimer);
            }                
            (sender as Trigger2D).IsActive = false;   
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
                            p.Speed += new Vector2(9, 0);
                            break;
                        }
                    case IGameplayModel.Direction.left:
                        {
                            p.Speed += new Vector2(-9, 0);
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


