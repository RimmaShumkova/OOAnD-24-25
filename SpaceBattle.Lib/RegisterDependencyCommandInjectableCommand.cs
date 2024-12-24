using App;
namespace SpaceBattle.Lib;

public class RegisterDependencyCommandInjectableCommand : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Commands.CommadInjectable",
                (object arg) => new InjectableCommand()
                ).Execute();
    }
}
