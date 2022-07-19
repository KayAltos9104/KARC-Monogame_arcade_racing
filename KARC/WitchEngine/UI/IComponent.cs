using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace KARC.WitchEngine
{
    public interface IComponent
    {
       string  Name { get; set; }
       string  Text { get; set; }
       public bool IsCentered { get; set; }
    }
}
