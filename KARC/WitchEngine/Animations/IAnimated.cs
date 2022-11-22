using Microsoft.Xna.Framework;
using System.Collections.Generic;

using System;
namespace KARC.WitchEngine.Animations;
public interface IAnimated
{
    Dictionary<string, Animator> Animations { get; }  
    Animator ActiveAnimation { get; set; }
    void PlayAnimation(string name)
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
    void StopAnimation()
    {
        ActiveAnimation.Deactivate();
    }
    void UpdateAnimation(GameTime gameTime)
    {
        ActiveAnimation.Update(gameTime);
    }
    void AddAnimation(string name, Animator animator)
    {
        Animations.Add(name, animator);
    }
}
