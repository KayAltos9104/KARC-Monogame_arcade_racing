using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace WitchEngine.MonogamePart;
/// <summary>
/// Single static class of system output console for debugging
/// </summary>
public static class GameConsole
{
    private static readonly List<string> _log;
    private static readonly SpriteFont? _font;
    /// <value>
    /// Property <c>IsShown</c> is flag for console visibility
    /// </value>
    public static bool IsShown { get; private set; } = false;

    static GameConsole()
    {
        _log = new List<string>
        {
            "Console log:"
        };
        try
        {
            _font = LoadableObjects.GetFont("SystemFont"); 
        }
        catch 
        {
            throw new ArgumentNullException("System font is missing");
        }
        
    }
    /// <summary>
    /// Switches <see cref="IsShown"/> field
    /// </summary>
    public static void SwitchVisibility()
    {
        IsShown = !IsShown;
    }
    /// <summary>
    /// Adds new string to <see cref="_log"/> which will be rendered 
    /// </summary>
    /// <param name="text">String which should be added</param>
    public static void WriteLine (string text)
    {
        _log.Add(text);
    }
    /// <summary>
    /// Clears console
    /// </summary>
    public static void Clear()
    {        
        _log.RemoveRange(1, _log.Count);
    }
    /// <summary>
    /// Renders console window with text
    /// </summary>
    /// <param name="spriteBatch"><see cref="SpriteBatch"/> for drawing</param>
    /// <exception cref="ArgumentNullException">Thrown when <see cref="_font"/> is null</exception>
    public static void Render(SpriteBatch spriteBatch)
    {
        var leftX = 0;
        var topY = Globals.Resolution.Height * 2 / 3;
        Graphics2D.FillRectangle(
            leftX, topY, Globals.Resolution.Width, Globals.Resolution.Height, 
            new Color(120, 120, 120, 120));
        //Graphics2D.DrawRectangle(
        //    leftX, topY, Globals.Resolution.Width, Globals.Resolution.Height, Color.Black);
        
        if (_font == null)
        {
            throw new ArgumentNullException("Missing system font for console");
        }
        else
        {
            Vector2 textShift = new Vector2(
            leftX + 10,
            topY + 10
            );
            string text = string.Join('\n', _log);
            spriteBatch.DrawString(
                        spriteFont: _font,
                        text,
                        position: textShift,
                        color: Color.Black,
                        rotation: 0,
                        origin: Vector2.Zero,
                        scale: 1,
                        SpriteEffects.None,
                        layerDepth: 0
                        );
        }        
    }
}
