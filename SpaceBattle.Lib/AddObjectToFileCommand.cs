using System.Text.Json;
namespace SpaceBattle.Lib;

public class AddObjectToFileCommand : ICommand
{
    private readonly object objectToAdd;
    private readonly string filePath;

    public AddObjectToFileCommand(string filePath, object objectToAdd)
    {
        this.objectToAdd = objectToAdd;
        this.filePath = filePath;
    }

    public void Execute()
    {
        var jsonString = JsonSerializer.Serialize(objectToAdd);
        File.WriteAllText(filePath, jsonString);
    }
}
