using System;
using System.Collections.Generic;

namespace KARC.Models;
internal class Map
{
    private Random _random = new Random();
    private int _width;
    private int _height;
    private Dictionary<byte, char> mapObjectSymbols = new Dictionary<byte, char>()
    {
        {(byte)MapObjects.player, 'P' },
        {(byte)MapObjects.wall, 'W' },
        {(byte)MapObjects.finish, 'F' },
        {(byte)MapObjects.car, 'C' },
        {(byte)MapObjects.shield, 'S' },

    };
    public char[,] GameField;
    
    public Map (int width, int length)
    {
        GameField = new char[width, length];
        _width = width;
        _height = length;
    }

    private void PlaceTileObject(byte objectKey, int xTile, int yTile)
    {
        if (objectKey != (byte)MapObjects.finish)
            GameField[xTile, yTile] = mapObjectSymbols[objectKey];
    }
    private void PlaceLongObject(byte objectKey, int xCorner, int yCorner, int tileWidth, int tileHeight)
    {
        if (objectKey!=(byte)MapObjects.finish && mapObjectSymbols.ContainsKey(objectKey))
            throw new Exception("Wrong key for long object");

        GameField[xCorner, yCorner] = mapObjectSymbols[objectKey];
        GameField[xCorner + tileWidth, yCorner +tileHeight] = mapObjectSymbols[objectKey];
    }
    private void GenerateMap()
    {
        PlaceTileObject((byte)MapObjects.player, _width / 2, _height - 1);
        PlaceLongObject(objectKey : 0, xCorner: 1, yCorner: 0, tileWidth : 1, tileHeight : _height - 1);
        PlaceLongObject(
            objectKey : 1, 
            xCorner: _width - 2, 
            yCorner: 0, 
            tileWidth: 1, 
            tileHeight: _height - 1);
        PlaceLongObject(
            objectKey: (byte)MapObjects.finish,
            xCorner: 2,
            yCorner: 0,
            tileWidth: _width - 3,
            tileHeight: 1);

       
        for (int y = 2; y < _height - 20; y++)
            for (int x = 2; x < _width - 3; x++)
            {
                bool isClearBorders = true;
                for (int yN = -1; yN <= 1; yN++)
                    for (int xN = -1; xN <= 1; xN++)
                    {
                        if (GameField[x + xN, y + yN] != '\0')
                            isClearBorders = false;
                    }
                if (GameField[x, y] == '\0' && isClearBorders && _random.NextDouble() <= 0.003f)
                {
                    PlaceTileObject((byte)MapObjects.wall, x, y);
                }

                if (GameField[x, y] == '\0' && isClearBorders && _random.NextDouble() <= 0.001f)
                {
                    PlaceTileObject((byte)MapObjects.shield, x, y);
                }
            }

    }
    private void GenerateEnemies(float enemiesFraction)
    {
        for (int y = 2; y < _height - 10; y++)
            for (int x = 2; x < _width - 3; x++)
            {
                bool isClearBorders = true;
                for (int yN = -1; yN <= 1; yN++)
                    for (int xN = -1; xN <= 1; xN++)
                    {
                        if (GameField[x + xN, y + yN] != '\0')
                            isClearBorders = false;
                    }
                if (GameField[x, y] == '\0' && isClearBorders && _random.NextDouble() <= enemiesFraction)
                {
                    PlaceTileObject((byte)MapObjects.car, x, y);
                }
            }
    }
    public enum MapObjects: byte
    {
        player,
        car,
        shield,
        wall,
        finish
    }
}
