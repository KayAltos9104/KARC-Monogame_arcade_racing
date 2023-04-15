using KARC.Objects;
using KARC.Prefabs;
using KARC.Settings;
using KARC.WitchEngine;
using KARC.WitchEngine.Animations;
using KARC.WitchEngine.Primitives;
using KARC.WitchEngine.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;


namespace KARC.MVP
{
    public class GameCycleView : Game, IGameplayView
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public event EventHandler<CycleViewEventArgs> CycleFinished = delegate { };
        public event EventHandler<ControlsEventArgs> PlayerSpeedChanged = delegate { };
        public event EventHandler GamePaused = delegate { };
        public event EventHandler<InitializeEventArgs> GameLaunched = delegate { };

        private Dictionary<int, IObject> _objects = new Dictionary<int, IObject>();        
        private Dictionary<string, IComponent> _components = new Dictionary<string, IComponent>();        
        private Dictionary<int, Texture2D> _textures = new Dictionary<int, Texture2D>();

        private Vector2 _visualShift = new Vector2(0, 0);

        private List<Keys> _pressedPrevFrame = new List<Keys>();

        private SpriteFont _textBlock;

        private int _frameCounter = 0;
        private int _timeRange = 1; //Время между измерениями в миллисекундах
        private int _elapsedFPSTime = 0;

        private FinishCounterUIGenerator finishCounterUIGenerator = new FinishCounterUIGenerator();

        public GameCycleView()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Graphics2D.SpriteBatch = _spriteBatch;
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.Window.Title = "KARC";
           

            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 1600;
            _graphics.PreferredBackBufferHeight = 900;
            _graphics.ApplyChanges();

            MessageBox MbxScore = new MessageBox(new Vector2(
                0, 0),
                "Очки: 0"
                );

            MessageBox MbxSpeed = new MessageBox(new Vector2(
                0, 100),
                "Скорость: 0 км/ч"
                );

            MessageBox MbxFps = new MessageBox(new Vector2(
                0, 200),
                "FPS: "
                );

            finishCounterUIGenerator.CreateObject(
                (_graphics.PreferredBackBufferWidth - SpriteParameters.Sprites[Sprite.finishCounterWindow].width) / 2, 0
                );
            FinishCounter finishCounter = (FinishCounter)finishCounterUIGenerator.GetObject();
            

            _components.Add("MbxScore", MbxScore);
            _components.Add("MbxSpeed", MbxSpeed);
            _components.Add("FPS", MbxFps);
            _components.Add("FinishCounter", finishCounter);

            GameLaunched.Invoke(this, new InitializeEventArgs() { 
                Resolution = (_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight) 
            });
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _textures.Add((byte)Sprite.car, Content.Load<Texture2D>("Base_car"));
            //_textures.Add((byte)Factory.ObjectTypes.car, Content.Load<Texture2D>("Base_car"));
            _textures.Add((byte)Sprite.wall, Content.Load<Texture2D>("Wall"));
            //_textures.Add((byte)Factory.ObjectTypes.wall, Content.Load<Texture2D>("Wall"));
            _textures.Add((byte)Sprite.window, Content.Load<Texture2D>("Message_Window"));
            //_textures.Add((byte)Factory.ObjectTypes.window, Content.Load<Texture2D>("Message_Window"));
            _textures.Add((byte)Sprite.finishTape, Content.Load<Texture2D>("FinishSprite"));
            //_textures.Add((byte)Factory.ObjectTypes.finish, Content.Load<Texture2D>("FinishSprite"));
            _textures.Add((byte)Sprite.finishCounterWindow, Content.Load<Texture2D>("FinishCounterField"));
            //_textures.Add((byte)Factory.ObjectTypes.finishCounterField, Content.Load<Texture2D>("FinishCounterField"));
            _textures.Add((byte)Sprite.shield, Content.Load<Texture2D>("Immortality"));
            //_textures.Add((byte)Factory.ObjectTypes.shield, Content.Load<Texture2D>("Immortality"));
            _textures.Add((byte)Sprite.explosion, Content.Load<Texture2D>("Explosion_Atlas"));
            //_textures.Add((byte)Factory.ObjectTypes.explosion, Content.Load<Texture2D>("Explosion_Atlas"));
            _textBlock = Content.Load<SpriteFont>("DescriptionFont");
        }

        public void LoadGameCycleParameters(
            Dictionary<int, IObject> Objects, 
            Vector2 POVShift, 
            int score, 
            int speed, 
            float distToFinish, 
            List<(byte effectSprite, int timeLeft)> effects)
        {
            _objects = Objects;
            _visualShift = POVShift;
            _components["MbxScore"].Text = "Очки: " + score;
            _components["MbxSpeed"].Text = "Скорость: " + Math.Abs(2*speed * (3600.0 / 1000.0)) + " км/ч";            
                
            foreach (var e in effects)
            {
                if (_components.ContainsKey(e.effectSprite.ToString()))
                {
                    _components[e.effectSprite.ToString()].Text = e.timeLeft.ToString();
                }                    
                else
                {
                    _components.Add(e.effectSprite.ToString(), 
                        new Parameter(
                        new Vector2(0, 100 + 50 * _components.Count - 4), 
                        e.timeLeft.ToString(), 
                        e.effectSprite)
                        );
                }
                if (e.timeLeft <= 0)
                    _components.Remove(e.effectSprite.ToString());
            }
            
            var f = (FinishCounter)_components["FinishCounter"];
            f.FinishDistance = distToFinish;
            f.Update(new GameTime());            
        }
        protected override void Update(GameTime gameTime)
        {
            var keys = Keyboard.GetState().GetPressedKeys();
            if (keys.Length > 0)
            {
                var k = keys[0];
                switch (k)
                {
                    case Keys.W:
                        {
                            PlayerSpeedChanged.Invoke(this, new ControlsEventArgs { Direction = IGameplayModel.Direction.forward });
                            break;
                        }
                    case Keys.S:
                        {
                            PlayerSpeedChanged.Invoke(this, new ControlsEventArgs { Direction = IGameplayModel.Direction.backward });
                            break;
                        }
                    case Keys.D:
                        {
                            PlayerSpeedChanged.Invoke(this, new ControlsEventArgs { Direction = IGameplayModel.Direction.right });
                            break;
                        }
                    case Keys.A:
                        {
                            PlayerSpeedChanged.Invoke(this, new ControlsEventArgs { Direction = IGameplayModel.Direction.left });
                            break;
                        }                   
                    case Keys.Escape:
                        {
                            break;
                        }
                }
            }

            if (IsSinglePressed(Keys.P))
                GamePaused.Invoke(this, new EventArgs());

            if(IsSinglePressed(Keys.R))
            {
                _components.Remove("MbxGameOver");
                
                GameLaunched.Invoke(this, new InitializeEventArgs()
                {
                    Resolution = (_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight)
                });
            }
                

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            base.Update(gameTime);            
            _pressedPrevFrame = new List<Keys>(keys);            
            CycleFinished.Invoke(this, new CycleViewEventArgs() { GameTime = gameTime});
        }

        private bool IsSinglePressed(Keys key)
        {            
            return Keyboard.GetState().IsKeyUp(key) && _pressedPrevFrame.Contains(key);
        }

        protected override void Draw(GameTime gameTime)
        {            
            GraphicsDevice.Clear(Color.DarkSeaGreen);
            _spriteBatch.Begin();
           
           
            foreach (var o in _objects.Values)
            {
                foreach (var sprite in o.Sprites)
                {
                    if (sprite.ImageId == -1)
                        continue;

                    Vector2 v = o.Pos + sprite.ImagePos - _visualShift;
                    if (v.X < -100 || v.X > _graphics.PreferredBackBufferWidth + 100 || v.Y < -100 || v.Y > _graphics.PreferredBackBufferHeight + 100)
                        continue;
                    
                    _spriteBatch.Draw(
                        texture: _textures[sprite.ImageId],
                        position: o.Pos - _visualShift + sprite.ImagePos,
                        sourceRectangle: null,
                        Color.White,
                        rotation: 0,
                        origin: Vector2.Zero,
                        scale: 1,
                        SpriteEffects.None,
                        layerDepth: o.Layer);
                }

                if (o is IAnimated && 
                    (o as IAnimated).Animation.ActiveAnimation != null && 
                    (o as IAnimated).Animation.ActiveAnimation.IsActive)
                {
                    var a = o as IAnimated;
                    var centerShift = a.Animation.ActiveAnimation.IsCentered ?
                        new Vector2(a.Animation.ActiveAnimation.CurrentFrame.Width / 2,
                        a.Animation.ActiveAnimation.CurrentFrame.Height / 2) :
                        Vector2.Zero;
                    Vector2 v = a.Animation.Pos - _visualShift - centerShift;

                    if (v.X < -100 || v.X > _graphics.PreferredBackBufferWidth + 100 || v.Y < -100 || v.Y > _graphics.PreferredBackBufferHeight + 100)
                        continue;

                    _spriteBatch.Draw(
                       _textures[a.Animation.ActiveAnimation.GetPictureId()],
                        v,
                        new Rectangle(
                            a.Animation.ActiveAnimation.CurrentFrame.Point,
                            new Point(
                            a.Animation.ActiveAnimation.CurrentFrame.Width,
                            a.Animation.ActiveAnimation.CurrentFrame.Height)),
                        Color.White,
                        0,
                        Vector2.Zero,
                        1,
                        SpriteEffects.None, a.Animation.Layer);
                }
            }
            foreach (var c in _components.Values)
            {
                var o = (IObject)c;
                foreach (var sprite in o.Sprites)
                {
                    if (sprite.ImageId == -1)
                        continue;

                    var s = _textBlock.MeasureString(c.Text) != Vector2.Zero ? 
                        _textBlock.MeasureString(c.Text) * 1.2f : 
                        new Vector2 (_textures[sprite.ImageId].Width, _textures[sprite.ImageId].Height) ;
                    Vector2 textPos = new Vector2(
                        o.Pos.X + (s.X - _textBlock.MeasureString(c.Text).X) / 2 - (c.IsCentered ? s.X / 2 : 0),
                        o.Pos.Y + (s.Y - _textBlock.MeasureString(c.Text).Y) / 2 - (c.IsCentered ? s.Y / 2 : 0)
                        );

                    _spriteBatch.Draw(
                        texture: _textures[sprite.ImageId],
                        position: o.Pos + sprite.ImagePos - (c.IsCentered ? s / 2 : Vector2.Zero),
                        sourceRectangle: null,
                        Color.White,
                        rotation: 0,
                        origin: Vector2.Zero,
                        scale: s==Vector2.Zero||!c.IsSpriteScaled ? Vector2.One : new Vector2(
                            s.X / _textures[sprite.ImageId].Width,
                            s.Y / _textures[sprite.ImageId].Height),
                        SpriteEffects.None,
                        layerDepth: o.Layer);

                    _spriteBatch.DrawString(
                        _textBlock,
                        c.Text,
                        textPos+c.TextPos,
                        Color.Black,
                        rotation: 0,
                        origin: Vector2.Zero,
                        scale: 1,
                        SpriteEffects.None,
                        layerDepth: 0
                        );
                }
            }
            _frameCounter++;
            
            if ((int)gameTime.TotalGameTime.TotalSeconds - _elapsedFPSTime > _timeRange)
            {
                
                _components["FPS"].Text = "FPS: " + (_frameCounter / 
                    ((int)gameTime.TotalGameTime.TotalSeconds - _elapsedFPSTime)).ToString("N0");
                _elapsedFPSTime = (int)gameTime.TotalGameTime.TotalSeconds;
                _frameCounter = 0;               
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
        public void ShowGameOver(bool isWin)
        {
            string message = isWin 
                ? "Игра окончена!\nВы выиграли!\nНажмите R, чтобы начать заново"
                : "Игра окончена!\nВы проиграли!\nНажмите R для перезагрузки";
            MessageBox gameOverMessage = new MessageBox(new Vector2(
                _graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2),
                message
                );
            gameOverMessage.IsCentered = true;
            _components.Add("MbxGameOver", gameOverMessage);            
        }
    }
}

