using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpownController : MonoBehaviour
{
    [SerializeField]
    WallSpowner _wallSpowner;

    private void Awake()
    {
        _wallSpowner.maze = SpownMazeData.Spown(20, 20);
    }
}
