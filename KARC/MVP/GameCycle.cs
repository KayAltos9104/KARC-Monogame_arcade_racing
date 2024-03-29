﻿using KARC.Objects;
using KARC.WitchEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using KARC.WitchEngine.Animations;
using KARC.Models;
using KARC.Maps;
using KARC.Settings;
using KARC.Animations;

namespace KARC.MVP;

public class GameCycle : IGameplayModel
{
    public event EventHandler<GameplayEventArgs> Updated = delegate { };
    public event EventHandler<GameOverEventArgs> GameFinished = delegate { };

    private Random _random = new Random();

    private double _deltaSecondsCounter = 0;
    private double _deltaTime = 0;

    private bool _isPaused;

    private bool _isGameOver;

    private MapBuilder _mapBuilder;
    private Map _map;

    private Vector2 _playerShift;
    private int _finishPos;
    private float _distance;

    private int _framesPassed;
    
    public ObjectsController ObjectsController { get; set; }
    public GameParameters GameParameters { get; set; }
    public GameTime GameTime { get; set; }
     
    public void Initialize((int width, int height) resolution)
    {
        GameParameters = new GameParameters();
        GameParameters.Resolution = resolution;        
        ObjectsController = new ObjectsController();
        ObjectsController.ObstacleGenerator.Width = GameParameters.TileSize / 2;
        ObjectsController.ObstacleGenerator.Height = GameParameters.TileSize / 2;

        ObjectsController.ScoreIncreased += GameParameters.IncreaseScore;

        _mapBuilder = new FirstLevelBuilder();
        //_mapBuilder = new TestLevelBuilder();
        _mapBuilder.GenerateMap();
        _map = _mapBuilder.GetMap();

        _playerShift = Vector2.Zero;
        GameParameters.Score = 0;

        _isPaused = false;
        _isGameOver = false;       

        for (int y = 0; y < _map.Height; y++)
            for (int x = 0; x < _map.Width; x++)
            {
                if (_map.GameField[x, y] != '\0')
                {  
                    if (int.TryParse(_map.GameField[x, y].ToString(), out int corner1))
                    {
                        _map.GameField[x, y] = '\0';
                        for (int yCorner = 0; yCorner < _map.Height; yCorner++)
                            for (int xCorner = 0; xCorner < _map.Width; xCorner++)
                            {
                                if (int.TryParse(_map.GameField[xCorner, yCorner].ToString(), out int corner2))
                                {
                                    if (corner1==corner2)
                                    {
                                        GenerateObject('W', x, y, xCorner, yCorner);                                           
                                        _map.GameField[xCorner, yCorner] = '\0';
                                    }
                                }
                            }
                    }
                    else if (_map.GameField[x,y] == 'F')
                    {
                        _map.GameField[x, y] = '\0';
                        for (int yCorner = 0; yCorner < _map.Height; yCorner++)
                            for (int xCorner = 0; xCorner < _map.Width; xCorner++)
                            {
                                if (_map.GameField[xCorner, yCorner] == 'F')
                                {
                                    GenerateObject('F', x, y, xCorner, yCorner);                                    
                                    _map.GameField[xCorner, yCorner] = '\0';                                        
                                }
                            }
                    }
                    else
                    {
                        GenerateObject(_map.GameField[x, y], x, y);
                    }
                   
                }
            }

        _distance = Math.Abs(_finishPos - ObjectsController.Player.Object.Pos.Y);

        _playerShift = new Vector2(
                -GameParameters.Resolution.width / 2 + ObjectsController.Player.Object.Pos.X,
                -GameParameters.Resolution.height * 0.8f + ObjectsController.Player.Object.Pos.Y
            );

        Updated.Invoke(this, new GameplayEventArgs()
        {
            Objects = ObjectsController.Storage.Objects,
            POVShift = _playerShift,
            Score = GameParameters.Score,
            Speed = (int)ObjectsController.Player.Object.Speed.Y,
            Effects = new List<(byte, int timeLeft)>(),
            DistanceToFinish = 1
        });
    }
    private void GenerateObject(char sign, int xTile, int yTile)
    {
        int x = xTile * GameParameters.TileSize;
        int y = yTile * GameParameters.TileSize;
        IObject generatedObject;
        if (sign == 'C')
        {            
            //ObjectsController.CarGenerator.CreateObject(x + GameParameters.TileSize / 2, y + GameParameters.TileSize / 2);
            ObjectsController.CarGenerator.CreateObject(x, y);
            generatedObject = ObjectsController.CarGenerator.GetObject();
            (generatedObject as Car).Speed = new Vector2(0, _random.Next(-800, -400));
        }
        else if (sign == 'P')
        {
            ObjectsController.PlayerGenerator.CreateObject(x + GameParameters.TileSize / 2, y + GameParameters.TileSize / 2);
            //ObjectsController.PlayerGenerator.CreateObject(x, y);
        }
        else if (sign == 'W')
        {
            ObjectsController.ObstacleGenerator.CreateObject(x + GameParameters.TileSize / 2, y + GameParameters.TileSize / 2);
            //ObjectsController.ObstacleGenerator.CreateObject(x, y); 
        }
        else if (sign == 'S')
        {
            ObjectsController.ShieldGenerator.CreateObject(x + GameParameters.TileSize / 2, y + GameParameters.TileSize / 2);
            //ObjectsController.ShieldGenerator.CreateObject(x, y);
        }
    }
    private void GenerateObject(char sign, int xInitTile, int yInitTile, int xEndTile, int yEndTile)
    {
        int xInit = xInitTile * GameParameters.TileSize;
        int yInit = yInitTile * GameParameters.TileSize;
        int xEnd = xEndTile * GameParameters.TileSize;
        int yEnd = yEndTile * GameParameters.TileSize;
        if (sign == 'W')
        {
            ObjectsController.WallGenerator.Width = xEnd - xInit + GameParameters.TileSize;
            ObjectsController.WallGenerator.Height = yEnd - yInit + GameParameters.TileSize;
            ObjectsController.WallGenerator.CreateObject(xInit, yInit);                        
        }
        if (sign == 'F')
        {
            ObjectsController.FinishGenerator.Width = xEnd - xInit + GameParameters.TileSize;
            ObjectsController.FinishGenerator.Height = yEnd - yInit + GameParameters.TileSize;
            ObjectsController.FinishGenerator.CreateObject(xInit, yInit);
            var trigger = (ITrigger)ObjectsController.Finish.Object;
            trigger.Triggered += CalculateWin;
            _finishPos = (int)ObjectsController.Finish.Object.Pos.Y;            
        }        
    }       
    public void Update()
    {
        // Если пауза, то ничего не обновляем
        if (_isPaused) return;            
        // На каком экране находится игрок
        (int screenX, int screenY) playerScreen = GetScreenNumber(ObjectsController.Player.Object.Pos);
        // Сохраняет позицию игрока на начало цикла
        Vector2 playerInitPos = ObjectsController.Player.Object.Pos;

        // Объекты, для которых будем обсчитывать столкновения
        Dictionary<int, Vector2> collisionObjects = new Dictionary<int, Vector2>();

        //Обновление состояния объектов
        foreach (var i in ObjectsController.Storage.Objects.Keys)
        {
            Vector2 initPos = ObjectsController.Storage.Objects[i].Pos;
            (int screenX, int screenY) objectScreen = GetScreenNumber(initPos);
            ObjectsController.Storage.Objects[i].Update(GameTime);
           
            //Запись тех объектов, для которых нужно обсчитывать столкновение
            if (ObjectsController.Storage.SolidObjects.ContainsKey(i))
            {
                if (IsOnNeighborScreen(playerScreen, objectScreen) || IsLongSolid(ObjectsController.Storage.SolidObjects[i]))
                {
                    collisionObjects.Add(i, initPos);                    
                }
                else if (_framesPassed >= GameParameters.FramesPerCollisionUpdate)
                {
                    _framesPassed = 0;
                    collisionObjects.Add(i, initPos);                    
                }
            }
        }

        //Обработка столкновений
        List<(int, int)> processedObjects = new List<(int, int)>();
        foreach (var i in collisionObjects.Keys)
        {
            foreach (var j in collisionObjects.Keys)
            {
                if (i == j || processedObjects.Contains((j, i)) || collisionObjects[i] == ObjectsController.Storage.Objects[i].Pos)
                    continue;
                if (CalculateObstacleCollision((collisionObjects[i], i), (collisionObjects[j], j)))
                {
                    CalculateCrushing(i, j);
                }
                processedObjects.Add((i, j));
            }
            var inactiveTriggers = new List<int>();
            foreach (var t in ObjectsController.Storage.Triggers)
            {
                CalculateTrigger(i, t.Value);
                if (!t.Value.IsActive)
                {
                    ObjectsController.Storage.Objects.Remove(t.Key);
                    inactiveTriggers.Add(t.Key);                    
                }
            }
            foreach (var t in inactiveTriggers)
            {
                ObjectsController.Storage.Triggers.Remove(t);
            }
        }

        _deltaSecondsCounter += GameTime.ElapsedGameTime.TotalMilliseconds;

        if (_deltaSecondsCounter > 1000)
        {
            foreach (var timer in ObjectsController.Storage.Timers.Values)
            {
                timer.Update();
            }
            _deltaSecondsCounter = 0;
        }

        Car player = (Car)ObjectsController.Player.Object;

        if (!player.IsLive)
            ProcessGameOver(isWin: false);

        //Сдвиг игрока для смещения камеры
        _playerShift.Y += ObjectsController.Player.Object.Pos.Y - playerInitPos.Y;

        //Сортировка объектов по слоям для отрисовки
        var s = ObjectsController.Storage.Objects.OrderBy(pair => pair.Value.Layer);
        Dictionary<int, IObject> sortedObjects = new Dictionary<int, IObject>(s);

        if ((int)ObjectsController.Player.Object.Speed.Y < 0) GameParameters.Score++;

        _framesPassed++;

        var effectsOut = new List<(byte, int timeLeft)>();
        foreach (var e in ObjectsController.Storage.Effects)
        {
            effectsOut.Add(((byte)e.Value, ObjectsController.Storage.Timers[e.Key].Time));
        }

        Updated.Invoke(this, new GameplayEventArgs
        {
            Objects = sortedObjects,
            POVShift = _playerShift,
            Score = GameParameters.Score,
            Speed = (int)ObjectsController.Player.Object.Speed.Y,
            Effects = effectsOut,
            DistanceToFinish = Math.Abs(_finishPos - ObjectsController.Player.Object.Pos.Y) / _distance
        });
    }

    //-----------------------------Физика. Begin---------------------------------------------
    private (int X, int Y) GetScreenNumber (Vector2 pos)
    {
        return ((int)pos.X / GameParameters.ScreenWidth, (int)pos.Y / GameParameters.ScreenWidth);
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
        return o.Colliders[0].Collider.Boundary.Width > GameParameters.ScreenWidth ||
                       o.Colliders[0].Collider.Boundary.Height > GameParameters.ScreenHeight;
    }
    private bool CalculateObstacleCollision((Vector2 initPos, int Id) obj1, (Vector2 initPos, int Id) obj2)
    {        
        bool isCollided = false;
        var rects1 = ObjectsController.Storage.SolidObjects[obj1.Id].Colliders;
        var rects2 = ObjectsController.Storage.SolidObjects[obj2.Id].Colliders;
      
        Vector2 oppositeDirection1 = Vector2.Zero;
        Vector2 oppositeDirection2 = Vector2.Zero;
        int tries = 100;
        if (RectangleCollider.IsCollided(
            rects1,
            rects2))
        {
            oppositeDirection1 = ObjectsController.Storage.Objects[obj1.Id].Pos != obj1.initPos ?
                  obj1.initPos - ObjectsController.Storage.Objects[obj1.Id].Pos : Vector2.Zero;
            oppositeDirection2 = ObjectsController.Storage.Objects[obj2.Id].Pos != obj2.initPos  ?
                 obj2.initPos - ObjectsController.Storage.Objects[obj2.Id].Pos : Vector2.Zero;
            if (oppositeDirection1 != Vector2.Zero)
                oppositeDirection1.Normalize();
            if (oppositeDirection2 != Vector2.Zero)
                oppositeDirection2.Normalize();            
            isCollided = true;
        }
        while (RectangleCollider.IsCollided(
            rects1,
            rects2) && tries > 0)
        {
            ObjectsController.Storage.Objects[obj1.Id].Move(ObjectsController.Storage.Objects[obj1.Id].Pos + oppositeDirection1);
            ObjectsController.Storage.Objects[obj2.Id].Move(ObjectsController.Storage.Objects[obj2.Id].Pos + oppositeDirection2); 
            tries--;
        }
        return isCollided;
    }
    private void CalculateCrushing (int Id1, int Id2)
    {
        ObjectsController.Storage.Objects[Id1].Speed = new Vector2(0, 0);
        ObjectsController.Storage.Objects[Id2].Speed = new Vector2(0, 0);
        if (ObjectsController.Storage.Objects[Id1] is Car)
        {
            Car c = (Car)ObjectsController.Storage.Objects[Id1];    
            c.Die();
        }
        if (ObjectsController.Storage.Objects[Id2] is Car)
        {
            Car c = (Car)ObjectsController.Storage.Objects[Id2];
            c.Die();
        }
    }
    //-----------------------------Физика. End---------------------------------------------
    
    private void CalculateTrigger (int i, ITrigger t)
    {
        if (RectangleCollider.IsCollided(ObjectsController.Storage.SolidObjects[i].Colliders, t.Collider))
        {
            t.OnTrigger(ObjectsController.Storage.Objects[i], i);
        }
    }       
    private void CalculateWin(object sender, TriggerEventArgs e)
    {
        if (e.ActivatorId == ObjectsController.Player.Id)                
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
            Car p = (Car)ObjectsController.Player.Object;
            switch (dir)
            {
                case IGameplayModel.Direction.forward:
                    {
                        p.Speed += new Vector2(0, -1000);
                        break;
                    }
                case IGameplayModel.Direction.backward:
                    {
                        p.Speed += new Vector2(0, 0);
                        break;
                    }
                case IGameplayModel.Direction.right:
                    {
                        p.Speed += new Vector2(500, 0);
                        break;
                    }
                case IGameplayModel.Direction.left:
                    {
                        p.Speed += new Vector2(-500, 0);
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