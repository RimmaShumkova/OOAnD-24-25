using Moq;
using SpaceBattle.Lib;

namespace SpaceBattle.Tests;

public class UnitTest1
{
    [Fact]
    public void PosTestMove()
    {
        var m = new Mock<IMovable>();
        m.Setup(_m => _m.Position).Returns(new Vector(12, 5)).Verifiable();
        m.Setup(_m => _m.Veloсity).Returns(new Vector(-7, 3)).Verifiable();
        var c = new MoveCommand(m.Object);
        c.Execute();
        m.VerifySet(_m => _m.Position = new Vector(5, 8));
        m.VerifyAll();
    }
    [Fact]
    public void TestPositionRaisesException()
    {
        var m = new Mock<IMovable>();
        m.Setup(_m => _m.Position).Throws<NullReferenceException>();
        m.Setup(_m => _m.Veloсity).Returns(new Vector(-7, 3));
        var c = new MoveCommand(m.Object);
        var act = () => c.Execute();
        Assert.Throws<NullReferenceException>(act);
    }
    [Fact]
    public void TestSpeedRaisesException()
    {
        var m = new Mock<IMovable>();
        m.Setup(_m => _m.Position).Returns(new Vector(12, 5)).Verifiable();
        m.Setup(_m => _m.Veloсity).Throws<NullReferenceException>();
        var c = new MoveCommand(m.Object);
        var act = () => c.Execute();
        Assert.Throws<NullReferenceException>(act);
    }
    [Fact]
    public void TestAddRaisesException()
    {
        var m = new Mock<IMovable>();
        m.Setup(_m => _m.Position).Returns(new Vector(12, 5)).Verifiable();
        m.Setup(_m => _m.Veloсity).Throws<ArgumentException>();
        var c = new MoveCommand(m.Object);
        var act = () => c.Execute();
        Assert.Throws<ArgumentException>(act);
    }
}
