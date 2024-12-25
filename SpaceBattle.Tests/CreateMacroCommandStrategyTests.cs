using App;
using App.Scopes;
using Moq;
namespace SpaceBattle.Lib.Tests;

public class CreateMacroCommandStrategyTests
{
    public CreateMacroCommandStrategyTests()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void SuccessfulConstructionMacroCommand()
    {
        List<string> MacroTestDependencies = ["Commands.Test1", "Commands.Test2"];

        Ioc.Resolve<App.ICommand>("IoC.Register", "Specs.Macro.Test", (object[] args) => MacroTestDependencies).Execute();

        var mockTest1 = new Mock<ICommand>();
        var mockTest2 = new Mock<ICommand>();

        Ioc.Resolve<App.ICommand>("IoC.Register", "Commands.Test1", (object[] args) => mockTest1.Object).Execute();
        Ioc.Resolve<App.ICommand>("IoC.Register", "Commands.Test2", (object[] args) => mockTest2.Object).Execute();

        var registerMc = new RegisterIoCDependencyMacroCommand();
        registerMc.Execute();

        var CreateMacro = new CreateMacroCommandStrategy("Macro.Test");

        CreateMacro.Resolve(new object[0]).Execute();
        mockTest1.Verify(x => x.Execute());
        mockTest2.Verify(x => x.Execute());

    }
    [Fact]
    public void ResolveThrowsExceptionWhenDependenciesMissing()
    {
        var CreateMacro = new CreateMacroCommandStrategy("Macro.Missing");

        Assert.Throws<Exception>(() => CreateMacro.Resolve(new object[0]));
    }
    [Fact]
    public void ResolveThrowsExceptionWhenCommandNotFound()
    {
        var MacroTestDependencies = new List<string> { "Commands.Test1", "Commands.Missing" };

        Ioc.Resolve<App.ICommand>("IoC.Register", "Specs.Macro.Test", (object[] args) => MacroTestDependencies).Execute();

        var mockTest1 = new Mock<ICommand>();

        Ioc.Resolve<App.ICommand>("IoC.Register", "Commands.Test1", (object[] args) => mockTest1.Object).Execute();

        var registerMc = new RegisterIoCDependencyMacroCommand();
        registerMc.Execute();

        var CreateMacro = new CreateMacroCommandStrategy("Macro.Test");

        Assert.Throws<Exception>(() => CreateMacro.Resolve(new object[0]));
    }
    [Fact]
    public void ResolveHandlesEmptyCommandSpec()
    {
        Ioc.Resolve<App.ICommand>("IoC.Register", "Specs.Macro.Empty", (object[] args) => new List<string>()).Execute();

        var CreateMacro = new CreateMacroCommandStrategy("Macro.Empty");

        var macroCommand = CreateMacro.Resolve(new object[0]);

        Assert.NotNull(macroCommand);
        macroCommand.Execute();
    }
    [Fact]
    public void MacroCommandExecutesAllCommands()
    {
        var MacroTestDependencies = new List<string> { "Commands.Test1", "Commands.Test2" };

        Ioc.Resolve<App.ICommand>("IoC.Register", "Specs.Macro.Test", (object[] args) => MacroTestDependencies).Execute();

        var mockTest1 = new Mock<ICommand>();
        var mockTest2 = new Mock<ICommand>();

        mockTest1.Setup(x => x.Execute());
        mockTest2.Setup(x => x.Execute());

        Ioc.Resolve<App.ICommand>("IoC.Register", "Commands.Test1", (object[] args) => mockTest1.Object).Execute();
        Ioc.Resolve<App.ICommand>("IoC.Register", "Commands.Test2", (object[] args) => mockTest2.Object).Execute();

        var CreateMacro = new CreateMacroCommandStrategy("Macro.Test");

        var macroCommand = CreateMacro.Resolve(new object[0]);
        macroCommand.Execute();

        mockTest1.Verify(x => x.Execute(), Times.Once);
        mockTest2.Verify(x => x.Execute(), Times.Once);
    }

    [Fact]
    public void ResolvePassesArgsToCommands()
    {
        var MacroTestDependencies = new List<string> { "Commands.Test1" };

        Ioc.Resolve<App.ICommand>("IoC.Register", "Specs.Macro.Test", (object[] args) => MacroTestDependencies).Execute();

        var mockTest1 = new Mock<ICommand>();
        Ioc.Resolve<App.ICommand>("IoC.Register", "Commands.Test1", (object[] args) =>
        {
            Assert.Equal("arg1", args[0]);
            return mockTest1.Object;
        }).Execute();

        var CreateMacro = new CreateMacroCommandStrategy("Macro.Test");

        var macroCommand = CreateMacro.Resolve(new object[] { "arg1" });
        macroCommand.Execute();

        mockTest1.Verify(x => x.Execute(), Times.Once);
    }
}
