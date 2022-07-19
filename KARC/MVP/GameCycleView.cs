using KARC.Objects;
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
        public event EventHandler GamePaused = delegate { };
        public event EventHandler<InitializeEventArgs> GameLaunched = delegate { };

        private Dictionary<int, IObject> _objects = new Dictionary<int, IObject>();
        private Dictionary<int, Texture2D> _textures = new Dictionary<int, Texture2D>();

        private Vector2 _visualShift = new Vector2(0, 0);

        private List<Keys> _pressedPrevFrame = new List<Keys>();

        private SpriteFont _textBlock;

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
            //_visualShift.X -= _graphics.PreferredBackBufferWidth / 2;
            //_visualShift.Y -= _graphics.PreferredBackBufferHeight * 0.8f-50;
            GameLaunched.Invoke(this, new InitializeEventArgs() { 
                Resolution = (_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight) 
            });
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _textures.Add((byte)Factory.ObjectTypes.car, Content.Load<Texture2D>("Base_car"));
            _textures.Add((byte)Factory.ObjectTypes.wall, Content.Load<Texture2D>("Wall"));
            _textures.Add((byte)Factory.ObjectTypes.window, Content.Load<Texture2D>("Message_Window"));
            _textures.Add((byte)Factory.ObjectTypes.finish, Content.Load<Texture2D>("FinishSprite"));
            _textBlock = Content.Load<SpriteFont>("DescriptionFont");
        }

        public void LoadGameCycleParameters(Dictionary<int, IObject> Objects, Vector2 POVShift)
        {
            _objects = Objects;
            _visualShift = POVShift;
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
                GameLaunched.Invoke(this, new InitializeEventArgs()
                {
                    Resolution = (_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight)
                });

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);

            _pressedPrevFrame = new List<Keys>(keys);
            CycleFinished.Invoke(this, new EventArgs());
        }

        private bool IsSinglePressed(Keys key)
        {            
            return Keyboard.GetState().IsKeyUp(key) && _pressedPrevFrame.Contains(key);
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.Clear(Color.DarkSeaGreen);


            //_spriteBatch.Begin(SpriteSortMode.BackToFront);
            _spriteBatch.Begin();           
            foreach (var o in _objects.Values)
            {                
                foreach (var sprite in o.Sprites)
                {
                    if (sprite.ImageId == -1)
                        continue;
                    if (o is IComponent component)
                    {
                        var s = _textBlock.MeasureString(component.Text) * 1.2f;
                        Vector2 textPos = new Vector2(
                            o.Pos.X + (s.X - _textBlock.MeasureString(component.Text).X) / 2 - (component.IsCentered?s.X/2:0),
                            o.Pos.Y + (s.Y - _textBlock.MeasureString(component.Text).Y) / 2 - (component.IsCentered ? s.Y / 2 : 0)
                            );

                        _spriteBatch.Draw(
                            texture: _textures[sprite.ImageId],
                            position: o.Pos + sprite.ImagePos - (component.IsCentered ? s / 2 : Vector2.Zero),
                            sourceRectangle: null,
                            Color.White,
                            rotation: 0,
                            origin: Vector2.Zero,
                            scale: new Vector2(
                                s.X / _textures[sprite.ImageId].Width,
                                s.Y / _textures[sprite.ImageId].Height),
                            SpriteEffects.None,
                            layerDepth: o.Layer);

                        _spriteBatch.DrawString(
                            _textBlock,
                            component.Text,
                            textPos,
                            Color.Black,
                            rotation: 0,
                            origin: Vector2.Zero,
                            scale: 1,
                            SpriteEffects.None,
                            layerDepth: 0
                            );

                    }
                    else
                    {
                        _spriteBatch.Draw(_textures[sprite.ImageId], o.Pos - _visualShift + sprite.ImagePos, Color.White);
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
                }                
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
