using KARC.Settings;
using KARC.WitchEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KARC.Objects
{
    public class FinishCounter : IObject, IComponent
    {
        public float Layer { get; set; }
        public List<(int ImageId, Vector2 ImagePos)> Sprites { get; set; }
        public Vector2 Pos { get; private set; }
        public Vector2 Speed { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public bool IsCentered { get; set; }
        public float FinishDistance { get; set; }
        public float Width { get; set; }   
        public float CarSignShift { get; set; }
        public float FinishSignShift { get; set; }
        public Vector2 TextPos { get; set; }
        public bool IsSpriteScaled { get; set; }

        private (int id, Vector2 pos) _carSign;
        private (int id, Vector2 pos) _finishSign;
        private bool _isFinished;

        public FinishCounter(Vector2 pos, string text)
        {
            Pos = pos;
            Text = text;
            Name = "";
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
            //FinishSignShift = -200;
            CarSignShift = 0.5f * SpriteParameters.Sprites[Sprite.wall].width;
            Layer = 1.0f; 
            _isFinished = false;
            FinishDistance = 1;
            //_finishSign.pos = new Vector2(Width + FinishSignShift, _finishSign.pos.Y);
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
    }
}
