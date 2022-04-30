using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace KARC
{
    public interface IGameplayView
    {
        event EventHandler CycleFinished; //Включается в конце каждого цикла в GameCycle, чтобы обновить модель
        event EventHandler<ControlsEventArgs> PlayerSpeedChanged; 
        void LoadGameCycleParameters(Dictionary<int, IObject> _objects);
        void Run();
    }

    public class ControlsEventArgs: EventArgs
    {
        public IGameplayModel.Direction Direction { get; set; }
    }
}
