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
            LogTool.ToolInfo("初始化", "Tool初始化开始");

            //资源加载模块初始化
            ProgramTimeCost.StartCount();
            LogTool.ToolInfo("初始化", $"ResTool初始化开始-{GameConfig.ResMode}");
            await ResTool.Init();
            Event_ResInited?.Invoke();
            LogTool.ToolInfo("初始化", $"ResTool初始化完成-cost: {ProgramTimeCost.EndCount():F4}s");
            
            //数据表配置模块初始化
            ProgramTimeCost.StartCount();
            await CfgTool.Init();
            Event_CfgInited?.Invoke();
            LogTool.ToolInfo("初始化", $"CfgTool表格配置加载完成-cost: {ProgramTimeCost.EndCount():F4}s");
            
            LogTool.ToolInfo("初始化", $"初始化完成");
        }
    }
}
