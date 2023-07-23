using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WitchEngine.MonogamePart;

namespace WitchEngine;

/// <summary>
/// Parent class for all views in game. View draws objects and processes intercation with user
/// </summary>
public abstract class View
{
    
    private ModelViewData _currentModelData;
    private Dictionary<string, IComponent> _interfaceElements;

    private List<Keys> _pressedCurrentFrame;
    private List<Keys> _pressedPrevFrame;
    private GameTime _gameTime;

    /// <value>
    /// Event <c>CycleFinished</c> that activates when View ended cycle processing
    /// </value>
    public EventHandler<CycleFinishedEventArgs> CycleFinished;

    /// <summary>
    /// Processes inputs, draws objects and invoke event about cycle ending.
    /// </summary>
    public void Update ()
    {
        ReadInput();
        CycleFinished?.Invoke(this, new CycleFinishedEventArgs() { GameTime = _gameTime});
    }
    /// <summary>
    /// Loads game model data (list of objects for example).
    /// </summary>
    /// <param name="currentModelData">
    /// Model data which should be loaded in view.
    /// </param>
    public abstract void LoadModelData(ModelViewData currentModelData);
    
    /// <summary>
    /// Draws all game objects and interface elements.
    /// </summary>
    /// <param name="gameTime">
    /// Time parameters - total and elapsed.
    /// </param>
    public virtual void Draw(GameTime gameTime)
    {
        Graphics2D.SpriteBatch.Begin();

        foreach (var o in _currentModelData.CurrentFrameObjects)
        {
            Graphics2D.RenderObject(o);
            if (o is IAnimated)
                Graphics2D.RenderAnimation(o as IAnimated);
        }
        Graphics2D.SpriteBatch.End();
    }
    /// <summary>
    /// Loads all inputs from user.
    /// </summary>
    public abstract void ReadInput();

}
public class CycleFinishedEventArgs : EventArgs
{
    public GameTime GameTime { get; set; }
}


