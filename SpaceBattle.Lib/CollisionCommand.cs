using App;
namespace SpaceBattle.Lib;

public class CollisionCommand : ICommand
{
    private readonly IColliding firstObj;

    private readonly IColliding secondObj;

    private readonly ICommand command;

    public CollisionCommand(IColliding firstObj, IColliding secondObj, ICommand command)
    {
        this.firstObj = firstObj;
        this.secondObj = secondObj;
        this.command = command;
    }
    public void Execute()
    {
        var deltaPosition = Ioc.Resolve<Array>("Game.GetVectorDifference", firstObj.Position, secondObj.Position);

        var deltaVelocity = Ioc.Resolve<Array>("Game.GetVectorDifference", firstObj.Velocity, secondObj.Velocity);

        var isCollision = Ioc.Resolve<bool>("Game.IsCollision", deltaPosition, deltaVelocity, firstObj.Shape, secondObj.Shape);
        if (isCollision)
        {
            command.Execute();
        }
    }
}
