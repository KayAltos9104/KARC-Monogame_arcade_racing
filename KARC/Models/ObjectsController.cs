using KARC.Objects;
using KARC.Prefabs;
using KARC.Settings;
using KARC.WitchEngine;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using static System.Formats.Asn1.AsnWriter;

namespace KARC.Models;

public class ObjectsController
{
    public (int Id, IObject Object) Player { get; private set; }
    public (int Id, IObject Object) Finish { get; private set; }
    public Storage Storage { get; }
    public Generator CarGenerator { get; }
    public Generator PlayerGenerator { get; }
    public Generator ObstacleGenerator { get; }
    public Generator WallGenerator { get; }
    public Generator FinishGenerator { get; }
    public Generator ShieldGenerator { get; }

    private Random _random = new Random();
    public ObjectsController ()
    {
        Storage = new Storage ();

        CarGenerator = new ComplexCarGenerator ();
        CarGenerator.OnCreated += AddNewObject;
        CarGenerator.OnCreated += AddNewSolid;

        PlayerGenerator = new ComplexCarGenerator();
        PlayerGenerator.OnCreated += AddNewObject;
        PlayerGenerator.OnCreated += AddNewSolid;
        PlayerGenerator.OnCreated += AddNewPlayer;
        

        ObstacleGenerator = new WallGenerator(
            SpriteParameters.Sprites[Sprite.wall].width*2,
            SpriteParameters.Sprites[Sprite.wall].height*2
            );
        ObstacleGenerator.OnCreated += AddNewObject;
        ObstacleGenerator.OnCreated += AddNewSolid;

        WallGenerator = new WallGenerator(
            SpriteParameters.Sprites[Sprite.wall].width,
            SpriteParameters.Sprites[Sprite.wall].height
            );
        WallGenerator.OnCreated += AddNewObject;
        WallGenerator.OnCreated += AddNewSolid;

        FinishGenerator = new FinishGenerator(
            SpriteParameters.Sprites[Sprite.finishTape].width,
            SpriteParameters.Sprites[Sprite.finishTape].height
            );
        FinishGenerator.OnCreated += AddNewObject;
        FinishGenerator.OnCreated += AddNewTrigger;
        FinishGenerator.OnCreated += AddFinish;

        ShieldGenerator = new ShieldGenerator();
        ShieldGenerator.OnCreated += AddNewObject;
        ShieldGenerator.OnCreated += AddNewTrigger;
    }

    public void AddNewObject(object sender, EventArgs e)
    {
        Storage.Objects.Add(Storage.CurrentId, (sender as Generator).GetObject());
        Storage.IncrementId();
    }
    public void AddNewSolid(object sender, EventArgs e)
    {
        var lastElement = Storage.Objects.Last();
        Storage.SolidObjects.Add(lastElement.Key, (ISolid)lastElement.Value);
    }
    public void AddNewTrigger(object sender, EventArgs e)
    {
        var lastElement = Storage.Objects.Last();
        Storage.Triggers.Add(lastElement.Key, (ITrigger)lastElement.Value);
    }

    public void AddFinish (object sender, EventArgs e)
    {
        if (Finish.Object is not null)
            throw new Exception("Finish is already exists");
        Finish = (Storage.Objects.Last().Key, Storage.Objects.Last().Value);
    }
    public void AddNewPlayer(object sender, EventArgs e)
    {
        if (Player.Object is not null)
            throw new Exception("Player is already exists");

        Player = (Storage.Objects.Last().Key, Storage.Objects.Last().Value);        
    }

    public void GenerateObjectsByMap(Map map)
    {
        for (int y = 0; y < map.Height; y++)
            for (int x = 0; x < map.Width; x++)
            {
                if (map.GameField[x, y] != '\0')
                { 
                    if (int.TryParse(map.GameField[x, y].ToString(), out int leftUpCorner))
                    {
                        map.GameField[x, y] = '\0';
                        for (int yCorner = x; yCorner < map.Height; yCorner++)
                            for (int xCorner = y; xCorner < map.Width; xCorner++)
                            {
                                if (xCorner == x && yCorner == y)
                                    continue;
                                if (int.TryParse(map.GameField[xCorner, yCorner].ToString(), out int rightDownCorner))
                                {
                                    if (leftUpCorner == rightDownCorner)
                                    {
                                        GenerateObject('W', x, y, xCorner, yCorner);
                                        map.GameField[xCorner, yCorner] = '\0';
                                    }
                                }
                            }
                    }
                    else if (map.GameField[x, y] == 'F')
                    {
                        map.GameField[x, y] = '\0';
                        for (int yCorner = 0; yCorner < map.Height; yCorner++)
                            for (int xCorner = 0; xCorner < map.Width; xCorner++)
                            {
                                if (map.GameField[xCorner, yCorner] == 'F')
                                {
                                    generatedObject = GenerateObject('F', x, y, xCorner, yCorner);
                                    map.GameField[xCorner, yCorner] = '\0';
                                }
                            }
                    }
                    else
                    {
                        generatedObject = GenerateObject(map.GameField[x, y], x, y);
                    }

                }
            }
    }

    private void GenerateObject(char sign, int xTile, int yTile, int tileSize)
    {
        int x = xTile * tileSize;
        int y = yTile * tileSize;
        IObject generatedObject = null;
        if (sign == 'C')
        {
            CarGenerator.CreateObject(x + tileSize / 2, y + tileSize / 2);
            generatedObject = CarGenerator.GetObject();
            (generatedObject as Car).Speed = new Vector2(0, _random.Next(-8, -4));
        }
        else if (sign == 'P')
        {
            PlayerGenerator.CreateObject(x + tileSize / 2, y + tileSize / 2);
        }
        else if (sign == 'W')
        {
            ObstacleGenerator.CreateObject(x + tileSize / 2, y + tileSize / 2);
        }
        else if (sign == 'S')
        {
            ShieldGenerator.CreateObject(x + tileSize / 2, y + tileSize / 2);
            var trigger = Storage.Triggers.Last().Value;
            trigger.Triggered += GiveShield;
        }       
    }
    private void GenerateObject(char sign, int xInitTile, int yInitTile, int xEndTile, int yEndTile, int tileSize)
    {
        int xInit = xInitTile * tileSize;
        int yInit = yInitTile * tileSize;
        int xEnd = xEndTile * tileSize;
        int yEnd = yEndTile * tileSize;
        IObject generatedObject = null;
        if (sign == 'W')
        {
            WallGenerator.Width = xEnd - xInit + tileSize;
            WallGenerator.Height = yEnd - yInit + tileSize;
            WallGenerator.CreateObject(xInit, yInit);
        }
        if (sign == 'F')
        {
            FinishGenerator.Width = xEnd - xInit + tileSize;
            FinishGenerator.Height = yEnd - yInit + tileSize;
            FinishGenerator.CreateObject(xInit, yInit);
            var trigger = (ITrigger)Finish.Object;
            trigger.Triggered += CalculateWin;
            _finishPos = (int)Finish.Object.Pos.Y;
        }
        return generatedObject;
    }

    private void GiveShield(object sender, TriggerEventArgs e)
    {
        if (e.ActivatorId == Player.Id)
        {
            var playerCar = Player.Object as Car;
            playerCar.IsImmortal = true;
            Timer immortalTimer = new Timer(4);
            var timerId = Guid.NewGuid().ToString();
            Storage.Effects.Add(timerId, Factory.ObjectTypes.shield);
            immortalTimer.TimeIsOver +=
            (s, a) =>
            {
                playerCar.IsImmortal = false;
                Storage.Effects.Remove(timerId);
            };

            Storage.Timers.Add(timerId, immortalTimer);            
            (sender as Trigger2D).IsActive = false;
        }

    }
}
