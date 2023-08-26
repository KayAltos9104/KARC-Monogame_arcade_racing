using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace WitchEngine;

public class Parameter : InterfaceComponent
{
    public Parameter (Vector2 pos, SpriteFont font, string text, string spriteName) : base(pos, font)
    {
        Pos = pos;
        Text = text;
        TextPos = new Vector2 (70, 0);    
        Sprites.Add((spriteName, Vector2.Zero));            
        Layer = 1.0f;
    }

    public void Move(Vector2 pos)
    {
        Pos = pos;
    }
    
    public override void Render(SpriteBatch spriteBatch)
    {            
        RenderSprites(spriteBatch);
        RenderText(spriteBatch);
    }
}
