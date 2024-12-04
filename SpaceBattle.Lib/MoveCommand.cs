namespace SpaceBattle.Lib;

public class MoveCommand : ICommand
{
    private readonly IMovable _objToMove;
    public MoveCommand(IMovable obj)
    {
        _objToMove = obj;
    }
    public void Execute()
    {
        _objToMove.Position += _objToMove.Veloсity;
    }
}
