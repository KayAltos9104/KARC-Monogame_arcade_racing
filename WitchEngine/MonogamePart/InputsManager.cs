using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitchEngine.MonogamePart;

public static class InputsManager
{
    public static KeyboardState PressedCurrentFrame { get; private set; }
    public static KeyboardState PressedPrevFrame { get; private set; }

    /// <summary>
    /// Loads all inputs from user.
    /// </summary>
    public static void ReadInputs()
    {
        PressedCurrentFrame = Keyboard.GetState();
        //TODO: Потом добавить мышу
    }

    /// <summary>
    /// Save all inputs from user to use on the next frame.
    /// </summary>
    public static void SaveInputs()
    {
        PressedPrevFrame = Keyboard.GetState();
    }

    /// <summary>
    /// Checks single pressing of button.
    /// </summary>
    /// <param name="key">
    /// Which single pressing key must be checked.
    /// </param>
    /// <returns>
    /// True if this key was pressed single time.
    /// </returns>
    public static bool IsSinglePressed(Keys key)
    {
        return PressedCurrentFrame.IsKeyUp(key) && PressedPrevFrame.IsKeyDown(key);
    }
}
