using KARC.Objects;
using KARC.WitchEngine;
using System.Collections.Generic;

namespace KARC.Models;

public class ObjectsStorage
{
    
    public Dictionary<int, ISolid> SolidObjects { get; set; }
    public Dictionary<int, ITrigger> Triggers { get; set; }
    public Dictionary<string, Timer> Timers { get; set; }
    public Dictionary<string, Factory.ObjectTypes> Effects { get; set; }

    public ObjectsStorage()
    {
        SolidObjects = new Dictionary<int, ISolid>();
        Triggers = new Dictionary<int, ITrigger>();
        Timers = new Dictionary<string, Timer>();
        Effects = new Dictionary<string, Factory.ObjectTypes>();
    }
}
