using App;
using App.Scopes;
using Moq;

namespace SpaceBattle.Lib.Tests
{
    public class RegisterIoCDependencyCollisionPrepareDataTests
    {
        public RegisterIoCDependencyCollisionPrepareDataTests()
        {
            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
        }

        [Fact]
        public void Execute_RegistersCollisionPrepareDataDependency()
        {
            var mockDataGenerator = new Mock<ICollisionDataGenerator>();

            var regCommand = new RegisterIoCDependencyCollisionPrepareData();

            regCommand.Execute();

            var collPrepareDataCommand = Ioc.Resolve<ICommand>("Collision.PrepareData", mockDataGenerator.Object);
            Assert.NotNull(collPrepareDataCommand);
            Assert.IsType<CollisionPrepareDataCommand>(collPrepareDataCommand);
        }

        [Fact]
        public void Execute_RegistersDependencyCorrectlyWithDifferentArguments()
        {
            var mockDataGenerator1 = new Mock<ICollisionDataGenerator>();
            var mockDataGenerator2 = new Mock<ICollisionDataGenerator>();
            var regCommand = new RegisterIoCDependencyCollisionPrepareData();

            regCommand.Execute();

            var collPrepareDataCommand1 = Ioc.Resolve<ICommand>("Collision.PrepareData", mockDataGenerator1.Object);
            var collPrepareDataCommand2 = Ioc.Resolve<ICommand>("Collision.PrepareData", mockDataGenerator2.Object);

            Assert.NotNull(collPrepareDataCommand1);
            Assert.NotNull(collPrepareDataCommand2);
            Assert.IsType<CollisionPrepareDataCommand>(collPrepareDataCommand1);
            Assert.IsType<CollisionPrepareDataCommand>(collPrepareDataCommand2);
        }
    }
}
