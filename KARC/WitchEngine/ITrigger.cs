using System;
using System.Collections.Generic;
using System.Text;

namespace KARC.WitchEngine
{
    public interface ITrigger
    {
        event EventHandler Triggered;
        RectangleCollider Collider { get; set; }
        void OnTrigger();        
    }
    
}
