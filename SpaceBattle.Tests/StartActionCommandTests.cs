using App;
using App.Scopes;
using Moq;
using SpaceBattle.Lib;

namespace SpaceBattle.Tests;

public class StartActionCommandTest
{
    public StartActionCommandTest()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }
    
    [Fact]
    public void StartActionCommandExecuteTest()    
    {   
        var mCommand = new Mock<SpaceBattle.Lib.ICommand>();
        var macroCommand = new Mock<SpaceBattle.Lib.ICommand>();
        var inj = new InjectableCommand();
        var q = new Queue<SpaceBattle.Lib.ICommand>();
        var sendCommand = new Mock<SpaceBattle.Lib.ICommand>();
        var objShip = new Mock<IDictionary<string, object>>();

        Ioc.Resolve<App.ICommand>("IoC.Register", "Macro.Move", (object[] args) => mCommand.Object).Execute();
        Ioc.Resolve<App.ICommand>("IoC.Register", "Commands.CommandInjectable", (object[] args) => inj).Execute();
        Ioc.Resolve<App.ICommand>("IoC.Register", "Game.Queue", (object[] args) => q).Execute();
        Ioc.Resolve<App.ICommand>("IoC.Register", "Commands.Send", (object[] args) => sendCommand.Object).Execute();
        Ioc.Resolve<App.ICommand>("IoC.Register", "Commands.Macro", (object[] args) => macroCommand.Object).Execute();
        Ioc.Resolve<App.ICommand>("IoC.Register", "Game.Object", (object[] args) => objShip.Object).Execute();

        var obj = new Mock<IDictionary<string, object>>();

        obj.Setup(x => x["Action"]).Returns("Move");
        obj.Setup(x => x["Args"]).Returns(new object[]{"test"});
        obj.Setup(x => x["Key"]).Returns("");
        
        
        var startCommand = new StartActionCommand(obj.Object);

        startCommand.Execute();

        sendCommand.Verify(c => c.Execute(), Times.Once);
    }
}
