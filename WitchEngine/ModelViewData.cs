

namespace WitchEngine;

public class ModelViewData
{
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
