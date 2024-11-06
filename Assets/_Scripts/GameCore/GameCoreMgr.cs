using System;
using System.Reflection;
using QFramework;
using UnityEngine;
using ZToolKit;

namespace Game.Core
{
    public class GameCoreMgr:
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
                        RegisterSystem((ISystem)Activator.CreateInstance(type), type);
                        LogTool.Info("QFGameCoreMgr",$"AutoRegister System: {type.Name}.");
                    }
                    else if (typeof(IModel).IsAssignableFrom(type))
                    {
                        RegisterModel((IModel)Activator.CreateInstance(type), type);
                        LogTool.Info("QFGameCoreMgr",$"AutoRegister Model: {type.Name}.");
                    }
                    else if (typeof(IUtility).IsAssignableFrom(type))
                    {
                        RegisterUtility((IUtility)Activator.CreateInstance(type), type);
                        LogTool.Info("QFGameCoreMgr",$"AutoRegister Utility: {type.Name}.");
                    }
                    else
                    {
                        LogTool.Error("QFGameCoreMgr",$"Wrong AutoRegister: {type.Name}.");
                    }
                }
            }
        }
    }
}