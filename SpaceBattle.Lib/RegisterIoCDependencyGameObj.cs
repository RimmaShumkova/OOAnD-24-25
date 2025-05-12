namespace SpaceBattle.Lib;
using App;

public class RegisterIoCDependencyGameObj : ICommand
{
    public void Execute()
    {
        var gameObj = new Dictionary<string, Dictionary<string, object>>();

        Ioc.Resolve<ICommand>(
            "Ioc.Register",
            "GameObject.Add",
            (object[] args) => new AddGameObjectCommand(
                gameObj,
                (Dictionary<string, object>)args[0]
            )
        ).Execute();

        Ioc.Resolve<ICommand>(
            "Ioc.Register",
            "GameObject.Remove",
            (object[] args) => new RemoveGameObjectCommand(
                gameObj,
                (string)args[0]
            )
        ).Execute();

        Ioc.Resolve<ICommand>(
            "Ioc.Register",
            "GameObject.Get",
            (object[] args) =>
            {
                var itemId = (string)args[0];
                if (!gameObj.TryGetValue(itemId, out var result))
                {
                    throw new Exception($"GameObject с '{itemId}' не найден.");
                }

                return result;
            }
        ).Execute();
    }
}
