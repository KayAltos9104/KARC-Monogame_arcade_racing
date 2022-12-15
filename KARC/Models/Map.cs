namespace KARC.Models;

public class Map
{
    public int Width { get; }
    public int Height { get; }
    public char[,] GameField;

    public Map (int width, int height)
    {
        Width = width;
        Height = height;
        GameField = new char[Width, Height]; 
    }

    public bool IsClearNeighborTiles(int x, int y)
    {
        for (int yN = -1; yN <= 1; yN++)
            for (int xN = -1; xN <= 1; xN++)
            {
                if (GameField[x + xN, y + yN] != '\0')
                    return false;
            }
        return true;
    }
}
