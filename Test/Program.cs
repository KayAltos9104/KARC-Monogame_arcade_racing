using System;
using System.IO;
using WitchEngine;
using WitchEngine.MonogamePart;

namespace Test;

public static class Program
{
    [STAThread]
    static void Main()
    {
        //using (var game = new GameCycleView())
        //var game = new GameCycleView();
        //game.Run();         
        //GameplayPresenter g = new GameplayPresenter(new GameCycleView(), new GameCycle());
        //var game = new GameProcessor();
        //game.Run();
        string workingDirectory = Environment.CurrentDirectory;
        string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
        var game = new GameProcessor();
        var scene1 = new Scene();
        scene1.View = new MainMenuView();
        game.Scenes.Add("Test1", scene1);
        game.SetCurrentScene("Test1");
        game.Run();
    }
}
