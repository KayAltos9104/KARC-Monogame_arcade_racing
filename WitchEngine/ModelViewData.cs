namespace WitchEngine;
/// <summary>
/// Class which contains data that model transfer to view
/// </summary>
public class ModelViewData
{
    /// <value>
    /// The <c>CurrentFrameObjects</c> property represents a list with all objects in scene
    /// </value>
    public List<IObject> CurrentFrameObjects { get; set; }
    
    public ModelViewData() 
    {
        CurrentFrameObjects = new List<IObject>();
    }

    public ModelViewData(List<IObject> currentFrameObjects)
    {
        CurrentFrameObjects = currentFrameObjects;
    }
}
