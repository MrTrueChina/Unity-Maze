using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpownController : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    WallSpowner _wallSpowner;

#pragma warning disable 0649
    [SerializeField]
    FullMazeSpowner _fullMazeSpowner;

    private void Awake()
    {
        Maze maze = SpownMazeData.Spown(20, 20);
        _wallSpowner.maze = maze;
        _fullMazeSpowner.maze = maze;
    }
}
