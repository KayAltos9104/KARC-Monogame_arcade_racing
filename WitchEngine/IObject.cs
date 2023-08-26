using Microsoft.Xna.Framework;

namespace WitchEngine;

/// <summary>
/// Base interface for game model, which represents game object (not interface element)
/// </summary>
/// <remarks>
/// This interface should be inherited by classes that implement every game object on model 
/// </remarks>
public interface IObject
{
    /// <value>
    /// The <c>Scenes</c> property represents a dictionary with all scenes used in game
    /// </value>
    float Layer { get; set; }
    /// <value>
    /// The <c>Sprites</c> property represents a dictionary with all object sprites
    /// </value>
    List<(string ImageName, Vector2 ImagePos)> Sprites { get; set; }
    /// <value>
    /// The <c>Pos</c> property represents position where object is placed
    /// </value>
    Vector2 Pos { get; set; }
    /// <summary>
    /// Updates object state
    /// </summary>
    /// <remarks>
    /// Should be called every frame
    /// </remarks>
    ///  <param name="gameTime"> GameTime element parameter</param>
    void Update();
}

