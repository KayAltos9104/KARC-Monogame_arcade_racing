using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace WitchEngine;

public class AnimationController 
{
    public float Layer { get; set; }
    public Vector2 Pos { get; private set; }
    public Dictionary<string, Animator> Animations { get; private set; }
    public Animator ActiveAnimation { get; set; }

    public AnimationController(Vector2 position)
    {
        Layer = 0.6f;        
        Pos = position;        
        Animations = new Dictionary<string, Animator>();
        ActiveAnimation = null;
    }

    public void PlayAnimation(string name)
    {
        foreach (Animator animation in Animations.Values)
            animation.Deactivate();
        if (Animations.ContainsKey(name))
        {
            ActiveAnimation = Animations[name];
            ActiveAnimation.Activate();
        }
        else
        {
            throw new Exception("Такой анимации нет");
        }
    }
    public void StopAnimation()
    {
        ActiveAnimation.Deactivate();
    }
    public void UpdateAnimation(GameTime gameTime)
    {
        ActiveAnimation.Update(gameTime);
    }
    public void AddAnimation(string name, Animator animator)
    {
        Animations.Add(name, animator);
    }

    public void Move(Vector2 pos)
    {
        Pos = pos;
    }    
}
