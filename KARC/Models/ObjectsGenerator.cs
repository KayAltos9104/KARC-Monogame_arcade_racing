using KARC.Objects;
using KARC.WitchEngine;
using System;
using System.Collections.Generic;

namespace KARC.Models;

public class ObjectsGenerator
{
    public delegate IObject GenerateSingle(char sign, int xTile, int yTile);
    public delegate IObject GenerateLong(char sign, int xInitTile, int yInitTile, int xEndTile, int yEndTile);


}
