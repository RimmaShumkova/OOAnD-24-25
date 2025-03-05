using App;
using App.Scopes;
using Moq;
using SpaceBattle.Lib;

namespace SpaceBattle.Tests;
public class RegisterIoCDependencyActionsStopTest
{
    public RegisterIoCDependencyActionsStopTest()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void CorrectResolveTest()
    {
        var objOrder = new Mock<IDictionary<string, object>>();

        var registerIoCDependencyActionsStop = new RegisterIoCDependencyActionsStop();
        registerIoCDependencyActionsStop.Execute();
        var res = Ioc.Resolve<SpaceBattle.Lib.ICommand>("Actions.Stop", objOrder.Object);

        Assert.IsType<StopActionCommand>(res);
    }
}
