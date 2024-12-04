using Moq;
using SpaceBattle.Lib;

namespace SpaceBattle.Tests;

public class RotateCommandTest
{
    [Fact]
    public void TestPostitive()
    {
        var rotating = new Mock<IRotatable>();
        rotating.SetupGet(r => r.angle).Returns(new Angle(45, 1));
        rotating.SetupGet(r => r.angleVelocity).Returns(new Angle(90, 1));

        var cmd = new RotateCommand(rotating.Object);
        cmd.Execute();

        rotating.VerifySet(r => r.angle = new Angle(135, 1));
    }

    [Fact]
    public void TestAngleRaisesException()
    {
        var rotating = new Mock<IRotatable>();
        rotating.SetupGet(r => r.angle).Throws<Exception>();
        rotating.SetupGet(r => r.angleVelocity).Returns(new Angle(90, 1));

        var cmd = new RotateCommand(rotating.Object);

        Assert.Throws<Exception>(
            () => cmd.Execute()
        );
    }

    [Fact]
    public void TestAngleVelocityRaisesException()
    {
        var rotating = new Mock<IRotatable>();
        rotating.SetupGet(r => r.angle).Returns(new Angle(45, 1));
        rotating.SetupGet(r => r.angleVelocity).Throws<Exception>();

        var cmd = new RotateCommand(rotating.Object);

        Assert.Throws<Exception>(
            () => cmd.Execute()
        );
    }

    [Fact]
    public void TestInclineRaisesException()
    {
        var rotating = new Mock<IRotatable>();
        rotating.SetupProperty(r => r.angle, new Angle(45, 1));
        rotating.SetupGet(r => r.angleVelocity).Returns(new Angle(90, 1));
        rotating.SetupGet(r => r.angle).Throws<ArithmeticException>();

        var cmd = new RotateCommand(rotating.Object);

        Assert.Throws<ArithmeticException>(
            () => cmd.Execute()
        );
    }

    [Fact]
    public void TestPostitiveCreateAngle()
    {
        Assert.IsType<Angle>(new Angle(60, 1));
    }

    [Fact]
    public void TestNegativeCreateAngle()
    {
        Assert.Throws<DivideByZeroException>(() => new Angle(45, 0));
    }
}
