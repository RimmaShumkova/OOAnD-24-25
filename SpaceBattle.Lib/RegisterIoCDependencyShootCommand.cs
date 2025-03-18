using App;
namespace SpaceBattle.Lib;

public class RegisterIoCDependencyShootCommand : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>("IoC.Register",
                            "Commands.Shoot",
        (object[] args) => new ShootCommand(Ioc.Resolve<IShootable>("Adapters.IShootableObject", args))).Execute();
    }
}
