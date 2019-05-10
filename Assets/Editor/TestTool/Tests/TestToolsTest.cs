using NUnit.Framework;
using System.Reflection;

[TestFixture]
public class TestToolsTest
{
    [Test]
    public void GetStaticPrivateMethod_CanFind()
    {
        MethodInfo method = TestTools.GetNonPublicStaticMethod(typeof(TestToolsTestTarget), "PrivateStaticMethod");
        Assert.IsNotNull(method);
        Assert.IsTrue((bool)method.Invoke(null, new object[] { true }));
    }

    [Test]
    public void GetStaticPrivateMethod_CantFind()
    {
        MethodInfo method = TestTools.GetNonPublicStaticMethod(typeof(TestToolsTestTarget), "tan90°");
        Assert.IsNull(method);
    }

    [Test]
    public void GetPrivateMethod_CanFind()
    {
        MethodInfo method = TestTools.GetNonPublicMethod(typeof(TestToolsTestTarget), "PrivateMethod");
        Assert.IsNotNull(method);
        Assert.AreEqual(0, (int)method.Invoke(new TestToolsTestTarget(), new object[0]));
    }

    [Test]
    public void GetPrivateMethod_CantFind()
    {
        MethodInfo method = TestTools.GetNonPublicMethod(typeof(TestToolsTestTarget), "不存在的");
        Assert.IsNull(method);
    }
}

public class TestToolsTestTarget
{
    private int PrivateMethod()
    {
        return 0;
    }

    private static bool PrivateStaticMethod(bool isTrue)
    {
        return isTrue;
    }
}
