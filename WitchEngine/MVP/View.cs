using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Security.Cryptography.X509Certificates;
using WitchEngine.MonogamePart;

namespace WitchEngine.MVP;

/// <summary>
/// Parent class for all views in game. View draws objects and processes intercation with user
/// </summary>
public abstract class View
{
    protected ModelViewData? _currentModelData;
    protected Dictionary<string, IComponent> _interfaceElements;

  

    /// <value>
    /// Event <c>CycleFinished</c> invokes when View ended cycle processing
    /// </value>
    public EventHandler<ViewCycleFinishedEventArgs>? CycleFinished;
    /// <value>
    /// Event <c>SceneFinished</c> invokes when we should finish of pause current scene and switch to another
    /// </value>
    public EventHandler<SceneFinishedEventArgs>? SceneFinished;

    public View()
    {        
        _interfaceElements = new Dictionary<string, IComponent>();
    }
    /// <summary>
    /// Invokes <see cref="SceneFinished"/> event
    /// </summary>
    /// <param name="e">Object of <see cref="SceneFinishedEventArgs"/> which contains new scene name</param>
    protected void OnSceneFinished(SceneFinishedEventArgs e)
    {
        SceneFinished?.Invoke(this, e);
    }

    /// <summary>
    /// Initialize all view elements. Must be called. 
    /// </summary>
    public abstract void Initialize();

    /// <summary>
    /// Processes inputs, draws objects and invoke event about cycle ending.
    /// </summary>
    public virtual void Update()
    {
        CycleFinished?.Invoke(this, new ViewCycleFinishedEventArgs());
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
    public virtual void Draw()
    {        
        if (_currentModelData != null)
        {
            foreach (var o in _currentModelData.CurrentFrameObjects)
            {
                Graphics2D.RenderObject(o);
                //if (o is IAnimated)
                //    Graphics2D.RenderAnimation(o as IAnimated);
            }
        }
        foreach (var ui in _interfaceElements.Values)
            ui.Render(Graphics2D.SpriteBatch);
        
    }
    /// <summary>
    /// Process all inputs from user (inputs should be taken from <see cref="InputsManager"/>).
    /// </summary>
    //public abstract void ProcessInputs();
    
    
}
/// <summary>
/// Class with fields for transfer from view to model after one cycle
/// </summary>
public class ViewCycleFinishedEventArgs : EventArgs
{
    public GameData CurrentViewData { get; set; }
}
/// <summary>
/// Class that contains name of scene for GameProcessor to switch for
/// </summary>
public class SceneFinishedEventArgs : EventArgs
{
    public string NewSceneName {  get; set;}
}

