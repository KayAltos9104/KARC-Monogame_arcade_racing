using Microsoft.Xna.Framework;


namespace WitchEngine;

public class Scene
{
    public View View { get; set; }

    public void Initialize ()
    {
        View.Initialize ();
    }
    public void Update (GameTime gameTime)
    {
        View.Update();
    }
    public void Draw(GameTime gameTime)
    {
        View.Draw(gameTime);
    }
}
