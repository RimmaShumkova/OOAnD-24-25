using App;
using App.Scopes;
using SpaceBattle.Lib;

namespace SpaceBattle.Tests;
public class RegisterIoCDependencyGameObjTests
{
    public RegisterIoCDependencyGameObjTests()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void GameObject_Add_ShouldPreserveExistingId()
    {
        new RegisterIoCDependencyGameObj().Execute();

        var gameObj = new Dictionary<string, object>
        {
            { "Id", "Id-test" }
        };

        Ioc.Resolve<SpaceBattle.Lib.ICommand>("GameObject.Add", gameObj).Execute();

        var retrieved = Ioc.Resolve<Dictionary<string, object>>("GameObject.Get", "Id-test");

        Assert.Equal("Id-test", retrieved["Id"]);
    }

    [Fact]
    public void GameObject_Add_Then_Get_ShouldReturnSameItem()
    {
        new RegisterIoCDependencyGameObj().Execute();

        var gameObj = new Dictionary<string, object> { { "Id", "Id-test" } };

        Ioc.Resolve<SpaceBattle.Lib.ICommand>("GameObject.Add", gameObj).Execute();

        var retrieved = Ioc.Resolve<Dictionary<string, object>>("GameObject.Get", "Id-test");

        Assert.NotNull(retrieved);
        Assert.Equal(gameObj, retrieved);
    }

    [Fact]
    public void GameObject_Add_Duplicate_ShouldThrowException()
    {
        new RegisterIoCDependencyGameObj().Execute();
        var gameObj = new Dictionary<string, object> { { "Id", "Id-test" } };

        Ioc.Resolve<SpaceBattle.Lib.ICommand>("GameObject.Add", gameObj).Execute();

        Assert.Throws<InvalidOperationException>(() =>
        {
            Ioc.Resolve<SpaceBattle.Lib.ICommand>("GameObject.Add", gameObj).Execute();
        });
    }

    [Fact]
    public void GameObject_Remove_ShouldRemoveItem()
    {
        new RegisterIoCDependencyGameObj().Execute();
        var gameObj = new Dictionary<string, object> { { "Id", "Id-test" } };

        Ioc.Resolve<SpaceBattle.Lib.ICommand>("GameObject.Add", gameObj).Execute();
        Ioc.Resolve<SpaceBattle.Lib.ICommand>("GameObject.Remove", "Id-test").Execute();

        Assert.Throws<Exception>(() =>
        {
            Ioc.Resolve<Dictionary<string, object>>("GameObject.Get", "Id-test");
        });
    }

    [Fact]
    public void GameObject_Get_Nonexistent_ShouldThrowException()
    {
        new RegisterIoCDependencyGameObj().Execute();

        Assert.Throws<Exception>(() =>
        {
            Ioc.Resolve<Dictionary<string, object>>("GameObject.Get", "non-existent-id");
        });
    }

    [Fact]
    public void GameObject_Add_WithoutId_ShouldThrowException()
    {
        new RegisterIoCDependencyGameObj().Execute();
        var gameObj = new Dictionary<string, object> { { "Id", "" } };

        var ex = Assert.Throws<InvalidOperationException>(() =>
        {
            Ioc.Resolve<SpaceBattle.Lib.ICommand>("GameObject.Add", gameObj).Execute();
        });

        Assert.Equal("Идентификатор объекта не может быть null или пустым.", ex.Message);
    }
}
