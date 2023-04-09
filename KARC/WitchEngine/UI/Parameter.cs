using KARC.Objects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KARC.WitchEngine.UI
{
    public class Parameter : IObject, IComponent
    {
        public float Layer { get; set; }
        public List<(int ImageId, Vector2 ImagePos)> Sprites { get; set; }

        public Vector2 Pos { get; private set; }

        public Vector2 Speed { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public bool IsCentered { get; set; }
        public Vector2 TextPos { get; set; }
        public bool IsSpriteScaled { get; set; }

        public Parameter (Vector2 pos, string text, byte sprite)
        {
            Pos = pos;
            Text = text;
            TextPos = new Vector2 (70, 0);
            Name = "";
            Sprites = new List<(int ImageId, Vector2 ImagePos)>();
            Sprites.Add((sprite, Vector2.Zero));
            IsSpriteScaled = false;
            Layer = 1.0f;
        }

        public void Move(Vector2 pos)
        {
            Pos = pos;
        }

        public void Update(GameTime gameTime)
        {
            
        }
    }
}
