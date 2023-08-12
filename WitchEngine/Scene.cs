using Microsoft.Xna.Framework;


namespace WitchEngine;
/// <summary>
/// Scene class which contains all scene MVP elements - View, Presenter and Model
/// </summary>
public class Scene
{
    public View View { get; set; }
    //TODO:
    // Добавить презентер и модель

    /// <summary>
    /// Initialize all scene elements - view, model and presenter 
    /// </summary>
    public void Initialize ()
    {
        View.Initialize ();
    }
    /// <summary>
    /// Update scene state 
    /// </summary>
    /// <param name="gameTime">
    /// GameTime element parameter
    /// </param>
    public void Update (GameTime gameTime)
    {
        View.Update();
    }
    /// <summary>
    /// Draw scene 
    /// </summary>
    /// <param name="gameTime">
    /// GameTime element parameter
    /// </param>
    public void Draw(GameTime gameTime)
    {
        View.Draw(gameTime);
    }
}
