using App;
using App.Scopes;
using Moq;

namespace SpaceBattle.Lib
{
    public class ShootCommandTests
    {
        public ShootCommandTests()
        {
            new InitCommand().Execute();
            var scope = Ioc.Resolve<object>("Scopes.New", Ioc.Resolve<object>("Scopes.Root"));
            Ioc.Resolve<ICommand>("Scopes.Current.Set", scope).Execute();
        }

        [Fact]
        public void TestCreatesProjectileAndMovesIt()
        {
            var shooterMock = new Mock<IShootable>();
            shooterMock.Setup(s => s.StartPosition).Returns(new Vector(0, 0));
            shooterMock.Setup(s => s.Velocity).Returns(new Vector(1, 1));

            var projectileProperties = new Dictionary<string, object>();
            var moveCommandMock = new Mock<ICommand>();

            Ioc.Resolve<ICommand>("IoC.Register", "Game.Projectile.Create",
                (object[] args) => projectileProperties).Execute();

            Ioc.Resolve<ICommand>("IoC.Register", "Commands.Move",
                (object[] args) => moveCommandMock.Object).Execute();

            var shootCommand = new ShootCommand(shooterMock.Object);

            shootCommand.Execute();

            Assert.Equal(shooterMock.Object.StartPosition, projectileProperties["Position"]);
            Assert.Equal(shooterMock.Object.Velocity, projectileProperties["Velocity"]);
            moveCommandMock.Verify(cmd => cmd.Execute(), Times.Once);
        }

        [Fact]
        public void TestThrowsWhenProjectileCreateNotRegistered()
        {
            var shooterMock = new Mock<IShootable>();
            var shootCommand = new ShootCommand(shooterMock.Object);

            Assert.Throws<ArgumentException>(() => shootCommand.Execute());
        }

        [Fact]
        public void TestThrowsWhenMoveCommandNotRegistered()
        {
            var shooterMock = new Mock<IShootable>();
            Ioc.Resolve<ICommand>("IoC.Register", "Game.Projectile.Create",
                (object[] args) => new Dictionary<string, object>()).Execute();

            var shootCommand = new ShootCommand(shooterMock.Object);

            Assert.Throws<ArgumentException>(() => shootCommand.Execute());
        }

        [Fact]
        public void TestUsesCorrectShooterProperties()
        {
            var expectedPosition = new Vector(5, 10);
            var expectedVelocity = new Vector(2, 3);

            var shooterMock = new Mock<IShootable>();
            shooterMock.Setup(s => s.StartPosition).Returns(expectedPosition);
            shooterMock.Setup(s => s.Velocity).Returns(expectedVelocity);

            var projectileProperties = new Dictionary<string, object>();
            var moveCommandMock = new Mock<ICommand>();

            Ioc.Resolve<ICommand>("IoC.Register", "Game.Projectile.Create",
                (object[] args) => projectileProperties).Execute();

            Ioc.Resolve<ICommand>("IoC.Register", "Commands.Move",
                (object[] args) => moveCommandMock.Object).Execute();

            var shootCommand = new ShootCommand(shooterMock.Object);

            shootCommand.Execute();

            Assert.Equal(expectedPosition, projectileProperties["Position"]);
            Assert.Equal(expectedVelocity, projectileProperties["Velocity"]);
        }
    }
}
