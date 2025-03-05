using Moq;
namespace SpaceBattle.Lib;

public class SendCommandTests
{
    [Fact]
    public void HasReceivedTest()
    {
        var mock_rec = new Mock<ICommandReceiver>();
        var mock_cmd = new Mock<ICommand>();
        var sendcmd = new SendCommand(mock_cmd.Object, mock_rec.Object);

        sendcmd.Execute();

        mock_rec.Verify(r => r.Receive(mock_cmd.Object));
    }

    [Fact]
    public void ReceiverDeniedTest()
    {
        var mock_rec = new Mock<ICommandReceiver>();
        var mock_cmd = new Mock<ICommand>();

        mock_rec.Setup(r => r.Receive(mock_cmd.Object)).Throws(new Exception());

        var sendcmd = new SendCommand(mock_cmd.Object, mock_rec.Object);

        Assert.Throws<Exception>(() => sendcmd.Execute());
    }
}
