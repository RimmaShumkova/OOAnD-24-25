using App;
using App.Scopes;

namespace SpaceBattle.Lib.Tests
{
    public class RegisterIoCDependencyGetCollisionNameTests
    {
        public RegisterIoCDependencyGetCollisionNameTests()
        {
            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
        }

        [Fact]
        public void Execute_RegistersGetCollisionNameDependency()
        {
            var regCommand = new RegisterIoCDependencyGetCollisionName();

            regCommand.Execute();

            var collName = Ioc.Resolve<string>("Collision.GetCollisionName", "Shape1", "Shape2");
            Assert.Equal("Shape1_Shape2", collName);
        }

        [Fact]
        public void Execute_RegistersDependencyCorrectlyWithDifferentArguments()
        {
            var regCommand = new RegisterIoCDependencyGetCollisionName();

            regCommand.Execute();

            var collName1 = Ioc.Resolve<string>("Collision.GetCollisionName", "Shape1", "Shape2");
            var collName2 = Ioc.Resolve<string>("Collision.GetCollisionName", "Shape11", "Shape22");

            Assert.Equal("Shape1_Shape2", collName1);
            Assert.Equal("Shape11_Shape22", collName2);
        }

        [Fact]
        public void Execute_RegistersDependencyWithCorrectArgumentHandling()
        {
            var regCommand = new RegisterIoCDependencyGetCollisionName();

            regCommand.Execute();

            var collName = Ioc.Resolve<string>("Collision.GetCollisionName", "Shape1", "Shape2");
            Assert.Equal("Shape1_Shape2", collName);

            Assert.Throws<InvalidCastException>(() =>
            {
                Ioc.Resolve<string>("Collision.GetCollisionName", 123, "Shape2");
            });
        }
    }
}
