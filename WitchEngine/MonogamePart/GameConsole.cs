using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace WitchEngine.MonogamePart;

public static class GameConsole
{
    private static List<string> _log;
    private static SpriteFont _font;
    public static bool IsShown { get; private set; } = false;

    static GameConsole()
    {
        _log = new List<string>();
        _log.Add("Console log:");
        try
        {
            _font = LoadableObjects.GetFont("SystemFont"); 
        }
        catch 
        {
            throw new ArgumentNullException("System font is missing");
        }
        
    }

    public static void SwitchVisibility()
    {
        IsShown = !IsShown;
    }

    public static void WriteLine (string text)
    {
        _log.Add(text);
    }

    public static void Render(SpriteBatch spriteBatch)
    {
        var leftX = 0;
        var topY = Globals.Resolution.Height * 2 / 3;
        Graphics2D.FillRectangle(
            leftX, topY, Globals.Resolution.Width, Globals.Resolution.Height, 
            new Color(120, 120, 120, 120));
        //Graphics2D.DrawRectangle(
        //    leftX, topY, Globals.Resolution.Width, Globals.Resolution.Height, Color.Black);
        

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
