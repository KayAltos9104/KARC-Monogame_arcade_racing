using KARC.Objects;
using KARC.Settings;
using KARC.WitchEngine;
using KARC.WitchEngine.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace KARC.WitchEngine.UI
{
    public class MessageBox : IComponent    
    {
        
        public List<(int ImageId, Vector2 ImagePos)> Sprites { get; set; }
        public Vector2 Pos { get; private set; } 
        public string Text { get; set; }       
        public float Layer { get; set; }
        public bool IsCentered { get; set; }
        public Vector2 TextPos { get; set; }
       
        
        public MessageBox(Vector2 pos, string text)
        {            

            Pos = pos;
            Text = text;
            TextPos = Vector2.Zero;  
            Layer = 1.0f;            
        }
        public void Move(Vector2 pos)
        {
            Pos = pos;
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Render(SpriteBatch spriteBatch)
        {
            float marginText = 20;

            var s = LoadableObjects.TextBlock.MeasureString(Text) != Vector2.Zero ?
                LoadableObjects.TextBlock.MeasureString(Text) + new Vector2(marginText, marginText) :
                Vector2.One;
            Vector2 textPos = new Vector2(
                Pos.X + (s.X - LoadableObjects.TextBlock.MeasureString(Text).X) / 2 - (IsCentered ? s.X / 2 : 0),
                Pos.Y + (s.Y - LoadableObjects.TextBlock.MeasureString(Text).Y) / 2 - (IsCentered ? s.Y / 2 : 0)
                );

            int x = (int)(Pos - (IsCentered ? s / 2 : Vector2.Zero)).X;
            int y = (int)(Pos - (IsCentered ? s / 2 : Vector2.Zero)).Y;

            Graphics2D.FillRectangle(x, y, (int)s.X, (int)s.Y, Color.DarkSeaGreen);
            Graphics2D.DrawRectangle(x, y, (int)s.X, (int)s.Y, Color.Black, 3);

            spriteBatch.DrawString(
                        spriteFont: LoadableObjects.TextBlock,
                        Text,
                        position: textPos + TextPos,
                        color: Color.Black,
                        rotation: 0,
                        origin: Vector2.Zero,
                        scale: 1,
                        SpriteEffects.None,
                        layerDepth: 0
                        );
            
        }
    }
}
