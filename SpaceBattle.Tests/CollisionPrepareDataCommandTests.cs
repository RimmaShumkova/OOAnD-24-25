using App;
using App.Scopes;
using Moq;

namespace SpaceBattle.Lib.Tests;

public class CollisionPrepareDataCommandTests
{
    public CollisionPrepareDataCommandTests()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void Execute_ThrowsWhenDataGeneratorIsNull()
    {
        Assert.Throws<ArgumentNullException>(
            () => new CollisionPrepareDataCommand(null!)
        );
    }

    [Fact]
    public void Execute_GeneratesCollisionData_AndCallsSaveCommand()
    {
        var mockDataGenerator = new Mock<ICollisionDataGenerator>();
        var mockSaveCommand = new Mock<ICommand>();

        mockDataGenerator
            .Setup(g => g.GenerateCollisionData())
            .Returns(new List<int[]> { new int[] { 6, 3, 46, 28 } });

        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Collision.DataSaveCommand",
            (object[] args) => mockSaveCommand.Object
        ).Execute();

        var regCollPrepareData = new RegisterIoCDependencyCollisionPrepareData();
        regCollPrepareData.Execute();
        var regCollName = new RegisterIoCDependencyGetCollisionName();
        regCollName.Execute();
        var collPrepareDataCommand = Ioc.Resolve<ICommand>("Collision.PrepareData", mockDataGenerator.Object);

        collPrepareDataCommand.Execute();

        mockDataGenerator.Verify(g => g.GenerateCollisionData(), Times.Once);
        mockSaveCommand.Verify(c => c.Execute(), Times.Once);
    }
}
