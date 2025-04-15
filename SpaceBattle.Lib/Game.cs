using System.Diagnostics;

namespace SpaceBattle.Lib
{
    public interface IQueue
    {
        ICommand Get();
        int Count();
    }
    public class Game : ICommand
    {
        private readonly IQueue? _gameQueue;
        private readonly Stopwatch _gameTimer;

        public Game(object queue)
        {
            _gameQueue = queue as IQueue;
            _gameTimer = new Stopwatch();
        }

        public void Execute()
        {
            if (_gameQueue == null || _gameQueue.Count() == 0)
            {
                return;
            }

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
                && _gameQueue != null
                && _gameQueue.Count() > 0;
        }

        private void ProcessCommand()
        {
            if (_gameQueue == null)
            {
                return;
            }

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
}
