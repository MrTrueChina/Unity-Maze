using NUnit.Framework;
using System;
using UnityEngine;

public class MazeTest
{
    const int WIDTH = 9;
    const int HEIGHT = 7;

    Maze _maze;

    [SetUp]
    public void Setup()
    {
        _maze = new Maze(WIDTH, HEIGHT);
    }

    [TearDown]
    public void TearDown()
    {
        _maze = null;
    }

    [Test]
    [TestCase(0, 5)]
    [TestCase(5, 0)]
    [TestCase(0, 0)]
    [TestCase(5, 5)]
    [TestCase(1, 1)]
    public void Maze_Normal(int width, int height)
    {
        Maze maze = new Maze(width, height);
        Assert.AreEqual(width, maze.width);
        Assert.AreEqual(height, maze.height);
    }

    [Test]
    [TestCase(-1, -1)]
    [TestCase(0, -1)]
    [TestCase(-1, 0)]
    public void Maze_OutOfRange(int width, int height)
    {
        Assert.Throws<OverflowException>(delegate ()
        {
            Maze maze = new Maze(width, height);
        });
    }

    [Test]
    public void Width()
    {
        Assert.AreEqual(WIDTH, _maze.width);
    }

    [Test]
    public void Height()
    {
        Assert.AreEqual(HEIGHT, _maze.height);
    }

    [Test]
    [TestCase(WIDTH - 1, HEIGHT - 1)]
    [TestCase(WIDTH - 1, 0)]
    [TestCase(0, 0)]
    [TestCase(0, HEIGHT - 1)]
    public void SetWall_Normal(int x, int y)
    {
        _maze.SetWall(x, y);
        Assert.AreEqual(false, _maze.quads[x, y]);
    }

    [Test]
    [TestCase(0, HEIGHT)]
    [TestCase(WIDTH, 0)]
    [TestCase(0, -1)]
    [TestCase(-1, 0)]
    public void SetWall_OutOfRange(int x, int y)
    {
        Assert.Throws<IndexOutOfRangeException>(delegate ()
        {
            _maze.SetWall(x, y);
        });
    }

    [Test]
    [TestCase(WIDTH - 1, HEIGHT - 1)]
    [TestCase(WIDTH - 1, 0)]
    [TestCase(0, 0)]
    [TestCase(0, HEIGHT - 1)]
    public void RemoveWall_Normal(int x, int y)
    {
        _maze.SetWall(x, y);
        _maze.RemoveWall(x, y);
        Assert.AreEqual(true, _maze.quads[x, y]);
    }

    [Test]
    [TestCase(0, HEIGHT)]
    [TestCase(WIDTH, 0)]
    [TestCase(0, -1)]
    [TestCase(-1, 0)]
    public void RemoveWall_outOfRange(int x, int y)
    {
        Assert.Throws<IndexOutOfRangeException>(delegate ()
        {
            _maze.RemoveWall(x, y);
        });
    }

    [Test]
    [TestCase(WIDTH - 1, HEIGHT - 1)]
    [TestCase(WIDTH - 1, 0)]
    [TestCase(0, 0)]
    [TestCase(0, HEIGHT - 1)]
    public void Contains_Contain(int x, int y)
    {
        Assert.IsTrue(_maze.Contains(new Vector2Int(x, y)));
    }

    [Test]
    [TestCase(0, HEIGHT)]
    [TestCase(WIDTH, 0)]
    [TestCase(0, -1)]
    [TestCase(-1, 0)]
    public void Contains_NotContain(int x, int y)
    {
        Assert.IsFalse(_maze.Contains(new Vector2Int(x, y)));
    }
}
