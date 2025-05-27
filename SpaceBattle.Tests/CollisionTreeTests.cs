namespace SpaceBattle.Lib.Tests;

public class CollisionTreeTests
{
    [Fact]
    public void SuccesfullCollisionTreeCreation()
    {
        var tree = new Dictionary<int, object>();
        var attributeList = new List<int> { 1 };
        new AddTreeResultCommand(tree, attributeList).Execute();

        Assert.IsType<Dictionary<int, object>>((Dictionary<int, object>)tree[1]);
    }

    [Fact]
    public void SuccesfullCollisionTreeCreationTwoLists()
    {
        var tree = new Dictionary<int, object>();
        var attributeList1 = new List<int> { 1, 2 };
        var attributeList2 = new List<int> { 1, 3 };

        new AddTreeResultCommand(tree, attributeList1).Execute();
        new AddTreeResultCommand(tree, attributeList2).Execute();

        Assert.IsType<Dictionary<int, object>>((Dictionary<int, object>)tree[1]);
    }

    [Fact]
    public void SuccesfullCollisionTreeCheck()
    {
        var tree = new Dictionary<int, object>();
        var attributeList = new List<int> { 1, 2, 3, 4 };
        new AddTreeResultCommand(tree, attributeList).Execute();
        new CheckTreeResultCommand(tree, attributeList).Execute();
    }

    [Fact]
    public void FailedCollisionTreeCheck()
    {
        var tree = new Dictionary<int, object>();
        var attributeListCreate = new List<int> { 1, 2, 3, 4 };
        var attributeListCheck = new List<int> { 4, 3, 2, 1 };

        new AddTreeResultCommand(tree, attributeListCreate).Execute();
        Assert.Throws<Exception>(() => new CheckTreeResultCommand(tree, attributeListCheck).Execute());
    }
}
