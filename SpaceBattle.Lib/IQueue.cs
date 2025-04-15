namespace SpaceBattle.Lib;

public interface IQueue
{
    ICommand Get();
    int Count();
}