using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ZToolKit
{
    public class ZToolKitCore : SingletonDontDestroy<ZToolKitCore>
    {
        /// <summary> 用以判断工具箱是否初始化完成 </summary>
        /// <remarks>
        /// <para> 一般情况下,该属性方便于存在同时初始化,</para>
        /// <para> 又依赖于ZToolKit的脚本调用,</para>
        /// <para> 游戏运行中该脚本只初始化1次,且只能存在1个 </para>
        /// </remarks>
        public bool Initialized { get; private set; }
        
        protected override void OnAwake()
        {
            
        }

        protected override async void OnStart()
        {
            LogTool.ZToolKitLog("初始化", "初始化开始");
            await ResTool.Init();
            LogTool.ZToolKitLog("初始化", "ResTool资源目录加载完成");
            await Config.Init();
            LogTool.ZToolKitLog("初始化", "Config表格配置加载完成");
            LogTool.ZToolKitLog("初始化", "初始化完成");

            Initialized = true;
        }
    }
}
