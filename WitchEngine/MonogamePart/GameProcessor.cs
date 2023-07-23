using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace WitchEngine.MonogamePart;

public class GameProcessor : Game
{
    private GraphicsDeviceManager _graphics;
    private View _currentView;
    //private IPresenter _currentPresenter;
    //public Dictionary<string, IView> _views;
    public GameProcessor()
    {
        _graphics = new GraphicsDeviceManager(this);
        Graphics2D.Graphics = _graphics;
        Content.RootDirectory = "Content";
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
        //_currentView.Initialize();
    }

    protected override void LoadContent()
    {
        Graphics2D.SpriteBatch = new SpriteBatch(GraphicsDevice);

        //LoadableObjects.Textures.Add((byte)Sprite.car, Content.Load<Texture2D>("Base_car"));
        //LoadableObjects.Textures.Add((byte)Sprite.wall, Content.Load<Texture2D>("Wall"));
        //LoadableObjects.Textures.Add((byte)Sprite.window, Content.Load<Texture2D>("Message_Window"));
        //LoadableObjects.Textures.Add((byte)Sprite.finishTape, Content.Load<Texture2D>("FinishSprite"));
        //LoadableObjects.Textures.Add((byte)Sprite.finishCounterWindow, Content.Load<Texture2D>("FinishCounterField"));
        //LoadableObjects.Textures.Add((byte)Sprite.shield, Content.Load<Texture2D>("Immortality"));
        //LoadableObjects.Textures.Add((byte)Sprite.explosion, Content.Load<Texture2D>("Explosion_Atlas"));
        //LoadableObjects.TextBlock = Content.Load<SpriteFont>("DescriptionFont");
    }

    protected override void Update(GameTime gameTime)
    {
        //_currentView.Update(gameTime);
        base.Update(gameTime);
    }
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.DarkSeaGreen);
        //_currentView.Draw(gameTime);
        base.Draw(gameTime);
    }

    public void SetCurrentScene(string sceneName)
    {
        //if (_views.ContainsKey(sceneName))
        //    _currentView = _views[sceneName];
    }
}
