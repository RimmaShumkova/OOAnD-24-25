namespace SpaceBattle.Lib;

public interface IRotatable
{
    Angle angle { get; set; }
    Angle angleVelocity { get; }
}
