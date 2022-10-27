using System;
using System.Collections.Generic;
using System.Text;

namespace KARC.WitchEngine
{
    public interface ITrigger
    {
        event EventHandler<TriggerEventArgs> Triggered;
        RectangleCollider Collider { get; set; }
        bool IsActive { get; set; }
        void OnTrigger(IObject activator, int id);
        
    }
    
    public class TriggerEventArgs:EventArgs
    {       
        public IObject Activator { get; set; }
        public int ActivatorId { get; set; }        
    }
}
