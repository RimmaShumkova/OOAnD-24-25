namespace SpaceBattle.Lib;

public class Angle
{
    public int _numerator;
    public int _denominator;

    private static int GCD(int n, int m)
    {
        return Math.Abs(m) == 0 ? Math.Abs(n) : GCD(Math.Abs(m), Math.Abs(n) % Math.Abs(m)); //Euclidean algorithm
    }

    public Angle(int numerator, int denominator)
    {
        if (denominator == 0)
        {
            throw new DivideByZeroException();
        }

        if (numerator >= 0 && denominator < 0 || numerator <= 0 && denominator < 0)
        {
            numerator *= -1;
            denominator *= -1;
        }

        _numerator = numerator / GCD(numerator, denominator);
        _denominator = denominator / GCD(numerator, denominator);
    }

    public static Angle operator +(Angle a, Angle b)
    {
        var numerator = a._numerator * b._denominator + b._numerator * a._denominator;
        var denominator = a._denominator * b._denominator;
        var gcd = GCD(numerator, denominator);
        return new Angle(numerator / gcd, denominator / gcd);
    }

    public override bool Equals(object? obj)
    {
        return obj is Angle angle &&
               _numerator == angle._numerator &&
               _denominator == angle._denominator;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_numerator, _denominator);
    }
}
