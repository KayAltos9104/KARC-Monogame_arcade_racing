﻿using Microsoft.Xna.Framework.Graphics;
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
        string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
        var game = new GameProcessor(workingDirectory,
            new List<(string, string)>()
            {
                ("base_car","Base_car")
            },
            new List<(string, string)>()
            //{
            //    ("MainFont", "DescriptionFont")
            //}
            );
        
        var scene1 = new Scene();
        scene1.View = new MainMenuView();
        game.Scenes.Add("Test1", scene1);
        game.SetCurrentScene("Test1");
        game.Run();
        
    }
}
