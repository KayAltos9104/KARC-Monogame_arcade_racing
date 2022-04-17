using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace KARC
{
    public class GameCycleView : Game, IGameplayView
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public event EventHandler CycleFinished = delegate { };
        public event EventHandler<ControlsEventArgs> PlayerMoved = delegate { };

        private GameplayPresenter gameplayPresenter;
        private Vector2 _playerPos = Vector2.Zero;
        private Texture2D _player;

        public GameCycleView()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;            
        }

        protected override void Initialize()
        { 
            base.Initialize();
            gameplayPresenter = new GameplayPresenter(this, new GameCycle());
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _player = Content.Load<Texture2D>("White_Placeholder");
        }

        public void LoadGameCycleParameters(Vector2 pos)
        {
            _playerPos = pos;
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
                            PlayerMoved.Invoke(this, new ControlsEventArgs { direction = IGameplayModel.Direction.forward });
                            break;
                        }
                    case Keys.S:
                        {
                            PlayerMoved.Invoke(this, new ControlsEventArgs { direction = IGameplayModel.Direction.backward });
                            break;
                        }
                    case Keys.D:
                        {
                            PlayerMoved.Invoke(this, new ControlsEventArgs { direction = IGameplayModel.Direction.right });
                            break;
                        }
                    case Keys.A:
                        {
                            PlayerMoved.Invoke(this, new ControlsEventArgs { direction = IGameplayModel.Direction.left });
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
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);

            _spriteBatch.Begin();
            _spriteBatch.Draw(_player, _playerPos, Color.White);
            _spriteBatch.End();
        }        
    }
}
