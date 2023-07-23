using Microsoft.Xna.Framework.Graphics;

namespace WitchEngine.MonogamePart;

public static class LoadableObjects
{
    public static SpriteFont TextBlock { get; set; }
    public static Dictionary<int, Texture2D> Textures = new Dictionary<int, Texture2D>();
}
