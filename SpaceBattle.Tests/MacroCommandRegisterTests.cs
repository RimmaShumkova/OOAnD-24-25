using App;
using App.Scopes;
using Moq;

namespace SpaceBattle.Lib
{
    public class MacroCommandRegisterTests
    {
        [Fact]
        public void TestExecute()
        {
            new InitCommand().Execute();

            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();

            var cmd1 = new Mock<ICommand>();
            var cmd2 = new Mock<ICommand>();
            var cmd3 = new Mock<ICommand>();

            var registerMacroCommand = new RegisterIoCDependencyMacroCommand();
            registerMacroCommand.Execute();

            ICommand[] arr = { cmd1.Object, cmd2.Object, cmd3.Object };
            var macroCommand = Ioc.Resolve<ICommand>("Commands.Macro", arr);

            Assert.IsType<MacroCommand>(macroCommand);
        }
    }
}
