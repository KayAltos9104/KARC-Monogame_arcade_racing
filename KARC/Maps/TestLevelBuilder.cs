namespace KARC.Maps;

using KARC.Models;
using G = KARC.Maps.MapEditor;
public class TestLevelBuilder : MapBuilder
{
    private Map _map;
    
    public override void GenerateMap()
    {
        _map = new Map(11, 10);
        _obstaclesFraction = 0.0f;
        _shieldsFraction = 0.0f;
        _enemiesFraction = 0.0f;

        // Размещение игрока
        G.PlaceTileObject(MapObjects.player, _map.Width / 2, _map.Height / 2, _map);

        // Левая граница
        G.PlaceLongObject(
            objectKey: 0,
            xCorner: 1,
            yCorner: 0,
            tileWidth: 0,
            tileHeight: _map.Height - 1,
            map: _map);

        // Правая граница
        G.PlaceLongObject(
            objectKey: 1,
            xCorner: _map.Width - 2,
            yCorner: 0,
            tileWidth: 0,
            tileHeight: _map.Height - 1,
            map: _map);

        // Финишная лента
        G.PlaceLongObject(
            objectKey: (byte)MapObjects.finish,
            xCorner: 2,
            yCorner: 0,
            tileWidth: _map.Width - 5,
            tileHeight: 1,
            map: _map);

        G.PlaceTileObject(MapObjects.shield, 5, 3, _map);
    }

    public override Map GetMap()
    {
        return _map;
    }
}
