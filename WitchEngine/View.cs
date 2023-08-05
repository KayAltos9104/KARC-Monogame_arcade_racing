using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WitchEngine.MonogamePart;

namespace WitchEngine;

/// <summary>
/// Parent class for all views in game. View draws objects and processes intercation with user
/// </summary>
public abstract class View
{
    
    protected ModelViewData _currentModelData;
    protected Dictionary<string, IComponent> _interfaceElements;

    protected KeyboardState _pressedCurrentFrame;
    protected KeyboardState _pressedPrevFrame;
    protected GameTime _gameTime;

    public (int Width, int Height) Resolution;
    /// <value>
    /// Event <c>CycleFinished</c> that activates when View ended cycle processing
    /// </value>
    public EventHandler<CycleFinishedEventArgs> CycleFinished;

    public View()
    {
        Resolution = (Graphics2D.Graphics.PreferredBackBufferWidth,
           Graphics2D.Graphics.PreferredBackBufferHeight);
        _interfaceElements = new Dictionary<string, IComponent>();        
    }
    /// <summary>
    /// Initialize all view elements. Must be called. 
    /// </summary>
    public abstract void Initialize();

    /// <summary>
    /// Processes inputs, draws objects and invoke event about cycle ending.
    /// </summary>
    public virtual void Update ()
    {        
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
        if (_currentModelData != null)
        {
            foreach (var o in _currentModelData.CurrentFrameObjects)
            {
                Graphics2D.RenderObject(o);
                if (o is IAnimated)
                    Graphics2D.RenderAnimation(o as IAnimated);
            }
        }        
        foreach (var ui in _interfaceElements.Values)
            ui.Render(Graphics2D.SpriteBatch);
        Graphics2D.SpriteBatch.End();
    }
    /// <summary>
    /// Loads all inputs from user.
    /// </summary>
    public virtual void ReadInputs()
    {
        _pressedCurrentFrame = Keyboard.GetState();
        // Потом добавить мышу
    }
    /// <summary>
    /// Save all inputs from user to use on the next frame.
    /// </summary>
    public void SaveInputs()
    {
        _pressedPrevFrame = Keyboard.GetState();
    }
    /// <summary>
    /// Checks single pressing of button.
    /// </summary>
    /// <param name="key">
    /// Which single pressing key must be checked.
    /// </param>
    /// <returns>
    /// Is this key was pressed single time.
    /// </returns>
    protected bool IsSinglePressed(Keys key)
    {
        //var a = Keyboard.GetState().IsKeyUp(key);
        //var b = _pressedPrevFrame.Contains(key);
        //return Keyboard.GetState().IsKeyUp(key) && _pressedPrevFrame.Contains(key);
        return _pressedCurrentFrame.IsKeyUp(key) && _pressedPrevFrame.IsKeyDown(key);
    }
}
public class CycleFinishedEventArgs : EventArgs
{
    public GameTime GameTime { get; set; }
}


