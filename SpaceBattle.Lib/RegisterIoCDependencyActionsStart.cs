using App;
namespace SpaceBattle.Lib;

public class RegisterIoCDependencyActionsStart : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Actions.Start",
            (object[] args) => new StartActionCommand((IDictionary<string, object>)args[0])
        ).Execute();
    }
}
