using App;
using App.Scopes;
using Moq;
namespace SpaceBattle.Lib.Tests;

public class RegisterIoCDependencyMacroMoveRotateTests
{
    [Fact]
    public void ConstructionTest()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();

        var registerMc = new RegisterIoCDependencyMacroCommand();
        registerMc.Execute();

        List<string> TestDependencies = ["Commands.Test1"];
        var mockTest1 = new Mock<ICommand>();
        Ioc.Resolve<App.ICommand>("IoC.Register", "Commands.Test1", (object[] _) => mockTest1.Object).Execute();

        Ioc.Resolve<App.ICommand>("IoC.Register", "Specs.Move", (object[] _) => TestDependencies).Execute();
        Ioc.Resolve<App.ICommand>("IoC.Register", "Specs.Rotate", (object[] _) => TestDependencies).Execute();

        var registerMoveRotate = new RegisterIoCDependencyMacroMoveRotate();
        registerMoveRotate.Execute();

        Assert.IsType<MacroCommand>(Ioc.Resolve<MacroCommand>("Macro.Move"));
        Assert.IsType<MacroCommand>(Ioc.Resolve<MacroCommand>("Macro.Rotate"));

    }
}
