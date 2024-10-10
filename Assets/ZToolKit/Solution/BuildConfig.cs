using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZToolKit
{
    [CreateAssetMenu(fileName = "BuildConfig", menuName = "ZTool-GameConfig/BuildConfig", order = 0)]
    public class BuildConfig : ScriptableObject
    {
        /// <summary> 在控制台中输出 </summary>
        public bool logToolKit;
    }
}
