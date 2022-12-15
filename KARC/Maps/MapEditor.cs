using KARC.Models;
using System.Collections.Generic;

namespace KARC.Maps;
internal static class MapEditor
{

    public static readonly Dictionary<byte, char> mapObjectSymbols = new Dictionary<byte, char>()
    {
        {(byte)MapObjects.player, 'P' },
        {(byte)MapObjects.wall, 'W' },
        {(byte)MapObjects.finish, 'F' },
        {(byte)MapObjects.car, 'C' },
        {(byte)MapObjects.shield, 'S' },
    };

    public static void PlaceTileObject(byte objectKey, int xTile, int yTile, Map map)
    {
        if (objectKey != (byte)MapObjects.finish)
            map.GameField[xTile, yTile] = mapObjectSymbols[objectKey];
    }
    public static void PlaceLongObject(byte objectKey, int xCorner, int yCorner, int tileWidth, int tileHeight, Map map)
    {
        map.GameField[xCorner, yCorner] = objectKey == (byte)MapObjects.finish
            ? mapObjectSymbols[objectKey]
            : char.Parse(objectKey.ToString());


        map.GameField[xCorner + tileWidth, yCorner + tileHeight] = objectKey == (byte)MapObjects.finish
            ? mapObjectSymbols[objectKey]
            : char.Parse(objectKey.ToString());
    }
}
