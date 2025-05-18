using App;
namespace SpaceBattle.Lib;

public class RegisterIoCDependencyShootCommand : ICommand
{
    public void Execute()
    {
        var createProjectile = new CreateProjectileStrategy();
        Ioc.Resolve<App.ICommand>("IoC.Register",
                "Game.Projectile.Create",
                (object[] args) => createProjectile.Resolve(args)).Execute();

        Ioc.Resolve<App.ICommand>("IoC.Register",
                "Commands.Shoot",
                (object[] args) => new ShootCommand((IShootable)args[0])).Execute();
    }
}
