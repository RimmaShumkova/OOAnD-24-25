namespace SpaceBattle.Lib;

public class RemoveGameObjectCommand : ICommand
{
    private readonly Dictionary<string, Dictionary<string, object>> _objectsStorage;
    private readonly string _itemId;

    public RemoveGameObjectCommand(Dictionary<string, Dictionary<string, object>> objectsStorage, string itemId)
    {
        _objectsStorage = objectsStorage;
        _itemId = string.IsNullOrWhiteSpace(itemId)
            ? throw new ArgumentException("Идентификатор объекта не может быть пустым.", nameof(itemId))
            : itemId;
    }

    public void Execute()
    {
        if (!_objectsStorage.ContainsKey(_itemId))
        {
            throw new Exception($"Не удалось найти объект с ID '{_itemId}' для удаления.");
        }

        _objectsStorage.Remove(_itemId);
    }
}
