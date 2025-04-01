using App;
using App.Scopes;
using Moq;

namespace SpaceBattle.Lib.Tests;

public class RegisterIoCDependencyShootCommandTests
{
    [Fact]
    public void ResolveDependencyShootCheck()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();

        var reg_shoot = new RegisterIoCDependencyShootCommand();
        reg_shoot.Execute();

        var mock_ishooting = new Mock<IShootable>();
        Ioc.Resolve<App.ICommand>("IoC.Register", "Adapters.IShootableObject", (object[] args) => mock_ishooting.Object).Execute();

        var res = Ioc.Resolve<ICommand>("Commands.Shoot");

        Assert.IsType<ShootCommand>(res);
    }
}
