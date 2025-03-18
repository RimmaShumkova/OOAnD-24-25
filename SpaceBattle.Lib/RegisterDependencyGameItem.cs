using App;
namespace SpaceBattle.Lib;

public class RegisterDependencyGameItem : ICommand
{
    public void Execute()
    {
        var gameItems = new Dictionary<string, Dictionary<string, object>>();

        // Регистрация команды для добавления игрового объекта
        Ioc.Resolve<ICommand>(
            "IoC.Register",
            "GameItem.Add",
            (Func<object[], ICommand>)(args =>
            {
                var gameObject = (Dictionary<string, object>)args[0];
                return new AddGameObjectCommand(gameItems, gameObject);
            })
        ).Execute();

        // Регистрация команды для удаления игрового объекта
        Ioc.Resolve<ICommand>(
            "IoC.Register",
            "GameItem.Remove",
            (Func<object[], ICommand>)(args =>
            {
                var key = (string)args[0];
                return new RemoveGameObjectCommand(gameItems, key);
            })
        ).Execute();

        // Регистрация команды для получения игрового объекта
        Ioc.Resolve<ICommand>(
            "IoC.Register",
            "GameItem.Get",
            (Func<object[], Dictionary<string, object>>)(args =>
            {
                var key = (string)args[0];
                if (!gameItems.TryGetValue(key, out var result))
                {
                    throw new Exception($"GameItem с ключом '{key}' не найден.");
                }

                return result;
            })
        ).Execute();
    }
}
