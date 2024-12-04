using Moq;
using SpaceBattle.Lib;

namespace SpaceBattle.Tests;

public class MovingTests
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
    public void TestPositionIsNotWritable()
    {
        var m = new Mock<IMovable>();
        m.Setup(_m => _m.Position).Returns(new Vector(12, 5));
        m.Setup(_m => _m.Veloсity).Returns(new Vector(-7, 3));
        m.SetupSet(_m => _m.Position = It.IsAny<Vector>()).Throws<InvalidOperationException>();

        var c = new MoveCommand(m.Object);

        Assert.Throws<InvalidOperationException>(() => c.Execute());
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
public class VectorTests
{
    [Fact]
    public void VectorIsInitializableTest()
    {
        int[] elements = { 1, 2, 3 };
        var vector = new Vector(elements);
        Assert.True(elements.SequenceEqual(vector.GetElements()));
    }

    [Fact]
    public void VectorEqualsReturnsTrueForIdenticalVectorsTest()
    {
        var v1 = new Vector(1, 2, 3);
        var v2 = new Vector(1, 2, 3);
        Assert.True(v1.Equals(v2));
    }

    [Fact]
    public void VectorEqualsReturnsFalseForDifferentVectorsTest()
    {
        var v1 = new Vector(1, 2, 3);
        var v2 = new Vector(4, 5, 6);
        Assert.False(v1.Equals(v2));
    }

    [Fact]
    public void VectorEqualsReturnsFalseForNullTest()
    {
        var v1 = new Vector(1, 2, 3);
        Assert.False(v1.Equals(null));
    }

    [Fact]
    public void VectorEqualsReturnsFalseForDifferentTypeTest()
    {
        var v1 = new Vector(1, 2, 3);
        var obj = new object();
        Assert.False(v1.Equals(obj));
    }

    [Fact]
    public void VectorOperatorPlusAddsVectorsCorrectlyTest()
    {
        var v1 = new Vector(1, 2, 3);
        var v2 = new Vector(4, 5, 6);
        var result = v1 + v2;
        var expected = new Vector(5, 7, 9);
        Assert.True(result.Equals(expected));
    }

    [Fact]
    public void VectorOperatorPlusThrowsForDifferentLengthsTest()
    {
        var v1 = new Vector(1, 2);
        var v2 = new Vector(3, 4, 5);
        Assert.Throws<InvalidOperationException>(() => _ = v1 + v2);
    }

    [Fact]
    public void VectorOperatorEqualsReturnsTrueForSameReferenceTest()
    {
        var v1 = new Vector(1, 2, 3);
        var v2 = v1;
        Assert.True(v1 == v2);
    }

    [Fact]
    public void VectorOperatorNotEqualsReturnsTrueForDifferentVectorsTest()
    {
        var v1 = new Vector(1, 2, 3);
        var v2 = new Vector(4, 5, 6);
        Assert.True(v1 != v2);
    }

    [Fact]
    public void VectorGetHashCodeReturnsSameForIdenticalVectorsTest()
    {
        var v1 = new Vector(1, 2, 3);
        var v2 = new Vector(1, 2, 3);
        var hash1 = v1.GetHashCode();
        var hash2 = v2.GetHashCode();
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void VectorGetHashCodeReturnsDifferentForDifferentVectorsTest()
    {
        var v1 = new Vector(1, 2, 3);
        var v2 = new Vector(4, 5, 6);
        var hash1 = v1.GetHashCode();
        var hash2 = v2.GetHashCode();
        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void ToStringReturnsCorrectStringRepresentation()
    {
        var vector = new Vector(1, 2, 3);
        var result = vector.ToString();
        Assert.Equal("Vector (1, 2, 3)", result);
    }

    [Fact]
    public void IndexerReturnsCorrectElement()
    {
        var vector = new Vector(1, 2, 3);
        Assert.Equal(2, vector[1]);
    }

    [Fact]
    public void ConstructorThrowsForEmptyVector()
    {
        Assert.Throws<ArgumentException>(() => new Vector());
    }
    [Fact]
    public void VectorEqualsReturnsFalseForNullVector()
    {
        var v1 = new Vector(1, 2, 3);
        Vector? v2 = null;
        Assert.False(v1.Equals(v2));
    }
}
