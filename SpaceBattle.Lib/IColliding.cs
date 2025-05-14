namespace SpaceBattle.Lib;

public interface IColliding
{
    Vector Position { get; set; }
    Vector Velocity { get; }
    string Shape { get; }
}
