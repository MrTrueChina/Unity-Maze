using System;
using System.Reflection;

public static class TestTools
{
    /// <summary>
    /// 获取指定类的指非公有定静态方法
    /// </summary>
    /// <param name="type"></param>
    /// <param name="methodName"></param>
    /// <returns></returns>
    public static MethodInfo GetNonPublicStaticMethod(Type type, string methodName)
    {
        return type.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic);
        //MthodInfo Type.GetMethod(string name, BindingFlags bindingAttr)：根据名称和约束获取方法
        //BindingFlags.Static：静态的。BindingFlags.NonPublic：非公有的（private、protected、不写，只要不是public都用这个）
        //有多个约束时使用【单个】竖线连接，这个连接本质上是位运算的或运算
        //约束是必须的，不能多不能少，否则都会查不到
    }

    /// <summary>
    /// 获取指定类的指定非公有非静态方法
    /// </summary>
    /// <param name="type"></param>
    /// <param name="methodName"></param>
    /// <returns></returns>
    public static MethodInfo GetNonPublicMethod(Type type, string methodName)
    {
        return type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
        //BindingFlags.Instance：非静态的，需要用对象来调用的方法
    }
}
