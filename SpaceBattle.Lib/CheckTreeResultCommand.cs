namespace SpaceBattle.Lib;

public class CheckTreeResultCommand : ICommand
{
    private Dictionary<int, object> tree;
    private readonly List<int> attributeList;
    public CheckTreeResultCommand(Dictionary<int, object> tree, List<int> attributeList)
    {
        this.tree = tree;
        this.attributeList = attributeList;
    }
    public void Execute()
    {
        attributeList.ForEach(attribute =>
        {
            if (!tree.ContainsKey(attribute))
            {
                throw new Exception();
            }

            tree = (Dictionary<int, object>)tree[attribute];
        });
    }
}
