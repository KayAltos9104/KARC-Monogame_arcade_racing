using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace KARC.WitchEngine;
public interface IComponent
{
    Vector2 Pos { get; }
    Vector2 TextPos { get; set; }
    string Text { get; set; }
    bool IsCentered { get; set; }
    float Layer { get; set; }
    void Render(SpriteBatch spriteBatch);
}
