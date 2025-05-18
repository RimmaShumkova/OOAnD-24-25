using App;
namespace SpaceBattle.Lib;

public class ShootCommand : ICommand
{
    private readonly IShootable shooter;

    public ShootCommand(IShootable shooter)
    {
        this.shooter = shooter;
    }

    public void Execute()
    {
        var projectileProperties = Ioc.Resolve<IDictionary<string, object>>("Game.Projectile.Create", shooter);
        Ioc.Resolve<ICommand>("Actions.Start", projectileProperties, "Move").Execute();
    }
}
