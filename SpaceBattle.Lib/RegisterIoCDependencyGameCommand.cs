using App;

namespace SpaceBattle.Lib;

public class RegisterIoCDependencyGameCommand : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>(
                "IoC.Register",
                "Commands.Game",
                (object[] args) => new GameCommand(args[0])).Execute();
    }
}
