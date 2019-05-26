using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGizmoDriver : MonoBehaviour
{
    #pragma warning disable 0649
    [SerializeField]
    WallSpowner _wallSpowner;

    void OnDrawGizmos()
    {
        Maze maze = _wallSpowner.maze;

        Gizmos.color = Color.red * 0.5f;
        if (maze != null)
            for (int x = 0; x < maze.width; x++)
                for (int z = 0; z < maze.height; z++)
                    if(!maze.CanThrough(x,z))
                        Gizmos.DrawCube(new Vector3(x, 0, z), Vector3.one);
    }
}
