namespace SpaceBattle.Lib;

public interface ICommandReceiver
{
    public void Receive(ICommand cmd);
}
