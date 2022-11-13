using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace KARC.WitchEngine
{
    public interface IComponent
    {
       string  Name { get; set; }
       Vector2 TextPos { get; set; }  
       string  Text { get; set; }
       bool IsCentered { get; set; }         
       bool IsSpriteScaled { get; set; } 
    }
}
