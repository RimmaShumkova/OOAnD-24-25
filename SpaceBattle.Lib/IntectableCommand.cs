namespace SpaceBattle.Lib;

public class InjectableCommand : ICommand, ICommandInjectable
{
    private ICommand? _cmd;
    public void Execute()
    {
        try
        {
            _cmd.Execute();
        }
        catch (NullReferenceException)
        {
            throw new Exception("No command");
        }
    }
    public void Inject(ICommand cmd)
    {
        _cmd = cmd;
    }
}
