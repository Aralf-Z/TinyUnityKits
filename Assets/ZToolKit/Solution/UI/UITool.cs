using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using cfg;
using UnityEngine;

namespace ZToolKit
{
    public static class UITool
    {
        /// <summary>
        /// 打开UI
        /// </summary>
        /// <param name="uiPanel">UI层级</param>
        /// <param name="uiData">UI数据</param>
        /// <typeparam name="T">UI</typeparam>
        /// <returns></returns>
        public static T OpenUI<T>(UIPanel uiPanel, object uiData = default) where T : UIScreen
        {
            return UIMgr.OpenUIScreen<T>(uiPanel, uiData);
        }

        /// <summary>
        /// 获得打开中的UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T TryGetUIOnOpen<T>() where T : UIScreen
        {
            return UIMgr.TryGetUIOnOpen<T>();
        }

        /// <summary>
        /// 隐藏UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void HideUI<T>() where T : UIScreen
        {
            UIMgr.HideUIScreen<T>();
        }

        /// <summary>
        /// 隐藏UI
        /// </summary>
        /// <param name="uiScreen"></param>
        public static void HideUI(UIScreen uiScreen)
        {
            UIMgr.HideUIScreen(uiScreen);
        }

        /// <summary>
        /// 隐藏所有UI
        /// </summary>
        public static void HideAllUI()
        {
            UIMgr.HideAllUIScreens();
        }

        /// <summary>
        /// UI是否打开中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool IsOpen<T>() where T : UIScreen
        {
            return UIMgr.IsOpen<T>();
        }
    }
}
