
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class SpownMazeData
{
    const int STEP_LENGTH = 2; // 步长，因为节点间实际不是紧邻的，所以涉及相邻节点的问题需要引入步长做调整
    static List<Vector2Int> _readyToCarveNodes; // 虽然 LinkedList 增删更快，但这个算法查的次数是增删的好几倍，最后竟然是List更快一点
    static List<Vector2Int> _carvedNodes;
    static Maze _maze;

    /// <summary>
    /// 生成并返回指定宽高的迷宫，如果输入的宽高是双数则会+1
    /// </summary>
    /// <param name="width">生成迷宫的宽度，如果是双数会自动+1</param>
    /// <param name="height">生成迷宫的高度，如果是双数会自动+1</param>
    /// <returns>生成的迷宫对象</returns>
    public static Maze Spown(int width, int height)
    {
        /*
         *  if(参数能用)
         *      生成并返回迷宫
         *  抛异常
         */
        if (ParameterAccess(width, height))
            return SpownMaze(width, height);

        throw new System.ArgumentOutOfRangeException();
    }

    static bool ParameterAccess(int width, int height)
    {
        return (width > 1 && height > 1);
    }

    static Maze SpownMaze(int width, int height)
    {
        /*
         *  准备环境
         *  生成并返回迷宫
         *  清理环境
         */
        try
        {
            SetupSpowner(width, height);
            return DoSpownMaze();
        }
        finally
        {
            ClearSpowner();
        }
    }

    static void SetupSpowner(int width, int height)
    {
        _readyToCarveNodes = new List<Vector2Int>();
        _carvedNodes = new List<Vector2Int>();

        CreateMaze(width, height);
    }

    static void CreateMaze(int width, int height)
    {
        /*
         *  宽高都改单数
         *  创建并返回迷宫
         */
        _maze = new Maze(width % 2 != 0 ? width : width + 1, height % 2 != 0 ? height : height + 1);
    }

    static void ClearSpowner()
    {
        _readyToCarveNodes = null;
        _carvedNodes = null;
        _maze = null;
    }

    static Maze DoSpownMaze()
    {
        /*
         *  初始化迷宫（给迷宫打孔）
         *  雕刻迷宫
         */
        SetupMaze();
        CarveMaze();
        return _maze;
    }

    static void SetupMaze()
    {
        for (int x = 0; x < _maze.width; x++)
            for (int y = 0; y < _maze.height; y++)
                if (x % 2 != 0 && y % 2 != 0)
                    _maze.RemoveWall(x, y);
                else
                    _maze.SetWall(x, y);
    }

    static void CarveMaze()
    {
        /*
         *  雕刻起点
         *  
         *  while(还有能雕刻的点)
         *      雕刻随机点
         */
        CarveStartNode();

        while (HaveReadyCarveNode())
            CarveRandomNode();
    }

    static void CarveStartNode()
    {
        /*
         *  雕刻左上角那个空的点
         */
        Vector2Int start = new Vector2Int(1, 1);
        Carve(start, start);
    }

    static void Carve(Vector2Int carvedNode, Vector2Int readyCarveNode)
    {
        /*
         *  把两个点中间的点改为可通过
         *  把新点相邻的未雕刻的点加入准备雕刻的点里
         *  把新点从准备雕刻点移除，放进已雕刻点里
         */
        BreakWall(carvedNode, readyCarveNode);
        AddContiguousNewNodesToReadyCarveNodes(readyCarveNode);
        MoveReadyCarveNodeToCarvedNodes(readyCarveNode);
    }

    static void BreakWall(Vector2Int startNode, Vector2Int endNode)
    {
        /*
         *  算出两个点之间的点
         *  修改为可通过
         */
        Vector2Int wall = new Vector2Int((startNode.x + endNode.x) / 2, (startNode.y + endNode.y) / 2);
        _maze.RemoveWall(wall);
    }

    static void AddContiguousNewNodesToReadyCarveNodes(Vector2Int center)
    {
        /*
         *  获取周围所有不是已雕刻点的点
         *  把这些点加进准备雕刻点里
         */
        _readyToCarveNodes.AddRange(GetContiguousNewNode(center)); // 这里很重要的一点是 Set 有防止重复的功能，所以不用剔除相邻的已经雕刻的点
    }

    static List<Vector2Int> GetContiguousNewNode(Vector2Int center)
    {
        /*
         *  遍历上下左右点
         *      if(在地图内、没雕刻、不在准备雕刻节点里)
         *          存下来
         *  返回去
         */
        List<Vector2Int> uncarvedNodes = new List<Vector2Int>();
        Vector2Int contiguousNode;


        contiguousNode = center + Vector2Int.up * STEP_LENGTH;
        if (_maze.Contains(contiguousNode) && !_carvedNodes.Contains(contiguousNode) && !_readyToCarveNodes.Contains(contiguousNode))
            uncarvedNodes.Add(contiguousNode);
        contiguousNode = center + Vector2Int.right * STEP_LENGTH;
        if (_maze.Contains(contiguousNode) && !_carvedNodes.Contains(contiguousNode) && !_readyToCarveNodes.Contains(contiguousNode))
            uncarvedNodes.Add(contiguousNode);
        contiguousNode = center + Vector2Int.down * STEP_LENGTH;
        if (_maze.Contains(contiguousNode) && !_carvedNodes.Contains(contiguousNode) && !_readyToCarveNodes.Contains(contiguousNode))
            uncarvedNodes.Add(contiguousNode);
        contiguousNode = center + Vector2Int.left * STEP_LENGTH;
        if (_maze.Contains(contiguousNode) && !_carvedNodes.Contains(contiguousNode) && !_readyToCarveNodes.Contains(contiguousNode))
            uncarvedNodes.Add(contiguousNode);

        return uncarvedNodes;
    }

    static void MoveReadyCarveNodeToCarvedNodes(Vector2Int readyCarveNode)
    {
        _readyToCarveNodes.Remove(readyCarveNode);
        _carvedNodes.Add(readyCarveNode);
    }

    static bool HaveReadyCarveNode()
    {
        return _readyToCarveNodes.Count > 0;
    }

    static void CarveRandomNode()
    {
        /*
         *  从准备雕刻的点里随机取一个
         *  从这个点周围随机找一个已经雕刻的点
         *  雕刻这两个点
         */
        Vector2Int readyCarveNode = GetRandomReadyToCarveNode();
        Vector2Int contiguousCarvedNode = GetRandomContiguousCarvedNode(readyCarveNode);
        Carve(contiguousCarvedNode, readyCarveNode);
    }

    static Vector2Int GetRandomReadyToCarveNode()
    {
        return _readyToCarveNodes.ElementAt(Random.Range(0, _readyToCarveNodes.Count));
    }

    static Vector2Int GetRandomContiguousCarvedNode(Vector2Int center)
    {
        /*
         *  获取所有相邻的已雕刻节点
         *  随机返回一个
         */
        List<Vector2Int> contiguousCarvedNodes = GetContiguousCarvedNodes(center);
        return contiguousCarvedNodes[Random.Range(0, contiguousCarvedNodes.Count)];
    }

    static List<Vector2Int> GetContiguousCarvedNodes(Vector2Int center)
    {
        /*
         *  遍历上下左右
         *      if(这个相邻节点在雕刻完成节点列表里)
         *          加进去
         *  返回这些点
         */
        List<Vector2Int> carvedNodes = new List<Vector2Int>();

        if (_carvedNodes.Contains(center + Vector2Int.up * STEP_LENGTH))
            carvedNodes.Add(center + Vector2Int.up * STEP_LENGTH);
        if (_carvedNodes.Contains(center + Vector2Int.right * STEP_LENGTH))
            carvedNodes.Add(center + Vector2Int.right * STEP_LENGTH);
        if (_carvedNodes.Contains(center + Vector2Int.down * STEP_LENGTH))
            carvedNodes.Add(center + Vector2Int.down * STEP_LENGTH);
        if (_carvedNodes.Contains(center + Vector2Int.left * STEP_LENGTH))
            carvedNodes.Add(center + Vector2Int.left * STEP_LENGTH);

        return carvedNodes;
    }
}
