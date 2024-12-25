﻿using App;
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
}
