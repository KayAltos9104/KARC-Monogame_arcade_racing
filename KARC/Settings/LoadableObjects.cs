using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KARC.Settings;

public static class LoadableObjects
{
    public static SpriteFont TextBlock { get; set; }
    public static Dictionary<int, Texture2D> Textures = new Dictionary<int, Texture2D>();
}
