using Moq;
using SpaceBattle.Lib;

namespace SpaceBattle.Tests;

public class MoveCommandTest
{
    [Fact]
    public void TestPostitive()
    {
        var moving = new Mock<IMoving>();

        moving.SetupGet(m => m.Position).Returns(new int[] { 12, 5 });
        moving.SetupGet(m => m.Velocity).Returns(new int[] { -7, 3 });

        var cmd = new MoveCommand(moving.Object);
        cmd.Execute();

        moving.VerifySet(m => m.Position = new int[] { 5, 8 });
    }

    [Fact]
    public void TestPositionRaisesException()
    {
        var moving = new Mock<IMoving>();
        moving.SetupGet(m => m.Position).Throws<Exception>();
        moving.SetupGet(m => m.Velocity).Returns(new int[] { 1, 1 });

        var cmd = new MoveCommand(moving.Object);

        Assert.Throws<Exception>(
            () => cmd.Execute()
        );

    }
}
