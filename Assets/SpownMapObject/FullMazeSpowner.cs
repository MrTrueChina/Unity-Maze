using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用于一次性生成整个迷宫的生成器
/// </summary>
public class FullMazeSpowner : MonoBehaviour
{
    const float WALL_Y = 0;

#pragma warning disable 0649
    [SerializeField]
    GameObject _single;
#pragma warning disable 0649
    [SerializeField]
    GameObject _up;
#pragma warning disable 0649
    [SerializeField]
    GameObject _upRight;
#pragma warning disable 0649
    [SerializeField]
    GameObject _upDown;
#pragma warning disable 0649
    [SerializeField]
    GameObject _upLeft;
#pragma warning disable 0649
    [SerializeField]
    GameObject _upRightDown;
#pragma warning disable 0649
    [SerializeField]
    GameObject _upRightLeft;
#pragma warning disable 0649
    [SerializeField]
    GameObject _upDownLeft;
#pragma warning disable 0649
    [SerializeField]
    GameObject _upRightDownLeft;
#pragma warning disable 0649
    [SerializeField]
    GameObject _right;
#pragma warning disable 0649
    [SerializeField]
    GameObject _rightDown;
#pragma warning disable 0649
    [SerializeField]
    GameObject _rightLeft;
#pragma warning disable 0649
    [SerializeField]
    GameObject _rightDownLeft;
#pragma warning disable 0649
    [SerializeField]
    GameObject _down;
#pragma warning disable 0649
    [SerializeField]
    GameObject _downLeft;
#pragma warning disable 0649
    [SerializeField]
    GameObject _left;

    public Maze maze
    {
        get
        {
            return _maze;
        }
        set
        {
            _maze = value;
        }
    }
    Maze _maze;

    /// <summary>
    /// 以byte值对应墙预制，对应规则是 0b左下右上 有墙1，没有是0
    /// </summary>
    Dictionary<byte, GameObject> _walls;

    private void Awake()
    {
        SetupWallsDictronary();
    }

    void SetupWallsDictronary()
    {
        _walls = new Dictionary<byte, GameObject>()
        {
            { 0b0000, _single },
            { 0b0001, _up },
            { 0b0010, _right },
            { 0b0011, _upRight },
            { 0b0100, _down },
            { 0b0101, _upDown },
            { 0b0110, _rightDown },
            { 0b0111, _upRightDown },
            { 0b1000, _left },
            { 0b1001, _upLeft },
            { 0b1010, _rightLeft },
            { 0b1011, _upRightLeft },
            { 0b1100, _downLeft },
            { 0b1101, _upDownLeft },
            { 0b1110, _rightDownLeft },
            { 0b1111, _upRightDownLeft },
        };
    }

    private void Update()
    {
        if (_maze != null)
        {
            Spown();
            enabled = false;
        }
    }

    /// <summary>
    /// 生成整个迷宫
    /// </summary>
    public void Spown()
    {
        /*
         *  这个问题很好解决
         *  
         *  遍历迷宫坐标
         *      if(是墙)
         *          生成墙
         */
        for (int x = 0; x < _maze.width; x++)
            for (int y = 0; y < _maze.height; y++)
                if (!_maze.CanThrough(x, y))
                    SpownWall(x, y);
    }

    void SpownWall(int x, int y)
    {
        /*
         *  根据坐标获取预制
         *  实例化
         */
        Instantiate(GetWallPrefab(x, y), new Vector3(x, WALL_Y, y), Quaternion.identity);
    }

    GameObject GetWallPrefab(int x, int y)
    {
        /*
         *  根据相邻的墙获取byte值
         *  根据byte值返回预制
         */

        return _walls[GetWallByte(x, y)];
    }

    byte GetWallByte(int x, int y)
    {
        /*
         *  遍历上下左右
         *      if(在地图里 && 是墙)
         *          增加byte值
         *  返回
         */
        byte wallValue = 0;

        if (_maze.Contains(new Vector2Int(x, y + 1)) && !_maze.CanThrough(x, y + 1))
            wallValue |= 0b0001;
        if (_maze.Contains(new Vector2Int(x + 1, y)) && !_maze.CanThrough(x + 1, y))
            wallValue |= 0b0010;
        if (_maze.Contains(new Vector2Int(x, y - 1)) && !_maze.CanThrough(x, y - 1))
            wallValue |= 0b0100;
        if (_maze.Contains(new Vector2Int(x - 1, y)) && !_maze.CanThrough(x - 1, y))
            wallValue |= 0b1000;

        return wallValue;
    }
}
