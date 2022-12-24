using KARC.Prefabs;
using KARC.WitchEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KARC.Models;

public class ObjectsController
{
    public int? PlayerId { get; private set; }
    public IObject Player { get; private set; }
    public Storage Storage { get; }
    public Generator CarGenerator { get; }
    public Generator PlayerGenerator { get; }

    
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
        PlayerId = null;
        Player = null;
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
    public void AddNewPlayer(object sender, EventArgs e)
    {
        if (Player is not null)
            throw new Exception("Player is already exists");

        Player = Storage.Objects.Last().Value;        
    }
}
