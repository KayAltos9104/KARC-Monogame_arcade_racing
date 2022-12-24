using KARC.Settings;
using KARC.WitchEngine;
using Microsoft.Xna.Framework;
using System;


namespace KARC.Models;

public abstract class Generator
{
    protected int _width;
    protected int _height;
    protected IObject _createdObj;
    public event EventHandler OnCreated;
    public Generator(int width, int height)
    {
        _width = width;
        _height = height;
    }

    public virtual void CreateObject (int xTile, int yTile)
    {
        OnCreated?.Invoke(this, EventArgs.Empty);
    }

    public IObject GetObject()
    {
        return _createdObj;
    }
    
    protected void MultipleSprites (Sprite sprite)
    {
        int segmentWidth = SpriteParameters.Sprites[sprite].width;
        int segmentHeight = SpriteParameters.Sprites[sprite].height;

        for (int i = 0; i < _width / segmentWidth; i++)
            for (int j = 0; j < _height / segmentHeight; j++)
            {
                _createdObj.Sprites.Add(((byte)sprite, new Vector2(i * segmentWidth, j * segmentHeight)));
            }
    }
}
