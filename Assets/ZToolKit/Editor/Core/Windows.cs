using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZToolKit.Editor
{
    public class WindowsDefine
    {

        /// <summary>
        /// 停靠窗口类型集合
        /// </summary>
        public static readonly Type[] DockedWindowTypes =
        {
            typeof(PanelWindow),
            typeof(BuildWindow),
        };
    }
}
