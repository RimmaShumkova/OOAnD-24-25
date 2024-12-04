namespace SpaceBattle.Lib;

public class RotateCommand : ICommand
{
    private readonly IRotatable obj;
    public RotateCommand(IRotatable obj)
    {
        this.obj = obj;
    }

    public void Execute()
    {
        obj.angle += obj.angleVelocity;
    }
}
