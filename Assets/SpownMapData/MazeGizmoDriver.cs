using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGizmoDriver : MonoBehaviour
{
    [Range(2,100)]
    [SerializeField]
    int _width = 5;
    [SerializeField]
    [Range(2, 100)]
    int _height = 5;

    Maze _maze;
    void Start()
    {
        _maze = SpownMazeData.Spown(_width, _height);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        if (_maze != null)
            for (int x = 0; x < _maze.width; x++)
                for (int z = 0; z < _maze.height; z++)
                    if(!_maze.CanThrough(x,z))
                        Gizmos.DrawCube(new Vector3(x, 0, z), Vector3.one);
    }

    private void OnValidate()
    {
        _width = Mathf.Clamp(_width, 2, 100);
        _height = Mathf.Clamp(_height, 2, 100);

        _maze = SpownMazeData.Spown(_width, _height);
    }
}
