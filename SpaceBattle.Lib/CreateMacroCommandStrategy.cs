using App;
namespace SpaceBattle.Lib;

public class CreateMacroCommandStrategy(string commandSpec)
{
    public ICommand Resolve(object[] args)
    {
        var commands = Ioc.Resolve<IEnumerable<string>>("Specs." + commandSpec) ??
                       throw new Exception("Dependencies not found");

        var add_commands = commands.Select(p =>
            Ioc.Resolve<ICommand>(p, args) ?? throw new Exception($"Command {p} not found"))
            .ToArray<ICommand>();

        return Ioc.Resolve<ICommand>("Commands.Macro", add_commands);
    }
}
