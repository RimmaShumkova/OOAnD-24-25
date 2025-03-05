using SpaceBattle.Lib;

namespace SpaceBattle.Tests;

public class EmptyCommandTest
{
    [Fact]
    public void EmptyCommandExecute()
    {
        var emptyCommand = new EmptyCommand();

        emptyCommand.Execute();
    }
}
