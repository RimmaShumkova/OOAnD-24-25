using App;
using App.Scopes;

namespace SpaceBattle.Lib.Tests;

public class RegisterIoCDependencyVectorDeltaTests
{
    public RegisterIoCDependencyVectorDeltaTests()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void ExecuteTestVectorDelta()
    {
        var registerCommand = new RegisterIoCDependencyVectorDelta();
        registerCommand.Execute();

        var vector1 = new Vector(5, 3);
        var vector2 = new Vector(2, 1);
        var result = Ioc.Resolve<object>("Game.GetVectorDelta", vector1, vector2);

        Assert.IsType<int[]>(result);
        var deltaArray = (int[])result;
        Assert.Equal([3, 2], deltaArray);
    }
}
