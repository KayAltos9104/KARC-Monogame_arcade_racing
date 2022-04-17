using Microsoft.Xna.Framework;
using System;

namespace KARC
{
    public interface IGameplayView
    {
        event EventHandler CycleFinished; //Включается в конце каждого цикла в GameCycle, чтобы обновить модель
        event EventHandler<ControlsEventArgs> PlayerMoved; 
        void LoadGameCycleParameters(Vector2 pos);
    }

    public class ControlsEventArgs
    {
        public IGameplayModel.Direction direction { get; set; }
    }
}
