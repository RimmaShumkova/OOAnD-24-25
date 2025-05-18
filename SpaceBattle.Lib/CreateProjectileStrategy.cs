using App;

namespace SpaceBattle.Lib;

public class CreateProjectileStrategy
{
    public IDictionary<string, object> Resolve(object[] args)
    {
        var projectileId = Guid.NewGuid().ToString();
        var shooter = (IShootable)args[0];
        var projectileProperties = new Dictionary<string, object> {
            {"Id", projectileId},
            { "Position", shooter.StartPosition },
            {"Velocity", shooter.Velocity}};
        Ioc.Resolve<ICommand>("GameObject.Add", projectileProperties).Execute();

        return projectileProperties;
    }
}
