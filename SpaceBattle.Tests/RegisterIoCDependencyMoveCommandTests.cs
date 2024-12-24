using App;
using App.Scopes;
using Moq;

namespace SpaceBattle.Lib.Tests;

public class RegisterIoCDependencyMoveCommandTests
{
    [Fact]
    public void ResolveDependencyCheck()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();

        var mock_reg_move = new Mock<RegisterIoCDependencyMoveCommand>();
        var reg_move = mock_reg_move.Object;
        reg_move.Execute();

        var mock_imoving = new Mock<IMovable>();
        Ioc.Resolve<App.ICommand>("IoC.Register", "Adapters.IMovableObject", (object[] args) => mock_imoving.Object).Execute();

        var res = Ioc.Resolve<ICommand>("Commands.Move");

        Assert.IsType<MoveCommand>(res);

    }
}
