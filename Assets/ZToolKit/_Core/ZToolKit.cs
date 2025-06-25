using System;
using Cysharp.Threading.Tasks;

namespace ZToolKit
{
    public static class ToolKit
    {
        public static event Action EvtResInited;
        public static event Action EvtCfgInited;

        public static async UniTask Init()
        {
            LogTool.ToolInfo("初始化", "Tool初始化开始");

            //资源加载模块初始化
            ProgramTimeCost.StartCount();
            LogTool.ToolInfo("初始化", $"ResTool初始化开始");
            await ResTool.Init();
            EvtResInited?.Invoke();
            LogTool.ToolInfo("初始化", $"ResTool初始化完成-cost: {ProgramTimeCost.EndCount():F4}s");
            
            //数据表配置模块初始化
            ProgramTimeCost.StartCount();
            await CfgTool.Init();
            EvtCfgInited?.Invoke();
            LogTool.ToolInfo("初始化", $"CfgTool表格配置加载完成-cost: {ProgramTimeCost.EndCount():F4}s");
            
            LogTool.ToolInfo("初始化", $"初始化完成");
        }
    }
}