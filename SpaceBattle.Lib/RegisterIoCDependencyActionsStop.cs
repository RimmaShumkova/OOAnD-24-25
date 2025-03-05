﻿using App;
namespace SpaceBattle.Lib;

public class RegisterIoCDependencyActionsStop : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Actions.Stop",
            (object[] args) => new StopActionCommand((IDictionary<string, object>)args[0])
        ).Execute();
    }
}
