using Microsoft.Xna.Framework.Graphics;

namespace WitchEngine.MonogamePart;

public static class LoadableObjects
{
    private static int _currentId = 1;
    private static Dictionary<int, Texture2D> Textures = new Dictionary<int, Texture2D>();
    private static Dictionary<string, SpriteFont> Fonts = new Dictionary<string, SpriteFont>();
    public static void AddTexture (Texture2D texture)
    {
        Textures.Add(_currentId, texture);
        _currentId++;
    }
    public static Texture2D? GetTexture (int key)
    {
        if (Textures.ContainsKey(key))
            return Textures[key];
        else
            return null;
    }

    public static void AddFont(string name, SpriteFont font)
    {
        Fonts.Add(name, font);        
    }
    public static SpriteFont? GetFont(string key)
    {
        if (Fonts.ContainsKey(key))
            return Fonts[key];
        else
            return null;
    }

}
