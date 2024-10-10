using System;
using Cysharp.Threading.Tasks;

namespace ZToolKit
{
    public static class ToolKit
    {
        public static event Action Event_ResInited;
        public static event Action Event_CfgInited;
        public static event Action Event_GameConfigInited;
        
        public static async UniTask Init()
        {
            ConfigLoader.Init();//最先加载
            Event_GameConfigInited?.Invoke();
            LogTool.ZToolKitLog("初始化", "基础配置加载完成");
            
            LogTool.ZToolKitLog("初始化", "Tool初始化开始");
            
            await ResTool.Init();
            Event_ResInited?.Invoke();
            LogTool.ZToolKitLog("初始化", "ResTool资源目录加载完成");
            
            await CfgTool.Init();//cfg初始化依赖于ResTool;
            Event_CfgInited?.Invoke();
            LogTool.ZToolKitLog("初始化", "CfgTool表格配置加载完成");
            
            
            
            LogTool.ZToolKitLog("初始化", "初始化完成");
            
        }
    }
}
