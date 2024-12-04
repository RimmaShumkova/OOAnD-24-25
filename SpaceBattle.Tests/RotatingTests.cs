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

    [Fact]
    public void TestChangeSigns()
    {
        var angle = new Angle(1, -1);
        Assert.Equal(-1, angle._numerator);
        Assert.Equal(1, angle._denominator);

        angle = new Angle(-1, -1);
        Assert.Equal(1, angle._numerator);
        Assert.Equal(1, angle._denominator);
    }

    [Fact]
    public void TestGetHashCode()
    {
        var angle1 = new Angle(1, 2);
        var angle2 = new Angle(1, 2);
        Assert.Equal(angle1.GetHashCode(), angle2.GetHashCode());
    }

    [Fact]
    public void TestPostitiveAngleEq()
    {
        var a = new Angle(45, -1);
        var b = new Angle(-135, 3);
        Assert.True(a == b);
    }

    [Fact]
    public void TestNegativeAngleEq()
    {
        var a = new Angle(45, -1);
        var b = new Angle(-135, 45);
        Assert.False(a == b);
    }

    [Fact]
    public void TestAngleEqMethod()
    {
        var a = new Angle(-30, -1);
        var b = 1;
        Assert.False(a.Equals(b));
    }

    [Fact]
    public void TestAngleNotEq()
    {
        var a = new Angle(45, -1);
        var b = new Angle(-135, 45);
        Assert.True(a != b);
    }

    [Fact]
    public void TestAngleAdd()
    {
        var a = new Angle(45, 1);
        var b = new Angle(90, 1);
        Assert.Equal(new Angle(135, 1), a + b);
    }
}
