using NUnit.Framework;
using System;
using System.Reflection;
using UnityEngine;

[TestFixture]
public class SpownMazeDataTest
{
    [Test]
    [TestCase(1, 5)]
    [TestCase(5, 1)]
    [TestCase(0, 0)]
    [TestCase(-1, -1)]
    public void Spown_TooSmall(int width, int height)
    {
        Assert.Throws<ArgumentOutOfRangeException>(delegate ()
        {
            SpownMazeData.Spown(width, height);
        });
    }
}
