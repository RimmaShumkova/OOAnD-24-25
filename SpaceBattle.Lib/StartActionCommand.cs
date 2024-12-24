using App;

namespace SpaceBattle.Lib;

public class StartActionCommand : ICommand
{
    private readonly string _action;
    private readonly object[] _args;
    private readonly string _key;
    public StartActionCommand(IDictionary<string, object> order)
    {
        _action = (string)order["Action"];
        _args = (object[])order["Args"];
        _key = (string)order["Key"];
    }

    public void Execute()
    { 
        var mc = Ioc.Resolve<ICommand>("Macro." + _action, _args);
        var inj = Ioc.Resolve<ICommand>("Commands.CommandInjectable");
        var q = Ioc.Resolve<Queue<ICommand>>("Game.Queue");
        var repeatableMacro = Ioc.Resolve<ICommand>("Commands.Macro", [mc, inj]);
        var repeatcmd = Ioc.Resolve<ICommand>("Commands.Send", q, repeatableMacro);
        var inj_injectable = (ICommandInjectable)inj;
        inj_injectable.Inject(repeatcmd);
        var sendcmd = Ioc.Resolve<ICommand>("Commands.Send", q, repeatcmd);
        sendcmd.Execute();
        var obj = Ioc.Resolve<IDictionary<string, object>>("Game.Object", _key);
        obj.Add(_action, inj);
    }
}
