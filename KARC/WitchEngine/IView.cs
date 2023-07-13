using Microsoft.Xna.Framework;
using System;

namespace KARC.WitchEngine
{
    public interface IView
    {
        //Включается в конце каждого цикла в GameCycle, чтобы обновить модель
        event EventHandler<CycleViewEventArgs> CycleFinished;

        void Initialize();
        void Update (GameTime gameTime);

        void Draw (GameTime gameTime);
    }
    
    public class CycleViewEventArgs
    {
        public GameTime GameTime { get; set; }
    }
}
