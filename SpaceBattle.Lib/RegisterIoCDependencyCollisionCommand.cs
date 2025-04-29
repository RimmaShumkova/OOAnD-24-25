using App;
namespace SpaceBattle.Lib;

public class RegisterIoCDependencyCollisionCommand : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Game.CollisionCommand",
                (object[] arg) => new CollisionCommand((IColliding)arg[0], (IColliding)arg[1], (ICommand)arg[2])).Execute();
    }
}
