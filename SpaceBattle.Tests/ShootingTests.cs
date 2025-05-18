using App;
using App.Scopes;
using Moq;

namespace SpaceBattle.Lib;

public class ShootCommandTests
{
    public ShootCommandTests()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void TestCommandSuccessfullyExecutes()
    {
        var properties = new Mock<IDictionary<string, object>>();

        var shootableMock = new Mock<SpaceBattle.Lib.IShootable>();
        var actionMock = new Mock<SpaceBattle.Lib.ICommand>();
        actionMock.Setup(x => x.Execute()).Verifiable();

        var gameAddMock = new Mock<SpaceBattle.Lib.ICommand>();
        gameAddMock.Setup(x => x.Execute()).Verifiable();

        new RegisterIoCDependencyShootCommand().Execute();

        Ioc.Resolve<App.ICommand>("IoC.Register", "GameObject.Add", (object[] args) => gameAddMock.Object).Execute();

        Ioc.Resolve<App.ICommand>("IoC.Register", "Actions.Start", (object[] args) => actionMock.Object).Execute();

        Ioc.Resolve<ICommand>("Commands.Shoot", shootableMock.Object).Execute();

        gameAddMock.VerifyAll();
        actionMock.VerifyAll();
    }

    [Fact]
    public void TestCommandIShootableException()
    {
        var properties = new Mock<IDictionary<string, object>>();

        var movableMock = new Mock<SpaceBattle.Lib.IMovable>();
        var actionMock = new Mock<SpaceBattle.Lib.ICommand>();
        var gameAddMock = new Mock<SpaceBattle.Lib.ICommand>();

        new RegisterIoCDependencyShootCommand().Execute();

        Ioc.Resolve<App.ICommand>("IoC.Register", "GameObject.Add", (object[] args) => gameAddMock.Object).Execute();

        Ioc.Resolve<App.ICommand>("IoC.Register", "Actions.Start", (object[] args) => actionMock.Object).Execute();

        Assert.Throws<InvalidCastException>(() => Ioc.Resolve<ICommand>("Commands.Shoot", movableMock.Object));
    }
}
