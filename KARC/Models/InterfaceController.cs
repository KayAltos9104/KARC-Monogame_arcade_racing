using KARC.WitchEngine;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace KARC.Models;
public class InterfaceController
{
    public Dictionary<string, IComponent> Components = new Dictionary<string, IComponent>();

    public void AddComponent (string name, IComponent component)
    {
        if (!Components.ContainsKey(name))
        {
            Components.Add(name, component);
        }
        else
        {
            throw new Exception("Такой элемент уже есть");
        }
    }
    public void RemoveComponent (string name)
    {
        if (Components.ContainsKey(name))
        {
            Components.Remove(name);
        }
        else
        {
            throw new Exception("Такого элемента интерфейса нет");
        }
    }

    public void RenderAll(SpriteBatch spriteBatch)
    {
        foreach (var c in Components.Values)
        {
            c.Render(spriteBatch);
        }
    }
}
