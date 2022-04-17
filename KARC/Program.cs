using System;

namespace KARC
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new GameCycleView())
                game.Run();
        }
    }
}
