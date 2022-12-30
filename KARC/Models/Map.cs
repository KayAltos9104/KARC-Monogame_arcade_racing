using System;

namespace KARC.Models;

public class Map
{
    public int Width { get; }
    public int Height { get; }
    public char[,] GameField;

    public int TileSize { get; }
    public Map (int width, int height, int tileSize)
    {
        if (width < 0 || height < 0)
            throw new ArgumentOutOfRangeException("Width or height is negative");

        Width = width;
        Height = height;
        GameField = new char[Width, Height];
        TileSize = tileSize;
    }

    public bool IsClearNeighborTiles(int x, int y)
    {
        if (IsBorderCrossed(x, y))
            throw new ArgumentOutOfRangeException("Point crossed map border");

        for (int yN = -1; yN <= 1; yN++)
            for (int xN = -1; xN <= 1; xN++)
            {
                if (IsBorderCrossed(x + xN, y + yN))
                    continue;
                if (GameField[x + xN, y + yN] != '\0')
                    return false;
            }
        return true;
    }
    public bool IsBorderCrossed (int x, int y)
    {
        return x < 0 || x >= Width || y < 0 || y >= Height;
    }
}
