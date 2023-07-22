using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace WitchEngine;

public abstract class View
{
    private ModelViewData _currentModelData;
    private Dictionary<string, IComponent> _interfaceElements;

    private List<Keys> _pressedCurrentFrame;
    private List<Keys> _pressedPrevFrame;
    private GameTime _gameTime;

    public EventHandler<CycleFinishedEventArgs> CycleFinished;

    public void Update ()
    {
        ReadInput();
        CycleFinished?.Invoke(this, new CycleFinishedEventArgs() { GameTime = _gameTime});
    }
    public abstract void LoadModelData(ModelViewData currentModelData);

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
    public abstract void ReadInput();

}
public class CycleFinishedEventArgs : EventArgs
{
    public GameTime GameTime { get; set; }
}


