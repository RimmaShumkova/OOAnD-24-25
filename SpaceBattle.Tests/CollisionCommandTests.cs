using App;
using App.Scopes;
using Moq;

namespace SpaceBattle.Lib.Tests
{
    public class CollisionCommandTests
    {
        public CollisionCommandTests()
        {
            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
        }

        [Fact(DisplayName = "При обнаружении коллизии команда должна выполниться")]
        public void Execute_CollisionDetected_CommandExecuted()
        {
            var position1 = new Vector(new[] { 10, 20 });
            var position2 = new Vector(new[] { 12, 18 });
            var velocity1 = new Vector(new[] { 1, -1 });
            var velocity2 = new Vector(new[] { -1, 1 });

            var mockFirstObject = CreateMockCollidingObject(position1, velocity1);
            var mockSecondObject = CreateMockCollidingObject(position2, velocity2);
            var mockCommand = new Mock<ICommand>();

            SetupIoCForCollisionTest(
                inSameSquare: true,
                hasCollision: true
            );

            var collisionCommand = CreateCollisionCommand(
                mockFirstObject,
                mockSecondObject,
                mockCommand.Object
            );

            collisionCommand.Execute();

            mockCommand.Verify(c => c.Execute(), Times.Once);
        }

        [Fact(DisplayName = "При отсутствии коллизии команда не должна выполняться")]
        public void Execute_NoCollisionDetected_CommandNotExecuted()
        {
            var position1 = new Vector(new[] { 50, 75 });
            var position2 = new Vector(new[] { 52, 73 });
            var velocity1 = new Vector(new[] { 2, -2 });
            var velocity2 = new Vector(new[] { -2, 2 });

            var mockFirstObject = CreateMockCollidingObject(position1, velocity1);
            var mockSecondObject = CreateMockCollidingObject(position2, velocity2);
            var mockCommand = new Mock<ICommand>();

            SetupIoCForCollisionTest(
                inSameSquare: true,
                hasCollision: false
            );

            var collisionCommand = CreateCollisionCommand(
                mockFirstObject,
                mockSecondObject,
                mockCommand.Object
            );

            collisionCommand.Execute();

            mockCommand.Verify(c => c.Execute(), Times.Never);
        }

        private Mock<IColliding> CreateMockCollidingObject(Vector position, Vector velocity)
        {
            var mock = new Mock<IColliding>();
            mock.SetupGet(o => o.Position).Returns(position);
            mock.SetupGet(o => o.Velocity).Returns(velocity);
            mock.SetupGet(o => o.Shape).Returns("Circle");
            return mock;
        }

        private void SetupIoCForCollisionTest(bool inSameSquare, bool hasCollision)
        {
            new RegisterIoCDependencyVectorDelta().Execute();

            Ioc.Resolve<ICommand>(
                "IoC.Register",
                "Game.IsObjectsInOneSquare",
                (object[] args) => (object)inSameSquare
            ).Execute();

            Ioc.Resolve<ICommand>(
                "IoC.Register",
                "Game.IsCollision",
                (object[] args) => (object)hasCollision
            ).Execute();

            new RegisterIoCDependencyCollisionCommand().Execute();
        }

        private ICommand CreateCollisionCommand(
            Mock<IColliding> obj1,
            Mock<IColliding> obj2,
            ICommand handlerCommand)
        {
            return Ioc.Resolve<ICommand>(
                "Game.CollisionCommand",
                obj1.Object,
                obj2.Object,
                handlerCommand
            );
        }
    }
}
