using System.Diagnostics;
using App;
using App.Scopes;
using Moq;
using SpaceBattle.Lib;

namespace SpaceBattle.Tests;

public class GameTests
{
    public GameTests()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }
    private readonly Mock<SpaceBattle.Lib.ICommand> mock_cmd = new();
    private readonly Mock<IQueue> mock_queue = new();

    [Fact]
    public void ConstructorInvalidObjectThrowsExceptionNullQueue()
    {
        Assert.Throws<InvalidCastException>(() => new GameCommand(new object()));
    }

    [Fact]
    public void ExecuteCommandsInQueueExecutesAtLeastOneWithinTime()
    {
        mock_cmd.Setup(c => c.Execute()).Verifiable();
        mock_queue.Setup(q => q.Get()).Returns(mock_cmd.Object);
        mock_queue.Setup(q => q.Count()).Returns(3);

        Ioc.Resolve<App.ICommand>("IoC.Register", "Game.Get.Time.Quantum", (object[] args) => (object)50).Execute();

        var gameCmd = new GameCommand(mock_queue.Object);
        var stopwatch = Stopwatch.StartNew();

        gameCmd.Execute();
        stopwatch.Stop();

        mock_cmd.Verify(c => c.Execute(), Times.AtLeastOnce());
        Assert.True(stopwatch.ElapsedMilliseconds < 90);
    }

    [Fact]
    public void ExecuteEmptyQueueNoCommandsRunAndFastExit()
    {
        mock_queue.Setup(q => q.Count()).Returns(0);

        Ioc.Resolve<App.ICommand>("IoC.Register", "Game.Get.Time.Quantum", (object[] args) => (object)50).Execute();

        var gameCmd = new GameCommand(mock_queue.Object);
        var stopwatch = Stopwatch.StartNew();

        gameCmd.Execute();
        stopwatch.Stop();

        mock_cmd.Verify(c => c.Execute(), Times.Never());
        Assert.True(stopwatch.ElapsedMilliseconds < 20);
    }

    [Fact]
    public void ExecuteCommandThrowsExceptionStopsWithError()
    {
        mock_cmd.Setup(c => c.Execute()).Throws<Exception>();
        mock_queue.Setup(q => q.Get()).Returns(mock_cmd.Object);
        mock_queue.Setup(q => q.Count()).Returns(3);

        Ioc.Resolve<App.ICommand>("IoC.Register", "Game.Get.Time.Quantum", (object[] args) => (object)50).Execute();

        var gameCmd = new GameCommand(mock_queue.Object);

        var exception = Assert.Throws<Exception>(() => gameCmd.Execute());
        Assert.Equal("Error executing command", exception.Message);
        mock_cmd.Verify(c => c.Execute(), Times.Once());
    }

    [Fact]
    public void ExecuteQueueGetThrowsExceptionFailsWithError()
    {
        mock_queue.Setup(q => q.Get()).Throws<Exception>();
        mock_queue.Setup(q => q.Count()).Returns(3);

        var gameCmd = new GameCommand(mock_queue.Object);

        Assert.Throws<Exception>(() => gameCmd.Execute());
    }

    [Fact]
    public void ExecuteQueueCountThrowsExceptionFailsWithError()
    {
        mock_queue.Setup(q => q.Count()).Throws<Exception>();

        var gameCmd = new GameCommand(mock_queue.Object);

        Assert.Throws<Exception>(() => gameCmd.Execute());
    }

    [Fact]
    public void ExecuteTimeExpiresStopsExecution()
    {
        mock_cmd.Setup(c => c.Execute()).Callback(() => Thread.Sleep(60)); // Задержка больше 50 мс
        mock_queue.Setup(q => q.Get()).Returns(mock_cmd.Object);
        mock_queue.Setup(q => q.Count()).Returns(10);

        Ioc.Resolve<App.ICommand>("IoC.Register", "Game.Get.Time.Quantum", (object[] args) => (object)50).Execute();

        var gameCmd = new GameCommand(mock_queue.Object);

        gameCmd.Execute();

        mock_cmd.Verify(c => c.Execute(), Times.AtMost(2)); // Не более 2 команд из-за времени
    }

    [Fact]
    public void ExecuteQueueBecomesEmptyStopsExecution()
    {
        var count = 2;
        mock_cmd.Setup(c => c.Execute()).Verifiable();
        mock_queue.Setup(q => q.Get())
            .Returns(mock_cmd.Object)
            .Callback(() => count--); // Уменьшаем count после Get
        mock_queue.Setup(q => q.Count())
            .Returns(() => count);

        Ioc.Resolve<App.ICommand>("IoC.Register", "Game.Get.Time.Quantum", (object[] args) => (object)50).Execute();

        var gameCmd = new GameCommand(mock_queue.Object);

        gameCmd.Execute();

        mock_cmd.Verify(c => c.Execute(), Times.Exactly(2));
    }

    [Fact]
    public void ExecuteResetsTimerAfterExecution()
    {
        mock_cmd.Setup(c => c.Execute()).Verifiable();
        mock_queue.Setup(q => q.Get()).Returns(mock_cmd.Object);
        mock_queue.Setup(q => q.Count()).Returns(1);

        Ioc.Resolve<App.ICommand>("IoC.Register", "Game.Get.Time.Quantum", (object[] args) => (object)50).Execute();

        var gameCmd = new GameCommand(mock_queue.Object);
        gameCmd.Execute();

        var fieldInfo = typeof(GameCommand).GetField("_gameTimer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var timer = (Stopwatch)fieldInfo.GetValue(gameCmd);
        Assert.False(timer.IsRunning);
        Assert.Equal(0, timer.ElapsedMilliseconds);
    }
}
