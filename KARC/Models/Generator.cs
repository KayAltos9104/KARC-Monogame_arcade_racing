using KARC.Settings;
using KARC.WitchEngine;
using Microsoft.Xna.Framework;
using System;


namespace KARC.Models;

public abstract class Generator
{
    private int _width;
    private int _height;
    protected IObject _createdObj;
    public event EventHandler OnCreated;

    public int Width
    {
        get 
        { 
            return _width; 
        }
        set 
        {
            if (value < 1)
                throw new ArgumentException("Width cannot be negative");
            _width = value; 
        }
    }

    public int Height
    {
        get
        {
            return _height;
        }
        set
        {
            if (value < 1)
                throw new ArgumentException("Height cannot be negative");
            _height = value;
        }
    }
    public Generator(int width, int height)
    {
        Width = width;
        Height = height;
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
