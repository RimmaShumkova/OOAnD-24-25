using System.Diagnostics;

namespace SpaceBattle.Lib;

public class Game : ICommand
{
    private readonly IQueue _gameQueue;
    private readonly Stopwatch _gameTimer;

    public Game(object queue)
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
        return _gameTimer.ElapsedMilliseconds < 50
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

