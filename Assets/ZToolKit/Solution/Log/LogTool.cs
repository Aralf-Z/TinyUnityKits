/*

这是一个核心层类
*/

using System;
using UnityEngine;

namespace ZToolKit
{   
    public static class LogTool
    {
        private static bool logZToolKit = true;

        #region 游戏日志

        /// <summary>
        /// 信息级别日志
        /// </summary>
        /// <param name="headStr"></param>
        /// <param name="messageStr"></param>
        /// <param name="color"></param>
        public static void Info(string headStr, string messageStr, Color color = default)
        {
            DefaultLog($"Info-{headStr}", messageStr, color);
        }
        
        /// <summary>
        /// 调试级别日志
        /// </summary>
        /// <param name="headStr"></param>
        /// <param name="messageStr"></param>
        /// <param name="color"></param>
        public static void Debug(string headStr, string messageStr, Color color = default)
        {
            DefaultLog($"Debug-{headStr}", messageStr, color);
        }

        /// <summary>
        /// 警告级别日志
        /// </summary>
        /// <param name="headStr"></param>
        /// <param name="messageStr"></param>
        public static void Warning(string headStr, string messageStr)
        {
            DefaultLog($"Warning-{headStr}", messageStr, Color.yellow);
        }
        
        /// <summary>
        /// 错误级别日志
        /// </summary>
        /// <param name="headStr"></param>
        /// <param name="messageStr"></param>
        public static void Error(string headStr, string messageStr)
        {
            DefaultLog($"Error-{headStr}", messageStr, Color.magenta);
        }
        
        #endregion
        
        #region 编辑器日志

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
            UnityEngine.Debug.Log($"<color={color.ToHex()}>[{headStr}]: {messageStr}</color>");
#endif
        }

        /// <summary>
        /// 编辑器错误日志
        /// </summary>
        /// <param name="messageStr"></param>
        public static void EditorError(string messageStr)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogError(messageStr);
#endif
        }

        #endregion

        #region ZToolKit日志

        /// <summary>
        /// ZToolKit信息日志
        /// </summary>
        /// <param name="headStr"></param>
        /// <param name="messageStr"></param>
        public static void ToolInfo(string headStr, string messageStr)
        {
            if (logZToolKit)
            {
                UnityEngine.Debug.Log($"<color=#bd868dff>[ZTool-{headStr}]: {messageStr}</color>");
            }
        }
        
        /// <summary>
        /// ZToolKit错误日志
        /// </summary>
        /// <param name="headStr"></param>
        /// <param name="messageStr"></param>
        public static void ToolError(string headStr, string messageStr)
        {
            if (logZToolKit)
            {
                UnityEngine.Debug.Log($"<color=#ca463dff>[ZTool-{headStr}]: {messageStr}</color>");
            }
        }

        #endregion

        private static void DefaultLog(string headStr, string messageStr, Color color = default)
        {
            color = color == default ? Color.white : color;
             UnityEngine.Debug.Log($"<color={color.ToHex()}>[{headStr}]: {messageStr}</color>");
        }
    }
} 