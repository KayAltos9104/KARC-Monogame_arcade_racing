using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace KARC.WitchEngine
{
    public interface IGameplayView
    {
        event EventHandler CycleFinished; //Включается в конце каждого цикла в GameCycle, чтобы обновить модель
        event EventHandler<ControlsEventArgs> PlayerSpeedChanged;
        event EventHandler<InitializeEventArgs> GameLaunched;
        event EventHandler GamePaused;
       
        void LoadGameCycleParameters(Dictionary<int, IObject> _objects, Vector2 POVShift);
        void ShowGameOver(bool isWin);
        void Run();
    }

    public class InitializeEventArgs
    {
        public (int width, int height) Resolution { get; set; }         
    }

    public class ControlsEventArgs : EventArgs
    {
        public IGameplayModel.Direction Direction { get; set; }
    }
}
