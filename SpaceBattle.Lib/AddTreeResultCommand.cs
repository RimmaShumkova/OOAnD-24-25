namespace SpaceBattle.Lib;

public class AddTreeResultCommand : ICommand
{
    private Dictionary<int, object> tree;
    private readonly List<int> attributeList;
    public AddTreeResultCommand(Dictionary<int, object> tree, List<int> attributeList)
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
                tree.Add(attribute, new Dictionary<int, object>());
            }
            tree = (Dictionary<int, object>)tree[attribute];
        });
    }
}
