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
            GameplayPresenter g = new GameplayPresenter(new GameCycleView(), new GameCycle());
            g.LaunchGame();
        }
    }
}
