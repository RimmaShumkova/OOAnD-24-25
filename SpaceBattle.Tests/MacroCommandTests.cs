using Moq;
namespace SpaceBattle.Lib.Tests;

public class MacroCommandTests
{
    [Fact]
    public void Execute_AllCommandsExecuted()
    {
        var command1 = new Mock<ICommand>();
        var command2 = new Mock<ICommand>();
        ICommand[] lst = [command1.Object, command2.Object];
        var mc = new MacroCommand(lst);
        mc.Execute();

        command1.Verify(x => x.Execute());
        command2.Verify(x => x.Execute());
    }

    [Fact]
    public void StopOnException()
    {
        var command1 = new Mock<ICommand>();
        var command2 = new Mock<ICommand>();
        var command3 = new Mock<ICommand>();

        command2.Setup(m => m.Execute()).Throws<Exception>();

        var macroCommand = new MacroCommand(new[] { command1.Object, command2.Object, command3.Object });

        Assert.Throws<Exception>(() => macroCommand.Execute());
        command1.Verify(m => m.Execute(), Times.Once());
        command2.Verify(m => m.Execute(), Times.Once());
        command3.Verify(m => m.Execute(), Times.Never());
    }
}
