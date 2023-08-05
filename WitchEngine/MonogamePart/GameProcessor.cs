using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace WitchEngine.MonogamePart;

public class GameProcessor : Game
{
    private GraphicsDeviceManager _graphics;
    private Scene _currentScene;
    private string _pathToResources;
    public Dictionary<string, Scene> _scenes;
    public GameProcessor()
    {
        _scenes = new Dictionary<string, Scene>();
        _graphics = new GraphicsDeviceManager(this);
        Graphics2D.Graphics = _graphics;
        Content.RootDirectory = "Resources";
        IsMouseVisible = true;

        

        //_views = new Dictionary<string, IView>()
        //{
        //    { "MainMenu", new MenuView()},
        //    { "GamePlay", new GameCycleView()},            
        //};
        //_currentView = _views.First().Value;
        //_currentPresenter = new GameplayPresenter((GameCycleView)_currentView, new GameCycle());
    }
    protected override void Initialize()
    {
        base.Initialize();
        Window.Title = "KARC";
        Graphics2D.Graphics.IsFullScreen = false;
        Graphics2D.Graphics.PreferredBackBufferWidth = 1600;
        Graphics2D.Graphics.PreferredBackBufferHeight = 900;
        Graphics2D.Graphics.ApplyChanges();
        Graphics2D.UpdateVisionArea();        
        _currentScene.Initialize();        
    }

    protected override void LoadContent()
    {
        Graphics2D.SpriteBatch = new SpriteBatch(GraphicsDevice);
        LoadableObjects.AddTexture(Content.Load<Texture2D>("Base_car"));
        LoadableObjects.AddFont("MainFont", Content.Load<SpriteFont>("DescriptionFont"));

        // Добавить создание элементов во View не в конструкторе, а сделать метод Initalize
    }

    protected override void Update(GameTime gameTime)
    {
        if (_currentScene != null)
        {
            _currentScene.Update(gameTime);
        }        
        base.Update(gameTime);
    }
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.DarkSeaGreen);
        if (_currentScene != null)
        {
            _currentScene.Draw(gameTime);
        }       
        base.Draw(gameTime);
    }

    public void SetCurrentScene(string sceneName)
    {
        if (_scenes.ContainsKey(sceneName))
            _currentScene = _scenes[sceneName];
    }

}
