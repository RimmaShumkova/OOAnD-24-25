using App;
using App.Scopes;

namespace SpaceBattle.Lib;

public class ShootCommandTests
{
    public ShootCommandTests()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }
}
