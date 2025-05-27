using Moq;
namespace SpaceBattle.Lib.Tests;

public class CollisionTreeTests
{
    [Fact]
    public void SuccesfullCollisionTree()
    {
        var tree = new Dictionary<int, object>();
        var attributeList = new List<int> { 1, 2, 3, 4 };
        new AddTreeResultCommand(tree, attributeList).Execute();
        tree = (Dictionary<int, object>)tree[1];
        tree = (Dictionary<int, object>)tree[2];
        tree = (Dictionary<int, object>)tree[3];
        tree = (Dictionary<int, object>) tree[4];
    }
}
