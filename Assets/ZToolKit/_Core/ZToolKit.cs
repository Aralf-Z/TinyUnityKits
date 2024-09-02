using System;
using Cysharp.Threading.Tasks;

namespace ZToolKit
{
    public static class ToolKit
    {
        public static event Action Event_ResInited;
        public static event Action Event_CfgInited;
        
        public static async UniTask Init()
        {
            LogTool.ZToolKitLog("初始化", "初始化开始");
            
            await ResTool.Init();
            Event_ResInited?.Invoke();
            LogTool.ZToolKitLog("初始化", "ResTool资源目录加载完成");
            
            await CfgTool.Init();//cfg初始化依赖于ResTool;
            Event_CfgInited?.Invoke();
            LogTool.ZToolKitLog("初始化", "Config表格配置加载完成");
            
            LogTool.ZToolKitLog("初始化", "初始化完成");
            
        }
    }
}
