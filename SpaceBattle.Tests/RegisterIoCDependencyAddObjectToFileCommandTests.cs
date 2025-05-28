using App;
using App.Scopes;

namespace SpaceBattle.Lib.Tests
{
    public class RegisterIoCDependencyAddObjectToFileCommandTests
    {
        public RegisterIoCDependencyAddObjectToFileCommandTests()
        {
            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
        }

        [Fact]
        public void Execute_RegistersDependencyCorrectly()
        {
            new RegisterIoCDependencyAddObjectToFileCommand().Execute();

            var cmd = Ioc.Resolve<ICommand>("Commands.AddObjectToFile", "testPath", new object());

            Assert.NotNull(cmd);
            Assert.IsType<AddObjectToFileCommand>(cmd);
        }

        [Fact]
        public void Execute_RegisteredDependencyCanBeResolvedWithCorrectArguments()
        {
            new RegisterIoCDependencyAddObjectToFileCommand().Execute();
            var path = Path.GetTempFileName();
            var data = new List<int[]> { new[] { 1, 2 }, new[] { 3, 4 } };

            var cmd = Ioc.Resolve<ICommand>("Commands.AddObjectToFile", path, data);

            Assert.NotNull(cmd);
            Assert.IsType<AddObjectToFileCommand>(cmd);

            File.Delete(path);
        }

        [Fact]
        public void Execute_ThrowsWhenResolvingWithIncorrectArguments()
        {
            new RegisterIoCDependencyAddObjectToFileCommand().Execute();

            Assert.Throws<InvalidCastException>(() => Ioc.Resolve<ICommand>("Commands.AddObjectToFile", 123, "notAnObject"));
        }
    }
}
