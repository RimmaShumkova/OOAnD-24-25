using App;

namespace SpaceBattle.Lib;

public class IsCollidingTreeStrategy
{
    public object Resolve(object[] args)
    {
        var deltaPosition = (int[])args[0];
        var deltaVector = (int[])args[1];

        var shape1 = (string)args[2];
        var shape2 = (string)args[3];

        var attributeList = new List<int>();

        deltaPosition.ToList().ForEach(x => attributeList.Add(x));
        deltaVector.ToList().ForEach(x => attributeList.Add(x));

        try
        {
            Ioc.Resolve<CheckTreeResultCommand>($"Game.CollisionTree.{shape1}_{shape2}.Check", attributeList).Execute();
        }
        catch
        {
            return false;
        }

        return true;
    }
}
