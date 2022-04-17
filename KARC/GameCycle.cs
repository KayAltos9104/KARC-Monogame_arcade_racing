using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace KARC
{
    public class GameCycle : IGameplayModel
    {
        public event EventHandler<GameplayEventArgs> Updated = delegate { };

        Vector2 pos = new Vector2(300, 300);
        public void Update()
        {            
            Updated.Invoke(this, new GameplayEventArgs { PlayerPos = pos });
        }

        public void MovePlayer(IGameplayModel.Direction dir)
        {
            switch (dir)
            {
                case IGameplayModel.Direction.forward:
                    {
                        pos += new Vector2(0, -1);
                        break;
                    }
                case IGameplayModel.Direction.backward:
                    {
                        pos += new Vector2(0, 1);
                        break;
                    }
                case IGameplayModel.Direction.right:
                    {
                        pos += new Vector2(1, 0);
                        break;
                    }
                case IGameplayModel.Direction.left:
                    {
                        pos += new Vector2(-1, 0);
                        break;
                    }
            }
        }
    }
}
