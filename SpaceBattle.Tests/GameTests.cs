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
        Assert.True(stopwatch.ElapsedMilliseconds < 20);
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

    [Fact]
    public void Execute_CommandThrowsException_StopsWithError()
    {
        _commandMock.Setup(c => c.Execute()).Throws<Exception>();
        _queueMock.Setup(q => q.Get()).Returns(_commandMock.Object);
        _queueMock.Setup(q => q.Count()).Returns(3);

        var game = new Game(_queueMock.Object);

        var exception = Assert.Throws<Exception>(() => game.Execute());
        Assert.Equal("Error executing command", exception.Message);
        _commandMock.Verify(c => c.Execute(), Times.Once());
    }

    [Fact]
    public void Execute_QueueGetThrowsException_FailsWithError()
    {
        _queueMock.Setup(q => q.Get()).Throws<Exception>();
        _queueMock.Setup(q => q.Count()).Returns(3);

        var game = new Game(_queueMock.Object);

        Assert.Throws<Exception>(() => game.Execute());
    }

    [Fact]
    public void Execute_QueueCountThrowsException_FailsWithError()
    {
        _queueMock.Setup(q => q.Count()).Throws<Exception>();

        var game = new Game(_queueMock.Object);

        Assert.Throws<Exception>(() => game.Execute());
    }

    [Fact]
    public void Execute_TimeExpires_StopsExecution()
    {
        _commandMock.Setup(c => c.Execute()).Callback(() => Thread.Sleep(60)); // Задержка больше 50 мс
        _queueMock.Setup(q => q.Get()).Returns(_commandMock.Object);
        _queueMock.Setup(q => q.Count()).Returns(10);

        var game = new Game(_queueMock.Object);

        game.Execute();

        _commandMock.Verify(c => c.Execute(), Times.AtMost(2)); // Не более 2 команд из-за времени
    }

    [Fact]
    public void Execute_QueueBecomesEmpty_StopsExecution()
    {
        var count = 2;
        _commandMock.Setup(c => c.Execute()).Verifiable();
        _queueMock.Setup(q => q.Get())
            .Returns(_commandMock.Object)
            .Callback(() => count--); // Уменьшаем count после Get
        _queueMock.Setup(q => q.Count())
            .Returns(() => count);

        var game = new Game(_queueMock.Object);

        game.Execute();

        _commandMock.Verify(c => c.Execute(), Times.Exactly(2));
    }

    [Fact]
    public void Execute_ResetsTimerAfterExecution()
    {
        _commandMock.Setup(c => c.Execute()).Verifiable();
        _queueMock.Setup(q => q.Get()).Returns(_commandMock.Object);
        _queueMock.Setup(q => q.Count()).Returns(1);

        var game = new Game(_queueMock.Object);
        game.Execute();

        var fieldInfo = typeof(Game).GetField("_gameTimer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var timer = (Stopwatch)fieldInfo.GetValue(game);
        Assert.False(timer.IsRunning);
        Assert.Equal(0, timer.ElapsedMilliseconds);
    }
}
