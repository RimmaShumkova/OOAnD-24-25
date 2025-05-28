using App;

namespace SpaceBattle.Lib;

public class CollisionSaveDataCommand : ICommand
{
    private readonly IList<int[]> collisionData;
    private readonly string collisionName;

    public CollisionSaveDataCommand(string collisionName, IList<int[]> collisionData)
    {
        this.collisionData = collisionData;
        this.collisionName = collisionName;
    }
    public void Execute()
    {
        var collisionFilePath = Ioc.Resolve<string>("Data.CollisionFilesPath");
        Ioc.Resolve<ICommand>("Commands.AddObjectToFile", collisionFilePath + collisionName, collisionData).Execute();
        Ioc.Resolve<ICommand>("Collision.LoadDataToMemory", collisionName, collisionData).Execute();
    }
}
