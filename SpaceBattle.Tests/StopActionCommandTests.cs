using App;
using App.Scopes;
using Moq;
using SpaceBattle.Lib;

namespace SpaceBattle.Tests;

public class StopActionCommandTest
{
    public StopActionCommandTest()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void StopActionCommandExecuteTest()
    {
        var inj = new Mock<ICommandInjectable>();
        var objShip = new Mock<IDictionary<string, object>>();

        objShip.Setup(x => x["Move"]).Returns(inj.Object);

        Ioc.Resolve<App.ICommand>("IoC.Register", "Game.Object", (object[] args) => objShip.Object).Execute();

        var obj = new Mock<IDictionary<string, object>>();

        obj.Setup(x => x["Action"]).Returns("Move");
        obj.Setup(x => x["Key"]).Returns("");

        var stopCommand = new StopActionCommand(obj.Object);

        stopCommand.Execute();

        inj.Verify(c => c.Inject(It.IsAny<SpaceBattle.Lib.ICommand>()), Times.Once);
    }
}
