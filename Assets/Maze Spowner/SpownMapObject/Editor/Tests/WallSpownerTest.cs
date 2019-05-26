using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

[TestFixture]
public class WallSpownerTest
{
    [Test]
    public void IsNeedRemove_True()
    {
        Transform wall = new GameObject("Wall").transform;
        try
        {
            wall.position = new Vector3(1, 1.5f, 6);
            List<Vector2Int> needSpownWallsPosition = new List<Vector2Int>() { new Vector2Int(1, 5), new Vector2Int(3, 6), new Vector2Int(2, 1) };
            Assert.IsTrue(IsNeedRemove(wall, needSpownWallsPosition));
        }
        finally
        {
            Object.DestroyImmediate(wall.gameObject);
        }
    }

    [Test]
    public void IsNeedRemove_False()
    {
        Transform wall = new GameObject("Wall").transform;
        try
        {
            wall.position = new Vector3(1, 1.5f, 6);
            List<Vector2Int> needSpownWallsPosition = new List<Vector2Int>() { new Vector2Int(1, 5), new Vector2Int(1, 6), new Vector2Int(2, 1) };
            Assert.IsFalse(IsNeedRemove(wall, needSpownWallsPosition));
        }
        finally
        {
            Object.DestroyImmediate(wall.gameObject);
        }
    }

    bool IsNeedRemove(Transform wall, List<Vector2Int> needSpoenWallsPosition)
    {
        return (bool)TestTools.GetNonPublicMethod(typeof(WallSpowner), "IsNeedRemove").Invoke(new WallSpowner(), new object[] { wall, needSpoenWallsPosition });
    }
}
