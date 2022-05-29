using KARC.WitchEngine;
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

        public event EventHandler CycleFinished = delegate { };
        public event EventHandler<ControlsEventArgs> PlayerSpeedChanged = delegate { };

        private Dictionary<int, IObject> _objects = new Dictionary<int, IObject>();
        private Dictionary<int, Texture2D> _textures = new Dictionary<int, Texture2D>();

        private Vector2 _visualShift = new Vector2(0, 0);

        public GameCycleView()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        protected override void Initialize()
        {
            base.Initialize();
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.PreferredBackBufferHeight = 768;
            _graphics.ApplyChanges();
            _visualShift.X -= _graphics.PreferredBackBufferWidth / 2;
            _visualShift.Y -= _graphics.PreferredBackBufferHeight * 0.8f-50;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _textures.Add((byte)GameCycle.ObjectTypes.car, Content.Load<Texture2D>("Base_car"));
            _textures.Add((byte)GameCycle.ObjectTypes.wall, Content.Load<Texture2D>("Wall"));
        }

        public void LoadGameCycleParameters(Dictionary<int, IObject> Objects, Vector2 POVShift)
        {
            _objects = Objects;
            _visualShift += POVShift;
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

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);


            CycleFinished.Invoke(this, new EventArgs());
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.Clear(Color.DarkSeaGreen);
            base.Draw(gameTime);
            
            _spriteBatch.Begin();           
            foreach (var o in _objects.Values)
            {
                _spriteBatch.Draw(_textures[o.ImageId], o.Pos - _visualShift, Color.White);
            }
            _spriteBatch.End();
        }
    }
}
