using App;
namespace SpaceBattle.Lib;

public class RegisterIoCDependencyCollisionPrepareData : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Collision.PrepareData",
                (object[] arg) => new CollisionPrepareDataCommand((ICollisionDataGenerator)arg[0])).Execute();
    }
}
