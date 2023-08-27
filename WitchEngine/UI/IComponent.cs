using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace WitchEngine;
public interface IComponent
{
    string Name { get; }
    Vector2 Pos { get; }
    Vector2 TextPos { get; set; }
    string Text { get; set; }
    bool IsCentered { get; set; }
    float Layer { get; set; }
    public bool IsChosen { get; set; }
    public bool IsInteractive { get; set; }
    void Render(SpriteBatch spriteBatch);
}
