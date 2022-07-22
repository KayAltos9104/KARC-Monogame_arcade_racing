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

        private (int id, Vector2 pos) _carSign;
        private (int id, Vector2 pos) _finishSign;
        private bool _isFinished;

        public FinishCounter(Vector2 pos, string text, (int id, Vector2 pos) carSign, (int id, Vector2 pos) finishSign, int width)
        {
            Pos = pos;
            Text = text;
            Name = "";
            Sprites = new List<(int ImageId, Vector2 ImagePos)>();
            Sprites.Add(finishSign);
            Sprites.Add(carSign);
            _carSign = Sprites.Single(s => s.ImageId == carSign.id);
            _finishSign = Sprites.Single(s => s.ImageId == finishSign.id);
            Width = width;
            Layer = 1.0f; 
            _isFinished = false;
            FinishDistance = 1;
        }       

        public void Move(Vector2 pos)
        {
            Pos = pos;
        }

        public void Update()
        {
            if (!_isFinished)
            {
                _carSign.pos = new Vector2((1 - FinishDistance) * Width, _carSign.pos.Y);
                _finishSign.pos = new Vector2(Width + FinishSignShift, _finishSign.pos.Y);
                Sprites[Sprites.FindIndex(s => s.ImageId == _carSign.id)] = _carSign;
                Sprites[Sprites.FindIndex(s => s.ImageId == _finishSign.id)] = _finishSign;
            }
            if ((1 - FinishDistance) * Width + CarSignShift >= Width)
                _isFinished = true;
            
        }
    }
}
