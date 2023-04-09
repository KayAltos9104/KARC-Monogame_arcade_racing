using KARC.Objects;
using KARC.Settings;
using KARC.WitchEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace KARC.WitchEngine.UI
{
    public class MessageBox : IObject, IComponent    
    {
        
        public List<(int ImageId, Vector2 ImagePos)> Sprites { get; set; }

        public Vector2 Pos { get; private set; }

        public Vector2 Speed { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
       
        public float Layer { get; set; }
        public bool IsCentered { get; set; }
        public Vector2 TextPos { get; set; }
        public bool IsSpriteScaled { get; set; }
        
        public MessageBox(Vector2 pos, string text)
        {            

            Pos = pos;
            Text = text;
            Name = "";
            TextPos = Vector2.Zero;
            Sprites = new List<(int ImageId, Vector2 ImagePos)>();
            Sprites.Add(((int)Sprite.window, Vector2.Zero));
            IsSpriteScaled = true;
            Layer = 1.0f;            
        }

        public MessageBox(Vector2 pos, string name, string text) : this(pos, text)
        {
            Name = name;
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
