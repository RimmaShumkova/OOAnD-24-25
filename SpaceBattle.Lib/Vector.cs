namespace SpaceBattle.Lib;

public class Vector
{
    private readonly int[] _elements;
    public int Size => _elements.Length;

    public Vector(params int[] elements)
    {
        if (elements.Length == 0)
        {
            throw new ArgumentException("Vector must contain at least one element!");
        }

        _elements = elements.ToArray();
    }

    public int[] GetElements()
    {
        return _elements.ToArray();
    }

    public override string ToString()
    {
        return $"Vector ({string.Join(", ", _elements)})";
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Vector other)
        {
            return false;
        }

        return _elements.SequenceEqual(other._elements);
    }

    public override int GetHashCode()
    {
        return _elements.Aggregate(0, (acc, x) => acc ^ x.GetHashCode());
    }

    public int this[int index] => _elements[index];

    public static Vector operator +(Vector x, Vector y)
    {
        if (x.Size != y.Size)
        {
            throw new ArgumentException("Vectors must have the same size!");
        }

        return new Vector(x._elements.Zip(y._elements, (a, b) => a + b).ToArray());
    }

    public static bool operator ==(Vector x, Vector y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (x is null || y is null)
        {
            return false;
        }

        return x._elements.SequenceEqual(y._elements);
    }

    public static bool operator !=(Vector x, Vector y)
    {
        return !(x == y);
    }
}
