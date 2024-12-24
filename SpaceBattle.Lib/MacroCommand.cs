namespace SpaceBattle.Lib;

public class MacroCommand : ICommand
{
    private readonly ICommand[] cmds;
    public MacroCommand(ICommand[] cmds)
    {
        this.cmds = cmds;
    }

    public void Execute()
    {
        var commandList = new List<ICommand>();
        commandList = cmds.ToList<ICommand>();
        commandList.ForEach(c => c.Execute());
    }
}
