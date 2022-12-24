using KARC.Models;
using System;
using G = KARC.Maps.MapEditor;

namespace KARC.Maps;

public class FirstLevelBuilder : MapBuilder
{
    private Map _map;
    private Random _random = new();
    public override void GenerateMap()
    {
        _map = new Map(11, 5000);
        _obstaclesFraction = 0.003f;
        _shieldsFraction = 0.001f;
        _enemiesFraction = 0.025f;

        // Размещение игрока
        G.PlaceTileObject(MapObjects.player, _map.Width / 2, _map.Height - 1, _map);

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

        // Генерация препятствий и щитов
        for (int y = 2; y < _map.Height - 20; y++)
            for (int x = 2; x < _map.Width - 3; x++)
            {
                bool isClearBorders = _map.IsClearNeighborTiles(x, y);

                if (_map.GameField[x, y] == '\0' && isClearBorders && _random.NextDouble() <= _obstaclesFraction)
                {
                    G.PlaceTileObject(MapObjects.wall, x, y, _map);
                }

                if (_map.GameField[x, y] == '\0' && isClearBorders && _random.NextDouble() <= _shieldsFraction)
                {
                    G.PlaceTileObject(MapObjects.shield, x, y, _map);
                }
            }

        // Генерация противников
        for (int y = 2; y < _map.Height - 10; y++)
            for (int x = 2; x < _map.Width - 3; x++)
            {
                bool isClearBorders = _map.IsClearNeighborTiles(x, y);

                if (_map.GameField[x, y] == '\0' && isClearBorders && _random.NextDouble() <= _enemiesFraction)
                {
                    G.PlaceTileObject(MapObjects.car, x, y, _map);
                }
            }
    }

    public override Map GetMap()
    {
        return _map;
    }
}
