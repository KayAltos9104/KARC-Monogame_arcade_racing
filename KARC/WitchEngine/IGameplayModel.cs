using KARC.Models;
using KARC.Objects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace KARC.WitchEngine
{
    public interface IGameplayModel
    {
        int PlayerId { get; set; }
        GameTime GameTime { get; set; }
        // Dictionary<int, IObject> Objects { get; set; }
        ObjectsController ObjectsController { get; set; }
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
        public int Score { get; set; }
        public int Speed { get; set; }
        public List<(byte, int timeLeft)>  Effects {get; set;}
        public float DistanceToFinish { get; set; }
    }
    
}