using KARC.Prefabs;

namespace KARC.Models;

public class ObjectsController
{
    public int PlayerId { get; set; }
    public Storage Storage { get; }
    public Generator CarGenerator { get; }
    public Generator PlayerGenerator { get; }

    public ObjectsController ()
    {
        Storage = new Storage ();
        CarGenerator = new ComplexCarGenerator ();        
        PlayerGenerator = new ComplexCarGenerator();
    }

    public void AddNewCar()
    {
        Storage.Objects.Add(Storage._currentId, CarGenerator.GetObject());
        Storage._currentId++;
    }

    public void AddNewPlayer()
    {
        Storage.Objects.Add(Storage._currentId, PlayerGenerator.GetObject());
        PlayerId = Storage._currentId;
        Storage._currentId++;
    }
}
