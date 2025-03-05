using App;
using App.Scopes;
using Moq;

namespace SpaceBattle.Lib;

public class SendCommandRegisterTest
{
    public SendCommandRegisterTest()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }
    [Fact]
    public void ExecuteTest()
    {
        var mock_rec = new Mock<ICommandReceiver>();
        var mock_cmd = new Mock<ICommand>();

        var registerSend = new RegisterIoCDependencySendCommand();
        registerSend.Execute();

        var sendcom = Ioc.Resolve<ICommand>("Commands.Send", mock_cmd.Object, mock_rec.Object);

        Assert.IsType<SendCommand>(sendcom);

    }
}
