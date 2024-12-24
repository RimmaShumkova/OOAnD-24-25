using App;
using App.Scopes;
using Moq;

namespace SpaceBattle.Lib.Tests;

public class RegisterIoCDependencyRotateCommandTests
{
    [Fact]
    public void ResolveDependencyCheckRotate()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();

        var mock_reg_rotate = new Mock<RegisterIoCDependencyRotateCommand>();
        var reg_rotate = mock_reg_rotate.Object;
        reg_rotate.Execute();

        var mock_irotate = new Mock<IRotatable>();
        Ioc.Resolve<App.ICommand>("IoC.Register", "Adapters.IRotatableObject", (object[] args) => mock_irotate.Object).Execute();

        var res = Ioc.Resolve<ICommand>("Commands.Rotate");

        Assert.IsType<RotateCommand>(res);
    }
}
