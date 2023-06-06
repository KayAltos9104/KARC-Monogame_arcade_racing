using KARC.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace KARC.WitchEngine.UI;

public abstract class InterfaceComponent : IComponent
{
    protected Vector2 _textSize;
    public Vector2 MarginText { get; set; }
    public List<(int ImageId, Vector2 ImagePos)> Sprites { get; set; }    
    public Vector2 Pos { get; set; }
    public Vector2 TextPos { get; set; }
    public string Text { get; set ; }
    public bool IsCentered { get; set; }
    public float Layer { get; set; }    

    public InterfaceComponent(Vector2 pos) 
    {
        Pos = pos;        
        Sprites = new List<(int ImageId, Vector2 ImagePos)>();       
        Layer = 1.0f;
        MarginText = Vector2.Zero;


    }
    public void LoadSprite(byte sprite, Vector2 pos)
    {
        Sprites.Add((sprite, pos));
    }
    public abstract void Render(SpriteBatch spriteBatch);  
    protected void RenderSprites(SpriteBatch spriteBatch)
    {
        foreach (var sprite in Sprites)
        {
            spriteBatch.Draw(
                   texture: LoadableObjects.Textures[sprite.ImageId],
                   position: Pos + sprite.ImagePos,
                   sourceRectangle: null,
                   Color.White,
                   rotation: 0,
                   origin: Vector2.Zero,
                   scale: Vector2.One,
                   SpriteEffects.None,
                   layerDepth: Layer);
        }
    }
    protected void RenderText (SpriteBatch spriteBatch) 
    {
        if (Text == null)
            return;
        _textSize = LoadableObjects.TextBlock.MeasureString(Text) != Vector2.Zero ?
                LoadableObjects.TextBlock.MeasureString(Text):
                Vector2.One;
        Vector2 textShift = new Vector2(
            Pos.X + (_textSize.X - LoadableObjects.TextBlock.MeasureString(Text).X) / 2 - (IsCentered ? _textSize.X / 2 : 0),
            Pos.Y + (_textSize.Y - LoadableObjects.TextBlock.MeasureString(Text).Y) / 2 - (IsCentered ? _textSize.Y / 2 : 0)
            );
        spriteBatch.DrawString(
                    spriteFont: LoadableObjects.TextBlock,
                    Text,
                    position: TextPos + MarginText + textShift,
                    color: Color.Black,
                    rotation: 0,
                    origin: Vector2.Zero,
                    scale: 1,
                    SpriteEffects.None,
                    layerDepth: 0
                    );
    }
}
