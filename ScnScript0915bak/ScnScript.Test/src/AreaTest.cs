using ScnScript;
namespace StructTests;

[TestClass]
public class AreaTest
{
    [TestMethod]
    public void IsAreaInArea()
    {
        var area1 = new Area(0,6);
        var area2 = new Area(0, 6);
        if (area1.IsAreaInArea(area2))
        {
            Assert.IsTrue(false);
        }
        else
        {
            Assert.IsTrue(true);
        }
    }
    [TestMethod]
    public void IsInArea()
    {
        var area1 = new Area(0, 6);
        if (area1.IsInArea(1))
        {
            Assert.IsTrue(true);
        }
        else
        {
            Assert.IsTrue(false);
        }
    }
    [TestMethod]
    public void IsInAreaNoOnNode()
    {
        var area1 = new Area(0, 6);
        if (!area1.IsInAreaNoOnNode(6))
        {
            Assert.IsTrue(true);
        }
        else
        {
            Assert.IsTrue(false);
        }
    }
}