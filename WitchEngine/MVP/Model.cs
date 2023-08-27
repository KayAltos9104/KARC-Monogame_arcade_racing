namespace WitchEngine.MVP;
/// <summary>
/// Abstract class which contains all game logic
/// </summary>
/// <remarks>
/// All game logic of <see cref="Scene">scene</see> should be written here
/// </remarks>
public abstract class Model
{
    /// <value>
    /// Property <c>GameObjects</c> contains all game objects in <see cref="Scene">scene</see>
    /// </value>
    public Dictionary<string, IObject> GameObjects { get; set; }
    /// <value>
    /// Event <c>CycleFinished</c> that activates when Model ended cycle processing
    /// </value>
    /// <remarks>
    /// This event sends <see cref="ModelCycleFinishedEventArgs"/> which contains game logic data for view
    /// </remarks>
    public EventHandler<ModelCycleFinishedEventArgs>? CycleFinished;
    public Model()
    {
        GameObjects = new Dictionary<string, IObject>();
    }
    /// <summary>
    /// Initializes model - objects, parameters, etc.
    /// </summary>
    /// <remarks>
    /// Should be called after <c>Model</c> creating
    /// </remarks>
    public abstract void Initialize();
    /// <summary>
    /// Updates game logic for scene
    /// </summary>
    /// <param name="e">Parameters from <see cref="View"/></param>
    /// <remarks>
    /// Should be called every frame
    /// </remarks>
    public virtual void Update(ViewCycleFinishedEventArgs e)
    {
        foreach (var obj in GameObjects.Values)
        {
            obj.Update();
        }
        CycleFinished?.Invoke(this, new ModelCycleFinishedEventArgs(GameObjects.Values.ToList()));
    }

    /// <summary>
    /// Class with fields for transfer from model to view after one cycle
    /// </summary>
    public class ModelCycleFinishedEventArgs : EventArgs
    {
        /// <value>
        /// Property that contains all data from game logic which should be sent to <see cref="View"/>
        /// </value>
        public ModelViewData ModelViewData { get; set; }

        public ModelCycleFinishedEventArgs(List<IObject> currentFrameObjects)
        {
            ModelViewData = new ModelViewData(currentFrameObjects);
        }
    }
}
