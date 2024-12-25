using App;
namespace SpaceBattle.Lib;
public class RegisterIoCDependencyMacroMoveRotate : ICommand
{
    public void Execute()
    {
        var CreateMacroMove = new CreateMacroCommandStrategy("Move");
        Ioc.Resolve<App.ICommand>("IoC.Register",
                    "Macro.Move",
                    (object[] args) => CreateMacroMove.Resolve(args)).Execute();

        var CreateMacroRotate = new CreateMacroCommandStrategy("Rotate");
        Ioc.Resolve<App.ICommand>("IoC.Register",
                    "Macro.Rotate",
                    (object[] args) => CreateMacroRotate.Resolve(args)).Execute();
    }
}
