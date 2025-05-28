using App;
namespace SpaceBattle.Lib;

public class RegisterIoCDependencyAddObjectToFileCommand : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Commands.AddObjectToFile",
                (object[] arg) => new AddObjectToFileCommand((string)arg[0], arg[1])).Execute();
    }
}
