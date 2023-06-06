using KARC.Settings;
using KARC.WitchEngine.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace KARC.WitchEngine.UI;
public class MessageBox : InterfaceComponent    
{   
    public MessageBox(Vector2 pos, string text) : base(pos)
    {
        Pos = pos;
        Text = text;
        TextPos = Vector2.Zero;  
        Layer = 1.0f; 
        MarginText = new Vector2 (20, 0);
    }
    public void Move(Vector2 pos)
    {
        Pos = pos;
    }

    public override void Render(SpriteBatch spriteBatch)
    {        
        int x = (int)(Pos - (IsCentered ? _textSize / 2 : Vector2.Zero)).X;
        int y = (int)(Pos - (IsCentered ? _textSize / 2 : Vector2.Zero)).Y;        
        Graphics2D.FillRectangle(x, y, (int)(_textSize.X + MarginText.X*2), (int)(_textSize.Y + MarginText.Y*2), Color.DarkSeaGreen);
        Graphics2D.DrawRectangle(x, y, (int)(_textSize.X + MarginText.X*2), (int)(_textSize.Y + MarginText.Y * 2), Color.Black, 3);
        RenderText(spriteBatch);
    }
}
