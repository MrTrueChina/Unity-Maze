using UnityEngine;

public class Maze
{
    public bool[,] quads
    {
        get
        {
            return _quads;
        }
    }
    bool[,] _quads;

    public int width
    {
        get
        {
            return _quads.GetLength(0);
        }
    }

    public int height
    {
        get
        {
            return _quads.GetLength(1);
        }
    }

    public Maze(int width, int height)
    {
        _quads = new bool[width, height];
    }

    public void SetWall(int x, int y)
    {
        _quads[x, y] = false;
    }

    public void RemoveWall(Vector2Int position)
    {
        RemoveWall(position.x, position.y);
    }

    public void RemoveWall(int x, int y)
    {
        _quads[x, y] = true;
    }

    public bool CanThrough(Vector2Int position)
    {
        return CanThrough(position.x, position.y);
    }

    public bool CanThrough(int x, int y)
    {
        return _quads[x, y];
    }

    public bool Contains(Vector2Int vector)
    {
        return vector.x >= 0 && vector.x < width && vector.y >= 0 && vector.y < height;
    }
}
