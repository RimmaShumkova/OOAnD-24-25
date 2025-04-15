using System.Diagnostics;
using App;

namespace SpaceBattle.Lib;

public class GameCommand : ICommand
{
    private readonly IQueue _gameQueue;
    private readonly Stopwatch _gameTimer;

    public GameCommand(object queue)
    {
        _gameQueue = (IQueue)queue;
        _gameTimer = new Stopwatch();
    }

    public void Execute()
    {
        _gameTimer.Start();

        while (IsActive())
        {
            ProcessCommand();
        }

        _gameTimer.Reset();
    }

    private bool IsActive()
    {
        return _gameTimer.ElapsedMilliseconds < Ioc.Resolve<int>("Game.Get.Time.Quantum")
            && _gameQueue.Count() > 0;
    }

    private void ProcessCommand()
    {
        var command = _gameQueue.Get();

        try
        {
            command.Execute();
        }
        catch
        {
            throw new Exception("Error executing command");
        }
    }
}

