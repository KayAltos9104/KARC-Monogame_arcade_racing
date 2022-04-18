﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace KARC
{
    public interface IGameplayModel
    {
        int PlayerId { get; set; }
        Dictionary<int, IObject> Objects { get; set;}
        event EventHandler<GameplayEventArgs> Updated;
        void Update();
        void MovePlayer(Direction dir);
        void Initialize();

        public enum Direction: byte
        {
            forward,
            backward,
            right,
            left
        }
    }

    public class GameplayEventArgs
    {
        public Dictionary<int, IObject> Objects{ get; set; }
    }
}