using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ZToolKit
{
    public static class ConsoleTool
    {
        public static async UniTask Init()
        {
            if(true)
            {
                await ConsoleCore.Init();
            }
        }
    }
}
