using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace KARC.WitchEngine
{
    public interface IGameplayModel
    {
        int PlayerId { get; set; }
        Dictionary<int, IObject> Objects { get; set; }
        event EventHandler<GameplayEventArgs> Updated; 
        event EventHandler<GameOverEventArgs> GameFinished;
       
        void Update();
        void ChangePlayerSpeed(Direction dir);
        void Initialize((int width, int height) resolution);
        void SwitchPause();

        public enum Direction : byte
        {
            forward,
            backward,
            right,
            left
        }
    }

    public class GameOverEventArgs
    {
        public bool IsWin { get; set; }
    }

    public class GameplayEventArgs : EventArgs
    {
        public Dictionary<int, IObject> Objects { get; set; }
        public Vector2 POVShift { get; set; }
    }
}