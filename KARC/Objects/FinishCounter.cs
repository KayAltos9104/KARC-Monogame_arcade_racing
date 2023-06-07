using KARC.Settings;
using KARC.WitchEngine;
using KARC.WitchEngine.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace KARC.Objects
{
    public class FinishCounter : InterfaceComponent
    {
        public Vector2 Speed { get; set; } 
        public float FinishDistance { get; set; }
        public float Width { get; set; }   
        public float CarSignShift { get; set; }
        public float FinishSignShift { get; set; }       
        public bool IsSpriteScaled { get; set; }

        private (int id, Vector2 pos) _carSign;
        private (int id, Vector2 pos) _finishSign;
        private bool _isFinished;

        public FinishCounter(Vector2 pos):base(pos)
        {
            Pos = pos;                      
            _carSign = (
                (int)Sprite.wall,
                new Vector2(0, 0));
            _finishSign = (
                (int)Sprite.finishTape,
                 new Vector2(SpriteParameters.Sprites[Sprite.finishCounterWindow].width / 2- SpriteParameters.Sprites[Sprite.finishTape].width, 0));
            Sprites = new List<(int ImageId, Vector2 ImagePos)>() {
                ((int)Sprite.finishCounterWindow, Vector2.Zero),
                _carSign,
                _finishSign,
                
            };                        
            IsSpriteScaled = false;
            Width = SpriteParameters.Sprites[Sprite.finishCounterWindow].width;            
            CarSignShift = 0.5f * SpriteParameters.Sprites[Sprite.wall].width;
            Layer = 1.0f; 
            _isFinished = false;
            FinishDistance = 1;           
        }       

        public void Move(Vector2 pos)
        {
            Pos = pos;
        }

        public void Update(GameTime gameTime)
        {
            if (!_isFinished)
            {
                _carSign.pos = new Vector2((1 - FinishDistance) * Width + CarSignShift - SpriteParameters.Sprites[Sprite.finishCounterWindow].width/2, _carSign.pos.Y);                
                Sprites[Sprites.FindIndex(s => s.ImageId == _carSign.id)] = _carSign;
                Sprites[Sprites.FindIndex(s => s.ImageId == _finishSign.id)] = _finishSign;
            }
            if ((1 - FinishDistance) * Width + CarSignShift >= Width)
                _isFinished = true;            
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            foreach (var sprite in Sprites)
            { 
                spriteBatch.Draw(
                       texture: LoadableObjects.Textures[sprite.ImageId],
                       position: Pos + sprite.ImagePos,
                       sourceRectangle: null,
                       Color.White,
                       rotation: 0,
                       origin: new Vector2(LoadableObjects.Textures[sprite.ImageId].Width / 2, LoadableObjects.Textures[sprite.ImageId].Height / 2),
                       scale: Vector2.One,
                       SpriteEffects.None,
                       layerDepth: Layer);
            }
        }
    }
}

