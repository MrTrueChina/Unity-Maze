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

    [Test]
    [TestCase(5, 7)]
    [TestCase(9, 5)]
    [TestCase(3, 3)]
    public void CreateMaze_Singular(int width, int height)
    {
        Maze maze = CreateMaze(width, height);
        Assert.AreEqual(maze.width, width);
        Assert.AreEqual(maze.height, height);
    }
    static Maze CreateMaze(int width, int height)
    {
        return (Maze)GetSpownMazeDataMethod("CreateMaze").Invoke(null, new object[] { width, height });
        //object Method.Invoke(object obj, object[] parameters)：让指定的对象用指定的参数执行这个方法
        //第一个参数是执行方法的对象，如果方法不需要对象就能执行则传入null，不能不传
        //第二个参数是执行方法使用的参数数组，必须按照方法需要的参数的数量和顺序存入，不需要参数时则要传入长度0的object数组
        //因为是反射过来的，所以返回值是object，需要强转才能使用
    }
    static MethodInfo GetSpownMazeDataMethod(string methodName)
    {
        Type spownerType = typeof(SpownMazeData); // 获取Maze的Type，反射是由Type进行的
        return spownerType.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic);
        //Type.GetMethod(string name,BindingFlags bindingAttr)：根据方法名和约束查找方法
        //有多个约束的时候用【单个】竖线连接，本质上是位运算里的或运算
        //约束是必须的，不能多不能少，多了少了都会查不到
        //因为 SpownMazeData 是个静态类，需要反射获取的方法都是隐藏的，因此这里可以直接固定约束：静态、不公开
    }
    [Test]
    [TestCase(4, 6)]
    [TestCase(12, 6)]
    [TestCase(8, 8)]
    public void CreateMaze_Even(int width, int height)
    {
        Maze maze = CreateMaze(width, height);
        Assert.AreEqual(maze.width, width + 1);
        Assert.AreEqual(maze.height, height + 1);
    }

    [Test]
    public void SetupMaze_Normal()
    {
        bool[,] quads = new bool[,]
        {
            { false,false,false,false,false},
            { false,true,false,true,false},
            { false,false,false,false,false},
            { false,true,false,true,false},
            { false,false,false,false,false}
        };

        Maze maze = new Maze(quads.GetLength(0), quads.GetLength(1));
        SetupMaze(maze);
        Assert.AreEqual(quads, maze.quads);
    }
    static void SetupMaze(Maze maze)
    {
        GetSpownMazeDataMethod("SetupMaze").Invoke(null, new object[] { maze });
        //MethodInfo.Invoke(object obj, object[] parameters)：由指定对象用指定参数调用这个方法
        //object obj：调用这个方法的对象，如果不需要对象（例如静态方法），这个参数可以传 null
        //object[] parameters：参数，要按照这个方法的参数顺序和数量传入，否则出错
    }
}
