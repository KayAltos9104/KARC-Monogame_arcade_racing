using KARC.WitchEngine;
using Microsoft.Xna.Framework;
using System;

namespace KARC.MVP;
public class MenuModel : IModel
{
    public GameTime GameTime { get; set; }

    public event EventHandler<GameplayEventArgs> Updated;

    public void Initialize((int width, int height) resolution)
    {
        throw new NotImplementedException();
    }

    public void Update()
    {
        throw new NotImplementedException();
    }
}
