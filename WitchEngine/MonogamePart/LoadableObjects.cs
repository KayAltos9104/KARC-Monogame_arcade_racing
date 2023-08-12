using Microsoft.Xna.Framework.Graphics;

namespace WitchEngine.MonogamePart;
/// <summary>
/// Static class which store game resources in xna format
/// </summary>
public static class LoadableObjects
{
    private static int _currentId = 1;
    private static Dictionary<int, Texture2D> Textures = new Dictionary<int, Texture2D>();
    private static Dictionary<string, SpriteFont> Fonts = new Dictionary<string, SpriteFont>();
    /// <summary>
    /// Adds new texture in storage
    /// </summary>
    /// <param name="texture">
    /// Texture that should be added
    /// </param>
    public static void AddTexture (Texture2D texture)
    {
        Textures.Add(_currentId, texture);
        _currentId++;
    }
    /// <summary>
    /// Tries to get texture from storage
    /// </summary>
    /// <param name="key">
    /// Texture id for search
    /// </param>
    public static Texture2D? GetTexture (int key)
    {
        if (Textures.ContainsKey(key))
            return Textures[key];
        else
            return null;
    }
    /// <summary>
    /// Adds new font in storage
    /// </summary>
    /// <param name="name">
    /// Name of font
    /// </param>
    /// <param name="font">
    /// Font that should be added
    /// </param>
    public static void AddFont(string name, SpriteFont font)
    {
        Fonts.Add(name, font);        
    }
    /// <summary>
    /// Tries to get font from storage
    /// </summary>
    /// <param name="key">
    /// Font name for search
    /// </param>
    public static SpriteFont? GetFont(string key)
    {
        if (Fonts.ContainsKey(key))
            return Fonts[key];
        else
            return null;
    }
}
