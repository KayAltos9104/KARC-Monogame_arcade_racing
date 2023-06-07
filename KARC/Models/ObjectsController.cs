using KARC.Objects;
using KARC.Prefabs;
using KARC.Settings;
using KARC.WitchEngine;
using System;
using System.Linq;


namespace KARC.Models;

public class ObjectsController
{
    public event EventHandler<int> ScoreIncreased;
    public (int Id, IObject Object) Player { get; private set; }
    public (int Id, IObject Object) Finish { get; private set; }
    public Storage Storage { get; }

    public GameParameters GameParameters { get; }
    public Generator CarGenerator { get; }
    public Generator PlayerGenerator { get; }
    public Generator ObstacleGenerator { get; }
    public Generator WallGenerator { get; }
    public Generator FinishGenerator { get; }
    public Generator ShieldGenerator { get; }

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

        ShieldGenerator = new ShieldGenerator(
            SpriteParameters.Sprites[Sprite.shield].width,
            SpriteParameters.Sprites[Sprite.shield].height
            );
        ShieldGenerator.OnCreated += AddNewObject;
        ShieldGenerator.OnCreated += AddNewShield;

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
    public void AddNewShield (object sender, EventArgs e)
    {
        var lastElement = Storage.Objects.Last();
        Storage.Triggers.Add(lastElement.Key, (ITrigger)lastElement.Value);
        (lastElement.Value as ITrigger).Triggered += GiveShield;

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

    public void GiveShield(object sender, TriggerEventArgs e)
    {
        if (e.ActivatorId == Player.Id)
        {
            var playerCar = Player.Object as Car;
            playerCar.IsImmortal = true;
            Timer immortalTimer = new Timer(4);
            var timerId = Guid.NewGuid().ToString();
            Storage.Effects.Add(timerId, Sprite.shield);

            immortalTimer.TimeIsOver +=
            (s, a) =>
            {
                playerCar.IsImmortal = false;
                Storage.Effects.Remove(timerId);
            };

            Storage.Timers.Add(timerId, immortalTimer);
            
            (sender as Trigger2D).IsActive = false;
            ScoreIncreased?.Invoke(this, 5000);
        }
    }
}
