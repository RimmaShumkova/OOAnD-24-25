using App;
using App.Scopes;
using Moq;

namespace SpaceBattle.Lib;

public class RegisterIoCDependencyCollisionCommandTests
{
    public RegisterIoCDependencyCollisionCommandTests()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }
    [Fact]
    public void ExecuteTest()
    {
        var mockFirstObject = new Mock<IColliding>();
        var mockSecondObject = new Mock<IColliding>();
        var mockCommand = new Mock<ICommand>();

        var registerCommand = new RegisterIoCDependencyCollisionCommand();
        registerCommand.Execute();

        var resolvedCommand = Ioc.Resolve<ICommand>("Game.CollisionCommand",
                mockFirstObject.Object,
                mockSecondObject.Object,
                mockCommand.Object);

        Assert.IsType<CollisionCommand>(resolvedCommand);

    }
}
