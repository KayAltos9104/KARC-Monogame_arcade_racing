using KARC.WitchEngine;
using System;

namespace KARC.Models;
public class GameParameters
{
    
    public readonly int ScreenWidth = 1000;
    public readonly int ScreenHeight = 1000;
    public readonly int TileSize = 100;
    
    public int Score { get; set; }


    public readonly int FramesPerCollisionUpdate = 1;    

    public (int width, int height) Resolution { get; set; }  

    public void IncreaseScore (object sender, int value)
    {
        Score += value;
    }
}
