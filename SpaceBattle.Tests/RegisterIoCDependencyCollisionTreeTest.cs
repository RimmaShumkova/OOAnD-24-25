using App;
using App.Scopes;
using SpaceBattle.Lib;

namespace SpaceBattle.Tests;
public class RegisterIoCDependencyCollisionTreeTest
{
    [Fact]
    public void CollisionConstructionTest()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();

        var testAttributes = new List<int> { 1, 2, 3, 4 };
        var shape1 = "Circle";
        var shape2 = "Circle";

        var reg = new RegisterIocDependencyCollisionTree(shape1, shape2);
        reg.Execute();

        Assert.IsType<AddTreeResultCommand>(Ioc.Resolve<AddTreeResultCommand>($"Game.CollisionTree.{shape1}_{shape2}.Add", testAttributes));
        Assert.IsType<CheckTreeResultCommand>(Ioc.Resolve<CheckTreeResultCommand>($"Game.CollisionTree.{shape1}_{shape2}.Check", testAttributes));
    }
}
