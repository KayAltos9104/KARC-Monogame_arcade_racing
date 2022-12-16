using KARC.Models;
using System;
using System.Collections.Generic;

namespace KARC.Maps;
internal static class MapEditor
{

    public static readonly Dictionary<MapObjects, char> mapObjectSymbols = new Dictionary<MapObjects, char>()
    {
        {MapObjects.player, 'P' },
        {MapObjects.wall, 'W' },
        {MapObjects.finish, 'F' },
        {MapObjects.car, 'C' },
        {MapObjects.shield, 'S' },
    };

    public static void PlaceTileObject(MapObjects objectKey, int xTile, int yTile, Map map)
    {
        if (map is null)
            throw new ArgumentNullException("Map is null");

        if (map.IsBorderCrossed(xTile, yTile))
            throw new ArgumentOutOfRangeException("Placing object out of border");
        
        if (objectKey !=MapObjects.finish)
            map.GameField[xTile, yTile] = mapObjectSymbols[objectKey];
    }
    public static void PlaceLongObject(byte objectKey, int xCorner, int yCorner, int tileWidth, int tileHeight, Map map)
    {
        if (map.IsBorderCrossed(xCorner, yCorner)
            || map.IsBorderCrossed(xCorner + tileWidth, yCorner + tileHeight))
            throw new ArgumentOutOfRangeException("Placing object out of border");

        map.GameField[xCorner, yCorner] = objectKey == (byte)MapObjects.finish
            ? mapObjectSymbols[(MapObjects)objectKey]
            : char.Parse(objectKey.ToString());
        

        map.GameField[xCorner + tileWidth, yCorner + tileHeight] = objectKey == (byte)MapObjects.finish
            ? mapObjectSymbols[(MapObjects)objectKey]
            : char.Parse(objectKey.ToString());
    }
}
