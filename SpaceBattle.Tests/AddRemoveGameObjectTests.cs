namespace SpaceBattle.Tests;

using SpaceBattle.Lib;

public class AddGameObjectCommandTests
{
    [Fact]
    public void Execute_HandlesNonStringId_ConvertsToString()
    {
        var repository = new Dictionary<string, Dictionary<string, object>>();
        var nonStringId = 12345;
        var gameObject = new Dictionary<string, object> { { "Id", nonStringId } };
        var command = new AddGameObjectCommand(repository, gameObject);

        command.Execute();

        Assert.True(repository.ContainsKey("12345"));
        Assert.Same(gameObject, repository["12345"]);
    }

    [Fact]
    public void Execute_AssignsId_WhenNotPresent_AndAddsGameObject()
    {
        var gameItems = new Dictionary<string, Dictionary<string, object>>();
        var gameObject = new Dictionary<string, object>();
        var command = new AddGameObjectCommand(gameItems, gameObject);
        command.Execute();
        Assert.True(gameObject.ContainsKey("Id"));
        var id = (string)gameObject["Id"];
        Assert.True(gameItems.ContainsKey(id));
        Assert.Same(gameObject, gameItems[id]);
    }

    [Fact]
    public void Execute_AddsGameObject_WhenIdPresentAndNotInRepository()
    {
        var repository = new Dictionary<string, Dictionary<string, object>>();
        var presetId = "id";
        var gameObject = new Dictionary<string, object>
        {
            { "Id", presetId }
        };
        var command = new AddGameObjectCommand(repository, gameObject);
        command.Execute();
        Assert.True(repository.ContainsKey(presetId));
        Assert.Same(gameObject, repository[presetId]);
    }

    [Fact]
    public void Execute_ThrowsException_WhenGameObjectWithSameIdAlreadyExists()
    {
        var repository = new Dictionary<string, Dictionary<string, object>>();
        var duplicateId = "id";
        var gameObject = new Dictionary<string, object>
        {
            { "Id", duplicateId }
        };
        repository[duplicateId] = gameObject;
        var command = new AddGameObjectCommand(repository, gameObject);
        var ex = Assert.Throws<InvalidOperationException>(() => command.Execute());
    }

    [Fact]
    public void Execute_ThrowsException_WhenIdIsNull()
    {
        var repository = new Dictionary<string, Dictionary<string, object>>();
        var gameObject = new Dictionary<string, object> { { "Id", null! } };
        var command = new AddGameObjectCommand(repository, gameObject);
        var ex = Assert.Throws<InvalidOperationException>(() => command.Execute());
        Assert.Equal("Идентификатор объекта не может быть null.", ex.Message);
    }
}

public class RemoveGameObjectCommandTests
{
    [Fact]
    public void Execute_ShouldRemoveGameItem_WhenGameItemExists()
    {
        var gameItems = new Dictionary<string, Dictionary<string, object>>();
        var id = "id";
        var gameObject = new Dictionary<string, object>
        {
            { "Id", id }
        };
        gameItems[id] = gameObject;
        var command = new RemoveGameObjectCommand(gameItems, id);
        command.Execute();
        Assert.False(gameItems.ContainsKey(id));
    }

    [Fact]
    public void Execute_ShouldThrowException_WhenGameItemNotFoundForRemoval()
    {
        var repository = new Dictionary<string, Dictionary<string, object>>();
        var nonExistentId = "id";
        var command = new RemoveGameObjectCommand(repository, nonExistentId);
        var ex = Assert.Throws<Exception>(() => command.Execute());
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenObjectsStorageIsNull()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => new RemoveGameObjectCommand(null!, "id"));
        Assert.Equal("Хранилище объектов не может быть null. (Parameter 'objectsStorage')", ex.Message);
    }

    [Fact]
    public void Constructor_ThrowsArgumentException_WhenItemIdIsNull()
    {
        var repository = new Dictionary<string, Dictionary<string, object>>();
        var ex = Assert.Throws<ArgumentException>(() => new RemoveGameObjectCommand(repository, null!));
        Assert.Equal("Идентификатор объекта не может быть пустым. (Parameter 'itemId')", ex.Message);
    }

    [Fact]
    public void Constructor_ThrowsArgumentException_WhenItemIdIsEmpty()
    {
        var repository = new Dictionary<string, Dictionary<string, object>>();
        var ex = Assert.Throws<ArgumentException>(() => new RemoveGameObjectCommand(repository, ""));
        Assert.Equal("Идентификатор объекта не может быть пустым. (Parameter 'itemId')", ex.Message);
    }

    [Fact]
    public void Constructor_ThrowsArgumentException_WhenItemIdIsWhitespace()
    {
        var repository = new Dictionary<string, Dictionary<string, object>>();
        var ex = Assert.Throws<ArgumentException>(() => new RemoveGameObjectCommand(repository, "   "));
        Assert.Equal("Идентификатор объекта не может быть пустым. (Parameter 'itemId')", ex.Message);
    }
}
