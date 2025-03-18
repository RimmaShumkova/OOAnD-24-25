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
        var projectile = Ioc.Resolve<IProjectile>("Game.Projectile.Create");
        
        var initialProperties = Ioc.Resolve<IDictionary<string, object>>("Game.Projectile.Properties", shooter);
        
        var moveCommand = Ioc.Resolve<ICommand>("Commands.Move", initialProperties);
        
        Ioc.Resolve<ICommand>("Commands.Macro.Create", new[] { moveCommand }).Execute();
    }
}