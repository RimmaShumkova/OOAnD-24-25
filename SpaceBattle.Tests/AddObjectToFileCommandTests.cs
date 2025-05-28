using System.Text.Json;
using App;
using App.Scopes;

namespace SpaceBattle.Lib.Tests;

public class AddObjectToFileCommandTests
{
    public AddObjectToFileCommandTests()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void Execute_ThrowsWhenFilePathIsNull()
    {
        var cmd = new AddObjectToFileCommand(null!, new object());
        Assert.Throws<ArgumentNullException>(() => cmd.Execute());
    }

    [Fact]
    public void Execute_WritesSerializedJsonToFile()
    {
        var data = new List<int[]> { new[] { 9, 22 }, new[] { 6, 18 } };
        var path = Path.GetTempFileName();

        new RegisterIoCDependencyAddObjectToFileCommand().Execute();
        var cmd = Ioc.Resolve<ICommand>("Commands.AddObjectToFile", path, data);
        cmd.Execute();

        var readText = File.ReadAllText(path);
        var deserialized = JsonSerializer.Deserialize<List<int[]>>(readText);

        Assert.NotNull(deserialized);
        Assert.Equal(2, deserialized!.Count);
        File.Delete(path);
    }
}
