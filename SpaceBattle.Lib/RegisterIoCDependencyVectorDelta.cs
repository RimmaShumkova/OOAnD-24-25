using App;
namespace SpaceBattle.Lib;

public class RegisterIoCDependencyVectorDelta : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Game.GetVectorDelta",
            (object[] args) => ((Vector)args[0]).GetElements().Zip(((Vector)args[1]).GetElements(), (a, b) => a - b).ToArray()
        ).Execute();
    }
}
