using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace KARC
{
    public class GameCycle : IGameplayModel
    {
        public event EventHandler<GameplayEventArgs> Updated = delegate { };

        private int _currentId;
        public int PlayerId { get; set;}
        public Dictionary<int, IObject> Objects { get; set; }
        public void Initialize()
        {
            Objects = new Dictionary<int, IObject>();
            _currentId = 1;
            Car player = new Car();
            player.Pos = new Vector2(512-90, 500);
            player.ImageId = 1;
            player.Speed = new Vector2(0, 0);
            Objects.Add(_currentId, player);
            PlayerId = _currentId;
            _currentId++;

            Car anotherCar = new Car();
            anotherCar.Pos = new Vector2(200, 50);
            anotherCar.Speed = new Vector2(0, 0);
            anotherCar.ImageId = 1;
            Objects.Add(_currentId, anotherCar);
            _currentId++;
        }
        public void Update()
        {
            Vector2 playerInitPos = Objects[PlayerId].Pos;
            foreach (var o in Objects.Values)
            {
                           
                o.Update();
            }
            Vector2 playerShift = Objects[PlayerId].Pos - playerInitPos;
            Updated.Invoke(this, new GameplayEventArgs { Objects = this.Objects, POVShift = playerShift });
        }

        public void ChangePlayerSpeed(IGameplayModel.Direction dir)
        {
            Car p = (Car)Objects[PlayerId];
            switch (dir)
            {
                case IGameplayModel.Direction.forward:
                    {
                        p.Speed += new Vector2(0, -1);                        
                        break;
                    }
                case IGameplayModel.Direction.backward:
                    {
                        p.Speed += new Vector2(0, 1);                      
                        break;
                    }
                case IGameplayModel.Direction.right:
                    {
                        p.Speed += new Vector2(1, 0);                       
                        break;
                    }
                case IGameplayModel.Direction.left:
                    {
                        p.Speed += new Vector2(-1, 0);                        
                        break;
                    }
            }
        }

        
    }
}
