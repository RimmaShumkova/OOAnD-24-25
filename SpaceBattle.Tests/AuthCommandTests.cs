using App;
using App.Scopes;

namespace SpaceBattle.Lib.Tests
{
    public class AuthCommandTests
    {
        public AuthCommandTests()
        {
            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
        }

        [Fact]
        public void AuthCommand_Successfully_Authorizes()
        {
            var subjectId = "player1";
            var action = "Move";
            var objectId = "ship1";

            Ioc.Resolve<App.ICommand>("IoC.Register",
                "Authorization.Check",
                new Func<object[], object>((object[] args) => (object)true)).Execute();

            var authCommand = new AuthCommand(subjectId, action, objectId);

            authCommand.Execute();
        }

        [Fact]
        public void AuthCommand_Throws_When_Unauthorized()
        {
            var subjectId = "player1";
            var action = "Move";
            var objectId = "ship1";

            Ioc.Resolve<App.ICommand>("IoC.Register",
                "Authorization.Check",
                new Func<object[], object>((object[] args) => (object)false)).Execute();

            var authCommand = new AuthCommand(subjectId, action, objectId);

            var exception = Assert.Throws<UnauthorizedAccessException>(() => authCommand.Execute());
            Assert.Equal("Игрок не имеет прав совершать действие над этим обьектом", exception.Message);
        }

        [Fact]
        public void AuthCommand_Passes_Correct_Parameters_To_AuthCheck()
        {
            var subjectId = "player1";
            var action = "Move";
            var objectId = "ship1";

            string? capturedSubjectId = null;
            string? capturedAction = null;
            string? capturedObjectId = null;

            Ioc.Resolve<App.ICommand>("IoC.Register",
                "Authorization.Check",
                new Func<object[], object>((object[] args) =>
                {
                    capturedSubjectId = (string)args[0];
                    capturedAction = (string)args[1];
                    capturedObjectId = (string)args[2];
                    return (object)true;
                })).Execute();

            var authCommand = new AuthCommand(subjectId, action, objectId);

            authCommand.Execute();

            Assert.Equal(subjectId, capturedSubjectId);
            Assert.Equal(action, capturedAction);
            Assert.Equal(objectId, capturedObjectId);
        }
    }
}
