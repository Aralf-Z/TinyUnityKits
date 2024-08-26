using System.Collections;
using System.Collections.Generic;
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

        protected override void OnStart()
        {
            // Debug.Log("游戏开始初始化");
            // StartCoroutine(GameInit());
        }

        // private IEnumerator GameInit()
        // {
        //     //资源包加载
        //     yield return StartCoroutine(ResTool.LoadJson());
        //     Debug.Log("游戏资源目录加载完成");
        //     
        //     Debug.Log("游戏初始化完成");
        //     
        //     Initialized = true;
        // }
    }
}
