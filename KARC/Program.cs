using KARC.MVP;
using Microsoft.Xna.Framework;
using System;

namespace KARC
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            //using (var game = new GameCycleView())
            //var game = new GameCycleView();
            //game.Run();         
            //GameplayPresenter g = new GameplayPresenter(new GameCycleView(), new GameCycle());
            var game = new GameProcessor();
            game.Run();
        }
    }
}
