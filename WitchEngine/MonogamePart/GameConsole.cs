using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static System.Net.Mime.MediaTypeNames;

namespace WitchEngine.MonogamePart;

public static class GameConsole
{
    private static string _log = "Console log: \n";
    private static SpriteFont _font = LoadableObjects.GetFont("SystemFont");
    public static bool IsShown { get; private set; } = false;

    

    public static void WriteLine (string text)
    {
        _log += text +'\n';
    }

    public static void Render(SpriteBatch spriteBatch)
    {
        var leftX = 0;
        var topY = Globals.Resolution.Height * 2 / 3;
        Graphics2D.FillRectangle(
            leftX, topY, Globals.Resolution.Width, Globals.Resolution.Height, 
            new Color(120, 120, 120, 120));
        Graphics2D.DrawRectangle(
            leftX, topY, Globals.Resolution.Width, Globals.Resolution.Height, Color.Black);
        

        Vector2 textShift = new Vector2(
            leftX + 10,
            topY + 10
            );
        spriteBatch.DrawString(
                    spriteFont: _font,
                    _log,
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
