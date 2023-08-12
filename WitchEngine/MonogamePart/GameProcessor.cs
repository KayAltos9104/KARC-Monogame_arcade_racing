using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace WitchEngine.MonogamePart;
/// <summary>
/// Main game class, which contains technical fields and manage game scenes
/// </summary>
public class GameProcessor : Game
{
    private GraphicsDeviceManager _graphics;
    private Scene? _currentScene;
    private string? _pathToResources;
    /// <value>
    /// The <c>Scenes</c> property represents a dictionary with all scenes used in game
    /// </value>
    public Dictionary<string, Scene> Scenes { get; }
    public GameProcessor()
    {
        Scenes = new Dictionary<string, Scene>();
        _graphics = new GraphicsDeviceManager(this);
        Graphics2D.Graphics = _graphics;
        Content.RootDirectory = "Resources";
        IsMouseVisible = true; 
    }
    /// <summary>
    /// Initialize game parameters 
    /// </summary>
    protected override void Initialize()
    {
        base.Initialize();
        Window.Title = "KARC";
        Graphics2D.Graphics.IsFullScreen = false;
        Graphics2D.Graphics.PreferredBackBufferWidth = 1600;
        Graphics2D.Graphics.PreferredBackBufferHeight = 900;
        Graphics2D.Graphics.ApplyChanges();
        Graphics2D.UpdateVisionArea();
        
        if (_currentScene == null)
        {
            throw new ArgumentNullException("Current scene was not choosed");
        }
        else
        {
            _currentScene.Initialize();
        }              
    }
    /// <summary>
    /// Loads graphics and game files
    /// </summary>    
    protected override void LoadContent()
    {
        Graphics2D.SpriteBatch = new SpriteBatch(GraphicsDevice);
        LoadableObjects.AddTexture(Content.Load<Texture2D>("Base_car"));
        LoadableObjects.AddFont("MainFont", Content.Load<SpriteFont>("DescriptionFont"));
    }
    /// <summary>
    /// Updates scene
    /// </summary>
    /// <param name="gameTime"> GameTime element parameter </param>
    protected override void Update(GameTime gameTime)
    {
        if (_currentScene != null)
        {
            _currentScene.Update(gameTime);
        }        
        base.Update(gameTime);
    }
    /// <summary>
    /// Draw scene elements
    /// </summary>
    /// <param name="gameTime"> GameTime element parameter </param>
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.DarkSeaGreen);
        if (_currentScene != null)
        {
            _currentScene.Draw(gameTime);
        }       
        base.Draw(gameTime);
    }
    /// <summary>
    /// Sets scene which should be processed
    /// </summary>
    /// <param name="sceneName"> Scene name which is in scenes dictionary </param>
    public void SetCurrentScene(string sceneName)
    {
        if (Scenes.ContainsKey(sceneName))
            _currentScene = Scenes[sceneName];
    }
}
