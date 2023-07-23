using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitchEngine;

public class Scene
{
    public View View { get; set; }

    public void Update (GameTime gameTime)
    {
        View.Update();
    }
    public void Draw(GameTime gameTime)
    {
        View.Draw(gameTime);
    }
}
