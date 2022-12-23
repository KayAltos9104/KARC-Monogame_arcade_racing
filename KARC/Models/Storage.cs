using KARC.Objects;
using KARC.WitchEngine;
using System.Collections.Generic;

namespace KARC.Models;

public class Storage
{
    public int _currentId;
    public Dictionary<int, IObject> Objects { get; set; }
    public Dictionary<int, ISolid> SolidObjects { get; set; }
    public Dictionary<int, ITrigger> Triggers { get; set; }
    public Dictionary<string, Timer> Timers { get; set; }
    public Dictionary<string, Factory.ObjectTypes> Effects { get; set; }

    public Storage()
    {
        Objects = new Dictionary<int, IObject>();
        SolidObjects = new Dictionary<int, ISolid>();
        Triggers = new Dictionary<int, ITrigger>();
        Timers = new Dictionary<string, Timer>();
        Effects = new Dictionary<string, Factory.ObjectTypes>();
    }
   
}
