using Microsoft.Xna.Framework;


namespace WitchEngine.MVP;
/// <summary>
/// Scene class which contains all scene MVP elements - View, Presenter and Model
/// </summary>
public class Scene
{
    public View View { get; set; }
    public Presenter Presenter { get; set; }
    public Model Model { get; set; }

    public Scene(View view, Model model, Presenter presenter)
    {
        View = view;
        Presenter = presenter;
        Model = model;
    }
    /// <summary>
    /// Initialize all scene elements - view, model and presenter 
    /// </summary>
    public void Initialize()
    {
        View.Initialize();
        if (Model != null)
        {
            Model.Initialize();
        }        
    }
    /// <summary>
    /// Update scene state 
    /// </summary>
    /// <param name="gameTime">
    /// GameTime element parameter
    /// </param>
    public void Update()
    {
        View.Update();
    }
    /// <summary>
    /// Draw scene 
    /// </summary>
    /// <param name="gameTime">
    /// GameTime element parameter
    /// </param>
    public void Draw()
    {
        View.Draw();
    }
}
