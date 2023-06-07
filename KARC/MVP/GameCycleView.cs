using KARC.Models;
using KARC.Objects;
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
        private InterfaceController _interfaceController = new InterfaceController();


        private List<Keys> _pressedPrevFrame = new List<Keys>();


        private int _frameCounter = 0;
        private int _timeRange = 1; //Время между измерениями в миллисекундах
        private int _elapsedFPSTime = 0;


        private bool _isCollidersShown = false;
        private bool _isPaused = false;

        public GameCycleView()
        {
            _graphics = new GraphicsDeviceManager(this);
            Graphics2D.Graphics = _graphics;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        protected override void Initialize()
        {
            base.Initialize();
            this.Window.Title = "KARC";


            Graphics2D.Graphics.IsFullScreen = false;
            Graphics2D.Graphics.PreferredBackBufferWidth = 1600;
            Graphics2D.Graphics.PreferredBackBufferHeight = 900;
            Graphics2D.Graphics.ApplyChanges();
            Graphics2D.UpdateVisionArea();

            MessageBox MbxScore = new MessageBox(new Vector2(
                10, 10),
                "Очки: 0"
                );

            MessageBox MbxSpeed = new MessageBox(new Vector2(
                10, 110),
                "Скорость: 0 км/ч"
                );

            MessageBox MbxFps = new MessageBox(new Vector2(
                10, 210),
                "FPS: "
                );

            MessageBox MbxInstructions = new MessageBox(new Vector2(
                10, 600),
                "Управление:\n" +
                "W - Вперед\n" +
                "A, D - Влево, вправо\n" +
                "R - Начать заново\n" +
                "P - Пауза\n" +
                "C - Коллайдеры"
                );

            FinishCounter finishCounter = new FinishCounter(pos:
                new Vector2(Graphics2D.Graphics.PreferredBackBufferWidth / 2,
                SpriteParameters.Sprites[Sprite.finishCounterWindow].height / 2));

            _interfaceController.AddComponent("MbxScore", MbxScore);
            _interfaceController.AddComponent("MbxSpeed", MbxSpeed);
            _interfaceController.AddComponent("FPS", MbxFps);
            _interfaceController.AddComponent("FinishCounter", finishCounter);
            _interfaceController.AddComponent("Instructions", MbxInstructions);


            GameLaunched.Invoke(this, new InitializeEventArgs()
            {
                Resolution = (Graphics2D.Graphics.PreferredBackBufferWidth, Graphics2D.Graphics.PreferredBackBufferHeight)
            });
        }
        protected override void LoadContent()
        {
            Graphics2D.SpriteBatch = new SpriteBatch(GraphicsDevice);

            LoadableObjects.Textures.Add((byte)Sprite.car, Content.Load<Texture2D>("Base_car"));
            LoadableObjects.Textures.Add((byte)Sprite.wall, Content.Load<Texture2D>("Wall"));
            LoadableObjects.Textures.Add((byte)Sprite.window, Content.Load<Texture2D>("Message_Window"));
            LoadableObjects.Textures.Add((byte)Sprite.finishTape, Content.Load<Texture2D>("FinishSprite"));
            LoadableObjects.Textures.Add((byte)Sprite.finishCounterWindow, Content.Load<Texture2D>("FinishCounterField"));
            LoadableObjects.Textures.Add((byte)Sprite.shield, Content.Load<Texture2D>("Immortality"));
            LoadableObjects.Textures.Add((byte)Sprite.explosion, Content.Load<Texture2D>("Explosion_Atlas"));
            LoadableObjects.TextBlock = Content.Load<SpriteFont>("DescriptionFont");
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
            Graphics2D.VisualShift = POVShift;
            _interfaceController.Components["MbxScore"].Text = "Очки: " + score;
            _interfaceController.Components["MbxSpeed"].Text = "Скорость: " + Math.Abs(2 * speed * (3600.0 / 1000.0)) + " км/ч";

            foreach (var e in effects)
            {
                if (_interfaceController.Components.ContainsKey(e.effectSprite.ToString()))
                {
                    _interfaceController.Components[e.effectSprite.ToString()].Text = e.timeLeft.ToString();
                }
                else
                {
                    _interfaceController.Components.Add(e.effectSprite.ToString(),
                        new Parameter(
                        new Vector2(10, 100 + 50 * _interfaceController.Components.Count - 4),
                        e.timeLeft.ToString(),
                        e.effectSprite)
                        );
                }
                if (e.timeLeft <= 0)
                    _interfaceController.Components.Remove(e.effectSprite.ToString());
            }

            var f = (FinishCounter)_interfaceController.Components["FinishCounter"];
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
            {
                GamePaused.Invoke(this, new EventArgs());
                if (_isPaused)
                {
                    _interfaceController.Components.Remove("MbxPause");
                    _isPaused = false;
                }
                else
                {
                    _isPaused = true;
                    ShowPauseMessage();
                }


            }

            if (IsSinglePressed(Keys.R))
            {
                _interfaceController.Components.Remove("MbxGameOver");

                GameLaunched.Invoke(this, new InitializeEventArgs()
                {
                    Resolution = (_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight)
                });
            }

            if (IsSinglePressed(Keys.C))
            {
                if (_isCollidersShown)
                    _isCollidersShown = false;
                else
                    _isCollidersShown = true;

            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            base.Update(gameTime);
            _pressedPrevFrame = new List<Keys>(keys);
            CycleFinished.Invoke(this, new CycleViewEventArgs() { GameTime = gameTime });
        }

        private bool IsSinglePressed(Keys key)
        {
            return Keyboard.GetState().IsKeyUp(key) && _pressedPrevFrame.Contains(key);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSeaGreen);
            Graphics2D.SpriteBatch.Begin();


            foreach (var o in _objects.Values)
            {
                Graphics2D.RenderObject(o);
                if (o is IAnimated)
                    Graphics2D.RenderAnimation(o as IAnimated);




                // Отрисовка коллайдеров
                if (_isCollidersShown && (o is Car))
                {
                    Vector2 v = o.Pos - Graphics2D.VisualShift;
                    if (!Graphics2D.IsInVisionArea(v))
                        continue;

                    foreach (var collider in (o as ISolid).Colliders)
                    {
                        Graphics2D.DrawRectangle(
                            collider.Collider.Boundary.X + (int)-Graphics2D.VisualShift.X,
                            collider.Collider.Boundary.Y + (int)-Graphics2D.VisualShift.Y,
                            collider.Collider.Boundary.Width,
                            collider.Collider.Boundary.Height,
                            Color.White);
                    }
                }
                else if (_isCollidersShown && (o is Trigger2D))
                {
                    Vector2 v = o.Pos - Graphics2D.VisualShift;
                    if (!Graphics2D.IsInVisionArea(v))
                        continue;

                    var collider = (o as ITrigger).Collider;
                    Graphics2D.DrawRectangle(
                        collider.Boundary.X + (int)-Graphics2D.VisualShift.X,
                        collider.Boundary.Y + (int)-Graphics2D.VisualShift.Y,
                        collider.Boundary.Width,
                        collider.Boundary.Height,
                        Color.White);

                }
            }

            //Рисуем компоненты последними, чтобы были поверх
            _interfaceController.RenderAll(Graphics2D.SpriteBatch);

            _frameCounter++;

            if ((int)gameTime.TotalGameTime.TotalSeconds - _elapsedFPSTime > _timeRange)
            {

                _interfaceController.Components["FPS"].Text = "FPS: " + (_frameCounter /
                    ((int)gameTime.TotalGameTime.TotalSeconds - _elapsedFPSTime)).ToString("N0");
                _elapsedFPSTime = (int)gameTime.TotalGameTime.TotalSeconds;
                _frameCounter = 0;
            }
            Graphics2D.SpriteBatch.End();
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
            _interfaceController.Components.Add("MbxGameOver", gameOverMessage);
        }

        public void ShowPauseMessage()
        {
            string message = "Пауза\nНажмите P, чтобы продолжить";
            MessageBox MbxPause = new MessageBox(new Vector2(
                _graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2),
                message
                );
            MbxPause.IsCentered = true;
            _interfaceController.Components.Add("MbxPause", MbxPause);
        }
    }
}

