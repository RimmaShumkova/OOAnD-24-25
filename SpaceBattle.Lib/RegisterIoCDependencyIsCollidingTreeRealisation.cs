using App;
namespace SpaceBattle.Lib;

public class RegisterIsCollidingTreeRealisation : ICommand
{
    public void Execute()
    {
        var isColliding = new IsCollidingTreeStrategy();
        Ioc.Resolve<App.ICommand>("IoC.Register",
                "Game.IsColliding",
                (object[] args) => isColliding.Resolve(args)).Execute();
    }
}
