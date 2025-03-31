using App;
using App.Scopes;
using Moq;
namespace SpaceBattle.Lib;

public class ShootingTests
{
    public ShootingTests()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void ShootCommandExecutesSuccessfully()
    {
        var shooterMock = new Mock<IShootable>();
        shooterMock.Setup(s => s.StartPosition).Returns(new Vector(10, 20));
        shooterMock.Setup(s => s.Velocity).Returns(new Vector(1, 0));

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

        moveCommandMock.Verify(c => c.Execute(), Times.Once);
    }

    [Fact]
    public void ShootCommandThrowsExceptionIfProjectileCreateNotRegistered()
    {
        var shooterMock = new Mock<IShootable>();
        shooterMock.Setup(s => s.StartPosition).Returns(new Vector(10, 20));
        shooterMock.Setup(s => s.Velocity).Returns(new Vector(1, 0));

        var shootCommand = new ShootCommand(shooterMock.Object);

        Assert.Throws<Exception>(() => shootCommand.Execute());
    }

    [Fact]
    public void ShootCommandThrowsExceptionIfMoveCommandNotRegistered()
    {
        var shooterMock = new Mock<IShootable>();
        shooterMock.Setup(s => s.StartPosition).Returns(new Vector(10, 20));
        shooterMock.Setup(s => s.Velocity).Returns(new Vector(1, 0));

        Ioc.Resolve<ICommand>("IoC.Register", "Game.Projectile.Create",
            (object[] args) => new Dictionary<string, object>()).Execute();

        var shootCommand = new ShootCommand(shooterMock.Object);

        Assert.Throws<Exception>(() => shootCommand.Execute());
    }

    [Fact]
    public void ShootCommandThrowsExceptionIfShooterPositionIsInvalid()
    {
        var shooterMock = new Mock<IShootable>();
        shooterMock.Setup(s => s.StartPosition).Throws<NullReferenceException>();
        shooterMock.Setup(s => s.Velocity).Returns(new Vector(1, 0));

        var shootCommand = new ShootCommand(shooterMock.Object);

        Assert.Throws<NullReferenceException>(() => shootCommand.Execute());
    }

    [Fact]
    public void ShootCommandThrowsExceptionIfShooterVelocityIsInvalid()
    {
        var shooterMock = new Mock<IShootable>();
        shooterMock.Setup(s => s.StartPosition).Returns(new Vector(10, 20));
        shooterMock.Setup(s => s.Velocity).Throws<InvalidOperationException>();

        var shootCommand = new ShootCommand(shooterMock.Object);

        Assert.Throws<InvalidOperationException>(() => shootCommand.Execute());
    }
}
