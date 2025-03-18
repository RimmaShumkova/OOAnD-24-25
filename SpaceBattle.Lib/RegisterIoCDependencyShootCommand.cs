using App;
namespace SpaceBattle.Lib;

public class RegisterIoCDependencyShootCommand : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Commands.Shoot",
                (object arg) => new ShootCommand(arg)
        ).Execute();
    }
}
