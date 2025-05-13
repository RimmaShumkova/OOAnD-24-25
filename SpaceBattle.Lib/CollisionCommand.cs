using App;
namespace SpaceBattle.Lib;

public class CollisionCommand : ICommand
{
    private readonly IColliding firstObject;
    private readonly IColliding secondObject;
    private readonly ICommand command;

    public CollisionCommand(IColliding firstObject, IColliding secondObject, ICommand command)
    {
        this.firstObject = firstObject;
        this.secondObject = secondObject;
        this.command = command;
    }
    public void Execute()
    {
        var deltaPosition = Ioc.Resolve<Array>("Game.GetVectorDifference", firstObject.Position, secondObject.Position);
        var deltaVelocity = Ioc.Resolve<Array>("Game.GetVectorDifference", firstObject.Velocity, secondObject.Velocity);

        var isColliding = Ioc.Resolve<bool>("Game.IsColliding", deltaPosition, deltaVelocity, firstObject.Shape, secondObject.Shape);
        if (isColliding)
        {
            command.Execute();
        }
    }
}
