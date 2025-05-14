using App;

namespace SpaceBattle.Lib;

public class CollisionPrepareDataCommand : ICommand
{
    private readonly ICollisionDataGenerator dataGenerator;
    public CollisionPrepareDataCommand(ICollisionDataGenerator dataGenerator)
    {
        this.dataGenerator = dataGenerator;
    }

    public void Execute()
    {
        var collisionName = Ioc.Resolve<string>("Collision.GetCollisionName", dataGenerator.firstShape, dataGenerator.secondShape);
        var collisionDataSaveCommand = Ioc.Resolve<ICommand>("Collision.DataSaveCommand", collisionName, dataGenerator.GenerateCollisionData());
        collisionDataSaveCommand.Execute();
    }
}
