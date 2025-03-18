using App;
namespace SpaceBattle.Lib;

public class ShootCommand : ICommand
{
    private readonly object shooter;

    public ShootCommand(object shooter)
    {
        this.shooter = shooter;
    }

    public void Execute()
    {
        _ = Ioc.Resolve<IProjectile>("Game.Projectile.Create");

        var initialProperties = Ioc.Resolve<IDictionary<string, object>>("Game.Projectile.Properties", shooter);

        Ioc.Resolve<ICommand>("Commands.Move", initialProperties).Execute();
    }
}
