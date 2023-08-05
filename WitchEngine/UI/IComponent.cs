using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace WitchEngine;
public interface IComponent
{
    Vector2 Pos { get; }
    Vector2 TextPos { get; set; }
    string Text { get; set; }
    bool IsCentered { get; set; }
    float Layer { get; set; }
    public bool IsChosen { get; set; }
    void Render(SpriteBatch spriteBatch);
}
