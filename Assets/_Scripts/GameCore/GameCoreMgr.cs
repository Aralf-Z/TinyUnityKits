using System;
using System.Reflection;
using QFramework;
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

    public override TSystem GetSystem<TSystem>()
    {
        var sys = base.GetSystem<TSystem>();
        if (sys is null)
        {
            LogTool.Error("QfArch", $"{typeof(TSystem)} is not Registered in GameCoreMgr");
        }

        return sys;
    }

    public override TModel GetModel<TModel>()
    {
        var mdl = base.GetModel<TModel>();
        if (mdl is null)
        {
            LogTool.Error("QfArch", $"{typeof(TModel)} is not Registered in GameCoreMgr");
        }

        return mdl;
    }
    
    public override TUtility GetUtility<TUtility>()
    {
        var util = base.GetUtility<TUtility>();
        if (util is null)
        {
            LogTool.Error("QfArch", $"{typeof(TUtility)} is not Registered in GameCoreMgr");
        }

        return util;
    }

    protected override void OnDeinit()
    {
    }
}