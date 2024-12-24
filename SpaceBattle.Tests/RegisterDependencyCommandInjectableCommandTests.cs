using App;
using App.Scopes;
namespace SpaceBattle.Lib.Tests;

public class RegisterDependencyCommandInjectableCommandTests
{
    [Fact]
    public void RegisterTest()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();

        var registerCm = new RegisterDependencyCommandInjectableCommand();
        registerCm.Execute();

        Ioc.Resolve<ICommand>("Commands.CommadInjectable");
        Ioc.Resolve<ICommandInjectable>("Commands.CommadInjectable");
        Ioc.Resolve<InjectableCommand>("Commands.CommadInjectable");

    }
}
