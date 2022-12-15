using KARC.Models;

namespace KARC.Maps;

public abstract class MapBuilder
{
    protected float _obstaclesFraction;
    protected float _shieldsFraction;
    protected float _enemiesFraction;
    public abstract Map GetMap();

    public abstract void GenerateMap();

}
