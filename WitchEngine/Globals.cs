using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitchEngine;

public static class Globals
{
    /// <value>
    /// The <c>Resolution</c> property represents a tuple of screen width and height
    /// </value>
    public static (int Width, int Height) Resolution { get; set; }
    public static bool IsFullScreen { get; set; }
    /// <summary>
    /// The <c>Time</c> property represents a <see cref="GameTime"/> object in game
    /// </summary>
    public static GameTime Time { get; set;}

    static Globals()
    {
        //TODO: Потом сделать, чтобы из файла с настройками тягал
        Resolution = (1600, 900);
        IsFullScreen = false;
    }

}
