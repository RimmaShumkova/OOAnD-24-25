using App;
namespace SpaceBattle.Lib;

public class RegisterIoCDependencyCollisionDataSaveCommand : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Collision.SaveDataCommand",
                (object[] arg) => new CollisionSaveDataCommand((string)arg[0], (IList<int[]>)arg[1])).Execute();
    }
}
