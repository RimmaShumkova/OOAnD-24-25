using App;
using App.Scopes;

namespace SpaceBattle.Lib;

public class RegisterIoCDependencyCollisionDataSaveCommandTests
{
    public RegisterIoCDependencyCollisionDataSaveCommandTests()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void ExecuteTestCollisionSaveDataCommand()
    {
        var collisionName = "testCollision";
        var collisionData = new List<int[]> { new int[] { 1, 2 }, new int[] { 3, 4 } };

        var registerCommand = new RegisterIoCDependencyCollisionDataSaveCommand();

        registerCommand.Execute();

        var resolvedCommand = Ioc.Resolve<ICommand>(
            "Collision.SaveDataCommand",
            collisionName,
            collisionData
        );

        Assert.IsType<CollisionSaveDataCommand>(resolvedCommand);
    }
}
