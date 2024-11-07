using System;
using System.Reflection;
using QFramework;
using UnityEngine;
using ZToolKit;


public class GameCoreMgr :
    Architecture<GameCoreMgr>
{
    protected override void Init()
    {
        var types = Assembly.GetExecutingAssembly().GetTypes();
        foreach (var type in types)
        {
            if (Attribute.GetCustomAttribute(type, typeof(AutoRegisterAttribute)) is AutoRegisterAttribute autoRegister
                && autoRegister.Archi == typeof(GameCoreMgr))
            {
                if (typeof(ISystem).IsAssignableFrom(type))
                {
                    RegisterSystem((ISystem) Activator.CreateInstance(type), type);
                    LogTool.Info("QfArch", $"AutoRegister System: {type.Name}.");
                }
                else if (typeof(IModel).IsAssignableFrom(type))
                {
                    RegisterModel((IModel) Activator.CreateInstance(type), type);
                    LogTool.Info("QfArch", $"AutoRegister Model: {type.Name}.");
                }
                else if (typeof(IUtility).IsAssignableFrom(type))
                {
                    RegisterUtility((IUtility) Activator.CreateInstance(type), type);
                    LogTool.Info("QfArch", $"AutoRegister Utility: {type.Name}.");
                }
                else
                {
                    LogTool.Error("QfArch", $"Wrong AutoRegister: {type.Name}.");
                }
            }
        }
    }

    protected override void OnDeinit()
    {
    }
}

[AutoRegister(typeof(GameCoreMgr))]
public class MyModel : AbstractModel
{
    protected override void OnInit()
    {
        enemyCount = 10;
    }

    public int enemyCount;
}

public struct CmdKillEnemy : ICommand
{
    private readonly int mKillCount;

    public CmdKillEnemy(int killCount)
    {
        mKillCount = killCount;
    }

    public void Execute(IArchitectureFunction arch)
    {
        arch.GetModel<MyModel>().enemyCount -= mKillCount;
        arch.SendEvent(new EvtOnKillEnemy(arch.GetModel<MyModel>().enemyCount));
    }
}

public struct EvtOnKillEnemy
{
    public readonly int enemyCount;

    public EvtOnKillEnemy(int enemyCount)
    {
        this.enemyCount = enemyCount;
    }
}