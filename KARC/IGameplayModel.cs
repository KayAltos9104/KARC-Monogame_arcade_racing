using Microsoft.Xna.Framework;
using System;

namespace KARC
{
    public interface IGameplayModel
    {
        event EventHandler<GameplayEventArgs> Updated;
        void Update();
        void MovePlayer(Direction dir);

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
        public Vector2 PlayerPos { get; set; }
    }
}