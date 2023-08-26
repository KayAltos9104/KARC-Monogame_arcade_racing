using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using WitchEngine;
using WitchEngine.MonogamePart;
using WitchEngine.MVP;

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
        string workingDirectory = Environment.CurrentDirectory+"\\Resources";
        string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.Parent.Parent.FullName+
            "\\WitchEngine\\SystemResources";
        var game = new GameProcessor(workingDirectory,
            new List<(string, string)>()
            {
                ("base_car","Base_car")
            },
            new List<(string, string)>()
            {
                ("MainFont", "DescriptionFont")
            }
            );

        var v1 = new MainMenuView();
        var scene1 = new Scene(v1, null, new Presenter(game, v1, null));
        game.Scenes.Add("Test1", scene1);

        var v2 = new SettingsMenuView();
        var scene2 = new Scene(v2, null, new Presenter(game, v2, null));
        game.Scenes.Add("Test2", scene2);

        game.SetCurrentScene("Test1");
        game.Run();
        

    }
}
