using Microsoft.Xna.Framework.Input;

namespace WitchEngine.MonogamePart;

/// <summary>
/// Single static class which reads keyboard and mouse inputs
/// </summary>
public static class InputsManager
{
    /// <value>
    /// Property <c>PressedCurrentFrame</c> stores all keys which was pressed in current frame
    /// </value>
    public static KeyboardState PressedCurrentFrame { get; private set; }
    /// <value>
    /// Property <c>PressedPrevFrame</c> stores all keys which was pressed in previous frame
    /// </value>
    public static KeyboardState PressedPrevFrame { get; private set; }

    /// <summary>
    /// Loads all inputs from user and saves in <see cref="PressedCurrentFrame"/>
    /// </summary>
    public static void ReadInputs()
    {
        PressedCurrentFrame = Keyboard.GetState();
        //TODO: Потом добавить мышу
    }

    /// <summary>
    /// Save all inputs from user in <see cref="PressedPrevFrame"/> to use on the next frame
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
