namespace SpaceBattle.Lib;

public interface IMoving
{
    int[] Position { get; set; }
    int[] Velocity { get; }
}
