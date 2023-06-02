using KARC.Objects;
using KARC.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KARC.WitchEngine.UI
{
    public class Parameter : IComponent
    {
        public float Layer { get; set; }
        public List<(int ImageId, Vector2 ImagePos)> Sprites { get; set; }
        public Vector2 Pos { get; private set; }       
        public string Text { get; set; }
        public bool IsCentered { get; set; }
        public Vector2 TextPos { get; set; }        

        public Parameter (Vector2 pos, string text, byte sprite)
        {
            Pos = pos;
            Text = text;
            TextPos = new Vector2 (70, 0);            
            Sprites = new List<(int ImageId, Vector2 ImagePos)>();
            Sprites.Add((sprite, Vector2.Zero));            
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
            foreach (var sprite in Sprites)
            {
                spriteBatch.Draw(
                       texture: LoadableObjects.Textures[sprite.ImageId],
                       position: Pos + sprite.ImagePos,
                       sourceRectangle: null,
                       Color.White,
                       rotation: 0,
                       origin: Vector2.Zero,
                       scale: Vector2.One,
                       SpriteEffects.None,
                       layerDepth: Layer);
            }

            float marginText = 20;

            var s = LoadableObjects.TextBlock.MeasureString(Text) != Vector2.Zero ?
                LoadableObjects.TextBlock.MeasureString(Text) + new Vector2(marginText, marginText) :
                Vector2.One;
            Vector2 textPos = new Vector2(
                Pos.X + (s.X - LoadableObjects.TextBlock.MeasureString(Text).X) / 2 - (IsCentered ? s.X / 2 : 0),
                Pos.Y + (s.Y - LoadableObjects.TextBlock.MeasureString(Text).Y) / 2 - (IsCentered ? s.Y / 2 : 0)
                );
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
