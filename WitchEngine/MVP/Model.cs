namespace WitchEngine.MVP;

public abstract class Model
{
    public Dictionary<string, IObject> GameObjects { get; set; }
    /// <value>
    /// Event <c>CycleFinished</c> that activates when Model ended cycle processing
    /// </value>
    public EventHandler<ModelCycleFinishedEventArgs>? CycleFinished;
    public Model()
    {
        GameObjects = new Dictionary<string, IObject>();
    }

    public abstract void Initialize();
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
        public ModelViewData ModelViewData { get; set; }

        public ModelCycleFinishedEventArgs(List<IObject> currentFrameObjects)
        {
            ModelViewData = new ModelViewData(currentFrameObjects);
        }
    }
}
