using App;
using App.Scopes;
using Moq;
using SpaceBattle.Lib;

namespace SpaceBattle.Tests
{
    public class RegisterIoCDependencyGameCommandTests
    {
        public RegisterIoCDependencyGameCommandTests()
        {
            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
        }

        [Fact]
        public void CorrectResolveGameCreateTest()
        {
            var mock_queue = new Mock<SpaceBattle.Lib.IQueue>();
            mock_queue.Setup(q => q.Get()).Returns(new Mock<SpaceBattle.Lib.ICommand>().Object);
            mock_queue.Setup(q => q.Count()).Returns(3);
            var registerIoCDependencyGameCommand = new RegisterIoCDependencyGameCommand();

            registerIoCDependencyGameCommand.Execute();

            var resolvedCommand = Ioc.Resolve<SpaceBattle.Lib.ICommand>("Commands.Game", mock_queue.Object);
            Assert.IsType<GameCommand>(resolvedCommand);
        }
    }
}
