namespace SpaceBattle.Lib;

public class AddGameObjectCommand : ICommand
{
    private readonly Dictionary<string, Dictionary<string, object>> _itemsCollection;
    private readonly Dictionary<string, object> _newItem;

    public AddGameObjectCommand(Dictionary<string, Dictionary<string, object>> itemsCollection, Dictionary<string, object> newItem)
    {
        _itemsCollection = itemsCollection;
        _newItem = newItem;
    }

    public void Execute()
    {
        string itemId;

        if (!_newItem.TryGetValue("Id", out var idValue))
        {
            itemId = Guid.NewGuid().ToString();
            _newItem.Add("Id", itemId);
        }
        else
        {
            itemId = idValue.ToString() ?? throw new InvalidOperationException("Идентификатор объекта не может быть null.");
        }

        if (_itemsCollection.ContainsKey(itemId))
        {
            throw new InvalidOperationException($"Объект с идентификатором '{itemId}' уже существует в коллекции.");
        }

        _itemsCollection.Add(itemId, _newItem);
    }
}
