using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WitchEngine.MVP;

namespace WitchEngine.MonogamePart;
/// <summary>
/// Main game class, which contains technical fields and manage game scenes
/// </summary>
public class GameProcessor : Game
{
    private GraphicsDeviceManager _graphics;
    private Scene? _currentScene;
    private string? _pathToResources;
    private List<(string key, string path)> _textures;
    private List<(string key, string path)> _fonts;
    
    /// <value>
    /// The <c>Scenes</c> property represents a dictionary with all scenes used in game
    /// </value>
    public Dictionary<string, Scene> Scenes { get; }
    /// <summary>
    /// This constructor initializes GameProcessor object with neccessary technical parameters 
    /// </summary>
    /// <param name="resourcesPath">Absolute path to the folder with game resources</param>
    /// <param name="textures">Pairs of texture name in code and relative path to its file</param>
    /// <param name="fonts">Pairs of font name in code and relative path to its file</param>
    public GameProcessor(string resourcesPath, 
        List<(string key, string path)> textures, 
        List<(string key, string path)> fonts)
    {
        Scenes = new Dictionary<string, Scene>();
        _graphics = new GraphicsDeviceManager(this);
        Graphics2D.Graphics = _graphics;
        Content.RootDirectory = resourcesPath;
        IsMouseVisible = true; 
        _textures = textures;
        _fonts = fonts;
    }
    /// <summary>
    /// Initialize game parameters 
    /// </summary>
    protected override void Initialize()
    {
        base.Initialize();
        Window.Title = "KARC";        
        SetResolution(Globals.Resolution.Width, Globals.Resolution.Height);
        SetFullScreenMode(Globals.IsFullScreen);
        
        
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
    /// Set screen resolution 
    /// </summary>
    /// <param name="width">Screen width</param>
    /// <param name="height">Screen height</param>
    public void SetResolution (int width, int height)
    {
        Globals.Resolution = (width, height);
        Graphics2D.Graphics.PreferredBackBufferWidth = Globals.Resolution.Width;
        Graphics2D.Graphics.PreferredBackBufferHeight = Globals.Resolution.Height;           
        Graphics2D.Graphics.ApplyChanges();
        Graphics2D.UpdateVisionArea();
    }

    public void SetFullScreenMode(bool isFullScreen)
    {
        Globals.IsFullScreen = isFullScreen;
        Graphics2D.Graphics.IsFullScreen = Globals.IsFullScreen;
        Graphics2D.Graphics.ApplyChanges();
        
    }
    /// <summary>
    /// Loads graphics and game files
    /// </summary>    
    protected override void LoadContent()
    {
        Graphics2D.SpriteBatch = new SpriteBatch(GraphicsDevice);
        LoadableObjects.AddFont("MainFont", Content.Load<SpriteFont>("DescriptionFont"));
        foreach (var t in _textures)
        {
            LoadTexture(t.key, t.path);
        }
        foreach (var f in _fonts)
        {
            LoadFont(f.key, f.path);
        }
    }
    /// <summary>
    /// Loads texture from resources to game
    /// </summary>
    /// <param name="name">Texture name that will be used in game code</param>
    /// <param name="path">Relative path to the texture including its name without extension</param>
    public void LoadTexture (string name, string path)
    {
        LoadableObjects.AddTexture(name, Content.Load<Texture2D>(path));
    }
    /// <summary>
    /// Loads font from resources to game
    /// </summary>
    /// <param name="name">Font name that will be used in game code</param>
    /// <param name="path">Relative path to the font including its name without extension</param>
    public void LoadFont(string name, string path)
    {
        LoadableObjects.AddFont(name, Content.Load<SpriteFont>(path));
    }
    /// <summary>
    /// Updates scene
    /// </summary>
    /// <param name="gameTime"> GameTime element parameter </param>
    protected override void Update(GameTime gameTime)
    {
        if (_currentScene != null)
        {
            _currentScene.Update();
        }        
        base.Update(gameTime);
        Globals.Time = gameTime;
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
            _currentScene.Draw();
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
