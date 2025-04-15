using System.Diagnostics;
using Moq;
using SpaceBattle.Lib;

namespace SpaceBattle.Tests;

public class GameTests
{
    private readonly Mock<ICommand> _commandMock = new();
    private readonly Mock<IQueue> _queueMock = new();

    [Fact]
    public void Constructor_NullQueue_InitializesCorrectly()
    {
        var game = new Game(null);
        Assert.NotNull(game);
    }

    [Fact]
    public void Constructor_InvalidObject_InitializesWithNullQueue()
    {
        var game = new Game(new object());
        Assert.NotNull(game);
    }

    [Fact]
    public void Execute_CommandsInQueue_ExecutesAtLeastOneWithinTime()
    {
        _commandMock.Setup(c => c.Execute()).Verifiable();
        _queueMock.Setup(q => q.Get()).Returns(_commandMock.Object);
        _queueMock.Setup(q => q.Count()).Returns(3);

        var game = new Game(_queueMock.Object);
        var stopwatch = Stopwatch.StartNew();

        game.Execute();
        stopwatch.Stop();

        _commandMock.Verify(c => c.Execute(), Times.AtLeastOnce());
        Assert.True(stopwatch.ElapsedMilliseconds < 60);
    }

    [Fact]
    public void Execute_EmptyQueue_NoCommandsRunAndFastExit()
    {
        _queueMock.Setup(q => q.Count()).Returns(0);

        var game = new Game(_queueMock.Object);
        var stopwatch = Stopwatch.StartNew();

        game.Execute();
        stopwatch.Stop();

        _commandMock.Verify(c => c.Execute(), Times.Never());
        Assert.True(stopwatch.ElapsedMilliseconds < 5);
    }

    [Fact]
    public void Execute_NullQueue_NoCommandsRunAndFastExit()
    {
        var game = new Game(null);
        var stopwatch = Stopwatch.StartNew();

        game.Execute();
        stopwatch.Stop();

        _commandMock.Verify(c => c.Execute(), Times.Never());
        Assert.True(stopwatch.ElapsedMilliseconds < 5);
    }
}
