using KARC.Models;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace KARC.WitchEngine
{
    public interface IModel
    {       
        GameTime GameTime { get; set; }        
        event EventHandler<GameplayEventArgs> Updated; 
       
        void Update();
        void Initialize((int width, int height) resolution);

        public enum Direction : byte
        {
            forward,
            backward,
            right,
            left
        }
    }

    public class GameplayEventArgs : EventArgs
    {
        public Dictionary<int, IObject> Objects { get; set; }
        public Vector2 POVShift { get; set; }       
    }

}