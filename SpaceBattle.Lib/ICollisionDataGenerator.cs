namespace SpaceBattle.Lib;

public interface ICollisionDataGenerator
{
    string firstShape { get; }
    string secondShape { get; }

    IList<int[]> GenerateCollisionData();
}
