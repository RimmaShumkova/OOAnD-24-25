using App;

namespace SpaceBattle.Lib;

public class StopActionCommand : ICommand
{
    private readonly string action;
    private readonly string key;
    public StopActionCommand(IDictionary<string, object> order)
    {
        action = (string)order["Action"];
        key = (string)order["Key"];
    }

    public void Execute()
    {
        var obj = Ioc.Resolve<IDictionary<string, object>>("Game.Object", key);
        var inj = (ICommandInjectable)obj[action];
        inj.Inject(new EmptyCommand());
    }
}
