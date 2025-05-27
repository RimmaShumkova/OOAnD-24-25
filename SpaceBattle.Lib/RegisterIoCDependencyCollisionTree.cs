using App;
namespace SpaceBattle.Lib;

public class RegisterIocDependencyCollisionTree : ICommand
{
    public readonly string shape1;
    public readonly string shape2;
    public RegisterIocDependencyCollisionTree(string shape1, string shape2)
    {
        this.shape1 = shape1;
        this.shape2 = shape2;
    }
    public void Execute()
    {
        var tree = new Dictionary<int, object>();
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            $"Game.CollisionTree.{shape1}_{shape2}.Add",
            (object[] args) =>
            {
                return new AddTreeResultCommand(tree, (List<int>)args[0]);
            }
        ).Execute();
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            $"Game.CollisionTree.{shape1}_{shape2}.Check",
            (object[] args) =>
            {
                return new CheckTreeResultCommand(tree, (List<int>)args[0]);
            }
        ).Execute();
    }
}
