using MtC.Tools.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpowner : MonoBehaviour
{
    public Maze maze
    {
        set
        {
            _maze = value;
        }
    }
    Maze _maze;

    [SerializeField]
    GameObject _wall;

    List<Transform> _walls = new List<Transform>();

    const float WALL_Y = 1.5f;

    /// <summary>
    /// 以指定位置为标准，生成在这个位置有可能看到的迷宫墙
    /// </summary>
    /// <param name="center"></param>
    public void UpdateWalls(Vector2Int center)
    {
        /*
         *  获取应该生成墙壁的位置
         *  根据应该生成位置和现有的墙获取应该移除的墙
         *  移除应该移除的墙
         *  生成应该生成的墙
         */
        List<Vector2Int> needSpownWallsPosition = GetNeedSpownWallsPosition(center);
        List<Transform> needRemoveWalls = GetNeedRemoveWalls(needSpownWallsPosition);
        RemoveAndUnstoreWalls(needRemoveWalls);
        SpownAndStoreWalls(needSpownWallsPosition);
    }

    List<Vector2Int> GetNeedSpownWallsPosition(Vector2Int center)
    {
        /*
         *  找到需要生成墙的路
         *  获取路周围的墙的位置
         */
        return GetWallsPositionAdjacentQuads(GetNeedSpownWallsQuads(center));
    }

    List<Vector2Int> GetNeedSpownWallsQuads(Vector2Int center)
    {
        /*
         *  把脚下的路加入列表
         *  
         *  上下左右四个方向
         *  {
         *      把这个方向需要生成墙的路加进列表
         *  }
         */
        List<Vector2Int> quads = new List<Vector2Int>();

        quads.Add(center);

        quads.AddRange(GetADirectionNeewSpownWallsQuads(center, Vector2Int.up));
        quads.AddRange(GetADirectionNeewSpownWallsQuads(center, Vector2Int.right));
        quads.AddRange(GetADirectionNeewSpownWallsQuads(center, Vector2Int.down));
        quads.AddRange(GetADirectionNeewSpownWallsQuads(center, Vector2Int.left));

        return quads;
    }

    List<Vector2Int > GetADirectionNeewSpownWallsQuads(Vector2Int center, Vector2Int direction)
    {
        //TODO：获取朝一个方向走能遇到的所有需要生成墙的地块
        /*
         *  把沿方向直到墙的地块加入列表
         *  
         *  沿着这个方向走到墙
         *      if(这个地块两边有路)
         *          获取这个方向一条直线上的所有路，加入列表
         */
        List<Vector2Int> quads = new List<Vector2Int>();
        Vector2Int currentPosition = center;
        while (_maze.CanThrough(currentPosition += direction)) // 这里用的是+=，所以效果是前进一步【之后】看是不是要在这一步执行
        {
            quads.Add(currentPosition);
        }
    }

    List<Vector2Int> GetALineCanThroughQuads(Vector2Int center,Vector2Int direction)
    {
        //TODO：返回一行地块直到墙
    }

    Vector2Int LeftDirection(Vector2Int direction)
    {
        if (direction == Vector2Int.up)
            return Vector2Int.left;
        if (direction == Vector2Int.right)
            return Vector2Int.up;
        if (direction == Vector2Int.down)
            return Vector2Int.right;
        if (direction == Vector2Int.left)
            return Vector2Int.down;
        throw new System.ArgumentException("左转方法必须传入VectorInt的上下左右之一");
    }

    Vector2Int RightDirection(Vector2Int direction)
    {
        if (direction == Vector2Int.up)
            return Vector2Int.right;
        if (direction == Vector2Int.right)
            return Vector2Int.down;
        if (direction == Vector2Int.down)
            return Vector2Int.left;
        if (direction == Vector2Int.left)
            return Vector2Int.up;
        throw new System.ArgumentException("右转方法必须传入VectorInt的上下左右之一");
    }

    List<Vector2Int> GetWallsPositionAdjacentQuads(List<Vector2Int> quads)
    {
        /*
         *  遍历地块位置
         *      遍历地块上下左右相邻位置
         *          if(这个位置应该是墙 && 这个位置不在列表里)
         *              加入列表
         *  返回列表
         */
        List<Vector2Int> walls = new List<Vector2Int>();

        foreach (Vector2Int quad in quads)
        {
            Vector2Int currentAdjacentQuad;

            currentAdjacentQuad = quad + Vector2Int.up;
            if (_maze.CanThrough(currentAdjacentQuad.x, currentAdjacentQuad.y) && !walls.Contains(currentAdjacentQuad))
                walls.Add(currentAdjacentQuad);
            currentAdjacentQuad = quad + Vector2Int.right;
            if (_maze.CanThrough(currentAdjacentQuad.x, currentAdjacentQuad.y) && !walls.Contains(currentAdjacentQuad))
                walls.Add(currentAdjacentQuad);
            currentAdjacentQuad = quad + Vector2Int.down;
            if (_maze.CanThrough(currentAdjacentQuad.x, currentAdjacentQuad.y) && !walls.Contains(currentAdjacentQuad))
                walls.Add(currentAdjacentQuad);
            currentAdjacentQuad = quad + Vector2Int.left;
            if (_maze.CanThrough(currentAdjacentQuad.x, currentAdjacentQuad.y) && !walls.Contains(currentAdjacentQuad))
                walls.Add(currentAdjacentQuad);
        }

        return walls;
    }

    List<Transform> GetNeedRemoveWalls(List<Vector2Int> needSpownWallsPosition)
    {
        //TODO：根据需要创建墙的位置和现在的墙列表返回应该移除的墙列表
        /*
         *  筛选出现在的墙里位置不在需要生成的墙的列表里的返回
         */
        return _walls.FindAll((Transform transform) => { return IsNeedRemove(transform, needSpownWallsPosition); });
        //TODO：如果这里正确了，加上FindAll的功能，疑似是遍历或相同效果
    }

    bool IsNeedRemove(Transform wall, List<Vector2Int> needSpoenWallsPosition)
    {
        foreach (Vector2Int position in needSpoenWallsPosition)
            if (wall.position.x == position.x && wall.position.z == position.y)
                return false;
        return true;
    }

    void RemoveAndUnstoreWalls(List<Transform> walls)
    {
        /*
         *  遍历墙
         *  {
         *      清除墙
         *      清除列表里的记录
         *  }
         */
        foreach (Transform wall in walls)
        {
            MPool.Set(wall.gameObject);
            _walls.Remove(wall);
        }
    }

    void SpownAndStoreWalls(List<Vector2Int> positions)
    {
        /*
         *  遍历位置
         *      if(这个位置没有墙)
         *      {
         *          生成墙
         *          加入列表
         *      }
         */
        foreach (Vector2Int position in positions)
            if (!HaveWall(position))
            {
                GameObject wall = MPool.Get(_wall, new Vector3(position.x, WALL_Y, position.y), Quaternion.identity);
                _walls.Add(wall.transform);
            }
    }

    bool HaveWall(Vector2Int position)
    {
        foreach (Transform wall in _walls)
            if (wall.position.x == position.x && wall.position.z == position.y)
                return true;
        return false;
    }
}
