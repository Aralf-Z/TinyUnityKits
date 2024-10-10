/*

这是一个核心层类
*/

using System;
using UnityEngine;

namespace ZToolKit
{   
    public static class LogTool
    {
        public static bool logZToolKit = true;
        
        /// <summary>
        /// 编辑器日志
        /// </summary>
        /// <param name="headStr">标题</param>
        /// <param name="messageStr">信息</param>
        /// <param name="color">颜色</param>
        public static void EditorLog(string headStr, string messageStr, Color color = default)
        {
#if UNITY_EDITOR
            color = color == default ? Color.white : color;
            Debug.Log($"<color={color.ToHex()}>[{headStr}]: {messageStr}</color>");
#endif
        }

        /// <summary>
        /// 编辑器错误日志
        /// </summary>
        /// <param name="messageStr"></param>
        public static void EditorLogError(string messageStr)
        {
#if UNITY_EDITOR
            Debug.LogError(messageStr);
#endif
        }
        
        /// <summary>
        /// 编辑器错误日志
        /// </summary>
        /// <param name="messageStr"></param>
        public static void EditorLogError(Exception messageStr)
        {
#if UNITY_EDITOR
            Debug.LogError(messageStr);
#endif
        }
        
        public static void ZToolKitLog(string headStr, string messageStr)
        {
            if (logZToolKit)
            {
                Debug.Log($"<color=#bd868dff>[ZTool-{headStr}]: {messageStr}</color>");
            }
        }
        
        public static void ZToolKitLogError(string headStr, string messageStr)
        {
            if (logZToolKit)
            {
                Debug.Log($"<color=#CA463Dff>[ZTool-{headStr}]: {messageStr}</color>");
            }
        }
    }
} 