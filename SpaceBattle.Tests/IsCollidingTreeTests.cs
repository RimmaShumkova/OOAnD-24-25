using App;
using App.Scopes;
using SpaceBattle.Lib;

namespace SpaceBattle.Tests;

public class IsCollidingTreeTests
{
    public IsCollidingTreeTests()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void ObjectCollidingTests()
    {
        var deltaPostion = new int[] { 1, 2, 3 };
        var deltaVelocity = new int[] { 1, 2, 3 };
        var testAttributes = new List<int> { 1, 2, 3, 1, 2, 3 };
        var shape1 = "Circle";
        var shape2 = "Circle";

        var reg_tree = new RegisterIocDependencyCollisionTree(shape1, shape2);
        reg_tree.Execute();

        Ioc.Resolve<SpaceBattle.Lib.ICommand>($"Game.CollisionTree.{shape1}_{shape2}.Add", testAttributes).Execute();

        var reg_colliding = new RegisterIsCollidingTreeRealisation();
        reg_colliding.Execute();

        var colliding_res = Ioc.Resolve<bool>("Game.IsColliding", deltaPostion, deltaVelocity, shape1, shape2);
        Assert.True(colliding_res);
    }

    [Fact]
    public void ObjectNotCollidingTests()
    {
        var deltaPostion = new int[] { 1, 2, 3 };
        var deltaVelocity = new int[] { 1, 2, 3 };
        var testAttributes = new List<int> { 1, 2, 3, 1 };
        var shape1 = "Circle";
        var shape2 = "Circle";

        var reg_tree = new RegisterIocDependencyCollisionTree(shape1, shape2);
        reg_tree.Execute();

        Ioc.Resolve<SpaceBattle.Lib.ICommand>($"Game.CollisionTree.{shape1}_{shape2}.Add", testAttributes).Execute();

        var reg_colliding = new RegisterIsCollidingTreeRealisation();
        reg_colliding.Execute();

        var colliding_res = Ioc.Resolve<bool>("Game.IsColliding", deltaPostion, deltaVelocity, shape1, shape2);
        Assert.False(colliding_res);
    }
}
