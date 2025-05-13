using App;
using App.Scopes;
using Moq;

namespace SpaceBattle.Lib
{
    public class CheckingSquareCollisionCommandTests
    {
        [Fact]
        public void CheckingSquareCollisionCommand_Executes_CollisionCommands_For_EachNearbyObject()
        {
            new InitCommand().Execute();
            var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
            Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();

            var collObj = new Mock<IColliding>().Object;

            var neighborObj1 = new Mock<IColliding>().Object;
            var neighborObj2 = new Mock<IColliding>().Object;
            var neighborObj = new List<IColliding> { neighborObj1, neighborObj2 };

            var collCmd1 = new Mock<ICommand>();
            var collCmd2 = new Mock<ICommand>();

            var regObj = new List<IColliding> { neighborObj1, neighborObj2 };
            var regCommands = new List<Mock<ICommand>> { collCmd1, collCmd2 };

            Ioc.Resolve<App.ICommand>("IoC.Register", "Game.GetObjectsInSameSquare", (object[] args) =>
            {
                return regObj;
            }).Execute();

            Ioc.Resolve<App.ICommand>("IoC.Register", "Game.HandleCollision", (object[] args) =>
            {
                return new Mock<ICommand>().Object;
            }).Execute();

            Ioc.Resolve<App.ICommand>("IoC.Register", "Game.CollisionCommand", (object[] args) =>
            {
                var second = (IColliding)args[1];
                var index = regObj.IndexOf(second);
                return regCommands[index].Object;
            }).Execute();

            new CheckingSquareCollisionCommand(collObj).Execute();

            collCmd1.Verify(cmd => cmd.Execute(), Times.Once);
            collCmd2.Verify(cmd => cmd.Execute(), Times.Once);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenNullArgument()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new CheckingSquareCollisionCommand(null!));
        }
    }
}
